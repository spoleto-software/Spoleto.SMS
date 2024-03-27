using Spoleto.SMS.Exceptions;

namespace Spoleto.SMS.Providers
{
    /// <summary>
    /// The abstract SMS provider.
    /// </summary>
    /// <typeparam name="TMessage">The type of SMS used in the provider.</typeparam>
    public abstract class SmsProviderBase<TMessage> : ISmsProvider where TMessage : SmsMessage
    {
        protected abstract List<string> LocalPrefixPhoneNumbers { get; }

        /// <inheritdoc/>
        public abstract string Name { get; }

        /// <inheritdoc/>
        public abstract bool IsAllowNullFrom { get; }

        /// <inheritdoc/>
        public virtual bool CanSend(string phoneNumber, bool isAllowSendToForeignNumbers = false)
        {
            if (!isAllowSendToForeignNumbers)
            {
                phoneNumber = CleanPhoneNumber(phoneNumber);
                if (!LocalPrefixPhoneNumbers.Any(phoneNumber.StartsWith))
                {
                    return false;
                }
            }

            return true;    
        }

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
            if (string.IsNullOrWhiteSpace(phoneNumber))
                throw new ArgumentNullException(nameof(phoneNumber));

            if (!CanSend(phoneNumber, isAllowSendToForeignNumbers))
            {
                throw new ArgumentException($"The phone number {phoneNumber} is not local number for the SMS provider {Name}.");
            }
        }

        protected static string CleanPhoneNumber(string phoneNumber)
        {
            phoneNumber = phoneNumber.Replace("(", "").Replace(")", "").Replace(" ", "").Replace("-", "").Replace("+", "");
            
            return phoneNumber;
        }

        protected virtual TMessage CreateMessage(SmsMessage originalMessage)
            => originalMessage is TMessage message
            ? message
            : throw new NotSupportedException($"The message type '{originalMessage?.GetType().Name}' is not supported in the SMS provider '{GetType().Name}");
    }
}
