using Spoleto.SMS.Providers;
using Spoleto.SMS.Providers.GetSms;

namespace Spoleto.SMS.DependencyInjection.GetSms
{
    /// <summary>
    /// Extension methods to configure an <see cref="SmsServiceFactory"/> with <see cref="GetSmsProvider"/>.
    /// </summary>
    public static class SmsServiceFactoryExtensions
    {
        /// <summary>
        /// Adds the SMSC provider to be used in the SMS service.
        /// </summary>
        /// <param name="builder">The <see cref="SmsServiceFactory"/> instance.</param>
        /// <param name="login">Login.</param>
        /// <param name="password">Password.</param>
        /// <returns>The instance of <see cref="SmsServiceFactory"/> to enable methods chaining.</returns>
        public static SmsServiceFactory UseGetSms(this SmsServiceFactory builder, string login, string password)
           => builder.UseGetSms(op => { op.Login = login; op.Password = password; });


        /// <summary>
        /// Adds the SMSC provider to be used in the SMS service.
        /// </summary>
        /// <param name="builder">The <see cref="SmsServiceFactory"/> instance.</param>
        /// <param name="config">The configuration builder instance.</param>
        /// <returns>The instance of <see cref="SmsServiceFactory"/> to enable methods chaining.</returns>
        public static SmsServiceFactory UseGetSms(this SmsServiceFactory builder, Action<GetSmsOptions> config)
        {
            // loads the configuration
            var configuration = new GetSmsOptions();
            config(configuration);

            // validates the configuration
            configuration.Validate();

            // add the provider to the SMS service factory
            builder.UseProvider(new GetSmsProvider(configuration));

            return builder;
        }

        /// <summary>
        /// Adds the SMSC provider to be used in the SMS service.
        /// </summary>
        /// <param name="builder">The <see cref="SmsServiceFactory"/> instance.</param>
        /// <param name="provider">The <see cref="GetSmsProvider"/> instance.</param>
        /// <returns>The instance of <see cref="SmsServiceFactory"/> to enable methods chaining.</returns>
        public static SmsServiceFactory UseGetSms(this SmsServiceFactory builder, GetSmsProvider provider)
        {
            // add the provider to the SMS service factory
            builder.UseProvider(provider);

            return builder;
        }
    }
}