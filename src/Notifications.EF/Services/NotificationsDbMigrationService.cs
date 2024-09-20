using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Notifications.Domain.Settings;
using Benraz.Infrastructure.Common.DataRedundancy;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Notifications.EF.Services
{
    /// <summary>
    /// Database migration service.
    /// </summary>
    public class NotificationsDbMigrationService : IDbMigrationService
    {
        private readonly IConfigurationRoot _configurationRoot;
        private readonly IDrChecker _drChecker;
        private readonly NotificationsDbContext _context;

        /// <summary>
        /// Creates service.
        /// </summary>
        /// <param name="configuration">Configuration root.</param>
        /// <param name="drChecker">Data redundancy checker.</param>
        /// <param name="context">Database context.</param>
        public NotificationsDbMigrationService(
            IConfiguration configuration,
            IDrChecker drChecker,
            NotificationsDbContext context)
        {
            _configurationRoot = (IConfigurationRoot)configuration;
            _drChecker = drChecker;
            _context = context;
        }

        /// <summary>
        /// Migrates database.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task MigrateAsync()
        {
            if (!_drChecker.IsActiveDR())
            {
                return;
            }

            await _context.Database.MigrateAsync();
            await MigrateDefautSettingsAsync();

            _configurationRoot.Reload();
        }

        private async Task MigrateDefautSettingsAsync()
        {
            var defaultEntities = SettingsEntry.GetDefaultValues();
            var dbEntities = await _context.SettingsEntries.ToListAsync();

            var entitiesToAdd = defaultEntities.Where(x => !dbEntities.Any(y => y.Id == x.Id)).ToList();
            entitiesToAdd.ForEach(x =>
            {
                x.CreateTimeUtc = DateTime.UtcNow;
                x.UpdateTimeUtc = DateTime.UtcNow;
            });

            _context.SettingsEntries.AddRange(entitiesToAdd);
            await _context.SaveChangesAsync();
        }
    }
}
