using System;
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
    public class RegistrationTest : Test
    {
        UserController userController;
        IDbContext dbContext;
        public RegistrationTest(DbFixture dbFixture) : base(dbFixture)
        {
            dbContext = serviceProvider.GetService<IDbContext>();
            userController = new UserController(
                    serviceProvider.GetService<IAccessControlService>(),
                    serviceProvider.GetService<IUserServices>()
                );
        }

        User user;

        // тест верной регистрации
        [Fact]
        public void Registation()
        {
            UserRegistrationValidator validationRules = new UserRegistrationValidator();
            Faker<User> newUser = new Faker<User>("en")
                .RuleFor(u => u.Username, (f, u) => f.Internet.UserName())
                .RuleFor(u => u.Email, (f, u) => f.Internet.Email())
                .RuleFor(u => u.PasswordHash, (f, u) => Cryptor.CalculateHashOfPassword(f.Internet.Password()));
            user = newUser.Generate();
            var valid = validationRules.ValidateAndThrowAsync(newUser);
            FluentValidation.Results.ValidationResult result = validationRules.Validate(newUser);
            Assert.True(result.IsValid);
        }

        // тест повторной регистрации
        [Fact]
        public void ReRegistration()
        {
            Assert.Throws<ArgumentNullException>(() => dbContext.Users.Add(user));
        }

        // тест неверной регистрации
        [Theory]
        [InlineData("", "", "")]
        [InlineData(" ", " ", " ")]
        [InlineData("           ", "            ", "            ")]
        [InlineData("", "asd@a.a", "qwerty")]
        [InlineData("user", "", "qwerty")]
        [InlineData("user", "asd@a.a", "")]
        public void IncorrectRegistration(string username, string email, string password)
        {
            UserRegistrationValidator validationRules = new UserRegistrationValidator();
            User newUser = new User
            {
                Username = username,
                Email = email,
                PasswordHash = Cryptor.CalculateHashOfPassword(password)
            };
            var valid = validationRules.ValidateAndThrowAsync(newUser);
            FluentValidation.Results.ValidationResult result = validationRules.Validate(newUser);
            Assert.False(result.IsValid);
        }
    }
}
