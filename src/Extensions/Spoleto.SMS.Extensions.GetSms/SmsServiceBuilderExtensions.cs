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
        /// Adds the GetSms provider to be used in the SMS service.
        /// </summary>
        /// <remarks>
        /// <see href="https://getsms.uz/page/index/16"/>
        /// </remarks>
        /// <param name="builder">The <see cref="SmsServiceBuilder"/> instance.</param>
        /// <param name="login">Login.</param>
        /// <param name="password">Password.</param>
        /// <returns>The <see cref="SmsServiceBuilder"/> instance is provided to support method chaining capabilities.</returns>
        public static SmsServiceBuilder AddGetSms(this SmsServiceBuilder builder, string login, string password)
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
        /// <param name="builder">The <see cref="SmsServiceBuilder"/> instance.</param>
        /// <param name="config">The action to configure the <see cref="GetSmsOptions"/> for the GetSms provider.</param>
        /// <returns>The <see cref="SmsServiceBuilder"/> instance is provided to support method chaining capabilities.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="config"/> is null.</exception>
        public static SmsServiceBuilder AddGetSms(this SmsServiceBuilder builder, Action<GetSmsOptions> config)
        {
            if (config is null)
                throw new ArgumentNullException(nameof(config));

            // loads the options
            var options = new GetSmsOptions();
            config(options);

            // validates the options
            options.Validate();

            builder.ServiceCollection.AddSingleton(s => options);
            builder.ServiceCollection.AddHttpClient<ISmsProvider, GetSmsProvider>();
            builder.ServiceCollection.AddHttpClient<IGetSmsProvider, GetSmsProvider>();

            return builder;
        }

        /// <summary>
        /// Adds the GetSms provider to be used in the SMS service.
        /// </summary>
        /// <remarks>
        /// <see href="https://getsms.uz/page/index/16"/>
        /// </remarks>
        /// <param name="builder">The <see cref="SmsServiceBuilder"/> instance.</param>
        /// <param name="provider">The <see cref="GetSmsProvider"/> instance.</param>
        /// <returns>The <see cref="SmsServiceBuilder"/> instance is provided to support method chaining capabilities.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="provider"/> is null.</exception>
        public static SmsServiceBuilder AddGetSms(this SmsServiceBuilder builder, GetSmsProvider provider)
        {
            if (provider is null)
                throw new ArgumentNullException(nameof(provider));

            builder.ServiceCollection.AddScoped<ISmsProvider>(x => provider);

            return builder;
        }
    }
}