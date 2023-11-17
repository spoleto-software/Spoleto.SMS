using Spoleto.SMS.Exceptions;
using Spoleto.SMS.Providers;

namespace Spoleto.SMS
{
    /// <summary>
    /// The SMS service used to abstract the SMS sending.
    /// </summary>
    public class SmsService : ISmsService
    {
        private readonly ISmsProvider _defaultProvider;
        private readonly IDictionary<string, ISmsProvider> _providers;

        /// <summary>
        /// The constructor with parameters.
        /// </summary>
        /// <param name="providers">the list of supported SMS providers.</param>
        /// <param name="options">the sms service options.</param>
        /// <exception cref="ArgumentNullException">if SMS provider or options are null.</exception>
        /// <exception cref="ArgumentException">if SMS provider list is empty.</exception>
        /// <exception cref="SmsProviderNotFoundException">if the default SMS provider cannot be found.</exception>
        public SmsService(IEnumerable<ISmsProvider> providers, SmsServiceOptions options)
        {
            if (providers is null)
                throw new ArgumentNullException(nameof(providers));

            if (!providers.Any())
                throw new ArgumentException("You must specify at least one sms provider, the list is empty.", nameof(providers));

            if (options is null)
                throw new ArgumentNullException(nameof(options));

            // Validates if the options are valid:
            options.Validate();

            Options = options;

            // Inits the providers dictionary:
            _providers = providers.ToDictionary(provider => provider.Name);

            // Checks if the default SMS provider exist:
            if (!_providers.ContainsKey(options.DefaultProvider))
                throw new SmsProviderNotFoundException(options.DefaultProvider);

            // Sets the default provider:
            _defaultProvider = _providers[options.DefaultProvider];
        }

        /// <summary>
        /// Get the sms service options instance
        /// </summary>
        public SmsServiceOptions Options { get; }

        /// <summary>
        /// Get the list of sms providers attached to this sms service.
        /// </summary>
        public IEnumerable<ISmsProvider> Providers => _providers.Values;

        /// <summary>
        /// Get the default sms provider attached to this sms service.
        /// </summary>
        public ISmsProvider DefaultProvider => _defaultProvider;

        /// <inheritdoc/>
        public SmsSendingResult Send(SmsMessage message)
            => Send(message, _defaultProvider);

        /// <inheritdoc/>
        public SmsSendingResult Send(SmsMessage message, string providerName)
        {
            if (!_providers.TryGetValue(providerName, out var provider))
                throw new SmsProviderNotFoundException(providerName);

            return Send(message, provider);
        }

        /// <inheritdoc/>
        public SmsSendingResult Send(SmsMessage message, SmsProviderName providerName)
            => Send(message, providerName.ToString());

        /// <inheritdoc/>
        public SmsSendingResult Send(SmsMessage message, ISmsProvider provider)
        {
            if (message is null)
                throw new ArgumentNullException(nameof(message));

            if (provider is null)
                throw new ArgumentNullException(nameof(provider));

            // check if the from is null
            CheckMessageFromValue(provider, message);

            // send the sms message
            return provider.Send(message);
        }

        /// <inheritdoc/>
        public Task<SmsSendingResult> SendAsync(SmsMessage message, CancellationToken cancellationToken = default)
            => SendAsync(message, _defaultProvider, cancellationToken);

        /// <inheritdoc/>
        public Task<SmsSendingResult> SendAsync(SmsMessage message, string providerName, CancellationToken cancellationToken = default)
        {
            if (!_providers.TryGetValue(providerName, out var provider))
                throw new SmsProviderNotFoundException(providerName);

            return SendAsync(message, provider, cancellationToken);
        }

        /// <inheritdoc/>
        public Task<SmsSendingResult> SendAsync(SmsMessage message, SmsProviderName providerName, CancellationToken cancellationToken = default)
            => SendAsync(message, providerName.ToString(), cancellationToken);

        /// <inheritdoc/>
        public Task<SmsSendingResult> SendAsync(SmsMessage message, ISmsProvider provider, CancellationToken cancellationToken = default)
        {
            if (message is null)
                throw new ArgumentNullException(nameof(message));

            if (provider is null)
                throw new ArgumentNullException(nameof(provider));

            CheckMessageFromValue(provider, message);

            return provider.SendAsync(message, cancellationToken);
        }

        /// <inheritdoc/>
        public SmsStatusResult GetStatus(string id, string? phoneNumber)
            => GetStatus(id, phoneNumber, _defaultProvider);

        /// <inheritdoc/>
        public SmsStatusResult GetStatus(string id, string? phoneNumber, SmsProviderName providerName)
        {
            if (!_providers.TryGetValue(providerName.ToString(), out var provider))
                throw new SmsProviderNotFoundException(providerName.ToString());

            return GetStatus(id, phoneNumber, provider);
        }

        /// <inheritdoc/>
        public SmsStatusResult GetStatus(string id, string? phoneNumber, ISmsProvider provider)
        {
            if (id is null)
                throw new ArgumentNullException(nameof(id));

            return provider.GetStatus(id, phoneNumber);
        }

        /// <inheritdoc/>
        public Task<SmsStatusResult> GetStatusAsync(string id, string? phoneNumber, CancellationToken cancellationToken = default)
            => GetStatusAsync(id, phoneNumber, _defaultProvider, cancellationToken);

        /// <inheritdoc/>
        public Task<SmsStatusResult> GetStatusAsync(string id, string? phoneNumber, SmsProviderName providerName, CancellationToken cancellationToken = default)
        {
            if (!_providers.TryGetValue(providerName.ToString(), out var provider))
                throw new SmsProviderNotFoundException(providerName.ToString());

            return GetStatusAsync(id, phoneNumber, provider, cancellationToken);
        }

        /// <inheritdoc/>
        public Task<SmsStatusResult> GetStatusAsync(string id, string? phoneNumber, ISmsProvider provider, CancellationToken cancellationToken = default)
        {
            if (id is null)
                throw new ArgumentNullException(nameof(id));

            return provider.GetStatusAsync(id, phoneNumber, cancellationToken);
        }

        /// <summary>
        /// Checks if the message from value is supplied.
        /// </summary>
        /// <param name="message">The message instance.</param>
        private void CheckMessageFromValue(ISmsProvider provider, SmsMessage message)
        {
            if (!provider.IsAllowNullFrom
                  && message.From is null)
            {
                if (Options.DefaultFrom is null)
                    throw new ArgumentException($"{nameof(SmsMessage)}.{nameof(SmsMessage.From)} is null. Either supply a {nameof(SmsMessage.From)} value in the message, or set a [{nameof(SmsServiceOptions.DefaultFrom)}] value in {nameof(SmsServiceOptions)}");

                message.SetFrom(Options.DefaultFrom);
            }
        }
    }
}
