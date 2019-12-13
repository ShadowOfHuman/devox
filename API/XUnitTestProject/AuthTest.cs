using System;
using System.Text;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

using API.Common;
using API.Controllers;
using API.BLL.Services.AccessControl;
using API.BLL.Services.Users;
using Moq;
using API.DAL.Models;
using API.BLL.Services;
using Auth = API.BLL.Services.AccessControl.Authentication.Models;

using System.Threading;
using System.Threading.Tasks;

namespace XUnitTestProject
{
    public class AuthTest : Test
    {
        UserController userController;
        private ServiceProvider serviceProvider;
        public AuthTest(DbFixture dbFixture) : base(dbFixture) {
            serviceProvider = dbFixture.ServiceProvider;
            userController = new UserController(
                    serviceProvider.GetService<IAccessControlService>(),
                    serviceProvider.GetService<IUserServices>()
                );
        }

        /*[Theory]
        [InlineData("a@a.a", "qwertyuiop")]
        [InlineData("a@a.a", "1234567890")]
        [InlineData("a@a.a", "qwerty12345")]
        [InlineData("a@a.a", "qwer")]
        [InlineData("asda@asd.sdad", "1q2w3e4r5")]
        public void TestTrueData(string email, string password)
        {
            Auth.InModel inModel = new Auth.InModel
            {
                Email = email,
                PasswordHash = Encoding.UTF8.GetString(Cryptor.CalculateHashOfPassword(password))
            };
        }*/

        [Theory]
        [InlineData("", "")]
        [InlineData("asdfa@a.ru", "aasdasda")]
        public void TestFalseData(string email, string password)
        {
            Auth.InModel inModel = new Auth.InModel
            {
                Email = email,
                PasswordHash = Encoding.UTF8.GetString(Cryptor.CalculateHashOfPassword(password))
            };

            try
            {
                Auth.OutModel outModel = userController.Authentication(inModel).Result;
            }
            catch (AggregateException ae)
            {
/*                if (ae.InnerExceptions != null)
                {

                    Assert.True();
                }*/

                //HARDCOOODE
                ae.Handle((x) =>
                {

                    if (x is ValidationException)
                    {
                        Assert.True(true);
                        return true;
                    }
                    Assert.True(false);
                    return false;
                });
            }
        }
    }
}
