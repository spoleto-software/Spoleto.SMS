using Spoleto.SMS.Exceptions;

namespace Spoleto.SMS.Providers
{
    public abstract class SmsProviderBase : ISmsProvider
    {
        protected const char Separator = ';';

        protected abstract List<string> LocalPrefixPhoneNumbers { get; }

        /// <inheritdoc/>
        public abstract string Name { get; }

        /// <inheritdoc/>
        public abstract bool IsAllowNullFrom { get; }

        /// <inheritdoc/>
        public abstract SmsSendingResult Send(SmsMessage message);

        /// <inheritdoc/>
        public abstract Task<SmsSendingResult> SendAsync(SmsMessage message, CancellationToken cancellationToken = default);

        /// <inheritdoc/>
        public abstract SmsStatusResult GetStatus(string id, string? phoneNumber);

        /// <inheritdoc/>
        public abstract Task<SmsStatusResult> GetStatusAsync(string id, string? phoneNumber, CancellationToken cancellationToken = default);


        protected void ValidateDataForSMS(string phoneNumber, string smsMessage, bool isAllowSendToForeignNumbers = false)
        {
            ValidatePhoneNumber(phoneNumber, isAllowSendToForeignNumbers);

            if (string.IsNullOrWhiteSpace(smsMessage))
            {
                throw new SmsBodyIsNullException("The SMS body is empty.");
            }
        }

        protected virtual void ValidatePhoneNumber(string phoneNumber, bool isAllowSendToForeignNumbers = false)
        {
            if (!isAllowSendToForeignNumbers)
            {
                if (!LocalPrefixPhoneNumbers.Any(phoneNumber.StartsWith))
                {
                    throw new ArgumentException($"The phone number {phoneNumber} is not local number.");
                }
            }
        }
    }
}
