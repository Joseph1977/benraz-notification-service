using Notifications.EF;
using NUnit.Framework;
using System.Net.Http;
using System.Threading.Tasks;

namespace Notifications.WebApi.IntegrationTests
{
    [TestFixture]
    public abstract class ControllerTestsBase
    {
        protected HttpClient HttpClient;
        protected NotificationsDbContext DBContext;

        [SetUp]
        public virtual async Task SetUpAsync()
        {
            HttpClient = Config.HttpClient;
            DBContext = Config.DBContext;

            await ClearDataAsync();
        }

        [TearDown]
        public virtual async Task TearDownAsync()
        {
            await ClearDataAsync();
        }

        protected virtual Task ClearDataAsync()
        {
            return Task.CompletedTask;
        }

        protected NotificationsDbContext CreateDbContext()
        {
            return Config.CreateDbContext();
        }
    }
}
