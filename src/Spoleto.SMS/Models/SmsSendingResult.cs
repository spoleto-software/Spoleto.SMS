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
        public bool Success { get; }

        /// <summary>
        /// Get the name of the provider used to send the SMS.
        /// </summary>
        public string ProviderName { get; }

        /// <summary>
        /// Get the errors associated with the sending failure.
        /// </summary>
        public IEnumerable<SmsSendingError> Errors { get; }
    }
}
