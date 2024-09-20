using System.Collections.Generic;

namespace Notifications.Domain.NotificationServices.Emails
{
    /// <summary>
    /// Email basic info.
    /// </summary>
    public class EmailBasicInfo
    {
        /// <summary>
        /// From.
        /// </summary>
        public string From { get; set; }

        /// <summary>
        /// Display name.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// To.
        /// </summary>
        public string To { get; set; }

        /// <summary>
        /// Tos.
        /// </summary>
        public List<string> Tos { get; set; }

        /// <summary>
        /// Attachmetns.
        /// </summary>
        public List<string> Attachments { get; set; }

        /// <summary>
        /// Subject.
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Content type.
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Template identifier.
        /// </summary>
        public string TempId { get; set; }

        /// <summary>
        /// Skip opt out check.
        /// </summary>
        public bool SkipOptOutCheck { get; set; }
    }
}
