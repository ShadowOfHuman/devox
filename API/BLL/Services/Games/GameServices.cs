using API.BLL.Models;
using API.BLL.Services.GameMoves;
using API.BLL.Services.Games;
using API.BLL.Services.Users;
using API.DAL.Context;
using API.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using GetAllGames = API.BLL.Services.Games.GetAllGames.Models;

namespace API.BLL.Services.Games
{
    public class GameServices : IGameServices
    {
        private readonly IDbContext _dbContext;
        private readonly IUserServices _userServices;
        private readonly IGameMoveService _gameMoveService;

        public GameServices(IDbContext dbContext, IUserServices userService, IGameMoveService gameMoveService )
        {
            this._dbContext = dbContext;
            this._userServices = userService;
        }
        //1 - крестики, 2 - нолики, 0 - ни че го
        async public Task<long> CreateGame(long IdUser, string title, int size)
        {
            var user = _dbContext.Users.Find(IdUser);
            if (user == null)
            {
                throw new InvalidOperationException($"User with id = {IdUser} not found.");
            }
            Game game = new Game
            {
                Title = title,
                FirstUser = user,
                SizeField = size,
                MoveCross = Convert.ToBoolean(new Random().Next(0, 2)),
                GameState = (int)StateGame.WaitingSecondUser
                //TODO: Generate url for game
            };
            await _dbContext.Games.AddAsync(game);
            _dbContext.SaveChanges();
            return game.Id;
        }
        async public Task ConnectToGame(long idGame, long idUser)
        {
            Game game = await this.Get(idGame);
            User user = await _dbContext.Users.FindAsync(idUser);
            if (user == null)
            {
                throw new InvalidOperationException("User not found");
            }
            game.TwoUser = user;
            game.GameState = (int)StateGame.StartedGame;
            _dbContext.Games.Update(game);
            await _dbContext.SaveChangesAsync();
        }
        async public Task<StateGame> MakeMove(long userId, long gameId, int newX, int newY)
        {
            User user;
            Game game;
            try
            {
                Console.WriteLine(gameId);
                game = await _dbContext.Games.Where(item => item.Id == gameId)
                    .Include(gameItem => gameItem.GameMoves).FirstOrDefaultAsync();

                user = await _dbContext.Users.Where(userItem => userItem.Id == userId)
                    .Include(userItem => userItem.Games).Where(x => x.Id == gameId)
                    .Include(gameItem => gameItem)
                    .FirstOrDefaultAsync();

            }
            catch
            {
                throw new InvalidOperationException("Can't getting user.");
            }

            GameMove gameMove;
            if(user.Games.First().GameState == (int)StateGame.GameOver)
            {
                throw new InvalidOperationException("Game already finished.");
            }

            gameMove = new GameMove {
                X = newX,
                Y = newY,
                Game = user.Games.First(),
                User = user,
                NumberMove = user.Games.First().GameMoves.Count() + 1
            };
            await _gameMoveService.Create(gameMove);
            return CheckStateGame(user.Games.First(), gameMove);
        }
        #region CRUD service
        public Task<long> Create(Game item, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
        async public Task Delete(long id, CancellationToken cancellationToken = default)
        {
            var game = await this.Get(id);
            _dbContext.Games.Remove(game);
            await _dbContext.SaveChangesAsync();
        }
        async public Task<Game> Get(long id, CancellationToken cancellationToken = default)
        {
            var game = await _dbContext.Games.Where(gameItem => gameItem.Id == id)
                    .Include(userItem => userItem.FirstUser)
                    .Include(gameItem => gameItem.TwoUser)
                    .FirstOrDefaultAsync();
            if (game == null)
            {
                throw new InvalidOperationException("Game not found.");
            }
            return game;
        }
        async public Task<IEnumerable<Game>> GetAll(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Games.Where(game => game.GameState == (int)StateGame.WaitingSecondUser).ToListAsync(cancellationToken);
        }
        async public Task<IEnumerable<Game>> GetAllGameByUser(long IdUser)
        {
            var result = await _dbContext.Games.Where(x => x.FirstUser.Id == IdUser).ToListAsync();
            return result;
        }
        public async Task<StateGame> GetStateGame(long idGame)
        {
            Game game = await this.Get(idGame);
            return (StateGame)game.GameState;
        }
        async public Task Update(Game item, CancellationToken cancellationToken = default)
        {
            _dbContext.Games.Update(item);
            await _dbContext.SaveChangesAsync();

        }
        async public Task<IEnumerable<GameMove>> GetAllMoveByGame(long gameId)
        {
            var move = await _dbContext.GameMoves.Where(x => x.Game.Id == gameId).ToListAsync();
            return move;
        }
        #endregion

        async public Task<IEnumerable<GetAllGames.Models.OutModel>> GetAllPublicGames()
        {
            List<GetAllGames.Models.OutModel> outModels = new List<GetAllGames.Models.OutModel>();
            var games = await GetAll();
            (await GetAll()).ToList().ForEach(item => outModels.Add(new GetAllGames.Models.OutModel(item.Id, item.Title, item.SizeField)));
            return outModels;
        }
        private StateGame CheckStateGame(Game game, GameMove gameMove)
        {
            List<GameMove> localGameMove;
            //Check row
            localGameMove = game.GameMoves.Where(x => x.Y == gameMove.Y && x.User == gameMove.User).ToList();
            if (localGameMove.Count == game.CountItemForWin)
            {
                int x = localGameMove[0].X;
                for (int i = 1; i < localGameMove.Count(); i++)
                {
                    if (Math.Abs(x - localGameMove[i].X) != 1)
                    {
                        break;
                    }
                    x = localGameMove[i].X;
                }
                MakeGameIsFinished(game, gameMove.User.Id);
                return StateGame.GameOver;
            }
            //Check column
            localGameMove = game.GameMoves.Where(x => x.X == gameMove.X && x.User == gameMove.User).ToList();
            if (localGameMove.Count == game.CountItemForWin)
            {
                int y = localGameMove[0].Y;
                for (int i = 1; i < localGameMove.Count(); i++)
                {
                    if (Math.Abs(y - localGameMove[i].Y) != 1)
                    {
                        break;
                    }
                    y = localGameMove[i].Y;
                }
                MakeGameIsFinished(game, gameMove.User.Id);              
                return StateGame.GameOver;
            }
            //Check diagonal
            int startPoint = -1, endPoint;
            int countXorNullinSequence = 1;
            GameMove currentGameMove = gameMove;
            while (true)
            {
                try
                {

                    currentGameMove = game.GameMoves.Where(x =>
                        x.X == currentGameMove.X + 1 &&
                        x.Y == currentGameMove.Y + 1 &&
                        x.User == gameMove.User).Single();
                    countXorNullinSequence++;
                }
                catch
                {
                    endPoint = currentGameMove.X;
                    startPoint = currentGameMove.X - game.CountItemForWin - 1;
                    break;
                }
            }

            while (countXorNullinSequence != 5)
            {
                try
                {

                    currentGameMove = game.GameMoves.Where(x =>
                        x.X == startPoint &&
                        x.Y == startPoint &&
                        x.User == gameMove.User).Single();
                    countXorNullinSequence++;
                    startPoint++;
                }
                catch
                {
                    return StateGame.StartedGame;
                }
            }
            MakeGameIsFinished(game, gameMove.User.Id);
            return StateGame.GameOver;

            /*var countSquareInRow = (plyingField.GetLength(0) - countItemForWin + 1);
            var countSquareInColumn = (plyingField.GetLength(1) - countItemForWin + 1);
            for (int i = 0; i < countSquareInRow; i++)
            {
                for (int j = 0; i < countSquareInColumn; i++)
                {
                    List<int[]> localPlyingField = new List<int[]>();
                    foreach (int k in Enumerable.Range(i, countItemForWin))
                    {
                        localPlyingField.Add(plyingField[k]);
                    }
                    int result = CheckCombination(localPlyingField.ToArray());
                    if (result == 0)
                    {
                        continue;
                    }
                    return players[result];
                }
            }
            return 0;*/
        }

        private async void MakeGameIsFinished(Game game, long userWinId)
        {
            game.IdWinUser = userWinId;
            game.GameState = (int)StateGame.GameOver;
            _dbContext.Games.Update(game);
            await _dbContext.SaveChangesAsync();

        }

        //next method not using
        private string[] PlayingFieldToArray(string field)
        {
            string[] rows = field.Split('*');
            return rows;
        }
        private string PlayingFieldToString(string[] field)
        {
            char sep = '*';
            string result = "";
            foreach(string item in field)
            {
                result+=sep+item;
            }
            return result;
        }
        public int CheckCombination(int[][] localPlyingField)
        {
            //Check row
            for (int i = 0; i < localPlyingField.GetLength(0); i++)
            {
                int result = localPlyingField[i][0];
                for (int j = 0; j < localPlyingField.GetLength(1); j++)
                {
                    result = result & localPlyingField[i][j];
                    if (result == 0)
                    {
                        break;
                    }
                }
                return result;
            }

            //Check columns
            for (int i = 0; i < localPlyingField.GetLength(1); i++)
            {
                int result = localPlyingField[0][i];
                for (int j = 0; j < localPlyingField.GetLength(0); j++)
                {
                    result = result & localPlyingField[j][i];
                    if (result == 0)
                    {
                        break;
                    }
                }
                return result;
            }
            //Check diagonal
            int result1 = localPlyingField[0][0];
            int result2 = localPlyingField[0][localPlyingField.GetLength(1) - 1];
            for (int i = 0; i < localPlyingField.GetLength(0); i++)
            {
                result1 = result1 & localPlyingField[i][i];
                result2 = result2 & localPlyingField[i][localPlyingField.GetLength(0) - i - 1];
                if (result1 == 0 && result2 == 0)
                {
                    break;
                }
            }
            if (result1 != 0)
            {
                return result2;
            }
            if (result2 != 0)
            {
                return result1;
            }
            return 0;
        }

        public async Task SetGameNoFinished(long gameId)
        {
            Game game = await this.Get(gameId);
            game.GameState = (int)StateGame.NotFinish;
            await this.Update(game);
        }
    }
}
