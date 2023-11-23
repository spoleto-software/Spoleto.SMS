namespace Spoleto.SMS
{
    /// <summary>
    /// The SMS status result.
    /// </summary>
    public record SmsStatusResult
    {
        /// <summary>
        /// Gets if the SMS has been sent successfully.
        /// </summary>
        public bool Success
        {
            get;
#if NET5_0_OR_GREATER
            init;
#else
            set;
#endif
        }

        /// <summary>
        /// Gets the name of the provider used to send the SMS.
        /// </summary>
        public string ProviderName
        {
            get;
#if NET5_0_OR_GREATER
            init;
#else
            set;
#endif
        }

        /// <summary>
        /// Gets the status info.
        /// </summary>
        public IEnumerable<SmsStatusData> SmsStatusData
        {
            get;
#if NET5_0_OR_GREATER
            init;
#else
            set;
#endif
        }

        /// <summary>
        /// Gets the errors.
        /// </summary>
        public IEnumerable<SmsSendingError> Errors
        {
            get;
#if NET5_0_OR_GREATER
            init;
#else
            set;
#endif
        }
    }
}
