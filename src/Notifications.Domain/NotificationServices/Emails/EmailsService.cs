using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using SendGrid;
using SendGrid.Helpers.Mail;
using SendGrid.Helpers.Mail.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Notifications.Domain.NotificationServices.Emails
{
    /// <summary>
    /// Emails service.
    /// </summary>
    public class EmailsService : IEmailsService
    {
        private readonly EmailsSettings _emailsSettings;

        /// <summary>
        /// Creates service.
        /// </summary>
        /// <param name="emailsSettings">Emails settings.</param>
        public EmailsService(
            IOptions<EmailsSettings> emailsSettings)
        {
            _emailsSettings = emailsSettings.Value;
        }

        /// <summary>
        /// Send an html email without using template.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="tos">To list.</param>
        /// <param name="subject">Subject.</param>
        /// <param name="displayname">Display name.</param>
        /// <param name="HtmlContent"> the full HTML email body</param>
        /// <returns>Task.</returns>
        public async Task SendHtmlContentAsync(
            string from,
            List<string> tos,
            string subject,
            string displayname,
            string HtmlContent)
        {
            var msg = new SendGridMessage();
            msg.SetFrom(new EmailAddress(from?.Trim(), displayname));
            msg.SetSubject(subject);
            // Check if there are multiple recipients
            if (tos.Count == 1)
            {
                // Add a single recipient
                msg.AddTo(new EmailAddress(tos[0].Trim(), displayname));
            }
            else
            {
                // Add multiple recipients
                var emailAddresses = tos.Select(to => new EmailAddress(to.Trim(), displayname)).ToList();
                msg.AddTos(emailAddresses);
            }

            msg.HtmlContent = HtmlContent;  // Use the full HTML content

            // Send the email
            var response = await new SendGridClient(_emailsSettings.SendGridApiKey).SendEmailAsync(msg);

            if (response.StatusCode != System.Net.HttpStatusCode.Accepted)
            {
                var err = await response.Body.ReadAsStringAsync();
                throw new Exception($"StatusCode: {response.StatusCode} Error: {err} Sent Data: {JObject.FromObject(msg)}");
            }
        }

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
        public async Task SendWhiteLabeledEmailTemplateAsync(
            string from,
            string to,
            string subject,
            string displayname,
            string templateId,
            Dictionary<string, string> substitutions,
            Dictionary<string, string> whiteLabelParams,
            object dynamicData = null)
        {
            var msg = new SendGridMessage();
            msg.SetFrom(new EmailAddress(from?.Trim(), displayname));
            msg.SetGlobalSubject(subject);
            msg.AddTo(new EmailAddress(to?.Trim(), displayname));
            msg.SetTemplateId(templateId);
            msg.Asm = SetAsm();

            if (!string.IsNullOrEmpty(_emailsSettings.SupportEmail))
                msg.AddBcc(_emailsSettings.SupportEmail);


            if (substitutions != null)
            {
                if (templateId.StartsWith("d-"))
                {
                    //remove after moving all emails working through dynamicData and return "msg.AddSubstitutions(substitutions);"
                    substitutions.TryGetValue("-html-", out var html);
                    var templateData = new
                    {
                        subject = subject,
                        html = html
                    };

                    msg.SetTemplateData(templateData);
                }
                else
                    msg.AddSubstitutions(substitutions);
            }

            if (whiteLabelParams != null)
                msg.AddSubstitutions(whiteLabelParams);

            if (dynamicData != null)
                msg.SetTemplateData(dynamicData);

            var response = await new SendGridClient(_emailsSettings.SendGridApiKey).SendEmailAsync(msg);

            if (response.StatusCode != System.Net.HttpStatusCode.Accepted)
            {
                var err = await response.Body.ReadAsStringAsync();
                throw new Exception($"StatusCode: {response.StatusCode} Error: {err} Sent Data: {JObject.FromObject(msg)}");
            }
        }

        /// <summary>
        /// Send multiple whitelabeled email template.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="tos">Tos.</param>
        /// <param name="subject">Subject.</param>
        /// <param name="displayname">Display name.</param>
        /// <param name="templateId">Template identifier.</param>
        /// <param name="substitutions">Substitutions.</param>
        /// <param name="whiteLabelParams">Whitelabel parameters.</param>
        /// <param name="dynamicData">Dynamic data.</param>
        /// <returns>Task.</returns>
        public async Task SendMultipleWhiteLabeledEmailTemplateAsync(
            string from,
            List<string> tos,
            string subject,
            string displayname,
            string templateId,
            Dictionary<string, string> substitutions,
            Dictionary<string, string> whiteLabelParams,
            List<string> attachments,
            object dynamicData = null)
        {
            var msg = new SendGridMessage();
            msg.SetFrom(new EmailAddress(from?.Trim(), displayname));
            msg.AddTo(from?.Trim(), displayname);
            msg.AddBccs(tos.Select(x => new EmailAddress(x)).ToList());
            msg.SetTemplateId(templateId);
            msg.SetGlobalSubject(subject);
            msg.Asm = SetAsm();
            
            if (substitutions != null)
            {
                if (templateId.StartsWith("d-"))
                {
                    //remove after moving all emails working through dynamicData and return "msg.AddSubstitutions(substitutions);"
                    substitutions.TryGetValue("-html-", out var html);
                    var templateData = new
                    {
                        subject = subject,
                        html = html
                    };
                    msg.SetTemplateData(templateData);
                }
                else
                    msg.AddSubstitutions(substitutions);
            }

            if (whiteLabelParams != null)
                msg.AddSubstitutions(whiteLabelParams);

            if (dynamicData != null)
                msg.SetTemplateData(dynamicData);

            if (attachments != null)
            {
                var attached = new List<Attachment>();
                foreach (var file in attachments)
                {
                    try
                    {
                        var fi = new FileInfo(file);
                        var AsBytes = File.ReadAllBytes(file);
                        var AsBase64String = Convert.ToBase64String(AsBytes);
                        attached.Add(new Attachment { Content = AsBase64String, Filename = fi.Name, Type = this.GetMimeMapping(fi.Name) });
                    }
                    catch (Exception)
                    {
                    }
                }
                if (attached.Count > 0)
                    msg.AddAttachments(attached);
            }

            var response = await new SendGridClient(_emailsSettings.SendGridApiKey).SendEmailAsync(msg);

            if (response.StatusCode != System.Net.HttpStatusCode.Accepted)
            {
                var err = await response.Body.ReadAsStringAsync();
                throw new Exception($"StatusCode: {response.StatusCode} Error: {err} Sent Data: {JObject.FromObject(msg)}");
            }
        }

        private string GetMimeMapping(string fileName)
        {
            new FileExtensionContentTypeProvider().TryGetContentType(fileName, out string contentType);
            return contentType ?? "application/octet-stream";
        }

        private ASM SetAsm()
        {
            if (_emailsSettings?.SendGridAsmId > 0)
            {
                return new ASM
                {
                    GroupId = _emailsSettings.SendGridAsmId,
                    GroupsToDisplay = new List<int>() { _emailsSettings.SendGridAsmId }
                };
            }
            return null;
        }
    }
}
