namespace Spoleto.SMS.Providers
{
    public abstract class SmsProviderBase : ISmsProvider
    {
        protected const char Separator = ';';

        /// <inheritdoc/>
        public abstract string Name { get; }

        /// <inheritdoc/>
        public abstract SmsSendingResult Send(SmsMessage message);

        /// <inheritdoc/>
        public abstract Task<SmsSendingResult> SendAsync(SmsMessage message, CancellationToken cancellationToken = default);

        /// <inheritdoc/>
        public abstract SmsStatusResult GetStatus(string id, string? phoneNumber);

        /// <inheritdoc/>
        public abstract Task<SmsStatusResult> GetStatusAsync(string id, string? phoneNumber, CancellationToken cancellationToken = default);
    }
}
