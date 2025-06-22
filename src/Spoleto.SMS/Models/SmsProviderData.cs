namespace Spoleto.SMS
{
    /// <summary>
    /// Additional SMS provider data.
    /// </summary>
    public record struct SmsProviderData
    {
        public SmsProviderData(string name, object value)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Value = value ?? throw new ArgumentNullException(nameof(value));

            if (String.IsNullOrWhiteSpace(Name))
                throw new ArgumentException($"{nameof(Name)} in the {nameof(SmsProviderData)} cannot be empty.");
        }

        /// <summary>
        /// Gets the name of the additional data.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the value of the additional data.
        /// </summary>
        public object Value { get; }

        /// <summary>
        /// Gets the value
        /// </summary>
        public TValue GetValue<TValue>() => (TValue)Value;
    }
}
