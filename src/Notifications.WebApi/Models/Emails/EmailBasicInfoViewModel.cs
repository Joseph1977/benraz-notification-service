namespace Notifications.WebApi.Models.Emails
{
    /// <summary>
    /// Email basic info view model.
    /// </summary>
    public class EmailBasicInfoViewModel
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
        /// To email address.
        /// </summary>
        public string To { get; set; }

        /// <summary>
        /// To email addresses.
        /// </summary>
        public string[] Tos { get; set; }

        /// <summary>
        /// Email subject.
        /// </summary>
        public string Subject { get; set; }

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
