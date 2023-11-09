namespace Spoleto.SMS.Exceptions
{
    /// <summary>
    /// The exception thrown when no SMS is not sent successfully.
    /// </summary>
    [Serializable]
    public class SmsSendingException : Exception
    {
        /// <inheritdoc/>
        public SmsSendingException(string message)
            : base(message)
        {
        }
    }
}
