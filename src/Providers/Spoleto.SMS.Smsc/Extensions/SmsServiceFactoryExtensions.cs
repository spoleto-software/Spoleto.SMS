using Spoleto.SMS.Providers.Smsc;

namespace Spoleto.SMS
{
    /// <summary>
    /// Extension methods to configure an <see cref="SmsServiceFactory"/> for <see cref="ISmsService"/>.
    /// </summary>
    public static class SmsServiceFactoryExtensions
    {
        /// <summary>
        /// Adds the SMSC provider to be used in the SMS service.
        /// </summary>
        /// <remarks>
        /// <see href="https://smsc.ru/api/code/libraries/http_smtp/cs/#menu"/>
        /// </remarks>
        /// <param name="builder">The <see cref="SmsServiceFactory"/> instance.</param>
        /// <param name="login">SMSC_LOGIN.</param>
        /// <param name="password">SMSC_PASSWORD.</param>
        /// <returns>The instance of <see cref="SmsServiceFactory"/> to enable methods chaining.</returns>
        public static SmsServiceFactory AddSmsc(this SmsServiceFactory builder, string login, string password)
           => builder.AddSmsc(x => { x.SMSC_LOGIN = login; x.SMSC_PASSWORD = password; });

        /// <summary>
        /// Adds the SMSC provider to be used in the SMS service.
        /// </summary>
        /// <remarks>
        /// <see href="https://smsc.ru/api/code/libraries/http_smtp/cs/#menu"/>
        /// </remarks>
        /// <param name="builder">The <see cref="SmsServiceFactory"/> instance.</param>
        /// <param name="config">The configuration builder instance.</param>
        /// <returns>The instance of <see cref="SmsServiceFactory"/> to enable methods chaining.</returns>
        public static SmsServiceFactory AddSmsc(this SmsServiceFactory builder, Action<SmscOptions> config)
        {
            // loads the options
            var options = new SmscOptions();
            config(options);

            // validates the options
            options.Validate();

            // add the provider to the SMS service factory
            builder.AddProvider(new SmscProvider(options));

            return builder;
        }

        /// <summary>
        /// Adds the SMSC provider to be used in the SMS service.
        /// </summary>
        /// <remarks>
        /// <see href="https://smsc.ru/api/code/libraries/http_smtp/cs/#menu"/>
        /// </remarks>
        /// <param name="builder">The <see cref="SmsServiceFactory"/> instance.</param>
        /// <param name="provider">The <see cref="SmscProvider"/> instance.</param>
        /// <returns>The instance of <see cref="SmsServiceFactory"/> to enable methods chaining.</returns>
        public static SmsServiceFactory AddSmsc(this SmsServiceFactory builder, SmscProvider provider)
        {
            // add the provider to the SMS service factory
            builder.AddProvider(provider);

            return builder;
        }
    }
}
