using System.Text.Json.Serialization;
using Spoleto.SMS.Converters;

namespace Spoleto.SMS
{
    /// <summary>
    /// The SMS status.
    /// </summary>
    public record SmsStatusData
    {
        /// <summary>
        ///  Gets the status info.
        /// </summary>
        [JsonPropertyName("status")]
        public string Status
        {
            get;
#if NET5_0_OR_GREATER
            init;
#else
            set;
#endif
        }

        /// <summary>
        /// Gets the status description.
        /// </summary>
        [JsonPropertyName("description")]
        public string Description
        {
            get;
#if NET5_0_OR_GREATER
            init;
#else
            set;
#endif
        }

        [JsonPropertyName("recipient")]
        public string Recipient
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
        public string UserId
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

        [JsonPropertyName("date_sent")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime DateSent
        {
            get;
#if NET5_0_OR_GREATER
            init;
#else
            set;
#endif
        }

        [JsonPropertyName("date_delivered")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime DateDelivered
        {
            get;
#if NET5_0_OR_GREATER
            init;
#else
            set;
#endif
        }

        [JsonPropertyName("message_id")]
        public string MessageId
        {
            get;
#if NET5_0_OR_GREATER
            init;
#else
            set;
#endif
        }

        [JsonPropertyName("request_id")]
        public string RequestId
        {
            get;
#if NET5_0_OR_GREATER
            init;
#else
            set;
#endif
        }

        [JsonPropertyName("count_messages")]
        public string CountMessages
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

        public override string ToString() => $"{nameof(Status)}: {Status}; {nameof(Text)}: {Text}";
    }
}
