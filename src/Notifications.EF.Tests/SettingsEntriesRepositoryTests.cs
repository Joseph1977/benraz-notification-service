using Notifications.Domain.Settings;
using Notifications.EF.Repositories;
using NUnit.Framework;

namespace Notifications.EF.Tests
{
    [TestFixture]
    public class SettingsEntriesRepositoryTests :
        NotificationsRepositoryTestsBase<string, SettingsEntry, SettingsEntriesRepository>
    {
        protected override SettingsEntry CreateDefaultEntity()
        {
            return new SettingsEntry
            {
                Id = "SettingsEntryId001",
                Value = "SettingsEntryValue001",
                Description = "SettingsDescription001"
            };
        }

        protected override SettingsEntry ChangeEntity(SettingsEntry entity)
        {
            entity.Value = "SettingsEntryValue002";
            entity.Description = "SettingsDescription002";
            return entity;
        }

        protected override SettingsEntriesRepository CreateRepository()
        {
            return new SettingsEntriesRepository(CreateContext());
        }
    }
}

