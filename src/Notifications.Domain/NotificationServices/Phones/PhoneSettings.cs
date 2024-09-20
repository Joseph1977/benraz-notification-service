namespace Notifications.Domain.NotificationServices.Phones
{
    /// <summary>
    /// Phone settings.
    /// </summary>
    public class PhoneSettings
    {
        /// <summary>
        /// Sms API key.
        /// </summary>
        public string AccountSID { get; set; }

        /// <summary>
        /// Auth Token.
        /// </summary>
        public string AuthToken { get; set; }

        /// <summary>
        /// Out Twillio number.
        /// </summary>
        public string OutTwillionumber { get; set; }
    }
}
