using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Notifications.EF;
using NUnit.Framework;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Notifications.WebApi.IntegrationTests
{
    [SetUpFixture]
    public class Config
    {
        private static IConfiguration _configuration;

        public static NotificationsDbContext DBContext;
        public static HttpClient HttpClient;

        [OneTimeSetUp]
        public static void SetUpFixture()
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            DBContext = CreateDbContext();

            var server = new TestServer(new WebHostBuilder()
                .UseConfiguration(_configuration)
                .UseStartup<StartupStub>());

            HttpClient = server.CreateClient();

            var token = _configuration.GetValue<string>("AccessToken");
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public static NotificationsDbContext CreateDbContext()
        {
            var dbContextOptions = new DbContextOptionsBuilder<NotificationsDbContext>()
                .EnableSensitiveDataLogging()
                .UseSqlServer(_configuration.GetConnectionString("Notifications"))
                .Options;

            return new NotificationsDbContext(dbContextOptions);
        }
    }
}
