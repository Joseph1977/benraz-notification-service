using System.Collections.Generic;
using System.Threading.Tasks;

namespace Notifications.Domain.NotificationServices.Phones
{
    /// <summary>
    /// Phone service.
    /// </summary>
    public interface IPhoneService
    {
        /// <summary>
        /// Send sms.
        /// </summary>
        /// <param name="body">Body.</param>
        /// <param name="recipients">Recipients.</param>
        /// <param name="sender">Sender.</param>
        /// <returns>Task.</returns>
        Task SendSmsAsync(string body, List<string> recipients, string sender);
    }
}
