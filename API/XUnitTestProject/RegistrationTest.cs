using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;

using API.Common;
using API.Controllers;
using API.BLL.Services.AccessControl;
using API.BLL.Services.Users;
using Registration = API.BLL.Services.AccessControl.Registration.Models;

namespace XUnitTestProject
{
    public class RegistrationTest : Test
    {
        UserController userController;
        private ServiceProvider serviceProvider;
        public RegistrationTest(DbFixture dbFixture) : base(dbFixture)
        {
            serviceProvider = dbFixture.ServiceProvider;
            userController = new UserController(
                    serviceProvider.GetService<IAccessControlService>(),
                    serviceProvider.GetService<IUserServices>()
                );
        }
        [Theory]
        [InlineData("a@a.a", "qwerty")]
        public void Test(string email, string password)
        {
            Registration.InModel inModel = new Registration.InModel
            {
                Email = email,
                PasswordHash = Encoding.UTF8.GetString(Cryptor.CalculateHashOfPassword(password))
            };
            serviceProvider.GetService<IAccessControlService>().Registration(inModel);
        }
    }
}
