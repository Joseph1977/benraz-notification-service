using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Notifications.Domain.Settings;

namespace BackOffice.EF.DataConfigurations
{
    class SettingsEntryDataConfiguration : IEntityTypeConfiguration<SettingsEntry>
    {
        public void Configure(EntityTypeBuilder<SettingsEntry> builder)
        {
            builder.HasData(
                Create("General:AuthorizationBaseUrl", null, "Base URL of Authorization service"),
                Create("General:AuthorizationAccessToken", null, "Access token to Authorization service"),
                Create("TokenValidation:Audience", null, "Comma separated audiences allowed"),
                Create("Emails:SendGridApiKey", null, "SendGrid API key for email"),
                Create("Emails:SupportEmail", null, "Support email"),
                Create("Emails:SupportPhone", null, "Support phone number"),
                Create("Emails:SendGridAsmId", null, "SendGrid ASM identifier for email"),
                Create("Phones:AccountSID", null, "SendGrid API key for sms"),
                Create("Phones:AuthToken", null, "Account authentication"),
                Create("Phones:OutTwillionumber", null, "Support phone number"),
                Create("MessagesFilter:Emails:IsEnabled", false, "Email filter enable/disable parameter"),
                Create("MessagesFilter:Emails:Type", null, "0 (eWhiteList) - Allow only from white list; 1 (ePatern) - Allo only if fit a regix pattern; 2 (eTraverse) -Travers ALL to specific email; 3 (eAllowAll) - Do not filter, allow all.; 4 (eDiableAll) - This mean No email will be passed, kind of disable feature; 5 (eWhiteDomainList) - Allow emails only with Domain in the DomainsWhitelist from EmailFilter list, not valid for phones"),
                Create("MessagesFilter:Emails:Value", null, "Email filter value (can be list with comma)"),
                Create("MessagesFilter:Phones:IsEnabled", false, "Phone filter enable/disable parameter"),
                Create("MessagesFilter:Phones:Type", null, "0 (eWhiteList) - Allow only from white list; 1 (ePatern) - Allo only if fit a regix pattern; 2 (eTraverse) -Travers ALL to specific email; 3 (eAllowAll) - Do not filter, allow all.; 4 (eDiableAll) - This mean No email will be passed, kind of disable feature; 5 (eWhiteDomainList) - Allow emails only with Domain in the DomainsWhitelist from EmailFilter list, not valid for phones"),
                Create("MessagesFilter:Phones:Value", null, "Phone filter value (can be list with comma)"));
        }

        private SettingsEntry Create(string id, object value, string description = null)
        {
            return new SettingsEntry
            {
                Id = id,
                Value = value?.ToString(),
                Description = description
            };
        }
    }
}


