﻿using Spoleto.SMS.Providers;

namespace Spoleto.SMS
{
    /// <summary>
    /// The SMS service factory used to create an instance of <see cref="SmsService"/>
    /// </summary>
    public class SmsServiceFactory
    {
        private readonly SmsServiceOptions _options = new();
        private readonly List<ISmsProvider> _providers = new();

        /// <summary>
        /// Sets the options of the SMS service.
        /// </summary>
        /// <param name="options">The SMS option initializer.</param>
        /// <returns>The instance of <see cref="SmsServiceFactory"/> to enable method chaining.</returns>
        public SmsServiceFactory WithOptions(Action<SmsServiceOptions> options)
        {
            if (options is null)
                throw new ArgumentNullException(nameof(options));

            // set the SMS options and validate it.
            options(_options);
            _options.Validate();

            return this;
        }

        /// <summary>
        /// Adds the <see cref="ISmsProvider"/> to be used by the SMS service.
        /// </summary>
        /// <param name="provider">The <see cref="ISmsProvider"/> instance.</param>
        /// <returns>The instance of <see cref="SmsServiceFactory"/> to enable method chaining.</returns>
        public SmsServiceFactory AddProvider(ISmsProvider provider)
        {
            if (provider is null)
                throw new ArgumentNullException(nameof(provider));

            _providers.Add(provider);

            return this;
        }

        /// <summary>
        /// Creates the SMS service instance.
        /// </summary>
        /// <returns>Instance of <see cref="SmsService"/>.</returns>
        public ISmsService Create() => new SmsService(_providers, _options);
    }
}
