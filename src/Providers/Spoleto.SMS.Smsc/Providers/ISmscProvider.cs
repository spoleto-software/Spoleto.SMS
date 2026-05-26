namespace Spoleto.SMS.Providers.Smsc
{
    /// <summary>
    /// The SMSC provider for sending SMS messages.
    /// </summary>
    /// <remarks>
    /// <see href="https://smsc.ru/api/code/libraries/http_smtp/cs/#menu"/>.
    /// </remarks>
    public interface ISmscProvider : ISmsProvider
    {
        /// <summary>
        /// Sends one message text to one or more recipients (<c>phones</c> + <c>mes</c> mode).
        /// Required API parameters: <c>login/apikey</c>, <c>psw</c>, <c>phones</c>, <c>mes</c>.
        /// </summary>
        /// <param name="phones">
        /// Semicolon-separated list of phone numbers.
        /// Group references use the form <c>G&lt;id&gt;</c> or <c>g&lt;id&gt;</c>.
        /// For Telegram bot messages use the phone number, @nick, or #ID format.
        /// </param>
        /// <param name="message">Message text (max 1000 characters for SMS).</param>
        /// <param name="sender">
        /// Sender ID shown to the recipient — up to 11 Latin characters or 15 digits.
        /// Must be pre-registered at smsc.ru/senders.
        /// </param>
        /// <param name="data">Optional extra parameters from <see cref="SmscMessageData"/>.</param>
        /// <param name="files">
        /// Local file paths to attach (MMS, e-mail, or voice). Forces HTTP POST.
        /// </param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>
        /// <see cref="SmscResult{SmscSendSuccessResult}"/> with <see cref="SmscResult{SmscSendSuccessResult}.Success"/> populated on success,
        /// or <see cref="SmscResult{SmscSendSuccessResult}.Error"/> populated on failure.
        /// Always check <see cref="SmscResult{SmscSendSuccessResult}.IsSuccess"/> before use.
        /// </returns>
        Task<SmscResult<SmscSendSuccessResult>> SendAsync(
            SmscMessage message,
            IEnumerable<string>? files = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Estimates the cost of sending without actually delivering the message (<c>cost=1</c>).
        /// </summary>
        /// <returns>
        /// <see cref="SmscResult{SmscCostSuccessResult}"/> with <see cref="SmscResult{SmscCostSuccessResult}.Success"/> populated on success,
        /// or <see cref="SmscResult{SmscCostSuccessResult}.Error"/> populated on failure.
        /// Always check <see cref="SmscResult{SmscCostSuccessResult}.IsSuccess"/> before use.
        /// </returns>
        Task<SmscResult<SmscCostSuccessResult>> GetSmsCostAsync(
            string phones,
            string message,
            string? sender = null,
            SmscMessageData? data = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves the delivery status of one or more messages via <c>status.php</c>.
        /// </summary>
        /// <param name="id">
        /// Message ID, or a comma-separated list of IDs for a batch query.
        /// </param>
        /// <param name="phone">
        /// Recipient phone number, or a comma-separated list matching the IDs.
        /// For e-mail messages supply the e-mail address.
        /// </param>
        /// <param name="all">
        /// 0 — basic status fields only (default).<br/>
        /// 1 — include send time, phone, cost, sender ID, status name, message text, comment, type.<br/>
        /// 2 — same as 1, plus country, operator, and region.
        /// </param>
        /// <param name="forTelegramBot">
        /// <see langword="true"/> when querying messages sent via a Telegram bot — adds <c>bot=1</c>.
        /// Requires matching <paramref name="phone"/> and <paramref name="id"/> values.
        /// </param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>
        /// <see cref="SmscStatusResult"/>.
        /// Always check <see cref="SmscStatusResult.IsSuccess"/> or <see cref="SmscStatusResult.IsBatchSuccess"/> before use.
        /// </returns>
        Task<SmscStatusResult> GetStatusAsync(
            string id,
            string phone,
            int all = 0,
            bool forTelegramBot = false,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns the current account balance via <c>balance.php</c>, or <see langword="null"/> on error.
        /// </summary>
        decimal? GetBalance(CancellationToken cancellationToken = default);

        /// <summary>
        /// Async returns the current account balance via <c>balance.php</c>, or <see langword="null"/> on error.
        /// </summary>
        Task<decimal?> GetBalanceAsync(CancellationToken cancellationToken = default);

        void CheckPhoneNumber(string phoneNumber, string sender, bool isAllowSendToForeignNumbers = false);

        Task CheckPhoneNumberAsync(string phoneNumber, string sender, bool isAllowSendToForeignNumbers = false);
    }
}
