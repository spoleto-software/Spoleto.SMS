namespace Spoleto.SMS.Exceptions
{
    /// <summary>
    /// The exception thrown when no SMS provider has been found.
    /// </summary>
    [Serializable]
    public class SmsProviderNotFoundException : Exception
    {
        private static readonly string _exceptionMessage = $"There is no SMS provider with the name <{{0}}>.{Environment.NewLine}Make sure you have registered this provider in the SMS service.";

        /// <summary>
        /// Gets the SMS provider name.
        /// </summary>
        public string SmsProviderName { get; }

        /// <inheritdoc/>
        public SmsProviderNotFoundException(string providerName)
            : this(string.Format(_exceptionMessage, providerName), providerName) { }

        /// <inheritdoc/>
        public SmsProviderNotFoundException(string message, string providerName)
            : base(message)
        {
            SmsProviderName = providerName;
        }
    }
}
