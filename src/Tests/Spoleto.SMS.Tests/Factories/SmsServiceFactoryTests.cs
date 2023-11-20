using CIS.Service.Client.Tests;
using Microsoft.Extensions.Configuration;
using Spoleto.SMS.DependencyInjection.GetSms;
using Spoleto.SMS.Providers.GetSms;
using Spoleto.SMS.Providers.Smsc;

namespace Spoleto.SMS.Tests.Factories
{
    public class SmsServiceFactoryTests
    {
        private SmsMessage _sms;
        private SentSmsMessage _sentSms;
        private ISmsService _smsService;

        [OneTimeSetUp]
        public void Setup()
        {
            _sms = ConfigurationHelper.GetSmsMessageSmsc();
            _sentSms = ConfigurationHelper.GetSentSmsMessageSmsc();
            var smscOptions = ConfigurationHelper.Configuration.GetSection(nameof(SmscOptions)).Get<SmscOptions>()!;
            var getSmsOptions = ConfigurationHelper.Configuration.GetSection(nameof(GetSmsOptions)).Get<GetSmsOptions>()!;

            _smsService = new SmsServiceFactory()
               .WithOptions(options =>
               {
                   options.DefaultFrom = "Default Sender ID";
                   options.DefaultProvider = SmscProvider.ProviderName;
               })
               // register the EDPs
               .AddSmsc(smscOptions.SMSC_LOGIN, smscOptions.SMSC_PASSWORD)
               .AddGetSms(getSmsOptions.Login, getSmsOptions.Password, getSmsOptions.ServiceUrl)
               .Create();

        }
        [Test]
        public void SendSms()
        {
            // Arrange
            var smsService = _smsService;

            // Act
            var result = smsService.Send(_sms);

            // Assert
            Assert.That(result.Success, Is.True);
        }
    }
}
