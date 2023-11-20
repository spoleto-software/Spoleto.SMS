using System.Text.Json.Serialization;
using Spoleto.SMS.Converters;

namespace Spoleto.SMS
{
    /// <summary>
    /// SMS sending errors.
    /// </summary>
    public record SmsSendingError
    {
        /// <summary>
        /// Gets the error code.
        /// </summary>
        public string Code
        {
            get;
#if NET5_0_OR_GREATER
            init;
#else
            set;
#endif
        }

        /// <summary>
        /// Gets the error message.
        /// </summary>
        [JsonPropertyName("error_text")]
        public string Message
        {
            get;
#if NET5_0_OR_GREATER
            init;
#else
            set;
#endif
        }

        /// <summary>
        /// Gets the numeric error code.
        /// </summary>
        [JsonPropertyName("error_no")]
        public int NumCode
        {
            get;
#if NET5_0_OR_GREATER
            init;
#else
            set;
#endif
        }

        [JsonPropertyName("error")]
        public int Error { get; set; }

        [JsonPropertyName("recipient")]
        public string Recipient { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; }

        [JsonPropertyName("user_id")]
        public int UserId { get; set; }

        [JsonPropertyName("date_received")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime DateReceived { get; set; }

        [JsonPropertyName("message_id")]
        public int MessageId { get; set; }

        [JsonPropertyName("request_id")]
        public int RequestId { get; set; }

        [JsonPropertyName("client_ip")]
        public string ClientIp { get; set; }
    }
}