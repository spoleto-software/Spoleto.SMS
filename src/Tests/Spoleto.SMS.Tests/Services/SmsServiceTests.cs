using Microsoft.Extensions.DependencyInjection;

namespace Spoleto.SMS.Tests.Services
{
    public class SmsServiceTests : BaseTest
    {
        private SmsMessage _smscMessage;
        private SmsMessage _smsTrafficMessage;
        private SmsMessage _getSmsMessage;

        private SentSmsMessage _sentSms;

        [OneTimeSetUp]
        public void Setup()
        {
            _smscMessage = ConfigurationHelper.GetSmsMessageSmsc();
            _smsTrafficMessage = ConfigurationHelper.GetSmsMessageSmsTraffic();
            _getSmsMessage = ConfigurationHelper.GetSmsMessageGetSms();

            _sentSms = ConfigurationHelper.GetSentSmsMessageSmsc();
        }

        [Test]
        public void SendSmsByDefaultProvider()
        {
            // Arrange
            var smsService = ServiceProvider.GetRequiredService<ISmsService>();

            // Act
            var result = smsService.Send(_smscMessage);

            // Assert
            Assert.That(result.Success, Is.True);
        }

        [Test]
        public async Task SendSmsByDefaultProviderAsync()
        {
            // Arrange
            var smsService = ServiceProvider.GetRequiredService<ISmsService>();

            // Act
            var result = await smsService.SendAsync(_smscMessage);

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
        public void SendSmsWithGetSms()
        {
            // Arrange
            var smsService = ServiceProvider.GetRequiredService<ISmsService>();

            // Act
            var result = smsService.Send(SmsProviderName.GetSMS, _getSmsMessage);

            // Assert
            Assert.That(result.Success, Is.True);
        }

        [Test]
        public async Task SendSmsWithGetSmsAsync()
        {
            // Arrange
            var smsService = ServiceProvider.GetRequiredService<ISmsService>();

            // Act
            var result = await smsService.SendAsync(SmsProviderName.GetSMS, _getSmsMessage);

            // Assert
            Assert.That(result.Success, Is.True);
        }

        [Test]
        public void SendSmsWithSmsTraffic()
        {
            // Arrange
            var smsService = ServiceProvider.GetRequiredService<ISmsService>();

            // Act
            var result = smsService.Send(SmsProviderName.SmsTraffic, _smsTrafficMessage);

            // Assert
            Assert.That(result.Success, Is.True);
        }

        [Test]
        public async Task SendSmsWithSmsTrafficAsync()
        {
            // Arrange
            var smsService = ServiceProvider.GetRequiredService<ISmsService>();

            // Act
            var result = await smsService.SendAsync(SmsProviderName.SmsTraffic, _smsTrafficMessage);

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