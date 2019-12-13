using API.BLL.Helpers;
using API.BLL.Services.AccessControl;
using API.BLL.Services.Emails;
using API.BLL.Services.Games;
using API.BLL.Services.Users;

using API.DAL.Context;
using BLL.Services.Games;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using Xunit;

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
    public class Test : IClassFixture<DbFixture>
    {
        public Test(DbFixture dbFixture) {}
    }
}
