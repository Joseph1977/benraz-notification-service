using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Notifications.EF;
using System;

namespace Notifications.WebApi.Configuration
{
    /// <summary>
    /// Database configuration source.
    /// </summary>
    public class DatabaseConfigurationSource : IConfigurationSource
    {
        private readonly Action<DbContextOptionsBuilder<NotificationsDbContext>> _options;

        /// <summary>
        /// Creates database configuration source.
        /// </summary>
        /// <param name="options">Database context options.</param>
        public DatabaseConfigurationSource(Action<DbContextOptionsBuilder<NotificationsDbContext>> options)
        {
            _options = options;
        }

        /// <summary>
        /// Returns configuration provider.
        /// </summary>
        /// <param name="builder">Configuration builder.</param>
        /// <returns>Configuration provider.</returns>
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new DatabaseConfigurationProvider(_options);
        }
    }
}
