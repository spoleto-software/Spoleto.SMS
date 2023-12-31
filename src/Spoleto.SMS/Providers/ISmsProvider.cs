﻿namespace Spoleto.SMS.Providers
{
    /// <summary>
    /// The SMS provider for sending SMS messages.
    /// </summary>
    public interface ISmsProvider
    {
        /// <summary>
        /// Gets the SMS provider unique name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the flag whether null in From is allowed.
        /// </summary>
        bool IsAllowNullFrom { get; }

        /// <summary>
        /// Determines whether the SMS provider can send a message to the specified phone number.
        /// </summary>
        /// <param name="phoneNumber">The phome number is checked as to whether it can be used with the SMS provider.</param>
        /// <param name="isAllowSendToForeignNumbers">The flag indicating whether the message can be sent to international numbers.</param>
        /// <returns>True if the phone number can be used with the SMS provider, false otherwise.</returns>
        bool CanSend(string phoneNumber, bool isAllowSendToForeignNumbers = false);

        /// <summary>
        /// Gets the status of the specified SMS message with the specified Id.
        /// </summary>
        /// <param name="id">The SMS message Id.</param>
        /// <param name="phoneNumber">The recipient's phone number.</param>
        /// <returns><see cref="SmsStatusResult"/> to indicate status result.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="id"/> is null.</exception>
        SmsStatusResult GetStatus(string id, string? phoneNumber = null);

        /// <summary>
        /// Async gets the status of the specified SMS message with the specified Id.
        /// </summary>
        /// <param name="id">The SMS message Id.</param>
        /// <param name="phoneNumber">The recipient's phone number.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns><see cref="SmsStatusResult"/> to indicate status result.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="id"/> is null.</exception>
        /// <exception cref="OperationCanceledException">Thrown when <paramref name="cancellationToken"/> is canceled.</exception>
        Task<SmsStatusResult> GetStatusAsync(string id, string? phoneNumber = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Sends the SMS message.
        /// </summary>
        /// <param name="message">The SMS message to be send</param>
        /// <returns><see cref="SmsSendingResult"/> to indicate sending result.</returns>
        SmsSendingResult Send(SmsMessage message);

        /// <summary>
        /// Async sends the SMS message.
        /// </summary>
        /// <param name="message">The SMS message to be send</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns><see cref="SmsSendingResult"/> to indicate sending result.</returns>
        /// <exception cref="OperationCanceledException">Thrown when <paramref name="cancellationToken"/> is canceled.</exception>
        Task<SmsSendingResult> SendAsync(SmsMessage message, CancellationToken cancellationToken = default);
    }
}