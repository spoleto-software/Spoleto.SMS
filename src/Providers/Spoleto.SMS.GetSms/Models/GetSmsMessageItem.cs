namespace Spoleto.SMS.Providers.GetSms
{
    /// <summary>
    /// A phone number with its own text for <see cref="GetSmsMessage"/>.
    /// </summary>
    public record GetSmsMessageItem
    {
        /// <summary>
        /// Creates the item.
        /// </summary>
        /// <param name="phone">The recipient phone number.</param>
        /// <param name="text">The text for this recipient.</param>
        public GetSmsMessageItem(string phone, string text)
        {
            Phone = phone;
            Text = text;
        }

        /// <summary>
        /// The recipient phone number.
        /// </summary>
        public string Phone { get; }

        /// <summary>
        /// The text for this recipient.
        /// </summary>
        public string Text { get; }
    }
}
