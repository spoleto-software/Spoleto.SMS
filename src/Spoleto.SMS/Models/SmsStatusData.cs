using System.Text.Json.Serialization;
using Spoleto.SMS.Converters;

namespace Spoleto.SMS
{
    /// <summary>
    /// SMS status.
    /// </summary>
    public record SmsStatusData
    {
        /// <summary>
        ///  Get the status info.
        /// </summary>
        [JsonPropertyName("status")]
        public string Status { get; init; }

        /// <summary>
        /// Get the status description.
        /// </summary>
        [JsonPropertyName("description")]
        public string Description { get; init; }

        [JsonPropertyName("recipient")]
        public string Recipient { get; init; }

        [JsonPropertyName("text")]
        public string Text { get; init; }

        [JsonPropertyName("user_id")]
        public string UserId { get; init; }

        [JsonPropertyName("date_received")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime DateReceived { get; init; }

        [JsonPropertyName("date_sent")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime DateSent { get; init; }

        [JsonPropertyName("date_delivered")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime DateDelivered { get; init; }

        [JsonPropertyName("message_id")]
        public string MessageId { get; init; }

        [JsonPropertyName("request_id")]
        public string RequestId { get; init; }

        [JsonPropertyName("count_messages")]
        public string CountMessages { get; init; }

        [JsonPropertyName("client_ip")]
        public string ClientIp { get; init; }
    }
}
