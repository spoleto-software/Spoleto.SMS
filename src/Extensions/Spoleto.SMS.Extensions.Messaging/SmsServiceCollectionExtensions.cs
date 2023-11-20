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
        /// <param name="config">The configuration initializer.</param>
        public static SmsServiceBuilder AddSMS(this IServiceCollection serviceCollection, Action<SmsServiceOptions> config)
        {
            if (config is null)
                throw new ArgumentNullException(nameof(config));

            // loads the configuration
            var configuration = new SmsServiceOptions();
            config(configuration);

            serviceCollection.AddSingleton((s) => configuration);
            serviceCollection.AddScoped<ISmsService, SmsService>();

            // registers the providers on this instance:
            return new SmsServiceBuilder(serviceCollection);
        }
    }
}