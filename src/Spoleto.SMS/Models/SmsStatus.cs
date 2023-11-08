namespace Spoleto.SMS
{
    /// <summary>
    /// SMS status.
    /// </summary>
    public record SmsStatus
    {
        /// <summary>
        ///  Get the status info.
        /// </summary>
        public string Status { get; }

        /// <summary>
        /// Get the status description.
        /// </summary>
        public string Description { get; }
    }
}
