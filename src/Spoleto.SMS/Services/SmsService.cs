using System.Collections.Specialized;
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
        private readonly OrderedDictionary _providers;

        /// <summary>
        /// The constructor with parameters.
        /// </summary>
        /// <param name="providers">The list of supported SMS providers.</param>
        /// <param name="options">The sms service options.</param>
        /// <exception cref="ArgumentNullException">If SMS provider or options are null.</exception>
        /// <exception cref="ArgumentException">If SMS provider list is empty.</exception>
        /// <exception cref="SmsProviderNotFoundException">If the default SMS provider cannot be found.</exception>
        public SmsService(IEnumerable<ISmsProvider> providers, SmsServiceOptions options)
        {
            if (providers is null)
                throw new ArgumentNullException(nameof(providers));

            if (!providers.Any())
                throw new ArgumentException("You must specify at least one SMS provider, the SMS provider list is empty.", nameof(providers));

            if (options is null)
                throw new ArgumentNullException(nameof(options));

            // Validates if the options are valid:
            options.Validate();

            Options = options;

            // Inits the providers dictionary:
            _providers = new OrderedDictionary(providers.Count());
            foreach (var provider in providers)
            {
                _providers.Add(provider.Name, provider);
            }

            // Checks if the default SMS provider exist:
            if (!TryGetSmsProvider(options.DefaultProvider, out var value))
                throw new SmsProviderNotFoundException(options.DefaultProvider);

            // Sets the default provider:
            _defaultProvider = value;
        }

        /// <summary>
        /// Gets the sms service options instance
        /// </summary>
        public SmsServiceOptions Options { get; }

        /// <summary>
        /// Gets the list of sms providers attached to this sms service.
        /// </summary>
        public IEnumerable<ISmsProvider> Providers
        {
            get
            {
                foreach (ISmsProvider provider in _providers.Values)
                    yield return provider;
            }
        }

        /// <summary>
        /// Gets the default sms provider attached to this sms service.
        /// </summary>
        public ISmsProvider DefaultProvider => _defaultProvider;

        /// <inheritdoc/>
        public SmsSendingResult Send(SmsMessage message)
            => Send(_defaultProvider, message);

        /// <inheritdoc/>
        public SmsSendingResult Send(string providerName, SmsMessage message)
        {
            if (!TryGetSmsProvider(providerName, out var provider))
                throw new SmsProviderNotFoundException(providerName);

            return Send(provider, message);
        }

        /// <inheritdoc/>
        public SmsSendingResult Send(SmsProviderName providerName, SmsMessage message)
            => Send(providerName.ToString(), message);

        /// <inheritdoc/>
        public SmsSendingResult Send(ISmsProvider provider, SmsMessage message)
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
            => SendAsync(_defaultProvider, message, cancellationToken);

        /// <inheritdoc/>
        public Task<SmsSendingResult> SendAsync(string providerName, SmsMessage message, CancellationToken cancellationToken = default)
        {
            if (!TryGetSmsProvider(providerName, out var provider))
                throw new SmsProviderNotFoundException(providerName);

            return SendAsync(provider, message, cancellationToken);
        }

        /// <inheritdoc/>
        public Task<SmsSendingResult> SendAsync(SmsProviderName providerName, SmsMessage message, CancellationToken cancellationToken = default)
            => SendAsync(providerName.ToString(), message, cancellationToken);

        /// <inheritdoc/>
        public Task<SmsSendingResult> SendAsync(ISmsProvider provider, SmsMessage message, CancellationToken cancellationToken = default)
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
            => GetStatus(_defaultProvider, id, phoneNumber);

        /// <inheritdoc/>
        public SmsStatusResult GetStatus(string providerName, string id, string? phoneNumber)
        {
            if (!TryGetSmsProvider(providerName.ToString(), out var provider))
                throw new SmsProviderNotFoundException(providerName.ToString());

            return GetStatus(provider, id, phoneNumber);
        }

        /// <inheritdoc/>
        public SmsStatusResult GetStatus(SmsProviderName providerName, string id, string? phoneNumber)
            => GetStatus(providerName.ToString(), id, phoneNumber);

        /// <inheritdoc/>
        public SmsStatusResult GetStatus(ISmsProvider provider, string id, string? phoneNumber)
        {
            if (id is null)
                throw new ArgumentNullException(nameof(id));

            return provider.GetStatus(id, phoneNumber);
        }

        /// <inheritdoc/>
        public Task<SmsStatusResult> GetStatusAsync(string id, string? phoneNumber, CancellationToken cancellationToken = default)
            => GetStatusAsync(_defaultProvider, id, phoneNumber, cancellationToken);

        /// <inheritdoc/>
        public Task<SmsStatusResult> GetStatusAsync(SmsProviderName providerName, string id, string? phoneNumber, CancellationToken cancellationToken = default)
            => GetStatusAsync(providerName.ToString(), id, phoneNumber, cancellationToken);

        public Task<SmsStatusResult> GetStatusAsync(string providerName, string id, string? phoneNumber, CancellationToken cancellationToken = default)
        {
            if (!TryGetSmsProvider(providerName.ToString(), out var provider))
                throw new SmsProviderNotFoundException(providerName.ToString());

            return GetStatusAsync(provider, id, phoneNumber, cancellationToken);
        }

        /// <inheritdoc/>
        public Task<SmsStatusResult> GetStatusAsync(ISmsProvider provider, string id, string? phoneNumber, CancellationToken cancellationToken = default)
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

        private bool TryGetSmsProvider(string providerName, out ISmsProvider smsProvider)
        {
            if (_providers[providerName] is not ISmsProvider provider)
            {
                smsProvider = null;
                return false;
            }

            smsProvider = provider;
            return true;
        }
    }
}
