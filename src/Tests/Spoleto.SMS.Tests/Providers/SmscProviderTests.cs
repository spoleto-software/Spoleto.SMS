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
        public async Task SendSmsListAsync()
        {
            // Arrange
            var provider = ServiceProvider.GetRequiredService<ISmscProvider>();
            var list = ConfigurationHelper.GetSmsMessageListSmsc();
            var providerData = new SmscMessageData
            {
                List = list
            };
            var smsMessage = new SmscMessage(null, _sms.From, (string)null, _sms.IsAllowSendToForeignNumbers, providerData);

            // Act
            var result = await provider.SendAsync(smsMessage);

            // Assert
            Assert.That(result.IsSuccess, Is.True);
        }

        [Test]
        public async Task SendSmsListUsingBaseAsync()
        {
            // Arrange
            var provider = ServiceProvider.GetRequiredService<ISmscProvider>();
            var list = ConfigurationHelper.GetSmsMessageListSmsc();
            var smsMessage = new SmsMessage(string.Empty, _sms.From, (string)null, _sms.IsAllowSendToForeignNumbers).WithProviderData(nameof(SmscProviderData.List), list);

            // Act
            var result = await provider.SendAsync(smsMessage);

            // Assert
            Assert.That(result.Success, Is.True);
        }

        [Test]
        public async Task SendSmsWithProviderDataAsync()
        {
            // Arrange
            var provider = ServiceProvider.GetRequiredService<ISmscProvider>();
            var providerData = new List<SmsProviderData>
            {
                new(SmscProviderData.OperatorInfo, true)
            };
            var smsMessage = new SmsMessage(_sms.Body, _sms.From, _sms.To, _sms.IsAllowSendToForeignNumbers, providerData);

            // Act
            var result = await provider.SendAsync(smsMessage);

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
            var result = await provider.GetStatusAsync(_sentSms.Id, _sentSms.To, CancellationToken.None);

            // Assert
            Assert.That(result.Success, Is.True);
        }

        [Test]
        public async Task GetBatchStatusAsync()
        {
            // Arrange
            var provider = ServiceProvider.GetRequiredService<ISmscProvider>();

            // Act
            var result = await provider.GetStatusAsync($"{_sentSms.Id},{_sentSms.Id}", $"{_sentSms.To},{_sentSms.To}", CancellationToken.None);

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