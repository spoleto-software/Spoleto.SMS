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
        /// Get the status info.
        /// </summary>
        public IEnumerable<SmsStatus> SmsStatuses { get; init; }

        /// <summary>
        /// Get the errors.
        /// </summary>
        public IEnumerable<SmsSendingError> Errors { get; init; }
    }
}
