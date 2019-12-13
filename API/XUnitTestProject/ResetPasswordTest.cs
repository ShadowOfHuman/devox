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
using ResetPassword = API.BLL.Services.Users.ResetPassword.Models;
using API.BLL.Services.Users;

namespace XUnitTestProject
{
    public class ResetPasswordTest : Test
    {
        UserController userController;
        IDbContext dbContext;
        public ResetPasswordTest(DbFixture dbFixture) : base(dbFixture)
        {
            dbContext = serviceProvider.GetService<IDbContext>();
            userController = new UserController(
                    serviceProvider.GetService<IAccessControlService>(),
                    serviceProvider.GetService<IUserServices>()
                );
        }

        // тест валидацию данных
        /*        [Theory]
                [InlineData("", "", "")]
                [InlineData(" ", " ", " ")]
                [InlineData("           ", "            ", "            ")]
                [InlineData("", "asd@a.a", "qwerty")]
                [InlineData("user", "", "qwerty")]
                [InlineData("user", "asd@a.a", "")]
                [InlineData("123123", "12312", "123123")]
                public void ResetPassword(string username, string email, string password)
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
                }*/

        // тест на изменение пароля
        [Theory]
        [InlineData(1, "password", "newpassword")]
        public void ChangePassword(long userID, string oldPassword, string newPassword)
        {
            ResetPassword.InModel inModel = new ResetPassword.InModel
            {
                IdUser = userID,
                OldPasswordHash = oldPassword,
                NewPasswordHash = newPassword
            };
/*            ResetPassword
            ResetPassword.OutModel outModel = userController.Authentication(inModel).Result;*/
            /*            var valid = validationRules.ValidateAndThrowAsync(newUser);
                        FluentValidation.Results.ValidationResult result = validationRules.Validate(newUser);
                        Assert.False(result.IsValid);*/

        }
    }
}
