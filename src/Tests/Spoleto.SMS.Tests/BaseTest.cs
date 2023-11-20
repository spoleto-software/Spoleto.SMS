using CIS.Service.Client.Tests;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Spoleto.SMS.Providers.GetSms;
using Spoleto.SMS.Providers.Smsc;

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

            //services.AddOptions();
            //services.Configure<SmscOptions>(ConfigurationHelper.Configuration.GetSection(nameof(SmscOptions)));

            services.AddSingleton(ConfigurationHelper.Configuration.GetSection(nameof(SmscOptions)).Get<SmscOptions>()!);
            services.AddSingleton<ISmscProvider, SmscProvider>();

            services.AddSingleton(ConfigurationHelper.Configuration.GetSection(nameof(GetSmsOptions)).Get<GetSmsOptions>()!);
            services.AddHttpClient<IGetSmsProvider, GetSmsProvider>();
            services.AddSingleton<IGetSmsProvider, GetSmsProvider>();

            _serviceProvider = services.BuildServiceProvider();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _serviceProvider.Dispose();
        }
    }
}
