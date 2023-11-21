using Microsoft.Extensions.Configuration;
using Spoleto.SMS.Providers.GetSms;
using Spoleto.SMS.Providers.Smsc;

namespace Spoleto.SMS.Tests.Services
{
    public class SmsServiceExtensionsTests
    {
        private ISmsService _smsService;

        [OneTimeSetUp]
        public void Setup()
        {
            var smscOptions = ConfigurationHelper.Configuration.GetSection(nameof(SmscOptions)).Get<SmscOptions>()!;
            var getSmsOptions = ConfigurationHelper.Configuration.GetSection(nameof(GetSmsOptions)).Get<GetSmsOptions>()!;

            _smsService = new SmsServiceFactory()
               .WithOptions(options =>
               {
                   options.DefaultFrom = "Default Sender ID";
                   options.DefaultProvider = SmscProvider.ProviderName;
               })
               .AddSmsc(smscOptions.SMSC_LOGIN, smscOptions.SMSC_PASSWORD)
               .AddGetSms(getSmsOptions.Login, getSmsOptions.Password)
               .Build();
        }

        [Test]
        public void GetProviderForPhoneNumberUz()
        {
            // Arrange
            var smsService = _smsService;

            // Act
            var provider = smsService.GetProviderForPhoneNumber("+998111111111");

            // Assert
            Assert.That(provider, Is.Not.Null);
            Assert.That(provider.Name, Is.EqualTo(GetSmsProvider.ProviderName));
        }

        [Test]
        public void GetProviderForPhoneNumberRus()
        {
            // Arrange
            var smsService = _smsService;

            // Act
            var provider = smsService.GetProviderForPhoneNumber("+71111111111");

            // Assert
            Assert.That(provider, Is.Not.Null);
            Assert.That(provider.Name, Is.EqualTo(SmscProvider.ProviderName));
        }
    }
}
