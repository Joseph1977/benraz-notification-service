using AutoMapper;
using Notifications.Domain.NotificationServices.Emails;
using Notifications.Domain.Settings;
using Notifications.WebApi.Models.Emails;
using Notifications.WebApi.Models.Settings;
using Benraz.Infrastructure.Common.Paging;
using System;

namespace Notifications.WebApi
{
    class NotificationsAutoMapperProfile : Profile
    {
        public NotificationsAutoMapperProfile()
        {
            CreateCommonMaps();
            CreateSettingsMaps();
            CreateEmailsMaps();
        }

        private void CreateCommonMaps()
        {
            CreateMap(typeof(Page<>), typeof(Page<>));
        }

        private void CreateSettingsMaps()
        {
            CreateMap<SettingsEntry, SettingsEntryViewModel>()
                .ForMember(x => x.CreateTimeUtc, o => o.MapFrom(x => SpecifyUtc(x.CreateTimeUtc)))
                .ForMember(x => x.UpdateTimeUtc, o => o.MapFrom(x => SpecifyUtc(x.UpdateTimeUtc)));
            CreateMap<AddSettingsEntryViewModel, SettingsEntry>();
            CreateMap<ChangeSettingsEntryViewModel, SettingsEntry>();
        }

        private void CreateEmailsMaps()
        {
            CreateMap<EmailBasicInfoViewModel, EmailBasicInfo>()
                .ForMember(x => x.Tos, opts => opts.PreCondition((src) => src.Tos != null));
        }
        
        private static DateTime? SpecifyUtc(DateTime? dateTime)
        {
            return dateTime.HasValue ? DateTime.SpecifyKind(dateTime.Value, DateTimeKind.Utc) : (DateTime?)null;
        }
    }
}
