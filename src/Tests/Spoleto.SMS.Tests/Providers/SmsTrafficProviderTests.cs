using Microsoft.Extensions.DependencyInjection;
using Spoleto.SMS.Providers.SmsTraffic;

namespace Spoleto.SMS.Tests.Providers
{
    public class SmsTrafficProviderTests : BaseTest
    {
        private SmsMessage _sms;
        private SentSmsMessage _sentSms;
        private SmsTrafficTestGroup _testGroup;

        [OneTimeSetUp]
        public void Setup()
        {
            _sms = ConfigurationHelper.GetSmsMessageSmsTraffic();
            _sentSms = ConfigurationHelper.GetSentSmsMessageSmsTraffic();
            _testGroup = ConfigurationHelper.GetSmsTrafficTestGroup();
        }

        [Test]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(5)]
        public void SendSms(int rus)
        {
            // Arrange
            var provider = ServiceProvider.GetRequiredService<ISmsTrafficProvider>();
            var smsTrafficMessage = new SmsTrafficMessage(_sms.Body + $" ({nameof(SmsTrafficMessageData.Rus)} = {rus})",
                _sms.From, _sms.To, providerData: new() { Rus = rus });

            // Act
            var result = provider.Send(smsTrafficMessage);

            // Assert
            Assert.That(result.Success, Is.True);
        }


        [Test]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(5)]
        public async Task SendSmsAsync(int rus)
        {
            // Arrange
            var provider = ServiceProvider.GetRequiredService<ISmsTrafficProvider>();
            var smsTrafficMessage = new SmsTrafficMessage(_sms.Body + $" ({nameof(SmsTrafficMessageData.Rus)} = {rus})",
                _sms.From, _sms.To, providerData: new() { Rus = rus });

            // Act
            var result = await provider.SendAsync(smsTrafficMessage);

            // Assert
            Assert.That(result.Success, Is.True);
        }

        [Test]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(5)]
        public void SendSmsWithStartDate(int rus)
        {
            // Arrange
            var provider = ServiceProvider.GetRequiredService<ISmsTrafficProvider>();
            var startDate = DateTime.Now.AddMinutes(1);
            var smsTrafficMessage = new SmsTrafficMessage(_sms.Body + $" ({nameof(SmsTrafficMessageData.StartDate)} = {startDate}, {nameof(SmsTrafficMessageData.Rus)} = {rus})",
                _sms.From, _sms.To, providerData: new() { Rus = rus, StartDate = startDate });

            // Act
            var result = provider.Send(smsTrafficMessage);

            // Assert
            Assert.That(result.Success, Is.True);
        }

        [Test]
        public async Task SendSmsWithProviderDataAsync()
        {
            // Arrange
            var provider = ServiceProvider.GetRequiredService<ISmsTrafficProvider>();
            var providerData = new List<SmsProviderData>
            {
                new(SmsTrafficProviderData.Rus, 0)
            };
            var smsTrafficMessage = new SmsMessage(_sms.Body + $" (Used {nameof(SmsTrafficProviderData)})",
                _sms.From, _sms.To, providerData: providerData);

            // Act
            var result = await provider.SendAsync(smsTrafficMessage);

            // Assert
            Assert.That(result.Success, Is.True);
        }

        [Test]
        public void GetStatus()
        {
            // Arrange
            var provider = ServiceProvider.GetRequiredService<ISmsTrafficProvider>();
            var smsTrafficMessage = new SmsTrafficMessage(_sms.Body + $" ({nameof(SmsTrafficMessageData.WantSmsIds)} = {1})",
                _sms.From, _sms.To, providerData: new() { WantSmsIds = true });

            // Act
            //var resultSms = provider.Send(smsTrafficMessage);
            var resultStatus = provider.GetStatus(_sentSms.Id);// resultSms.SmsSendingData?.FirstOrDefault()?.MessageId.ToString());

            // Assert
            Assert.That(resultStatus.Success, Is.True);
        }

        [Test]
        public async Task GetStatusAsync()
        {
            // Arrange
            var provider = ServiceProvider.GetRequiredService<ISmsTrafficProvider>();

            // Act
            var result = await provider.GetStatusAsync(_sentSms.Id);

            // Assert
            Assert.That(result.Success, Is.True);
        }

        [Test]
        public void GetBalance()
        {
            // Arrange
            var provider = ServiceProvider.GetRequiredService<ISmsTrafficProvider>();

            // Act
            var result = provider.GetBalance();

            // Assert
            Assert.Pass();
        }

        [Test]
        public async Task GetBalanceAsync()
        {
            // Arrange
            var provider = ServiceProvider.GetRequiredService<ISmsTrafficProvider>();

            // Act
            var result = await provider.GetBalanceAsync();

            // Assert
            Assert.Pass();
        }

        [Test]
        public void GetGroupListInformation()
        {
            // Arrange
            var provider = ServiceProvider.GetRequiredService<ISmsTrafficProvider>();

            // Act
            var result = provider.GetGroupListInformation();

            // Assert
            Assert.That(result.Code, Is.EqualTo(0));
        }

        [Test]
        public async Task GetGroupListInformationAsync()
        {
            // Arrange
            var provider = ServiceProvider.GetRequiredService<ISmsTrafficProvider>();

            // Act
            var result = await provider.GetGroupListInformationAsync();

            // Assert
            Assert.That(result.Code, Is.EqualTo(0));
        }

        [Test]
        public void GetGroupInformation()
        {
            // Arrange
            var provider = ServiceProvider.GetRequiredService<ISmsTrafficProvider>();

            // Act
            var result = provider.GetGroupInformation(_testGroup.Id);

            // Assert
            Assert.Pass();
        }

        [Test]
        public async Task GetGroupInformationAsync()
        {
            // Arrange
            var provider = ServiceProvider.GetRequiredService<ISmsTrafficProvider>();

            // Act
            var result = await provider.GetGroupInformationAsync(_testGroup.Id);

            // Assert
            Assert.Pass();
        }

        [Test]
        public void AddMemberGroupInformation()
        {
            // Arrange
            var provider = ServiceProvider.GetRequiredService<ISmsTrafficProvider>();
            var fakeNumber = "71111111111";

            // Act
            var result = provider.AddGroupMember(_testGroup.Id, fakeNumber);

            // Assert
            Assert.That(result.Code, Is.EqualTo(0));
        }

        [Test]
        public async Task AddMemberGroupInformationAsync()
        {
            // Arrange
            var provider = ServiceProvider.GetRequiredService<ISmsTrafficProvider>();
            var fakeNumber = "71111111111";

            // Act
            var result = await provider.AddGroupMemberAsync(_testGroup.Id, fakeNumber);

            // Assert
            Assert.That(result.Code, Is.EqualTo(0));
        }

        [Test]
        public void RemoveMemberGroupInformation()
        {
            // Arrange
            var provider = ServiceProvider.GetRequiredService<ISmsTrafficProvider>();
            var fakeNumber = "71111111111";

            // Act
            var result = provider.RemoveGroupMember(_testGroup.Id, fakeNumber);

            // Assert
            Assert.That(result.Code, Is.EqualTo(0));
        }

        [Test]
        public async Task RemoveGroupMemberAsync()
        {
            // Arrange
            var provider = ServiceProvider.GetRequiredService<ISmsTrafficProvider>();
            var fakeNumber = "71111111111";

            // Act
            var result = await provider.RemoveGroupMemberAsync(_testGroup.Id, fakeNumber);

            // Assert
            Assert.That(result.Code, Is.EqualTo(0));
        }

        [Test]
        public void AddMembersGroupInformation()
        {
            // Arrange
            var provider = ServiceProvider.GetRequiredService<ISmsTrafficProvider>();
            var fakeNumber1 = "71111111111";
            var fakeNumber2 = "72222222222";
            var fakeNumber3 = "73333333333";

            // Act
            var result = provider.AddGroupMembers(_testGroup.Id, new List<string> { fakeNumber1, fakeNumber2, fakeNumber3 });

            // Assert
            Assert.That(result.Code, Is.EqualTo(0));
        }

        [Test]
        public async Task AddMembersGroupInformationAsync()
        {
            // Arrange
            var provider = ServiceProvider.GetRequiredService<ISmsTrafficProvider>();
            var fakeNumber1 = "71111111111";
            var fakeNumber2 = "72222222222";
            var fakeNumber3 = "73333333333";

            // Act
            var result = await provider.AddGroupMembersAsync(_testGroup.Id, new List<string> { fakeNumber1, fakeNumber2, fakeNumber3 });

            // Assert
            Assert.That(result.Code, Is.EqualTo(0));
        }

        [Test]
        public void RemoveMembersGroupInformation()
        {
            // Arrange
            var provider = ServiceProvider.GetRequiredService<ISmsTrafficProvider>();
            var fakeNumber1 = "71111111111";
            var fakeNumber2 = "72222222222";
            var fakeNumber3 = "73333333333";

            // Act
            var result = provider.RemoveGroupMembers(_testGroup.Id, new List<string> { fakeNumber1, fakeNumber2, fakeNumber3 });

            // Assert
            Assert.That(result.Code, Is.EqualTo(0));
        }

        [Test]
        public async Task RemoveGroupMembersAsync()
        {
            // Arrange
            var provider = ServiceProvider.GetRequiredService<ISmsTrafficProvider>();
            var fakeNumber1 = "71111111111";
            var fakeNumber2 = "72222222222";
            var fakeNumber3 = "73333333333";

            // Act
            var result = await provider.RemoveGroupMembersAsync(_testGroup.Id, new List<string> { fakeNumber1, fakeNumber2, fakeNumber3 });

            // Assert
            Assert.That(result.Code, Is.EqualTo(0));
        }
    }
}