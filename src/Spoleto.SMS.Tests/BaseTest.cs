using CIS.Service.Client.Tests;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Spoleto.SMS.Providers;

namespace Spoleto.SMS.Tests
{
    public class BaseTest
    {
        private ServiceProvider _serviceProvider;

        protected ServiceProvider ServiceProvider => _serviceProvider;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var services = new ServiceCollection();
            //services.AddHttpClient();

            //services.AddOptions();
            //services.Configure<SmscOptions>(ConfigurationHelper.Configuration.GetSection(nameof(SmscOptions)));

            services.AddSingleton(ConfigurationHelper.Configuration.GetSection(nameof(SmscOptions)).Get<SmscOptions>()!);
            services.AddSingleton<ISmscProvider, SmscProvider>();


            _serviceProvider = services.BuildServiceProvider();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _serviceProvider.Dispose();
        }
    }
}
