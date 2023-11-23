namespace Spoleto.SMS
{
    /// <summary>
    /// The options for configuring the SMS service
    /// </summary>
    public record SmsServiceOptions
    {
        /// <summary>
        /// Gets or sets the default ID to be used as the "From" value (phone number or another ID).
        /// </summary>
        public string DefaultFrom { get; set; }

        /// <summary>
        /// Gets or sets the name of the default SMS provider.
        /// </summary>
        public string DefaultProvider { get; set; }

        /// <summary>
        /// Checks that all the settings within the options are configured properly.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown when <see cref="DefaultProvider"/> is not specified.</exception>
        public void Validate()
        {
            if (String.IsNullOrWhiteSpace(DefaultProvider))
                throw new ArgumentException("You have to specify a valid SMS provider to be used as the default.", nameof(DefaultProvider));
        }
    }
}
