using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Notifications.EF;
using System;

namespace Notifications.WebApi.Configuration
{
    /// <summary>
    /// Configuration builder extensions.
    /// </summary>
    public static class ConfigurationBuilderExtensions
    {
        /// <summary>
        /// Adds database configuration source.
        /// </summary>
        /// <param name="builder">Configuration builder.</param>
        /// <param name="setup">Database context setup.</param>
        /// <returns>Configuration builder.</returns>
        public static IConfigurationBuilder AddDatabaseConfiguration(
            this IConfigurationBuilder builder, Action<DbContextOptionsBuilder<NotificationsDbContext>> setup)
        {
            return builder.Add(new DatabaseConfigurationSource(setup));
        }
    }
}
