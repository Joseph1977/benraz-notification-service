using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace Notifications.Domain.NotificationServices.Phones
{
    /// <summary>
    /// Phone service.
    /// </summary>
    public class PhoneService : IPhoneService
    {
        private readonly PhoneSettings _smsSettings;

        /// <summary>
        /// Creates service.
        /// </summary>
        /// <param name="smsSettings">Sms settings.</param>
        public PhoneService(
            IOptions<PhoneSettings> smsSettings)
        {
            _smsSettings = smsSettings.Value;
        }

        /// <summary>
        /// Send sms.
        /// </summary>
        /// <param name="body">Body.</param>
        /// <param name="recipients">Recipients.</param>
        /// <param name="sender">Sender.</param>
        /// <returns>Task.</returns>
        public async Task SendSmsAsync(
            string body,
            List<string> recipients,
            string sender)
        {
            string accountSid = _smsSettings.AccountSID;
            string authToken = _smsSettings.AuthToken;
            sender = String.IsNullOrWhiteSpace(sender) ? sender : _smsSettings.OutTwillionumber;
            TwilioClient.Init(accountSid, authToken);
            foreach (var number in recipients)
            {
                var message = await MessageResource.CreateAsync(
                body: body,
                from: new Twilio.Types.PhoneNumber(sender),
                to: new Twilio.Types.PhoneNumber(number));
            }
        }
    }
}
