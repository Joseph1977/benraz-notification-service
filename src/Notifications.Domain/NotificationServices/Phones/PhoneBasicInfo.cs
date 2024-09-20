using System.Collections.Generic;

namespace Notifications.Domain.NotificationServices.Phones
{
    /// <summary>
    /// Phone basic information.
    /// </summary>
    public class PhoneBasicInfo
    {
        /// <summary>
        /// Text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// From.
        /// </summary>
        public string From { get; set; }

        /// <summary>
        /// To.
        /// </summary>
        public string To { get; set; }
    }
}
