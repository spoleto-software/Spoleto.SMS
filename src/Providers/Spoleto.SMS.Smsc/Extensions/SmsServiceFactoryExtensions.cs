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
        /// <param name="builder">The <see cref="SmsServiceFactory"/> instance.</param>
        /// <param name="login">SMSC_LOGIN.</param>
        /// <param name="password">SMSC_PASSWORD.</param>
        /// <returns>The instance of <see cref="SmsServiceFactory"/> to enable methods chaining.</returns>
        public static SmsServiceFactory UseSmsc(this SmsServiceFactory builder, string login, string password)
           => builder.UseSmsc(op => { op.SMSC_LOGIN = login; op.SMSC_PASSWORD = password; });

        /// <summary>
        /// Adds the SMSC provider to be used in the SMS service.
        /// </summary>
        /// <param name="builder">The <see cref="SmsServiceFactory"/> instance.</param>
        /// <param name="config">The configuration builder instance.</param>
        /// <returns>The instance of <see cref="SmsServiceFactory"/> to enable methods chaining.</returns>
        public static SmsServiceFactory UseSmsc(this SmsServiceFactory builder, Action<SmscOptions> config)
        {
            // loads the configuration
            var configuration = new SmscOptions();
            config(configuration);

            // validates the configuration
            configuration.Validate();

            // add the provider to the SMS service factory
            builder.UseProvider(new SmscProvider(configuration));

            return builder;
        }

        /// <summary>
        /// Adds the SMSC provider to be used in the SMS service.
        /// </summary>
        /// <param name="builder">The <see cref="SmsServiceFactory"/> instance.</param>
        /// <param name="provider">The <see cref="SmscProvider"/> instance.</param>
        /// <returns>The instance of <see cref="SmsServiceFactory"/> to enable methods chaining.</returns>
        public static SmsServiceFactory UseSmsc(this SmsServiceFactory builder, SmscProvider provider)
        {
            // add the provider to the SMS service factory
            builder.UseProvider(provider);

            return builder;
        }
    }
}
