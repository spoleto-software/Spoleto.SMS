using CIS.Service.Client.Tests;
using Microsoft.Extensions.DependencyInjection;
using Spoleto.SMS.Providers.Smsc;

namespace Spoleto.SMS.Tests.Providers
{
    public class SmscProviderTests : BaseTest
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
        public void SendSms()
        {
            // Arrange
            var provider = ServiceProvider.GetRequiredService<ISmscProvider>();

            // Act
            var result = provider.Send(_sms);

            // Assert
            Assert.That(result.Success, Is.True);
        }

        [Test]
        public async Task SendSmsAsync()
        {
            // Arrange
            var provider = ServiceProvider.GetRequiredService<ISmscProvider>();

            // Act
            var result = await provider.SendAsync(_sms);

            // Assert
            Assert.That(result.Success, Is.True);
        }

        [Test]
        public void GetStatus()
        {
            // Arrange
            var provider = ServiceProvider.GetRequiredService<ISmscProvider>();

            // Act
            var result = provider.GetStatus(_sentSms.Id, _sentSms.To);

            // Assert
            Assert.That(result.Success, Is.True);
        }

        [Test]
        public async Task GetStatusAsync()
        {
            // Arrange
            var provider = ServiceProvider.GetRequiredService<ISmscProvider>();

            // Act
            var result = await provider.GetStatusAsync(_sentSms.Id, _sentSms.To);

            // Assert
            Assert.That(result.Success, Is.True);
        }

        [Test]
        public void GetBalance()
        {
            // Arrange
            var provider = ServiceProvider.GetRequiredService<ISmscProvider>();

            // Act
            var result = provider.GetBalance();

            // Assert
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void CheckPhoneNumber()
        {
            // Arrange
            var provider = ServiceProvider.GetRequiredService<ISmscProvider>();

            // Act
            provider.CheckPhoneNumber(_sms.To, _sms.From);

            // Assert
            Assert.Pass();
        }
    }
}