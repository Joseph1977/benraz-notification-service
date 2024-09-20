namespace Notifications.Domain.NotificationServices.Emails
{
    /// <summary>
    /// Emails settings.
    /// </summary>
    public class EmailsSettings
    {
        /// <summary>
        /// SendGrid API key.
        /// </summary>
        public string SendGridApiKey { get; set; }

        /// <summary>
        /// SendGrid ASM identifier.
        /// </summary>
        public int SendGridAsmId { get; set; }

        /// <summary>
        /// Support email.
        /// </summary>
        public string SupportEmail { get; set; }

        /// <summary>
        /// Support phone.
        /// </summary>
        public string SupportPhone { get; set; }
    }
}
