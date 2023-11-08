namespace Spoleto.SMS
{
    /// <summary>
    /// Options for configuring the SMS service
    /// </summary>
    public record SmsServiceOptions
    {
        /// <summary>
        /// Get or set the name of the default SMS provider.
        /// </summary>
        public string DefaultProvider { get; set; }

        /// <summary>
        /// Get or set the default ID to be used as the "From" value (phone number or another ID).
        /// </summary>
        public string DefaultFrom { get; set; }

        /// <summary>
        /// validate if the options are all set correctly
        /// </summary>
        /// <exception cref="ArgumentException">if the required options are not specified</exception>
        public void Validate()
        {
            if (String.IsNullOrWhiteSpace(DefaultProvider))
                throw new ArgumentException("You must specify a valid SMS provider to be used as the default.", nameof(DefaultProvider));
        }
    }
}
