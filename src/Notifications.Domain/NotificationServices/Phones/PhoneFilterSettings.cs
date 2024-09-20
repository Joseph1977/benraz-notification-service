using Notifications.Domain.Messages.MessagesFilter;
using System.Collections.Generic;

namespace Notifications.Domain.NotificationServices.Phones
{
    /// <summary>
    /// Phones filter settings.
    /// </summary>
    public class PhonesFilterSettings
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
        /// Returns phones filter value collection.
        /// </summary>
        /// <returns>Phones filter collection.</returns>
        public IEnumerable<string> GetPhonesValueCollection()
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