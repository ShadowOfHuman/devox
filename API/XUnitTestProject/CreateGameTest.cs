using Xunit;
using Microsoft.Extensions.DependencyInjection;
using API.Controllers;
using API.BLL.Services.AccessControl;
using API.BLL.Services.Users;
using API.DAL.Context;
using API.BLL.Services.Games;
using System;

namespace XUnitTestProject
{
    public class CreateGameTest : Test
    {
        IGameServices gameServices;
        UserController userController;
        IDbContext dbContext;
        public CreateGameTest(DbFixture dbFixture) : base(dbFixture)
        {
            dbContext = serviceProvider.GetService<IDbContext>();
            userController = new UserController(
                    serviceProvider.GetService<IAccessControlService>(),
                    serviceProvider.GetService<IUserServices>()
                );
            gameServices = serviceProvider.GetService<IGameServices>();
        }

        // тест на создания лоббиы
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        public void CreateGame(long userID)
        {
            var result = gameServices.CreateGame(userID).Result;
            Assert.NotNull(result);
        }

        // обработка исключения
        [Theory]
        [InlineData(null)]
        [InlineData(-1)]
        [InlineData(-2)]
        [InlineData(-3)]
        [InlineData(-4)]
        public void CreateGameExeption(long userID)
        {
            Assert.Throws<AggregateException>(() => gameServices.CreateGame(userID).Result);
        }
    }
}
