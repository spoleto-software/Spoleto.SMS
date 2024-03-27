using Microsoft.Extensions.DependencyInjection;
using Spoleto.SMS.Extensions.Messaging;
using Spoleto.SMS.Providers;
using Spoleto.SMS.Providers.SmsTraffic;

namespace Spoleto.SMS.Extensions.SmsTraffic
{
    /// <summary>
    /// Extension methods to configure an <see cref="SmsServiceBuilder"/> with <see cref="SmsTrafficProvider"/>.
    /// </summary>
    public static class SmsServiceBuilderExtensions
    {
        /// <summary>
        /// Adds the SmsTraffic provider to be used in the SMS service.
        /// </summary>
        /// <remarks>
        /// <see href="https://www.smstraffic.ru/api"/>.
        /// </remarks>
        /// <param name="builder">The <see cref="SmsServiceBuilder"/> instance.</param>
        /// <param name="login">Login.</param>
        /// <param name="password">Password.</param>
        /// <returns>The <see cref="SmsServiceBuilder"/> instance is provided to support method chaining capabilities.</returns>
        public static SmsServiceBuilder AddSmsTraffic(this SmsServiceBuilder builder, string login, string password)
           => builder.AddSmsTraffic(x =>
           {
               x.Login = login;
               x.Password = password;
           });


        /// <summary>
        /// Adds the SmsTraffic provider to be used in the SMS service.
        /// </summary>
        /// <remarks>
        /// <see href="https://www.smstraffic.ru/api"/>.
        /// </remarks>
        /// <param name="builder">The <see cref="SmsServiceBuilder"/> instance.</param>
        /// <param name="config">The action to configure the <see cref="SmsTrafficOptions"/> for the SmsTraffic provider.</param>
        /// <returns>The <see cref="SmsServiceBuilder"/> instance is provided to support method chaining capabilities.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="config"/> is null.</exception>
        public static SmsServiceBuilder AddSmsTraffic(this SmsServiceBuilder builder, Action<SmsTrafficOptions> config)
        {
            if (config is null)
                throw new ArgumentNullException(nameof(config));

            // loads the options
            var options = new SmsTrafficOptions();
            config(options);

            // validates the options
            options.Validate();

            builder.ServiceCollection.AddSingleton(s => options);
            builder.ServiceCollection.AddHttpClient<ISmsProvider, SmsTrafficProvider>();
            builder.ServiceCollection.AddHttpClient<ISmsTrafficProvider, SmsTrafficProvider>();

            return builder;
        }

        /// <summary>
        /// Adds the SmsTraffic provider to be used in the SMS service.
        /// </summary>
        /// <remarks>
        /// <see href="https://www.smstraffic.ru/api"/>.
        /// </remarks>
        /// <param name="builder">The <see cref="SmsServiceBuilder"/> instance.</param>
        /// <param name="provider">The <see cref="SmsTrafficProvider"/> instance.</param>
        /// <returns>The <see cref="SmsServiceBuilder"/> instance is provided to support method chaining capabilities.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="provider"/> is null.</exception>
        public static SmsServiceBuilder AddSmsTraffic(this SmsServiceBuilder builder, SmsTrafficProvider provider)
        {
            if (provider is null)
                throw new ArgumentNullException(nameof(provider));

            builder.ServiceCollection.AddScoped<ISmsProvider>(x => provider);
            builder.ServiceCollection.AddScoped<ISmsTrafficProvider>(x => provider);

            return builder;
        }
    }
}