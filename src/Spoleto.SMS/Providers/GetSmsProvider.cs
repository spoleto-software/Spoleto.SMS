namespace Spoleto.SMS.Providers
{
    /// <summary>
    /// The GetSMS provider for sending SMS messages.
    /// </summary>
    /// <remarks>
    /// <see href="https://getsms.uz/page/index/16"/>.
    /// </remarks>
    public class GetSmsProvider : ISmsProvider
    {
        private const string ProviderName = nameof(SmsProviderName.GetSMS);

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
    }
}
