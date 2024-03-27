namespace Spoleto.SMS
{
    /// <summary>
    /// The SMS message.
    /// </summary>
    public record SmsMessage : ISmsMessage
    {
        /// <summary>
        /// Creates the SMS message.
        /// </summary>
        public SmsMessage(string body, string? from, string to, bool isAllowSendToForeignNumbers = false, List<SmsProviderData>? providerData = null)
        {
            Body = body ?? throw new ArgumentNullException(nameof(body));
            To = to ?? throw new ArgumentNullException(nameof(to));
            IsAllowSendToForeignNumbers = isAllowSendToForeignNumbers;
            From = from;
            ProviderData = providerData ?? [];
        }

        /// <summary>
        /// Creates the SMS message with list of recipients.
        /// </summary>
        public SmsMessage(string body, string? from, List<string> listOfTo, bool isAllowSendToForeignNumbers = false, List<SmsProviderData>? providerData = null)
        {
            if (listOfTo == null)
                throw new ArgumentNullException(nameof(listOfTo));

            Body = body ?? throw new ArgumentNullException(nameof(body));
            To =
#if NET5_0_OR_GREATER
                  string.Join(PhoneNumberSeparator, listOfTo);
#else
                  string.Join(PhoneNumberSeparator.ToString(), listOfTo);
#endif
            IsAllowSendToForeignNumbers = isAllowSendToForeignNumbers;
            From = from;
            ProviderData = providerData ?? [];
        }

        /// <summary>
        /// The phone number separator.
        /// </summary>
        public virtual char PhoneNumberSeparator => ';';

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
        /// Gets the additional provider data.
        /// </summary>
        public List<SmsProviderData> ProviderData { get; }

        /// <summary>
        /// Add the additional provider data.
        /// </summary>
        public SmsMessage WithProviderData(string name, object value)
        {
            ProviderData.Add(new(name, value));

            return this;
        }

        /// <summary>
        /// Sets the sender phone number or another ID.
        /// </summary>
        /// <param name="from">The sender phone number or another ID.</param>
        public void SetFrom(string from) => From = from;
    }
}
