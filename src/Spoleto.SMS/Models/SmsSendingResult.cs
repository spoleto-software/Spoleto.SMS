namespace Spoleto.SMS
{
    /// <summary>
    /// The SMS sending result
    /// </summary>
    public record SmsSendingResult
    {
        /// <summary>
        /// Get if the SMS has been sent successfully.
        /// </summary>
        public bool Success { get; init; }

        /// <summary>
        /// Get the name of the provider used to send the SMS.
        /// </summary>
        public string ProviderName { get; init; }

        public IEnumerable<SmdSendingData> SmsSendingData { get; init; }

        /// <summary>
        /// Get the errors associated with the sending failure.
        /// </summary>
        public IEnumerable<SmsSendingError> Errors { get; init; }
    }
}
