using System.Collections.Generic;

namespace Notifications.WebApi.Models.Phones
{
    /// <summary>
    /// Sms view model.
    /// </summary>
    public class SmsViewModel
    {
        /// <summary>
        /// Body.
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Sender.
        /// </summary>
        public string Sender { get; set; }

        /// <summary>
        /// Recipients.
        /// </summary>
        public List<string> Recipients { get; set; }
    }
}
