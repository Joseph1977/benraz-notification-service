using Notifications.Domain.Settings;
using Benraz.Infrastructure.EF;

namespace Notifications.EF.Repositories
{
    /// <summary>
    /// Settings entries repository.
    /// </summary>
    public class SettingsEntriesRepository :
        RepositoryBase<string, SettingsEntry, NotificationsDbContext>,
        ISettingsEntriesRepository
    {
        /// <summary>
        /// Creates repository.
        /// </summary>
        /// <param name="context">Context.</param>
        public SettingsEntriesRepository(NotificationsDbContext context)
            : base(context)
        {
        }
    }
}

