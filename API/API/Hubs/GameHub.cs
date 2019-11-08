using API.BLL.Models;
using API.BLL.Services.Games;
using API.BLL.Services.Users;
using API.DAL.Models;
using Microsoft.AspNetCore.SignalR;
using System;
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
            await Clients.Caller.SendAsync("CreatedGame", new CreateGame.OutModel { IdGame = gameName });
        }

        async public Task ConnectToGame(ConnectToGame.InModel inModel)
        {
            //TODO check active this game or not
            await _gameServices.ConnectToGame(inModel.IdGame, inModel.IdGame);
            await Groups.AddToGroupAsync(Context.ConnectionId, inModel.IdGame.ToString());
        }

        async public Task MakeAMove(MakeAMoveModel.InModel inModel)
        {
            if(await _gameServices.MakeMove(inModel.IdUser, inModel.IdGame, inModel.X, inModel.Y)
                == StateGame.GameOver)
            {
                await Clients.OthersInGroup(inModel.IdGame.ToString()).SendAsync("GameOver");
                await Clients.Caller.SendAsync("UserWin");
                await Clients.Groups(inModel.IdGame.ToString()).SendAsync("Disconnect");
            }
            else
            {
                await Clients.OthersInGroup(inModel.IdGame.ToString()).SendAsync("NextMove", 
                    new MakeAMoveModel.OutModel {
                        LastX = inModel.X,
                        LastY = inModel.Y,
                    });
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

        public override Task OnDisconnectedAsync(Exception exception)
        {
            
            return base.OnDisconnectedAsync(exception);
        }
    }
}
