using Microsoft.EntityFrameworkCore;
using Notifications.Domain.Settings;
using System.Reflection;

namespace Notifications.EF
{
    /// <summary>
    /// Database context.
    /// </summary>
    public class NotificationsDbContext : DbContext
    {
        /// <summary>
        /// Settings entries.
        /// </summary>
        public DbSet<SettingsEntry> SettingsEntries { get; set; }

        /// <summary>
        /// Creates context.
        /// </summary>
        /// <param name="options">Context options.</param>
        public NotificationsDbContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(
                Assembly.GetAssembly(typeof(NotificationsDbContext)));
        }
    }
}
