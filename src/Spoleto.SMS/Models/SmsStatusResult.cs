namespace Spoleto.SMS
{
    /// <summary>
    /// SMS status result.
    /// </summary>
    public record SmsStatusResult
    {
        /// <summary>
        /// Get the status info.
        /// </summary>
        public IEnumerable<SmsStatus> SmsStatuses { get; }

        /// <summary>
        /// Get the errors.
        /// </summary>
        public IEnumerable<SmsSendingError> Errors { get; }
    }
}
