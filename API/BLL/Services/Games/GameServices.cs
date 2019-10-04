using API.BLL.Models;
using API.BLL.Services.Games;
using API.DAL.Context;
using API.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BLL.Services.Games
{
    class GameServices : IGameServices
    {
        private readonly IDbContext _dbContext;

        public GameServices(IDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        //1 - крестики, 2 - нолики, 0 - ни че го
        public long CheckStateGame(int[][] plyingField, Dictionary<int, long> players, int countItemForWin)
        {
            var countSquareInRow = (plyingField.GetLength(0) - countItemForWin + 1);
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
            return 0;
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
            //Check diagonal one
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

        async public Task ConnectToGame(long idGame, long idUser)
        {
            Game game = await this.Get(idGame);
            User user = await _dbContext.Users.FindAsync(idUser);
            if (user == null)
            {
                throw new InvalidOperationException("User not found");
            }
            game.User = user;
            game.GameState = (int)StateGame.StartedGame;
            _dbContext.Games.Update(game);
            await _dbContext.SaveChangesAsync();
        }

        public Task<long> Create(Game item, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        async public Task<long> CreateGame(long IdUser)
        {
            var user = _dbContext.Users.Find(IdUser);
            if (user == null)
            {
                throw new InvalidOperationException($"User with id = {IdUser} not found.");
            }
            Game game = new Game
            {
                CreatedUser = user,
                //TODO: Generate url for game
            };
            await _dbContext.Games.AddAsync(game);
            await _dbContext.SaveChangesAsync();
            return game.Id;
        }

        async public Task Delete(long id, CancellationToken cancellationToken = default)
        {
            var game = await this.Get(id);
            _dbContext.Games.Remove(game);
            await _dbContext.SaveChangesAsync();
        }

        async public Task<Game> Get(long id, CancellationToken cancellationToken = default)
        {
            var game = await _dbContext.Games.FindAsync(id);
            if (game == null)
            {
                throw new InvalidOperationException("Game not found.");
            }
            return game;
        }

        async public Task<IEnumerable<Game>> GetAll(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Games.ToListAsync(cancellationToken);
        }

        async public Task<IEnumerable<Game>> GetAllGameByUser(long IdUser)
        {
            var result = await _dbContext.Games.Where(x => x.CreatedUser.Id == IdUser).ToListAsync();
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

    }
}
