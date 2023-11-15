using System.Text.Json.Serialization;
using Spoleto.SMS.Converters;

namespace Spoleto.SMS
{
    public record SmdSendingData
    {
        [JsonPropertyName("recipient")]
        public long Recipient { get; init; }

        [JsonPropertyName("text")]
        public string Text { get; init; }

        [JsonPropertyName("user_id")]
        public int UserId { get; init; }

        [JsonPropertyName("date_received")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime DateReceived { get; init; }

        [JsonPropertyName("message_id")]
        public int MessageId { get; init; }

        [JsonPropertyName("request_id")]
        public int RequestId { get; init; }

        [JsonPropertyName("client_ip")]
        public string ClientIp { get; init; }
    }
}