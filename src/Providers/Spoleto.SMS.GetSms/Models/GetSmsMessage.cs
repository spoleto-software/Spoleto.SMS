namespace Spoleto.SMS.Providers.GetSms
{
    /// <summary>
    /// The GetSMS message with an individual text per phone number: the API takes a text per number,
    /// so all the items are sent in one request.
    /// </summary>
    public record GetSmsMessage : SmsMessage
    {
        /// <summary>
        /// Creates the message with an individual text per phone number.
        /// </summary>
        /// <param name="from">The sender nickname.</param>
        /// <param name="items">The phone numbers with their texts.</param>
        /// <param name="isAllowSendToForeignNumbers">Whether sending to foreign numbers is allowed.</param>
        public GetSmsMessage(string? from, List<GetSmsMessageItem> items, bool isAllowSendToForeignNumbers = false)
            : base(string.Empty, from, (items ?? throw new ArgumentNullException(nameof(items))).Select(x => x.Phone).ToList(), isAllowSendToForeignNumbers)
        {
            Items = items;
        }

        /// <summary>
        /// The phone numbers with their texts.
        /// </summary>
        public List<GetSmsMessageItem> Items { get; }
    }
}
