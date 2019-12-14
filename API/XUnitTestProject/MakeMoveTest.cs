using Xunit;
using Microsoft.Extensions.DependencyInjection;
using API.Controllers;
using API.BLL.Services.AccessControl;
using API.BLL.Services.Users;
using API.DAL.Context;
using API.BLL.Services.Games;
using System;
using API.DAL.Models;
using API.BLL.Models;

namespace XUnitTestProject
{
    public class MakeMoveTest : Test
    {
        IGameServices gameServices;
        UserController userController;
        IDbContext dbContext;
        public MakeMoveTest(DbFixture dbFixture) : base(dbFixture)
        {
            dbContext = serviceProvider.GetService<IDbContext>();
            userController = new UserController(
                    serviceProvider.GetService<IAccessControlService>(),
                    serviceProvider.GetService<IUserServices>()
                );
            gameServices = serviceProvider.GetService<IGameServices>();
        }
       /* [Theory]
        [InlineData(1, 2, 1, 0, 0)]
        [InlineData(1, 2, 1, 1, 0)]
        [InlineData(1, 2, 1, 1, 1)]
        [InlineData(1, 2, 1, 2, 0)]
        [InlineData(1, 2, 1, 3, 0)]
        public void MakeMove(long userID1, long userID2,  long gameId, int newX, int newY)
        {
            var result = gameServices.CreateGame(userID1).Result;
            gameServices.ConnectToGame(gameId, userID2);
            Assert.Equal(StateGame.GameOver, StateGame.GameOver);
        }*/
    }
}
