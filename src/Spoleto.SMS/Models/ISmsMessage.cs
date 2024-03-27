namespace Spoleto.SMS
{
    /// <summary>
    /// The SMS message.
    /// </summary>
    public interface ISmsMessage
    {
        /// <summary>
        /// Gets the message body.
        /// </summary>
        string Body { get; }

        /// <summary>
        /// Gets the phone number or another ID of the sender.
        /// </summary>
        string? From { get; }

        /// <summary>
        /// Gets the phone numbers of recipients to send the SMS message to.
        /// </summary>
        string To { get; }

        /// <summary>
        /// Gets whether sending this message to foreign numbers is allowed.
        /// </summary>
        bool IsAllowSendToForeignNumbers { get; }

        /// <summary>
        /// Gets the additional provider data.
        /// </summary>
        List<SmsProviderData> ProviderData { get; }

        /// <summary>
        /// The phone number separator.
        /// </summary>
        char PhoneNumberSeparator { get; }

        /// <summary>
        /// Sets the sender phone number or another ID.
        /// </summary>
        /// <param name="from">The sender phone number or another ID.</param>
        void SetFrom(string from);
    }
}