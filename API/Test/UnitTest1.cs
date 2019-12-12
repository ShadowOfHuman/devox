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
using API.DAL.Models;
using API.BLL.Services.Users;
using API.DAL.Context;
using System;
using API.BLL.Helpers;
using Microsoft.Extensions.Options;
using System.Web.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using API.BLL.Services.Games;
using BLL.Services.Games;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace Tests
{
    public class DbFixture
    {
        public DbFixture()
        {
            var serviceCollection = new ServiceCollection();
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            AppSettings = config.GetSection("AppSettings") as AppSettings;
            serviceCollection.AddScoped<IDbContext, ApplicationDBContext>();
            serviceCollection.AddDbContext<ApplicationDBContext>(options => options.UseMySql(AppSettings.ConnectionString));
            serviceCollection.AddScoped<IAccessControlService, AccessControlService>();
            serviceCollection.AddScoped<IUserServices, UserService>();
            serviceCollection.AddScoped<IGameServices, GameServices>();
            ServiceProvider = serviceCollection.BuildServiceProvider();
        }
        public AppSettings AppSettings { get; set; }
        public ServiceProvider ServiceProvider { get; private set; }
    }
    [TestFixture]
    public class Tests : IClassFixture<DbFixture>
    {
        ServiceProvider _serviceProvider;
        IDbContext dbContext;
        IUserServices userService;
        IAccessControlService accessControlService;

        private ServiceProvider _serviceProvide;

        public Tests(DbFixture fixture)
        {
            _serviceProvide = fixture.ServiceProvider;
        }

        [SetUp]
        public void SetUp()
        {
            dbContext = _serviceProvider.GetService<IDbContext>();
            accessControlService =_serviceProvider.GetService<IAccessControlService>();
            userService = _serviceProvider.GetService<IUserServices>(); 

        }

        [TestCase("a@a.a", "12345")]
        [TestCase("fsdf@mail.ru", "12312")]
        public void TestAuth(string userEmail, string userPassword)
        {
            var controller = new UserController(accessControlService, userService);
            Auth.InModel inModel = new Auth.InModel();
            inModel.Email = userEmail;
            inModel.PasswordHash = Encoding.UTF8.GetString(Cryptor.CalculateHashOfPassword(userPassword));
            Auth.OutModel outModel;
            try
            {
                outModel = controller.Authentication(inModel).Result;
            }
            catch(Exception e)
            {
                outModel = controller.Authentication(inModel).Result;
            }
            

             /*AppSettings appSettings = new AppSettings() {
                 ConnectionString = "" };
             IOptions<AppSettings> options = Options.Create(appSettings);
             UserService userService = new UserService(null);
             UserController controller = new UserController(
                 new AccessControlService(null, options, userService, null), 
                 userService);
             Auth.InModel inModel = new Auth.InModel();
             inModel.Email = userEmail;
             inModel.PasswordHash = Encoding.UTF8.GetString(Cryptor.CalculateHashOfPassword(userPassword));
             Auth.OutModel outModel = controller.Authentication(inModel).Result;*/

             Assert.AreNotEqual(outModel.IdUser, 0);

            //var password = Cryptor.CalculateHashOfPassword(userPassword);
            //Auth.InModel inModel = new Auth.InModel
            //{
            //    Email = userEmail,
            //    PasswordHash = Encoding.UTF8.GetString(password, 0, password.Length)
            //};

            //ValidationException validationException = new ValidationException(It.IsAny<string>());
            //var mock = new Mock<IAccessControlService>();
            //mock.Setup(a => a.Authentication(inModel, It.IsAny<CancellationToken>())).ThrowsAsync(validationException);

            //var controller = new UserController(mock.Object, null);
            //Assert.ThrowsAsync<ValidationException>(async () => await controller.Authentication(inModel));
        }
    }
}