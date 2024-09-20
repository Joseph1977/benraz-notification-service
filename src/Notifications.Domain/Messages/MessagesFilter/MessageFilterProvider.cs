using Microsoft.Extensions.Options;
using Notifications.Domain.NotificationServices.Emails;
using Notifications.Domain.NotificationServices.Phones;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace Notifications.Domain.Messages.MessagesFilter
{
    /// <summary>
    /// Message filter provider.
    /// </summary>
    public class MessageFilterProvider : IMessageFilterProvider
    {
        private readonly EmailsFilterSettings _emailsFilterSettings;
        private readonly PhonesFilterSettings _phonesFilterSettings;

        /// <summary>
        /// Creates provider.
        /// </summary>
        public MessageFilterProvider(
            IOptions<EmailsFilterSettings> emailsFilterSettings,
            IOptions<PhonesFilterSettings> phonesFilterSettings)
        {
            _emailsFilterSettings = emailsFilterSettings.Value;
            _phonesFilterSettings = phonesFilterSettings.Value;
        }

        /// <summary>
        /// Is message allowed.
        /// </summary>
        /// <param name="to">To.</param>
        /// <param name="msgType">Message type.</param>
        /// <returns>Is message allowed.</returns>
        public bool IsMessageAllowed(string to, MessageType msgType)
        {
            var filterEnabled = GetIsFilterEnabled(msgType);
            if (!filterEnabled)
            {
                return true;
            }
            else if (string.IsNullOrEmpty(to))
            {
                return false;
            }

            var filterSourceType = GetFilterSourceType(msgType);
            var sorceFilterList = GetSourceFilterList(msgType);

            switch (filterSourceType)
            {
                case FilterModeType.eAllowAll:
                    return true;
                case FilterModeType.eDiableAll:
                    return false;
                case FilterModeType.eWhiteDomainList:
                    {
                        if (sorceFilterList == null) return false;
                        MailAddress address = new MailAddress(to.Trim());
                        return sorceFilterList.Contains(address.Host);
                    }
                case FilterModeType.eWhiteList:
                    {
                        if (sorceFilterList == null) return false;
                        return sorceFilterList.Contains(to.Trim());
                    }
                case FilterModeType.ePatern:
                    {
                        Regex rgx = new Regex(sorceFilterList?.FirstOrDefault());
                        return rgx.IsMatch(to);
                    }
            }

            return true;
        }

        /// <summary>
        /// Final to address to send.
        /// </summary>
        /// <param name="to">To.</param>
        /// <param name="msgType">Message type.</param>
        /// <returns>Final to address to send.</returns>
        public string FinalToAddressToSend(string to, MessageType msgType)
        {
            var filterEnabled = GetIsFilterEnabled(msgType);
            if ((!filterEnabled) || (to == null))
            {
                return to;
            }

            var finalToSend = new List<string>() { to };
            finalToSend = FinalTosAddressToSend(finalToSend, msgType);

            if (finalToSend?.Count > 0)
            {
                return finalToSend.First();
            }
            return null;
        }

        /// <summary>
        /// Recive a list of emails, filter what should be filterd, change what should be changed.
        /// </summary>
        /// <param name="tos">The list of email we want to send.</param>
        /// <param name="msgType">The type of the message Email or SMS.</param>
        /// <returns> Null if email not allowed, or email send disabled 
        /// otherwise, the list of email(s) to be send, same or after modified incase traverse mode.
        /// </returns>
        public List<string> FinalTosAddressToSend(List<string> tos, MessageType msgType)
        {
            var filterEnabled = GetIsFilterEnabled(msgType);
            if ((!filterEnabled) || (tos == null) || (tos.Count < 1))
            {
                return tos;
            }

            var filterSourceType = GetFilterSourceType(msgType);
            var sorceFilterList = GetSourceFilterList(msgType);

            if (filterSourceType == FilterModeType.eTraverse)
            {
                return sorceFilterList.ToList();
            }

            var finalToSend = new List<string>();
            foreach (string sourceTo in tos)
            {
                if (this.IsMessageAllowed(sourceTo, msgType))
                {
                    finalToSend.Add(sourceTo);
                }
            }

            return finalToSend;
        }

        private bool GetIsFilterEnabled(MessageType msgType)
        {
            return msgType == MessageType.eEmail ? _emailsFilterSettings.IsEnabled : _phonesFilterSettings.IsEnabled;
        }

        private FilterModeType GetFilterSourceType(MessageType msgType)
        {
            return msgType == MessageType.eEmail ? _emailsFilterSettings.Type : _phonesFilterSettings.Type;
        }

        private IEnumerable<string> GetSourceFilterList(MessageType msgType)
        {
            return msgType == MessageType.eEmail ?
                _emailsFilterSettings.GetEmailValueCollection() :
                _phonesFilterSettings.GetPhonesValueCollection();
        }
    }
}