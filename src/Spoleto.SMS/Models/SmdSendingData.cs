using System.Text.Json.Serialization;
using Spoleto.SMS.Converters;

namespace Spoleto.SMS
{
    /// <summary>
    /// The SMS sending data.
    /// </summary>
    public record SmdSendingData
    {
        [JsonPropertyName("recipient")]
        public long Recipient
        {
            get;
#if NET5_0_OR_GREATER
            init;
#else
            set;
#endif
        }

        [JsonPropertyName("text")]
        public string Text
        {
            get;
#if NET5_0_OR_GREATER
            init;
#else
            set;
#endif
        }

        [JsonPropertyName("user_id")]
        public int UserId
        {
            get;
#if NET5_0_OR_GREATER
            init;
#else
            set;
#endif
        }

        [JsonPropertyName("date_received")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime DateReceived
        {
            get;
#if NET5_0_OR_GREATER
            init;
#else
            set;
#endif
        }

        [JsonPropertyName("message_id")]
        public long MessageId
        {
            get;
#if NET5_0_OR_GREATER
            init;
#else
            set;
#endif
        }

        [JsonPropertyName("request_id")]
        public int RequestId
        {
            get;
#if NET5_0_OR_GREATER
            init;
#else
            set;
#endif
        }

        [JsonPropertyName("client_ip")]
        public string ClientIp
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