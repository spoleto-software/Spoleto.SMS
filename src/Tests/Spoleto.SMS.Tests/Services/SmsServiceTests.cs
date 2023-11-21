using Microsoft.Extensions.DependencyInjection;

namespace Spoleto.SMS.Tests.Services
{
    public class SmsServiceTests : BaseTest
    {
        private SmsMessage _sms;
        private SentSmsMessage _sentSms;

        [OneTimeSetUp]
        public void Setup()
        {
            _sms = ConfigurationHelper.GetSmsMessageSmsc();
            _sentSms = ConfigurationHelper.GetSentSmsMessageSmsc();
        }

        [Test]
        public void SendSmsByDefaultProvider()
        {
            // Arrange
            var smsService = ServiceProvider.GetRequiredService<ISmsService>();

            // Act
            var result = smsService.Send(_sms);

            // Assert
            Assert.That(result.Success, Is.True);
        }

        [Test]
        public async Task SendSmsByDefaultProviderAsync()
        {
            // Arrange
            var smsService = ServiceProvider.GetRequiredService<ISmsService>();

            // Act
            var result = await smsService.SendAsync(_sms);

            // Assert
            Assert.That(result.Success, Is.True);
        }

        [Test]
        public void GetStatusByDefaultProvider()
        {
            // Arrange
            var smsService = ServiceProvider.GetRequiredService<ISmsService>();

            // Act
            var result = smsService.GetStatus(_sentSms.Id, _sentSms.To);

            // Assert
            Assert.That(result.Success, Is.True);
        }

        [Test]
        public async Task GetStatusByDefaultProviderAsync()
        {
            // Arrange
            var smsService = ServiceProvider.GetRequiredService<ISmsService>();

            // Act
            var result = await smsService.GetStatusAsync(_sentSms.Id, _sentSms.To);

            // Assert
            Assert.That(result.Success, Is.True);
        }

        [Test]
        public void SendSms()
        {
            // Arrange
            var smsService = ServiceProvider.GetRequiredService<ISmsService>();

            // Act
            var result = smsService.Send(SmsProviderName.GetSMS, _sms);

            // Assert
            Assert.That(result.Success, Is.True);
        }

        [Test]
        public async Task SendSmsAsync()
        {
            // Arrange
            var smsService = ServiceProvider.GetRequiredService<ISmsService>();

            // Act
            var result = await smsService.SendAsync(SmsProviderName.GetSMS, _sms);

            // Assert
            Assert.That(result.Success, Is.True);
        }

        [Test]
        public void GetStatus()
        {
            // Arrange
            var smsService = ServiceProvider.GetRequiredService<ISmsService>();

            // Act
            var result = smsService.GetStatus(SmsProviderName.GetSMS, _sentSms.Id, _sentSms.To);

            // Assert
            Assert.That(result.Success, Is.True);
        }

        [Test]
        public async Task GetStatusAsync()
        {
            // Arrange
            var smsService = ServiceProvider.GetRequiredService<ISmsService>();

            // Act
            var result = await smsService.GetStatusAsync(SmsProviderName.GetSMS, _sentSms.Id, _sentSms.To);

            // Assert
            Assert.That(result.Success, Is.True);
        }
    }
}