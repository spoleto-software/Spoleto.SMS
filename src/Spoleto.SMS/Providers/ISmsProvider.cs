namespace Spoleto.SMS.Providers
{
    /// <summary>
    /// The SMS provider for sending SMS messages.
    /// </summary>
    public interface ISmsProvider
    {
        /// <summary>
        /// The SMS provider unique name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Sends the SMS message.
        /// </summary>
        /// <param name="message">the SMS message to be send</param>
        /// <returns>a <see cref="SmsSendingResult"/> to indicate sending result.</returns>
        SmsSendingResult Send(SmsMessage message);

        /// <summary>
        /// Async sends the SMS message.
        /// </summary>
        /// <param name="message">the SMS message to be send</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>a <see cref="SmsSendingResult"/> to indicate sending result.</returns>
        /// <exception cref="OperationCanceledException">If the System.Threading.CancellationToken is canceled.</exception>
        Task<SmsSendingResult> SendAsync(SmsMessage message, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the status of the specified SMS message with the given Id.
        /// </summary>
        /// <param name="id">The SMS message Id.</param>
        /// <param name="phoneNumber">The recipient's phone number.</param>
        /// <returns>a <see cref="SmsStatusResult"/> to indicate status result.</returns>
        SmsStatusResult GetStatus(string id, string? phoneNumber);

        /// <summary>
        /// Async gets the status of the specified SMS message with the given Id.
        /// </summary>
        /// <param name="id">The SMS message Id.</param>
        /// <param name="phoneNumber">The recipient's phone number.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>a <see cref="SmsStatusResult"/> to indicate status result.</returns>
        /// <exception cref="OperationCanceledException">If the System.Threading.CancellationToken is canceled.</exception>
        Task<SmsStatusResult> GetStatusAsync(string id, string? phoneNumber, CancellationToken cancellationToken = default);
    }
}
