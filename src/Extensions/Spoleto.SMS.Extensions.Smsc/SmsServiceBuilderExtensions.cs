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
        /// <returns>The instance of <see cref="SmsServiceBuilder"/> to enable methods chaining.</returns>
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
        /// <param name="config">The configuration builder instance.</param>
        /// <returns>The instance of <see cref="SmsServiceBuilder"/> to enable methods chaining.</returns>
        public static SmsServiceBuilder AddSmsc(this SmsServiceBuilder builder, Action<SmscOptions> config)
        {
            // loads the options
            var options = new SmscOptions();
            config(options);

            // validates the options
            options.Validate();

            builder.ServiceCollection.AddSingleton(s => options);
            builder.ServiceCollection.AddScoped<ISmsProvider, SmscProvider>();

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
        /// <returns>The instance of <see cref="SmsServiceBuilder"/> to enable methods chaining.</returns>
        public static SmsServiceBuilder AddSmsc(this SmsServiceBuilder builder, SmscProvider provider)
        {
            builder.ServiceCollection.AddScoped<ISmsProvider>(x => provider);

            return builder;
        }
    }
}