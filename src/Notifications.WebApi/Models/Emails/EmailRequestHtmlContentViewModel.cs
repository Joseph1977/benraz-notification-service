using System.Collections.Generic;

namespace Notifications.WebApi.Models.Emails
{
    /// <summary>
    /// Email request view model.
    /// </summary>
    public class EmailRequestHtmlContentViewModel
    {
        /// <summary>
        /// From email address.
        /// </summary>
        public string From { get; set; }

        /// <summary>
        /// From display name.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// To email addresses.
        /// </summary>
        public string[] Tos { get; set; }

        /// <summary>
        /// Email subject.
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// The email html content.
        /// </summary>
        public string HtmlContent { get; set; }

        /// <summary>
        /// Skip opt out check.
        /// </summary>
        public bool SkipOptOutCheck { get; set; }
    }
}
