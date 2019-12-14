using Xunit;
using Microsoft.Extensions.DependencyInjection;
using API.Controllers;
using API.BLL.Services.AccessControl;
using API.BLL.Services.Users;
using API.DAL.Context;
using API.BLL.Services.Games;
using System;
using API.DAL.Models;

namespace XUnitTestProject
{
    public class ConnectToGame : Test
    {
        IGameServices gameServices;
        UserController userController;
        IDbContext dbContext;
        public ConnectToGame(DbFixture dbFixture) : base(dbFixture)
        {
            dbContext = serviceProvider.GetService<IDbContext>();
            userController = new UserController(
                    serviceProvider.GetService<IAccessControlService>(),
                    serviceProvider.GetService<IUserServices>()
                );
            gameServices = serviceProvider.GetService<IGameServices>();
        }

        // тест подключения к игре
        [Theory]
        [InlineData(1,1,2)]
        [InlineData(2,2,3)]
        [InlineData(3,4,5)]
        [InlineData(4,3,2)]
        [InlineData(5,2,1)]
        public void ConnectToGameTest(long idGame, long userID1, long userID2)
        {
            var result = gameServices.CreateGame(userID1).Result;
            gameServices.ConnectToGame(idGame, userID2);
            Game game = gameServices.Get(idGame).Result;
            Assert.Equal(userID2, game.TwoUser.Id);
        }

        // тест исключений
        [Theory]
        [InlineData(1, -1, 2)]
        [InlineData(1, 1, -2)]
        [InlineData(-1, 1, 2)]
        [InlineData(-1, -1, -2)]
        [InlineData(null, null, null)]
        public void ExeptionConnectToGameTest(long idGame, long userID1, long userID2)
        {
            try
            {
                var result = gameServices.CreateGame(userID1).Result;
                gameServices.ConnectToGame(idGame, userID2);
                Assert.ThrowsAsync<AggregateException>(async () => await gameServices.Get(idGame));
            }
            catch (AggregateException ae)
            {
                Assert.Equal(typeof(InvalidOperationException), ae.InnerException.GetType());
            }
        }
    }
}
