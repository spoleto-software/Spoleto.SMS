using Spoleto.SMS.Exceptions;
using Spoleto.SMS.Providers;

namespace Spoleto.SMS
{
    /// <summary>
    /// The SMS service used to abstract the SMS sending.
    /// </summary
    public interface ISmsService
    {
        /// <summary>
        /// Sends the specified SMS message using the default <see cref="ISmsProvider"/>.
        /// </summary>
        /// <param name="message">The SMS message to be send.</param>
        /// <remarks>
        /// The default SMS delivery provider should be specified in <see cref="SmsServiceOptions.DefaultProvider"/> option supplied to the SmsService instance.
        /// </remarks>
        /// <exception cref="ArgumentNullException">The message instance is null.</exception>
        /// <exception cref="ArgumentException">The message doesn't contain a 'From' (sender) phone number, 
        /// and no default 'From' (sender) phone number is set in the <see cref="SmsServiceOptions.DefaultFrom"/> option supplied to the SmsService instance.
        /// </exception>
        SmsSendingResult Send(SmsMessage message);

        /// <summary>
        /// Sends the specified SMS message using the SMS delivery provider with the given name.
        /// </summary>
        /// <param name="message">The SMS message to be send</param>
        /// <param name="providerName">The name of the SMS delivery provider used for sending the SMS message.</param>
        /// <exception cref="ArgumentNullException">The given message instance is null, or the provider name is null.</exception>
        /// <exception cref="SmsProviderNotFoundException">Couldn't find any delivery provider with the given name.</exception>
        /// <exception cref="ArgumentException">The message doesn't contain a 'From' (sender) phone number, 
        /// and no default 'From' (sender) phone number is set in the <see cref="SmsServiceOptions.DefaultFrom"/> option supplied to the SmsService instance.
        /// </exception>
        SmsSendingResult Send(SmsMessage message, string providerName);

        /// <summary>
        /// Sends the specified SMS message using the SMS delivery provider with the given name.
        /// </summary>
        /// <param name="message">The SMS message to be send</param>
        /// <param name="providerName">The name of the SMS delivery provider used for sending the SMS message.</param>
        /// <exception cref="ArgumentNullException">The given message instance is null, or the provider name is null.</exception>
        /// <exception cref="SmsProviderNotFoundException">Couldn't find any delivery provider with the given name.</exception>
        /// <exception cref="ArgumentException">The message doesn't contain a 'From' (sender) phone number, 
        /// and no default 'From' (sender) phone number is set in the <see cref="SmsServiceOptions.DefaultFrom"/> option supplied to the SmsService instance.
        /// </exception>
        SmsSendingResult Send(SmsMessage message, SmsProviderName providerName);

        /// <summary>
        /// Sends the specified SMS message using the given SMS delivery provider.
        /// </summary>
        /// <param name="message">The SMS message to be send</param>
        /// <param name="provider">The SMS delivery provider used for sending the SMS message.</param>
        /// <exception cref="ArgumentNullException">The message instance is null, or the delivery provider is null.</exception>
        /// <exception cref="ArgumentException">The message doesn't contain a 'From' (sender) phone number, 
        /// and no default 'From' (sender) phone number is set in the <see cref="SmsServiceOptions.DefaultFrom"/> option supplied to the SmsService instance.
        /// </exception>
        SmsSendingResult Send(SmsMessage message, ISmsProvider provider);

        /// <summary>
        /// Async sends the specified SMS message using the default <see cref="ISmsProvider"/>.
        /// </summary>
        /// <param name="message">The SMS message to be send</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <remarks>
        /// The default SMS delivery provider should be specified in <see cref="SmsServiceOptions.DefaultProvider"/> option supplied to the SmsService instance.
        /// </remarks>
        /// <exception cref="ArgumentNullException">The message instance is null.</exception>
        /// <exception cref="ArgumentException">The message doesn't contain a 'From' (sender) phone number, 
        /// and no default 'From' (sender) phone number is set in the <see cref="SmsServiceOptions.DefaultFrom"/> option supplied to the SmsService instance.
        /// </exception>
        /// <exception cref="OperationCanceledException">If the System.Threading.CancellationToken is canceled.</exception>
        Task<SmsSendingResult> SendAsync(SmsMessage message, CancellationToken cancellationToken = default);

        /// <summary>
        /// Async sends the specified SMS message using the SMS delivery provider with the given name.
        /// </summary>
        /// <param name="message">The SMS message to be send</param>
        /// <param name="providerName">The name of the SMS delivery provider used for sending the SMS message.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <exception cref="ArgumentNullException">The message instance is null, or the delivery provider name is null</exception>
        /// <exception cref="SmsProviderNotFoundException">Couldn't find any provider with the given name.</exception>
        /// <exception cref="ArgumentException">The message doesn't contain a 'From' (sender) phone number, 
        /// and no default 'From' (sender) phone number is set in the <see cref="SmsServiceOptions.DefaultFrom"/> option supplied to the SmsService instance.
        /// </exception>
        /// <exception cref="OperationCanceledException">If the System.Threading.CancellationToken is canceled.</exception>
        Task<SmsSendingResult> SendAsync(SmsMessage message, string providerName, CancellationToken cancellationToken = default);

        /// <summary>
        /// Async sends the specified SMS message using the SMS delivery provider with the given name.
        /// </summary>
        /// <param name="message">The SMS message to be send</param>
        /// <param name="providerName">The name of the SMS delivery provider used for sending the SMS message.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <exception cref="ArgumentNullException">The message instance is null, or the delivery provider name is null</exception>
        /// <exception cref="SmsProviderNotFoundException">Couldn't find any provider with the given name.</exception>
        /// <exception cref="ArgumentException">The message doesn't contain a 'From' (sender) phone number, 
        /// and no default 'From' (sender) phone number is set in the <see cref="SmsServiceOptions.DefaultFrom"/> option supplied to the SmsService instance.
        /// </exception>
        /// <exception cref="OperationCanceledException">If the System.Threading.CancellationToken is canceled.</exception>
        Task<SmsSendingResult> SendAsync(SmsMessage message, SmsProviderName providerName, CancellationToken cancellationToken = default);

        /// <summary>
        /// Async sends the specified SMS message using the given SMS delivery provider.
        /// </summary>
        /// <param name="message">The SMS message to be send</param>
        /// <param name="provider">The SMS delivery provider used for sending the SMS message.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <exception cref="ArgumentNullException">The message instance is null, or the delivery provider is null.</exception>
        /// <exception cref="ArgumentException">The message doesn't contain a 'From' (sender) phone number, 
        /// and no default 'From' (sender) phone number is set in the <see cref="SmsServiceOptions.DefaultFrom"/> option supplied to the SmsService instance.
        /// </exception>
        /// <exception cref="OperationCanceledException">If the System.Threading.CancellationToken is canceled.</exception>
        Task<SmsSendingResult> SendAsync(SmsMessage message, ISmsProvider provider, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the status of the specified SMS message with the given Id.
        /// </summary>
        /// <param name="id">The SMS message Id.</param>
        /// <param name="phoneNumber">The recipient's phone number.</param>
        /// <remarks>
        /// The default SMS delivery provider should be specified in <see cref="SmsServiceOptions.DefaultProvider"/> option supplied to the SmsService instance.
        /// </remarks>
        /// <exception cref="ArgumentNullException">The message instance is null.</exception>
        /// <exception cref="ArgumentException">The message doesn't contain a 'From' (sender) phone number, 
        /// and no default 'From' (sender) phone number is set in the <see cref="SmsServiceOptions.DefaultFrom"/> option supplied to the SmsService instance.
        /// </exception>
        SmsStatusResult GetStatus(string id, string? phoneNumber);

        /// <summary>
        /// Gets the status of the specified SMS message with the given Id.
        /// </summary>
        /// <param name="id">The SMS message Id.</param>
        /// <param name="phoneNumber">The recipient's phone number.</param>
        /// <param name="providerName">The name of the SMS delivery provider used for sending the SMS message.</param>
        /// <exception cref="ArgumentNullException">The given message instance is null, or the provider name is null.</exception>
        /// <exception cref="SmsProviderNotFoundException">Couldn't find any delivery provider with the given name.</exception>
        /// <exception cref="ArgumentException">The message doesn't contain a 'From' (sender) phone number, 
        /// and no default 'From' (sender) phone number is set in the <see cref="SmsServiceOptions.DefaultFrom"/> option supplied to the SmsService instance.
        /// </exception>
        SmsStatusResult GetStatus(string id, string? phoneNumber, SmsProviderName providerName);

        /// <summary>
        /// Gets the status of the specified SMS message with the given Id.
        /// </summary>
        /// <param name="id">The SMS message Id.</param>
        /// <param name="phoneNumber">The recipient's phone number.</param>
        /// <param name="provider">The SMS delivery provider used for sending the SMS message.</param>
        /// <exception cref="ArgumentNullException">The message instance is null, or the delivery provider is null.</exception>
        /// <exception cref="ArgumentException">The message doesn't contain a 'From' (sender) phone number, 
        /// and no default 'From' (sender) phone number is set in the <see cref="SmsServiceOptions.DefaultFrom"/> option supplied to the SmsService instance.
        /// </exception>
        SmsStatusResult GetStatus(string id, string? phoneNumber, ISmsProvider provider);

        /// <summary>
        /// Async gets the status of the specified SMS message with the given Id.
        /// </summary>
        /// <param name="id">The SMS message Id.</param>
        /// <param name="phoneNumber">The recipient's phone number.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <remarks>
        /// The default SMS delivery provider should be specified in <see cref="SmsServiceOptions.DefaultProvider"/> option supplied to the SmsService instance.
        /// </remarks>
        /// <exception cref="ArgumentNullException">The message instance is null.</exception>
        /// <exception cref="ArgumentException">The message doesn't contain a 'From' (sender) phone number, 
        /// and no default 'From' (sender) phone number is set in the <see cref="SmsServiceOptions.DefaultFrom"/> option supplied to the SmsService instance.
        /// </exception>
        /// <exception cref="OperationCanceledException">If the System.Threading.CancellationToken is canceled.</exception>
        Task<SmsStatusResult> GetStatusAsync(string id, string? phoneNumber, CancellationToken cancellationToken = default);

        /// <summary>
        /// Async gets the status of the specified SMS message with the given Id.
        /// </summary>
        /// <param name="id">The SMS message Id.</param>
        /// <param name="phoneNumber">The recipient's phone number.</param>
        /// <param name="providerName">The name of the SMS delivery provider used for sending the SMS message.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <exception cref="ArgumentNullException">The message instance is null, or the delivery provider name is null</exception>
        /// <exception cref="SmsProviderNotFoundException">Couldn't find any provider with the given name.</exception>
        /// <exception cref="ArgumentException">The message doesn't contain a 'From' (sender) phone number, 
        /// and no default 'From' (sender) phone number is set in the <see cref="SmsServiceOptions.DefaultFrom"/> option supplied to the SmsService instance.
        /// </exception>
        /// <exception cref="OperationCanceledException">If the System.Threading.CancellationToken is canceled.</exception>
        Task<SmsStatusResult> GetStatusAsync(string id, string? phoneNumber, SmsProviderName providerName, CancellationToken cancellationToken = default);

        /// <summary>
        /// Async gets the status of the specified SMS message with the given Id.
        /// </summary>
        /// <param name="id">The SMS message Id.</param>
        /// <param name="phoneNumber">The recipient's phone number.</param>
        /// <param name="provider">The SMS delivery provider used for sending the SMS message.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <exception cref="ArgumentNullException">The message instance is null, or the delivery provider is null.</exception>
        /// <exception cref="ArgumentException">The message doesn't contain a 'From' (sender) phone number, 
        /// and no default 'From' (sender) phone number is set in the <see cref="SmsServiceOptions.DefaultFrom"/> option supplied to the SmsService instance.
        /// </exception>
        /// <exception cref="OperationCanceledException">If the System.Threading.CancellationToken is canceled.</exception>
        Task<SmsStatusResult> GetStatusAsync(string id, string? phoneNumber, ISmsProvider provider, CancellationToken cancellationToken = default);
    }
}
