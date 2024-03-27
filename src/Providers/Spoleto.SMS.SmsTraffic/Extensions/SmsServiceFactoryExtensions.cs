using Spoleto.SMS.Providers.SmsTraffic;

namespace Spoleto.SMS
{
    /// <summary>
    /// Extension methods to configure an <see cref="SmsServiceFactory"/> with <see cref="SmsTrafficProvider"/>.
    /// </summary>
    public static class SmsServiceFactoryExtensions
    {
        /// <summary>
        /// Adds the SmsTraffic provider to be used in the SMS service.
        /// </summary>
        /// <remarks>
        /// <see href="https://www.smstraffic.ru/api"/>.
        /// </remarks>
        /// <param name="builder">The <see cref="SmsServiceFactory"/> instance.</param>
        /// <param name="login">Login.</param>
        /// <param name="password">Password.</param>
        /// <returns>The <see cref="SmsServiceFactory"/> instance is provided to support method chaining capabilities.</returns>
        public static SmsServiceFactory AddSmsTraffic(this SmsServiceFactory builder, string login, string password)
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
        /// <param name="builder">The <see cref="SmsServiceFactory"/> instance.</param>
        /// <param name="config">The action to configure the <see cref="SmsTrafficOptions"/> for the SmsTraffic provider.</param>
        /// <returns>The <see cref="SmsServiceFactory"/> instance is provided to support method chaining capabilities.</returns>
        public static SmsServiceFactory AddSmsTraffic(this SmsServiceFactory builder, Action<SmsTrafficOptions> config)
        {
            // loads the options:
            var options = new SmsTrafficOptions();
            config(options);

            // validates the options:
            options.Validate();

            // add the provider to the SMS service factory
            builder.AddProvider(new SmsTrafficProvider(options));

            return builder;
        }

        /// <summary>
        /// Adds the SmsTraffic provider to be used in the SMS service.
        /// </summary>
        /// <remarks>
        /// <see href="https://www.smstraffic.ru/api"/>.
        /// </remarks>
        /// <param name="builder">The <see cref="SmsServiceFactory"/> instance.</param>
        /// <param name="provider">The <see cref="SmsTrafficProvider"/> instance.</param>
        /// <returns>The <see cref="SmsServiceFactory"/> instance is provided to support method chaining capabilities.</returns>
        public static SmsServiceFactory AddSmsTraffic(this SmsServiceFactory builder, SmsTrafficProvider provider)
        {
            // add the provider to the SMS service factory
            builder.AddProvider(provider);

            return builder;
        }
    }
}