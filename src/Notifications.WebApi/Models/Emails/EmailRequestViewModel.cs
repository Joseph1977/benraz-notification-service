using System.Collections.Generic;

namespace Notifications.WebApi.Models.Emails
{
    /// <summary>
    /// Email request view model.
    /// </summary>
    public class EmailRequestViewModel
    {
        /// <summary>
        /// Basic information.
        /// </summary>
        public EmailBasicInfoViewModel BasicInfo { get; set; }

        /// <summary>
        /// Email parameters.
        /// </summary>
        /// <remarks>Null by default.</remarks>
        public Dictionary<string, string> EmailParams { get; set; }

        /// <summary>
        /// White label parameters.
        /// </summary>
        /// <remarks>Null by default.</remarks>
        public Dictionary<string, string> WhileLabelParams { get; set; }
    }
}
