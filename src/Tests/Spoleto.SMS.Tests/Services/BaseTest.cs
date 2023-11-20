using CIS.Service.Client.Tests;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Spoleto.SMS;
using Spoleto.SMS.Extensions.GetSms;
using Spoleto.SMS.Extensions.Messaging;
using Spoleto.SMS.Extensions.Smsc;
using Spoleto.SMS.Providers.GetSms;
using Spoleto.SMS.Providers.Smsc;

namespace Spoleto.SMS.Tests.Services
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
            var getSmsOptions = ConfigurationHelper.Configuration.GetSection(nameof(GetSmsOptions)).Get<GetSmsOptions>()!;

            services.AddSMS(SmscProvider.ProviderName)
                .AddGetSms(getSmsOptions.Login, getSmsOptions.Password, getSmsOptions.ServiceUrl)
                .AddSmsc(smscOptions.SMSC_LOGIN, smscOptions.SMSC_PASSWORD);

            _serviceProvider = services.BuildServiceProvider();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _serviceProvider.Dispose();
        }
    }
}
