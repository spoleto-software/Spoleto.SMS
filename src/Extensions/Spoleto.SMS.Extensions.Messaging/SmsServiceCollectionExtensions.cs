using Microsoft.Extensions.DependencyInjection;

namespace Spoleto.SMS.Extensions.Messaging
{
    /// <summary>
    /// Extension methods to configure an <see cref="IServiceCollection"/> for <see cref="ISmsService"/>.
    /// </summary>
    public static class SmsServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the Spoleto.SMS service.
        /// </summary>
        /// <param name="serviceCollection">The service collection instance.</param>
        /// <param name="defaultProviderName">The name of the default delivery provider to be used.</param>
        public static SmsServiceBuilder AddSMS(this IServiceCollection serviceCollection, string defaultProviderName)
            => AddSMS(serviceCollection, options => options.DefaultProvider = defaultProviderName);

        /// <summary>
        /// Adds the Spoleto.SMS service.
        /// </summary>
        /// <param name="serviceCollection">The service collection instance.</param>
        /// <param name="config">The action to configure the <see cref="SmsServiceOptions"/> for the SmsService.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="config"/> is null.</exception>
        public static SmsServiceBuilder AddSMS(this IServiceCollection serviceCollection, Action<SmsServiceOptions> config)
        {
            if (config is null)
                throw new ArgumentNullException(nameof(config));

            // loads the options
            var options = new SmsServiceOptions();
            config(options);

            serviceCollection.AddSingleton(s => options);
            serviceCollection.AddScoped<ISmsService, SmsService>();

            // registers the providers on this instance:
            return new SmsServiceBuilder(serviceCollection);
        }
    }
}