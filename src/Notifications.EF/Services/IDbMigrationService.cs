using System.Threading.Tasks;

namespace Notifications.EF.Services
{
    /// <summary>
    /// Database migration service.
    /// </summary>
    public interface IDbMigrationService
    {
        /// <summary>
        /// Migrates database.
        /// </summary>
        /// <returns>Task.</returns>
        Task MigrateAsync();
    }
}

