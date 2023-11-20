namespace Spoleto.SMS
{
    /// <summary>
    /// The SMS sending result
    /// </summary>
    public record SmsSendingResult
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

        public IEnumerable<SmdSendingData> SmsSendingData
        {
            get;
#if NET5_0_OR_GREATER
            init;
#else
            set;
#endif
        }

        /// <summary>
        /// Gets the errors associated with the sending failure.
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
