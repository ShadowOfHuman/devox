using Xunit;
using Bogus;
using Microsoft.Extensions.DependencyInjection;
using API.Common;
using API.Controllers;
using API.BLL.Services.AccessControl;
using API.BLL.Services.Users;
using API.DAL.Context;
using API.DAL.Models;
using API.BLL.Validators.DalValidators;
using FluentValidation;

namespace XUnitTestProject
{
    public class AuthTest : Test
    {
        UserController userController;
        IDbContext dbContext;
        public AuthTest(DbFixture dbFixture) : base(dbFixture)
        {
            dbContext = serviceProvider.GetService<IDbContext>();
            userController = new UserController(
                    serviceProvider.GetService<IAccessControlService>(),
                    serviceProvider.GetService<IUserServices>()
                );
        }

        // тест верной авторизации
        [Fact]
        public void Auth()
        {
            UserAuthValidator validationRules = new UserAuthValidator();
            Faker<User> newUser = new Faker<User>("en")
                .RuleFor(u => u.Email, (f, u) => f.Internet.Email())
                .RuleFor(u => u.PasswordHash, (f, u) => Cryptor.CalculateHashOfPassword(f.Internet.Password()));
            var user = newUser.Generate();
            var valid = validationRules.ValidateAndThrowAsync(newUser);
            FluentValidation.Results.ValidationResult result = validationRules.Validate(newUser);
            Assert.True(result.IsValid);
        }

        // тест неверной авторизации
        [Theory]
        [InlineData("", "")]
        [InlineData(" ", " ")]
        [InlineData("           ", "            ")]
        [InlineData("", "asd@a.a")]
        [InlineData("user", "")]
        [InlineData("user", "asd@a.a")]
        [InlineData("123123", "12312")]
        public void IncorrectAuth(string email, string password)
        {
            UserAuthValidator validationRules = new UserAuthValidator();
            User newUser = new User
            {
                Email = email,
                PasswordHash = Cryptor.CalculateHashOfPassword(password)
            };
            var valid = validationRules.ValidateAndThrowAsync(newUser);
            FluentValidation.Results.ValidationResult result = validationRules.Validate(newUser);
            Assert.False(result.IsValid);
        }
    }
}
