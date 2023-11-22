namespace Spoleto.SMS
{
    /// <summary>
    /// SMS message.
    /// </summary>
    public record SmsMessage
    {
        /// <summary>
        /// The phone number separator.
        /// </summary>
        public const char PhoneNumberSeparator = ';';

        /// <summary>
        /// Creates the SMS message.
        /// </summary>
        public SmsMessage(string body, string from, string to, bool isAllowSendToForeignNumbers = false)
        {
            Body = body ?? throw new ArgumentNullException(nameof(body));
            To = to ?? throw new ArgumentNullException(nameof(to));
            IsAllowSendToForeignNumbers = isAllowSendToForeignNumbers;
            From = from;
        }

        /// <summary>
        /// Creates the SMS message with list of recipients.
        /// </summary>
        public SmsMessage(string body, string from, List<string> listOfTo, bool isAllowSendToForeignNumbers = false)
            : this(body, from,
#if NET5_0_OR_GREATER
                  string.Join(PhoneNumberSeparator, listOfTo),
#else
                  string.Join(PhoneNumberSeparator.ToString(), listOfTo),
#endif
                  isAllowSendToForeignNumbers)
        {
        }

        /// <summary>
        /// Gets the message body.
        /// </summary>
        public string Body { get; }

        /// <summary>
        /// Gets the phone number or another ID of the sender.
        /// </summary>
        public string? From { get; private set; }

        /// <summary>
        /// Gets the phone numbers of recipients to send the SMS message to.
        /// </summary>
        public string To { get; }

        /// <summary>
        /// Gets whether sending this message to foreign numbers is allowed.
        /// </summary>
        public bool IsAllowSendToForeignNumbers { get; }

        /// <summary>
        /// Set the sender phone number or another ID.
        /// </summary>
        /// <param name="from">The sender phone number or another ID.</param>
        public void SetFrom(string from)
            => From = from;
    }
}
