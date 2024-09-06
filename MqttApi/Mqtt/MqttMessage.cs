using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Globalization;

namespace MqttApi.Mqtt
{
    public class Message
    {
        [JsonProperty("MessageId")]
        public long MessageId { get; set; }

        [JsonProperty("Topic")]
        public string Topic { get; set; }

        [JsonProperty("From")]
        public string From { get; set; }

        [JsonProperty("To")]
        public string? To { get; set; }

        [JsonProperty("MessageBody")]
        public string MessageBody { get; set; }

        public static Message FromJson(string json) => JsonConvert.DeserializeObject<Message>(json, Converter.Settings);

    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}
