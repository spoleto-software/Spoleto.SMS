﻿using Microsoft.Extensions.DependencyInjection;
using Spoleto.SMS.Extensions.Messaging;
using Spoleto.SMS.Providers;
using Spoleto.SMS.Providers.Smsc;

namespace Spoleto.SMS.Extensions.Smsc
{
    /// <summary>
    /// Extension methods to configure an <see cref="SmsServiceBuilder"/> with <see cref="SmscProvider"/>.
    /// </summary>
    public static class SmsServiceBuilderExtensions
    {
        /// <summary>
        /// Adds the SMSC provider to be used in the SMS service.
        /// </summary>
        /// <remarks>
        /// <see href="https://smsc.ru/api/code/libraries/http_smtp/cs/#menu"/>
        /// </remarks>
        /// <param name="builder">The <see cref="SmsServiceBuilder"/> instance.</param>
        /// <param name="login">SMSC_LOGIN.</param>
        /// <param name="password">SMSC_PASSWORD.</param>
        /// <returns>The <see cref="SmsServiceBuilder"/> instance is provided to support method chaining capabilities.</returns>
        public static SmsServiceBuilder AddSmsc(this SmsServiceBuilder builder, string login, string password)
           => builder.AddSmsc(x =>
           {
               x.SMSC_LOGIN = login;
               x.SMSC_PASSWORD = password;
           });

        /// <summary>
        /// Adds the SMSC provider to be used in the SMS service.
        /// </summary>
        /// <remarks>
        /// <see href="https://smsc.ru/api/code/libraries/http_smtp/cs/#menu"/>
        /// </remarks>
        /// <param name="builder">The <see cref="SmsServiceBuilder"/> instance.</param>
        /// <param name="config">The action to configure the <see cref="SmscOptions"/> for the SMSC provider.</param>
        /// <returns>The <see cref="SmsServiceBuilder"/> instance is provided to support method chaining capabilities.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="config"/> is null.</exception>
        public static SmsServiceBuilder AddSmsc(this SmsServiceBuilder builder, Action<SmscOptions> config)
        {
            if (config is null)
                throw new ArgumentNullException(nameof(config));

            // loads the options
            var options = new SmscOptions();
            config(options);

            // validates the options
            options.Validate();

            builder.ServiceCollection.AddSingleton(s => options);
            builder.ServiceCollection.AddScoped<ISmsProvider, SmscProvider>();
            builder.ServiceCollection.AddScoped<ISmscProvider, SmscProvider>();

            return builder;
        }

        /// <summary>
        /// Adds the SMSC provider to be used in the SMS service.
        /// </summary>
        /// <remarks>
        /// <see href="https://smsc.ru/api/code/libraries/http_smtp/cs/#menu"/>
        /// </remarks>
        /// <param name="builder">The <see cref="SmsServiceBuilder"/> instance.</param>
        /// <param name="provider">The <see cref="SmscProvider"/> instance.</param>
        /// <returns>The <see cref="SmsServiceBuilder"/> instance is provided to support method chaining capabilities.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="provider"/> is null.</exception>
        public static SmsServiceBuilder AddSmsc(this SmsServiceBuilder builder, SmscProvider provider)
        {
            if (provider is null)
                throw new ArgumentNullException(nameof(provider));

            builder.ServiceCollection.AddScoped<ISmsProvider>(x => provider);
            builder.ServiceCollection.AddScoped<ISmscProvider>(x => provider);

            return builder;
        }
    }
}