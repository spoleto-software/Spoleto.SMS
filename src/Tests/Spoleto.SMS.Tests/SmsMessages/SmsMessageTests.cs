namespace Spoleto.SMS.Tests.SmsMessages
{
    public class SmsMessageTests
    {
        [Test]
        public void CreateSmsMessageWithListOfRecipients()
        {
            // Arrange
            var listOfRecipients = new List<string> { "70000000000", "71111111111" };

            // Act
            var smsMessage = new SmsMessage("SMS body", "from", listOfRecipients);

            // Assert
            Assert.That(smsMessage.To, Is.EqualTo(String.Join(smsMessage.PhoneNumberSeparator, listOfRecipients)));
        }
    }
}
