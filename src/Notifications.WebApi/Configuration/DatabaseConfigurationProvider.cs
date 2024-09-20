using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Notifications.EF;
using Notifications.EF.Repositories;
using System;
using System.Linq;

namespace Notifications.WebApi.Configuration
{
    /// <summary>
    /// Database configuration provider.
    /// </summary>
    public class DatabaseConfigurationProvider : ConfigurationProvider
    {
        private readonly Action<DbContextOptionsBuilder<NotificationsDbContext>> _options;

        /// <summary>
        /// Creates database configuration provider.
        /// </summary>
        /// <param name="options">Database context options.</param>
        public DatabaseConfigurationProvider(
            Action<DbContextOptionsBuilder<NotificationsDbContext>> options)
        {
            _options = options;
        }

        /// <summary>
        /// Loads configuration from database.
        /// </summary>
        public override void Load()
        {
            var builder = new DbContextOptionsBuilder<NotificationsDbContext>();
            _options(builder);

            using (var context = new NotificationsDbContext(builder.Options))
            {
                var repository = new SettingsEntriesRepository(context);
                try
                {
                    var settingsEntries = repository.GetAllAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                    Data = settingsEntries.ToDictionary(x => x.Id, x => x.Value);
                }
                catch (Exception)
                {
                    // Could be caused on the first migration
                }
            }
        }
    }
}
