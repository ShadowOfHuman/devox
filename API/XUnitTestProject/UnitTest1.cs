using API.BLL.Helpers;
using API.BLL.Services.AccessControl;
using API.BLL.Services.Emails;
using API.BLL.Services.Games;
using API.BLL.Services.Users;
using API.Common;
using API.Controllers;
using API.DAL.Context;
using BLL.Services.Games;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Text;
using Xunit;


using Auth = API.BLL.Services.AccessControl.Authentication.Models;
using Registration = API.BLL.Services.AccessControl.Registration.Models;

namespace XUnitTestProject
{
    public class DbFixture
    {
        public DbFixture()
        {
            var serviceCollection = new ServiceCollection();
            IConfigurationRoot config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .Build();
            
            var connectionString = config.GetSection("AppSettings").Get<AppSettings>();
            serviceCollection.AddOptions();
            serviceCollection.AddScoped<IDbContext, ApplicationDBContext>();
            serviceCollection.AddDbContext<ApplicationDBContext>(options => options.UseMySql(connectionString.ConnectionString));
            serviceCollection.AddScoped<IAccessControlService, AccessControlService>();
            serviceCollection.AddScoped<IUserServices, UserService>();
            serviceCollection.AddScoped<IGameServices, GameServices>();
            serviceCollection.AddScoped<IEmailService, EmailService>();
            ServiceProvider = serviceCollection.BuildServiceProvider();
        }
        public AppSettings AppSettings { get; set; }
        public ServiceProvider ServiceProvider { get; private set; }
    }

    public class UnitTest1 :IClassFixture<DbFixture>
    {
        UserController userController;
        private ServiceProvider serviceProvider;

        public UnitTest1( DbFixture dbFixture)
        {
            serviceProvider = dbFixture.ServiceProvider;
            userController = new UserController(
                    serviceProvider.GetService<IAccessControlService>(),
                    serviceProvider.GetService<IUserServices>()
                );
        }

        [Fact]
        public void Test1()
        {
            Auth.InModel inModel = new Auth.InModel();
            inModel.Email = "asdasda";
            inModel.PasswordHash = Encoding.UTF8.GetString(Cryptor.CalculateHashOfPassword("asdasda"));
            try
            {
                Auth.OutModel outModel = userController.Authentication(inModel).Result;
            }
            catch (AggregateException ae)
            {
                //HARDCOOOD
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
