using Xunit;
using Microsoft.Extensions.DependencyInjection;
using API.Common;
using API.Controllers;
using API.BLL.Services.AccessControl;
using API.BLL.Services.Users;
using API.DAL.Context;
using API.DAL.Models;
using API.BLL.Validators.DalValidators;
using FluentValidation;
using API.BLL.Services.Games;
using CreateGame = API.BLL.Services.Games.CreateGame.Models;
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
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        public void createGame(long userID)
        {
          /*  CreateGame.InModel inModel = new CreateGame.InModel
            {
                IdCreatedUser = userID,
            };*/
            var result = gameServices.CreateGame(userID);
        }
    }
}
