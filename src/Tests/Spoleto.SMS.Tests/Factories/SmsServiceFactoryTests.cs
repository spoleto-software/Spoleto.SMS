using Microsoft.Extensions.Configuration;
using Spoleto.SMS.Providers.GetSms;
using Spoleto.SMS.Providers.Smsc;
using Spoleto.SMS.Providers.SmsTraffic;

namespace Spoleto.SMS.Tests.Factories
{
    public class SmsServiceFactoryTests
    {
        private SmsMessage _smscMessage;
        private SmsMessage _smsTrafficMessage;

        private SentSmsMessage _sentSms;
        private ISmsService _smsService;

        [OneTimeSetUp]
        public void Setup()
        {
            _smscMessage = ConfigurationHelper.GetSmsMessageSmsc();
            _smsTrafficMessage = ConfigurationHelper.GetSmsMessageSmsTraffic();
            _sentSms = ConfigurationHelper.GetSentSmsMessageSmsc();

            var smscOptions = ConfigurationHelper.Configuration.GetSection(nameof(SmscOptions)).Get<SmscOptions>()!;
            var getSmsOptions = ConfigurationHelper.Configuration.GetSection(nameof(GetSmsOptions)).Get<GetSmsOptions>()!;
            var smsTrafficOptions = ConfigurationHelper.Configuration.GetSection(nameof(SmsTrafficOptions)).Get<SmsTrafficOptions>()!;

            _smsService = new SmsServiceFactory()
               .WithOptions(options =>
               {
                   options.DefaultFrom = "Default Sender ID";
                   options.DefaultProvider = SmscProvider.ProviderName;
               })
               .AddSmsc(smscOptions.SMSC_LOGIN, smscOptions.SMSC_PASSWORD)
               .AddGetSms(getSmsOptions.Login, getSmsOptions.Password)
               .AddSmsTraffic(smsTrafficOptions.Login, smsTrafficOptions.Password)
               .Build();
        }

        [Test]
        public void SendSms()
        {
            // Arrange
            var smsService = _smsService;

            // Act
            var result = smsService.Send(_smscMessage);

            // Assert
            Assert.That(result.Success, Is.True);
        }


        [Test]
        public void SendUsingProviderName()
        {
            // Arrange
            var smsService = _smsService;

            // Act
            var result = smsService.Send(SmsTrafficProvider.ProviderName, _smsTrafficMessage.WithProviderData(SmsTrafficProviderData.IgnorePhoneFormat, true));

            // Assert
            Assert.That(result.Success, Is.True);
        }
    }
}
