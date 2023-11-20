using Spoleto.SMS.Providers.GetSms;

namespace Spoleto.SMS.DependencyInjection.GetSms
{
    /// <summary>
    /// Extension methods to configure an <see cref="SmsServiceFactory"/> with <see cref="GetSmsProvider"/>.
    /// </summary>
    public static class SmsServiceFactoryExtensions
    {
        /// <summary>
        /// Adds the GetSms provider to be used in the SMS service.
        /// </summary>
        /// <remarks>
        /// <see href="https://getsms.uz/page/index/16"/>
        /// </remarks>
        /// <param name="builder">The <see cref="SmsServiceFactory"/> instance.</param>
        /// <param name="login">Login.</param>
        /// <param name="password">Password.</param>
        /// /// <param name="serviceUrl">Service Url.</param>
        /// <returns>The instance of <see cref="SmsServiceFactory"/> to enable methods chaining.</returns>
        public static SmsServiceFactory AddGetSms(this SmsServiceFactory builder, string login, string password, string serviceUrl)
           => builder.AddGetSms(op =>
           {
               op.Login = login;
               op.Password = password;
               op.ServiceUrl = serviceUrl;
           });


        /// <summary>
        /// Adds the GetSms provider to be used in the SMS service.
        /// </summary>
        /// <remarks>
        /// <see href="https://getsms.uz/page/index/16"/>
        /// </remarks>
        /// <param name="builder">The <see cref="SmsServiceFactory"/> instance.</param>
        /// <param name="config">The configuration builder instance.</param>
        /// <returns>The instance of <see cref="SmsServiceFactory"/> to enable methods chaining.</returns>
        public static SmsServiceFactory AddGetSms(this SmsServiceFactory builder, Action<GetSmsOptions> config)
        {
            // loads the configuration
            var configuration = new GetSmsOptions();
            config(configuration);

            // validates the configuration
            configuration.Validate();

            // add the provider to the SMS service factory
            builder.AddProvider(new GetSmsProvider(configuration));

            return builder;
        }

        /// <summary>
        /// Adds the GetSms provider to be used in the SMS service.
        /// </summary>
        /// <remarks>
        /// <see href="https://getsms.uz/page/index/16"/>
        /// </remarks>
        /// <param name="builder">The <see cref="SmsServiceFactory"/> instance.</param>
        /// <param name="provider">The <see cref="GetSmsProvider"/> instance.</param>
        /// <returns>The instance of <see cref="SmsServiceFactory"/> to enable methods chaining.</returns>
        public static SmsServiceFactory AddGetSms(this SmsServiceFactory builder, GetSmsProvider provider)
        {
            // add the provider to the SMS service factory
            builder.AddProvider(provider);

            return builder;
        }
    }
}