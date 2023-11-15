using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Spoleto.SMS.Converters
{
    public class DateTimeConverter : JsonConverter<DateTime>
    {
        private const string _format = "yyyy-MM-dd HH:mm:ss";
        private const string _format2 = "yyyyMMddHHmmss";

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Number)
            {
                var unixTimestamp = reader.GetInt64();

                var dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(unixTimestamp);

                var dateTime = dateTimeOffset.UtcDateTime;

                return dateTime;
            }

            if (DateTime.TryParseExact(reader.GetString(), _format, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt))
                return dt;

            if (DateTime.TryParseExact(reader.GetString(), _format2, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt2))
                return dt2;

            return DateTime.Parse(reader.GetString());
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(_format));
        }
    }
}
