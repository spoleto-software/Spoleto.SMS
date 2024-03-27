using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Spoleto.SMS.Providers.GetSms;
using Spoleto.SMS.Providers.Smsc;
using Spoleto.SMS.Providers.SmsTraffic;

namespace Spoleto.SMS.Tests.Providers
{
    public class BaseTest
    {
        private ServiceProvider _serviceProvider;

        protected ServiceProvider ServiceProvider => _serviceProvider;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var services = new ServiceCollection();

            var smscOptions = ConfigurationHelper.Configuration.GetSection(nameof(SmscOptions)).Get<SmscOptions>()!;
            services.AddSingleton(smscOptions);
            services.AddSingleton<ISmscProvider, SmscProvider>();

            var getSmsOptions = ConfigurationHelper.Configuration.GetSection(nameof(GetSmsOptions)).Get<GetSmsOptions>()!;
            services.AddSingleton(getSmsOptions);
            services.AddHttpClient<IGetSmsProvider, GetSmsProvider>();

            var smsTrafficOptions = ConfigurationHelper.Configuration.GetSection(nameof(SmsTrafficOptions)).Get<SmsTrafficOptions>()!;
            services.AddSingleton(smsTrafficOptions);
            services.AddHttpClient<ISmsTrafficProvider, SmsTrafficProvider>();

            _serviceProvider = services.BuildServiceProvider();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _serviceProvider.Dispose();
        }
    }
}
