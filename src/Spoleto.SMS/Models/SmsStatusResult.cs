namespace Spoleto.SMS
{
    /// <summary>
    /// SMS status result.
    /// </summary>
    public record SmsStatusResult
    {
        /// <summary>
        /// Get if the SMS has been sent successfully.
        /// </summary>
        public bool Success { get; init; }

        /// <summary>
        /// Get the name of the provider used to send the SMS.
        /// </summary>
        public string ProviderName { get; init; }

        /// <summary>
        /// Get the status info.
        /// </summary>
        public IEnumerable<SmsStatusData> SmsStatusData { get; init; }

        /// <summary>
        /// Get the errors.
        /// </summary>
        public IEnumerable<SmsSendingError> Errors { get; init; }
    }
}
