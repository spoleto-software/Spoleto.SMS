namespace Spoleto.SMS
{
    /// <summary>
    /// SMS sending errors.
    /// </summary>
    public record SmsSendingError
    {
        /// <summary>
        /// Get the error code.
        /// </summary>
        public string Code { get; }

        /// <summary>
        /// Get the error message.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Get the numeric error code.
        /// </summary>
        public int NumCode { get; }
    }
}