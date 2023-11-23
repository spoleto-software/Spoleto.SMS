using Spoleto.SMS.Exceptions;
using Spoleto.SMS.Providers;

namespace Spoleto.SMS
{
    /// <summary>
    /// The SMS service serves as an abstraction layer for sending SMS messages.
    /// </summary>
    public interface ISmsService
    {
        /// <summary>
        /// Gets the list of sms providers attached to this sms service.
        /// </summary>
        IEnumerable<ISmsProvider> Providers { get; }

        /// <summary>
        /// Gets the default sms provider attached to this sms service.
        /// </summary>
        ISmsProvider DefaultProvider { get; }

        /// <summary>
        /// Gets the status of the specified SMS message with the specified Id.
        /// </summary>
        /// <param name="id">The SMS message Id.</param>
        /// <param name="phoneNumber">The recipient's phone number.</param>
        /// <remarks>
        /// The designated default SMS provider must be defined within the <see cref="SmsServiceOptions.DefaultProvider"/> setting, which is provided to the <see cref="SmsService"/> when it is instantiated.
        /// </remarks>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="id"/> is null.</exception>
        SmsStatusResult GetStatus(string id, string? phoneNumber);

        /// <summary>
        /// Gets the status of the specified SMS message with the specified Id.
        /// </summary>
        /// <param name="providerName">The name of the SMS provider used for delivering the SMS message.</param>
        /// <param name="id">The SMS message Id.</param>
        /// <param name="phoneNumber">The recipient's phone number.</param>
        /// <exception cref="ArgumentNullException">Thrown when either <paramref name="message"/> is null or <paramref name="providerName"/> is null.</exception>
        /// <exception cref="SmsProviderNotFoundException">Thrown when the identified SMS service provider cannot be found.</exception>
        SmsStatusResult GetStatus(string providerName, string id, string? phoneNumber);

        /// <summary>
        /// Gets the status of the specified SMS message with the specified Id.
        /// </summary>
        /// <param name="providerName">The name of the SMS provider used for delivering the SMS message.</param>
        /// <param name="id">The SMS message Id.</param>
        /// <param name="phoneNumber">The recipient's phone number.</param>
        /// <exception cref="ArgumentNullException">Thrown when either <paramref name="id"/> is null or <paramref name="providerName"/> is null.</exception>
        /// <exception cref="SmsProviderNotFoundException">Thrown when the identified SMS service provider cannot be found.</exception>
        SmsStatusResult GetStatus(SmsProviderName providerName, string id, string? phoneNumber);

        /// <summary>
        /// Gets the status of the specified SMS message with the specified Id.
        /// </summary>
        /// <param name="id">The SMS message Id.</param>
        /// <param name="phoneNumber">The recipient's phone number.</param>
        /// <param name="provider">The SMS provider used for delivering the SMS message.</param>
        /// <exception cref="ArgumentNullException">Thrown when either <paramref name="id"/> is null or <paramref name="provider"/> is null.</exception>
        SmsStatusResult GetStatus(ISmsProvider provider, string id, string? phoneNumber);

        /// <summary>
        /// Async gets the status of the specified SMS message with the specified Id.
        /// </summary>
        /// <param name="id">The SMS message Id.</param>
        /// <param name="phoneNumber">The recipient's phone number.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <remarks>
        /// The designated default SMS provider must be defined within the <see cref="SmsServiceOptions.DefaultProvider"/> setting, which is provided to the <see cref="SmsService"/> when it is instantiated.
        /// </remarks>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="id"/> is null.</exception>
        /// <exception cref="OperationCanceledException">Thrown when <paramref name="cancellationToken"/> is canceled.</exception>
        Task<SmsStatusResult> GetStatusAsync(string id, string? phoneNumber, CancellationToken cancellationToken = default);

        /// <summary>
        /// Async gets the status of the specified SMS message with the specified Id.
        /// </summary>
        /// <param name="providerName">The name of the SMS provider used for delivering the SMS message.</param>
        /// <param name="id">The SMS message Id.</param>
        /// <param name="phoneNumber">The recipient's phone number.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <exception cref="ArgumentNullException">Thrown when either <paramref name="id"/> is null or <paramref name="providerName"/> is null.</exception>
        /// <exception cref="SmsProviderNotFoundException">Thrown when the identified SMS service provider cannot be found.</exception>
        /// <exception cref="OperationCanceledException">Thrown when <paramref name="cancellationToken"/> is canceled.</exception>
        Task<SmsStatusResult> GetStatusAsync(string providerName, string id, string? phoneNumber, CancellationToken cancellationToken = default);

        /// <summary>
        /// Async gets the status of the specified SMS message with the specified Id.
        /// </summary>
        /// <param name="providerName">The name of the SMS provider used for delivering the SMS message.</param>
        /// <param name="id">The SMS message Id.</param>
        /// <param name="phoneNumber">The recipient's phone number.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <exception cref="ArgumentNullException">Thrown when either <paramref name="id"/> is null or <paramref name="providerName"/> is null.</exception>
        /// <exception cref="SmsProviderNotFoundException">Thrown when the identified SMS service provider cannot be found.</exception>
        /// <exception cref="OperationCanceledException">Thrown when <paramref name="cancellationToken"/> is canceled.</exception>
        Task<SmsStatusResult> GetStatusAsync(SmsProviderName providerName, string id, string? phoneNumber, CancellationToken cancellationToken = default);

        /// <summary>
        /// Async gets the status of the specified SMS message with the specified Id.
        /// </summary>
        /// <param name="provider">The SMS provider used for delivering the SMS message.</param>
        /// <param name="id">The SMS message Id.</param>
        /// <param name="phoneNumber">The recipient's phone number.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <exception cref="ArgumentNullException">Thrown when either <paramref name="message"/> is null or <paramref name="provider"/> is null.</exception>
        /// <exception cref="OperationCanceledException">Thrown when <paramref name="cancellationToken"/> is canceled.</exception>
        Task<SmsStatusResult> GetStatusAsync(ISmsProvider provider, string id, string? phoneNumber, CancellationToken cancellationToken = default);

        /// <summary>
        /// Sends the specified SMS message using the default <see cref="ISmsProvider"/>.
        /// </summary>
        /// <param name="message">The SMS message intended to be delivered.</param>
        /// <remarks>
        /// The designated default SMS provider must be defined within the <see cref="SmsServiceOptions.DefaultProvider"/> setting, which is provided to the <see cref="SmsService"/> when it is instantiated.
        /// </remarks>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="message"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="message"/> lacks a 'From' (sender) phone number and the <see cref="SmsServiceOptions.DefaultFrom"/> property has not been set within the <see cref="SmsService"/> configuration.</exception>
        SmsSendingResult Send(SmsMessage message);

        /// <summary>
        /// Sends the specified SMS message using the SMS provider with the specified name.
        /// </summary>
        /// <param name="providerName">The name of the SMS provider used for delivering the SMS message.</param>
        /// <param name="message">The SMS message intended to be delivered.</param>
        /// <exception cref="ArgumentNullException">Thrown when either <paramref name="message"/> is null or <paramref name="providerName"/> is null.</exception>
        /// <exception cref="SmsProviderNotFoundException">Thrown when the identified SMS service provider cannot be found.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="message"/> lacks a 'From' (sender) phone number and the <see cref="SmsServiceOptions.DefaultFrom"/> property has not been set within the <see cref="SmsService"/> configuration.</exception>
        SmsSendingResult Send(string providerName, SmsMessage message);

        /// <summary>
        /// Sends the specified SMS message using the SMS provider with the specified name.
        /// </summary>
        /// <param name="providerName">The name of the SMS provider used for delivering the SMS message.</param>
        /// <param name="message">The SMS message intended to be delivered.</param>
        /// <exception cref="ArgumentNullException">Thrown when either <paramref name="message"/> is null or <paramref name="providerName"/> is null.</exception>
        /// <exception cref="SmsProviderNotFoundException">Thrown when the identified SMS service provider cannot be found.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="message"/> lacks a 'From' (sender) phone number and the <see cref="SmsServiceOptions.DefaultFrom"/> property has not been set within the <see cref="SmsService"/> configuration.</exception>
        SmsSendingResult Send(SmsProviderName providerName, SmsMessage message);

        /// <summary>
        /// Sends the specified SMS message using the specified SMS provider.
        /// </summary>
        /// <param name="provider">The SMS provider used for delivering the SMS message.</param>
        /// <param name="message">The SMS message intended to be delivered.</param>
        /// <exception cref="ArgumentNullException">Thrown when either <paramref name="message"/> is null or <paramref name="provider"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="message"/> lacks a 'From' (sender) phone number and the <see cref="SmsServiceOptions.DefaultFrom"/> property has not been set within the <see cref="SmsService"/> configuration.</exception>
        SmsSendingResult Send(ISmsProvider provider, SmsMessage message);

        /// <summary>
        /// Async sends the specified SMS message using the default <see cref="ISmsProvider"/>.
        /// </summary>
        /// <param name="message">The SMS message intended to be delivered.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <remarks>
        /// The designated default SMS provider must be defined within the <see cref="SmsServiceOptions.DefaultProvider"/> setting, which is provided to the <see cref="SmsService"/> when it is instantiated.
        /// </remarks>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="message"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="message"/> lacks a 'From' (sender) phone number and the <see cref="SmsServiceOptions.DefaultFrom"/> property has not been set within the <see cref="SmsService"/> configuration.</exception>
        /// <exception cref="OperationCanceledException">Thrown when <paramref name="cancellationToken"/> is canceled.</exception>
        Task<SmsSendingResult> SendAsync(SmsMessage message, CancellationToken cancellationToken = default);

        /// <summary>
        /// Async sends the specified SMS message using the SMS provider with the specified name.
        /// </summary>
        /// <param name="providerName">The name of the SMS provider used for delivering the SMS message.</param>
        /// <param name="message">The SMS message intended to be delivered.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <exception cref="ArgumentNullException">The SMS message instance is null, or the the SMS provider name is null</exception>
        /// <exception cref="SmsProviderNotFoundException">Couldn't find any provider with the specified name.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="message"/> lacks a 'From' (sender) phone number and the <see cref="SmsServiceOptions.DefaultFrom"/> property has not been set within the <see cref="SmsService"/> configuration.</exception>
        /// <exception cref="OperationCanceledException">Thrown when <paramref name="cancellationToken"/> is canceled.</exception>
        Task<SmsSendingResult> SendAsync(string providerName, SmsMessage message, CancellationToken cancellationToken = default);

        /// <summary>
        /// Async sends the specified SMS message using the SMS provider with the specified name.
        /// </summary>
        /// <param name="providerName">The name of the SMS provider used for delivering the SMS message.</param>
        /// <param name="message">The SMS message intended to be delivered.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <exception cref="ArgumentNullException">The SMS message instance is null, or the the SMS provider name is null</exception>
        /// <exception cref="SmsProviderNotFoundException">Couldn't find any provider with the specified name.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="message"/> lacks a 'From' (sender) phone number and the <see cref="SmsServiceOptions.DefaultFrom"/> property has not been set within the <see cref="SmsService"/> configuration.</exception>
        /// <exception cref="OperationCanceledException">Thrown when <paramref name="cancellationToken"/> is canceled.</exception>
        Task<SmsSendingResult> SendAsync(SmsProviderName providerName, SmsMessage message, CancellationToken cancellationToken = default);

        /// <summary>
        /// Async sends the specified SMS message using the specified SMS provider.
        /// </summary>
        /// <param name="provider">The SMS provider used for delivering the SMS message.</param>
        /// <param name="message">The SMS message intended to be delivered.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <exception cref="ArgumentNullException">Thrown when either <paramref name="message"/> is null or <paramref name="provider"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="message"/> lacks a 'From' (sender) phone number and the <see cref="SmsServiceOptions.DefaultFrom"/> property has not been set within the <see cref="SmsService"/> configuration.</exception>
        /// <exception cref="OperationCanceledException">Thrown when <paramref name="cancellationToken"/> is canceled.</exception>
        Task<SmsSendingResult> SendAsync(ISmsProvider provider, SmsMessage message, CancellationToken cancellationToken = default);
    }
}
