using Notifications.Domain.Messages.MessagesFilter;
using System.Collections.Generic;

namespace Notifications.Domain.NotificationServices.Emails
{
    /// <summary>
    /// Emails filter settings.
    /// </summary>
    public class EmailsFilterSettings
    {
        /// <summary>
        /// Is enabled.
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Type.
        /// </summary>
        public FilterModeType Type { get; set; }

        /// <summary>
        /// Value.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Returns email filter value collection.
        /// </summary>
        /// <returns>Email filter collection.</returns>
        public IEnumerable<string> GetEmailValueCollection()
        {
            return ToCollection(Value);
        }

        private IEnumerable<string> ToCollection(string values)
        {
            if (string.IsNullOrEmpty(values))
            {
                return new List<string>();
            }

            return values.Split(',', ';');
        }
    }
}