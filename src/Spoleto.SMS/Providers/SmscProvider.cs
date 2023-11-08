namespace Spoleto.SMS.Providers
{
    /// <summary>
    /// The SMSC provider for sending SMS messages.
    /// </summary>
    /// <remarks>
    /// <see href="https://smsc.ru/api/code/libraries/http_smtp/cs/#menu"/>.
    /// </remarks>
    public class SmscProvider : ISmscProvider
    {
        private const string ProviderName = nameof(SmsProviderName.SMSC);

        /// <inheritdoc/>
        public string Name => ProviderName;

        /// <inheritdoc/>
        public SmsSendingResult Send(SmsMessage message)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<SmsSendingResult> SendAsync(SmsMessage message, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public SmsStatusResult GetStatus(string id, string? phoneNumber)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<SmsStatusResult> GetStatusAsync(string id, string? phoneNumber, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void CheckPhoneNumber(string phoneNumber, bool isAllowSendToForeignNumbers = false)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public string GetBalance()
        {
            throw new NotImplementedException();
        }
    }
}
