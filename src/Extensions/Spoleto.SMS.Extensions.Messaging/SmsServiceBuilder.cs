using Microsoft.Extensions.DependencyInjection;

namespace Spoleto.SMS.Extensions.Messaging
{
    /// <summary>
    /// Spoleto.SMS dependency injection builder.
    /// </summary>
    public class SmsServiceBuilder
    {
        /// <summary>
        /// Creates an instance of <see cref="SmsServiceBuilder"/>.
        /// </summary>
        /// <param name="serviceCollection">The services collection instance.</param>
        internal SmsServiceBuilder(IServiceCollection serviceCollection)
        {
            ServiceCollection = serviceCollection;
        }

        /// <summary>
        /// Gets the service collection.
        /// </summary>
        public IServiceCollection ServiceCollection { get; }
    }
}
