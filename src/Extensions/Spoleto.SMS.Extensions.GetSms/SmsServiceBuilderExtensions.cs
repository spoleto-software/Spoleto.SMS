using Microsoft.Extensions.DependencyInjection;
using Spoleto.SMS.Extensions.Messaging;
using Spoleto.SMS.Providers;
using Spoleto.SMS.Providers.GetSms;

namespace Spoleto.SMS.Extensions.GetSms
{
    /// <summary>
    /// Extension methods to configure an <see cref="SmsServiceBuilder"/> with <see cref="GetSmsProvider"/>.
    /// </summary>
    public static class SmsServiceBuilderExtensions
    {
        /// <summary>
        /// Adds the SMSC provider to be used in the SMS service.
        /// </summary>
        /// <param name="builder">The <see cref="SmsServiceBuilder"/> instance.</param>
        /// <param name="login">Login.</param>
        /// <param name="password">Password.</param>
        /// <returns>The instance of <see cref="SmsServiceBuilder"/> to enable methods chaining.</returns>
        public static SmsServiceBuilder UseGetSms(this SmsServiceBuilder builder, string login, string password)
           => builder.UseGetSms(op => { op.Login = login; op.Password = password; });


        /// <summary>
        /// Adds the SMSC provider to be used in the SMS service.
        /// </summary>
        /// <param name="builder">The <see cref="SmsServiceBuilder"/> instance.</param>
        /// <param name="config">The configuration builder instance.</param>
        /// <returns>The instance of <see cref="SmsServiceBuilder"/> to enable methods chaining.</returns>
        public static SmsServiceBuilder UseGetSms(this SmsServiceBuilder builder, Action<GetSmsOptions> config)
        {
            // loads the configuration
            var configuration = new GetSmsOptions();
            config(configuration);

            // validates the configuration
            configuration.Validate();

            builder.ServiceCollection.AddSingleton((s) => configuration);
            builder.ServiceCollection.AddHttpClient<IGetSmsProvider, GetSmsProvider>();

            return builder;
        }

        /// <summary>
        /// Adds the SMSC provider to be used in the SMS service.
        /// </summary>
        /// <param name="builder">The <see cref="SmsServiceBuilder"/> instance.</param>
        /// <param name="provider">The <see cref="GetSmsProvider"/> instance.</param>
        /// <returns>The instance of <see cref="SmsServiceBuilder"/> to enable methods chaining.</returns>
        public static SmsServiceBuilder UseGetSms(this SmsServiceBuilder builder, GetSmsProvider provider)
        {
            builder.ServiceCollection.AddScoped<ISmsProvider>(x => provider);

            return builder;
        }
    }
}