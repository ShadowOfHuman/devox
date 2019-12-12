using NUnit.Framework;
using API.BLL.Services.AccessControl;
using Auth = API.BLL.Services.AccessControl.Authentication.Models;
using Moq;
using API.Common;
using System.Threading.Tasks;
using System.Text;
using FluentValidation;
using System.Threading;
using API.Controllers;

namespace Tests
{
    public class Tests
    {
        [TestCase("a@a.a", "12345")]
        [TestCase("fsdf@mail.ru", "12312")]
        public void TestValidation(string userEmail, string userPassword)
        {
            var password = Cryptor.CalculateHashOfPassword(userPassword);
            Auth.InModel inModel = new Auth.InModel
            {
                Email = userEmail,
                PasswordHash = Encoding.UTF8.GetString(password, 0, password.Length)
            };

            ValidationException validationException = new ValidationException(It.IsAny<string>());
            var mock = new Mock<IAccessControlService>();
            mock.Setup(a => a.Authentication(inModel, It.IsAny<CancellationToken>())).ThrowsAsync(validationException);

            var controller = new UserController(mock.Object, null);
            Assert.ThrowsAsync<ValidationException>(async () => await controller.Authentication(inModel));
        }
    }