namespace Spoleto.SMS.Providers.GetSms
{
    /// <summary>
    /// GetSms provider.
    /// </summary>
    /// <remarks>
    /// <see href="https://getsms.uz/page/index/16"/>.
    /// </remarks>
    public interface IGetSmsProvider : ISmsProvider
    {
        /// <summary>
        /// Sends the SMS message.
        /// </summary>
        /// <param name="message">The SMS message to be send</param>
        /// <returns><see cref="SmsSendingResult"/> to indicate sending result.</returns>
        SmsSendingResult Send(GetSmsMessage message);

        /// <summary>
        /// Async sends the SMS message.
        /// </summary>
        /// <param name="message">The SMS message to be send</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns><see cref="SmsSendingResult"/> to indicate sending result.</returns>
        /// <exception cref="OperationCanceledException">Thrown when <paramref name="cancellationToken"/> is canceled.</exception>
        Task<SmsSendingResult> SendAsync(GetSmsMessage message, CancellationToken cancellationToken = default);
    }
}
