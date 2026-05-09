using System.Text.Json;
using System.Text.Json.Serialization;

namespace backend.Converters
{
    public class UtcDateTimeJsonConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                var dateString = reader.GetString();
                if (DateTime.TryParse(dateString, out var dateTime))
                {
                    // If the datetime is in local time, convert to UTC
                    if (dateTime.Kind == DateTimeKind.Unspecified)
                    {
                        return DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
                    }
                    return dateTime;
                }
            }
            throw new JsonException("Unable to parse DateTime value");
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            // Always write as UTC with Z suffix
            var utcValue = value.Kind == DateTimeKind.Local 
                ? value.ToUniversalTime() 
                : value;
            
            writer.WriteStringValue(utcValue.ToString("yyyy-MM-ddTHH:mm:ssZ"));
        }
    }
}
