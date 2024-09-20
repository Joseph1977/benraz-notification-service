using System.Collections.Generic;
using System.Threading.Tasks;

namespace Notifications.Domain.NotificationServices.Emails
{
    /// <summary>
    /// Emails service.
    /// </summary>
    public interface IEmailsService
    {
        /// <summary>
        /// Send an html email without using template.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="tos">To list.</param>
        /// <param name="subject">Subject.</param>
        /// <param name="displayname">Display name.</param>
        /// <param name="HtmlContent"> the full HTML email body</param>
        /// <returns>Task.</returns>
        Task SendHtmlContentAsync(
            string from,
            List<string> tos,
            string subject,
            string displayname,
            string HtmlContent);

        /// <summary>
        /// Send white labeled email template.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <param name="subject">Subject.</param>
        /// <param name="displayname">Display name.</param>
        /// <param name="templateId">Template identifier.</param>
        /// <param name="substitutions">Substitutions.</param>
        /// <param name="whiteLabelParams">Whitelabel parameters.</param>
        /// <param name="dynamicData">Dynamic data.</param>
        /// <returns>Task.</returns>
        Task SendWhiteLabeledEmailTemplateAsync(
            string from,
            string to,
            string subject,
            string displayname,
            string templateId,
            Dictionary<string, string> substitutions,
            Dictionary<string, string> whiteLabelParams,
            object dynamicData = null);

        /// <summary>
        /// Send multiple whitelabeled email template.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <param name="subject">Subject.</param>
        /// <param name="displayname">Display name.</param>
        /// <param name="templateId">Template identifier.</param>
        /// <param name="substitutions">Substitutions.</param>
        /// <param name="whiteLabelParams">Whitelabel parameters.</param>
        /// <param name="dynamicData">Dynamic data.</param>
        /// <returns>Task.</returns>
        Task SendMultipleWhiteLabeledEmailTemplateAsync(
            string from,
            List<string> tos,
            string subject,
            string displayname,
            string templateId,
            Dictionary<string, string> substitutions,
            Dictionary<string, string> whiteLabelParams,
            List<string> attachments,
            object dynamicData = null);
    }
}
