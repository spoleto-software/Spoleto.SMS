namespace Spoleto.SMS.Exceptions
{
    /// <summary>
    /// The exception thrown when no SMS body is null.
    /// </summary>
    [Serializable]
    public class SmsBodyIsNullException : Exception
    {
        /// <inheritdoc/>
        public SmsBodyIsNullException(string message)
            : base(message)
        {
        }
    }
}
