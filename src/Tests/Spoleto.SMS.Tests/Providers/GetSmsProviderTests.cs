using Microsoft.Extensions.DependencyInjection;
using Spoleto.SMS.Providers.GetSms;

namespace Spoleto.SMS.Tests.Providers
{
    public class GetSmsProviderTests : BaseTest
    {
        private SmsMessage _sms;
        private SentSmsMessage _sentSms;

        [OneTimeSetUp]
        public void Setup()
        {
            _sms = ConfigurationHelper.GetSmsMessageGetSms();
            _sentSms = ConfigurationHelper.GetSentSmsMessageGetSms();
        }

        [Test]
        public void SendSms()
        {
            // Arrange
            var provider = ServiceProvider.GetRequiredService<IGetSmsProvider>();

            // Act
            var result = provider.Send(_sms);

            // Assert
            Assert.That(result.Success, Is.True);
        }

        [Test]
        public async Task SendSmsAsync()
        {
            // Arrange
            var provider = ServiceProvider.GetRequiredService<IGetSmsProvider>();

            // Act
            var result = await provider.SendAsync(_sms);

            // Assert
            Assert.That(result.Success, Is.True);
        }

        [Test]
        public void SendGetSms()
        {
            // Arrange
            var provider = ServiceProvider.GetRequiredService<IGetSmsProvider>();
            var getSms = new GetSmsMessage(_sms.From, [new GetSmsMessageItem(_sms.To, _sms.Body)], _sms.IsAllowSendToForeignNumbers);

            // Act
            var result = provider.Send(getSms);

            // Assert
            Assert.That(result.Success, Is.True);
        }

        [Test]
        public void SendSmsWithProviderData()
        {
            // Arrange
            var provider = ServiceProvider.GetRequiredService<IGetSmsProvider>();
            var providerData = new List<SmsProviderData>
            {
                new(GetSmsProviderData.Items, new List<GetSmsMessageItem>{new(_sms.To, _sms.Body)})
            };
            var smsMessage = new SmsMessage(_sms.Body, _sms.From, _sms.To, _sms.IsAllowSendToForeignNumbers, providerData);

            // Act
            var result = provider.Send(smsMessage);

            // Assert
            Assert.That(result.Success, Is.True);
        }

        [Test]
        public void GetStatus()
        {
            // Arrange
            var provider = ServiceProvider.GetRequiredService<IGetSmsProvider>();

            // Act
            var result = provider.GetStatus(_sentSms.Id);

            // Assert
            Assert.That(result.Success, Is.True);
        }

        [Test]
        public async Task GetStatusAsync()
        {
            // Arrange
            var provider = ServiceProvider.GetRequiredService<IGetSmsProvider>();

            // Act
            var result = await provider.GetStatusAsync(_sentSms.Id);

            // Assert
            Assert.That(result.Success, Is.True);
        }
    }
}