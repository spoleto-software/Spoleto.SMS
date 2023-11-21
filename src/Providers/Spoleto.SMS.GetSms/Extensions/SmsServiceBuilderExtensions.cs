using Spoleto.SMS.Providers.GetSms;

namespace Spoleto.SMS
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
        public static SmsServiceFactory AddGetSms(this SmsServiceFactory builder, string login, string password)
           => builder.AddGetSms(x =>
           {
               x.Login = login;
               x.Password = password;
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
            // loads the options:
            var options = new GetSmsOptions();
            config(options);

            // validates the options:
            options.Validate();

            // add the provider to the SMS service factory
            builder.AddProvider(new GetSmsProvider(options));

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