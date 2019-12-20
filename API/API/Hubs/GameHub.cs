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

        async public Task CreateGame(long gameId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, gameId.ToString());
        }

        async public Task ConnectToGame(ConnectToGame.InModel inModel)
        {
            //TODO check active this game or not
            await _gameServices.ConnectToGame(inModel.IdGame, inModel.IdUser);
            await Groups.AddToGroupAsync(Context.ConnectionId, inModel.IdGame.ToString());
            User user = await _userServices.Get(inModel.IdUser);
            await Clients.GroupExcept(inModel.IdGame.ToString(), inModel.IdGame.ToString()).SendAsync("UserWasBeenConnection", inModel.IdUser, user.Username);

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
