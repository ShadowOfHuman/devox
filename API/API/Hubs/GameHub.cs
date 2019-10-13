using API.BLL.Models;
using API.BLL.Services.Games;
using API.BLL.Services.Users;
using API.DAL.Models;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ConnectToGame = API.BLL.Services.Games.ConnectToGame.Models;
using CreateGame = API.BLL.Services.Games.CreateGame.Models;
using MakeAMoveModel = API.BLL.Services.Games.MakeMove.Models;

namespace API.Hubs
{
    public class GameHub : Hub
    {
        //TODO: Add bitchcoin to user whole win
        private readonly IGameServices _gameServices;
        private readonly IUserServices _userServices;
        public GameHub(IGameServices iGameServices, IUserServices iUserServices)
        {
            _gameServices = iGameServices;
            _userServices = iUserServices;
        }

        async public Task CreateGame(CreateGame.InModel inModel)
        {
            long gameName = await _gameServices.CreateGame(inModel.IdCreatedUser);
            await Groups.AddToGroupAsync(Context.ConnectionId, gameName.ToString());
            //TODO generate link and send it to user for share game
            await Clients.Caller.SendAsync("name method on UI", new CreateGame.OutModel { IdGame = gameName });
        }

        async public Task ConnectToGame(ConnectToGame.InModel inModel)
        {
            //TODO check active this game or not
            await Groups.AddToGroupAsync(Context.ConnectionId, inModel.IdGame.ToString());
        }

        async public Task MakeAMove(MakeAMoveModel.InModel inModel)
        {
            long winUserId = _gameServices.CheckStateGame(inModel.PlayingField,
                inModel.Players, inModel.CountItemForWin);
            Game game = await _gameServices.Get(inModel.IdGame);
            MakeAMoveModel.OutModel outModel = new MakeAMoveModel.OutModel
            {
                IdGame = inModel.IdGame,
                Players = inModel.Players,
                PlayingField = inModel.PlayingField,
                CountItemForWin = inModel.CountItemForWin,
                StateGame = inModel.StateGame

            };
            if (winUserId == 0)
            {
                await Clients.GroupExcept(inModel.IdGame.ToString(),
                    new List<string> { Context.ConnectionId }).SendAsync("Method name", outModel);
                game.PlayingField = ConvertToArrayInt(inModel.PlayingField);
                await _gameServices.Update(game);
            }
            else
            {
                outModel.StateGame = StateGame.GameOver;
                outModel.IdWinUser = winUserId;
                await Clients.Group(inModel.IdGame.ToString()).SendAsync("Method name", outModel);
                game.GameState = (int)StateGame.GameOver;
                game.IdWinUser = winUserId;
                game.PlayingField = ConvertToArrayInt(inModel.PlayingField);
                await _gameServices.Update(game);
                await Clients.Group(inModel.IdGame.ToString()).SendAsync("Close connection", outModel);
            }
        }

        private string ConvertToArrayInt(int[][] input)
        {
            StringBuilder output = new StringBuilder();

            foreach (int [] item in input){
                foreach(int itemTwo in item)
                {
                    output.Append(itemTwo);
                }
            }
            return output.ToString();
        }
    }
}
