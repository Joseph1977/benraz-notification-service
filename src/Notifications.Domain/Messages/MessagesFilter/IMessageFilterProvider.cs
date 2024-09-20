using System.Collections.Generic;

namespace Notifications.Domain.Messages.MessagesFilter
{
    /// <summary>
    /// Message filter provider.
    /// </summary>
    public interface IMessageFilterProvider
    {
        /// <summary>
        /// Is message allowed.
        /// </summary>
        /// <param name="To">To.</param>
        /// <param name="msgType">Message type.</param>
        /// <returns>Is message allowed.</returns>
        bool IsMessageAllowed(string To, MessageType msgType);

        /// <summary>
        /// Final to address to send.
        /// </summary>
        /// <param name="To">To.</param>
        /// <param name="msgType">Message type.</param>
        /// <returns>Final to address to send.</returns>
        string FinalToAddressToSend(string To, MessageType msgType);

        /// <summary>
        /// Recive a list of emails, filter what should be filterd, change what should be changed.
        /// </summary>
        /// <param name="Tos">The list of email we want to send.</param>
        /// <param name="msgType">The type of the message Email or SMS.</param>
        /// <returns> Null if email not allowed, or email send disabled 
        /// otherwise, the list of email(s) to be send, same or after modified incase traverse mode.
        /// </returns>
        List<string> FinalTosAddressToSend(List<string> Tos, MessageType msgType);
    }
}
