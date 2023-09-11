using System.Text.Json.Serialization;

namespace Content.Server.AlphaCentauri.Monitoring
{
    public struct MonitoringPayload
    {
        [JsonPropertyName("embeds")]
        public List<MonitoringPayloadEmbed>? Embeds { get; set; } = null;

        public MonitoringPayload()
        { }
    }

    public struct MonitoringPayloadEmbed
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = "";

        [JsonPropertyName("title")]
        public string Title { get; set; } = "";

        [JsonPropertyName("description")]
        public string Description { get; set; } = "";

        [JsonPropertyName("color")]
        public string Color { get; set; } = "";
        [JsonPropertyName("footer")]
        public MonitoringPayloadEmbedFooter? Footer { get; set; } = null;

        public MonitoringPayloadEmbed()
        { }
    }

    public struct MonitoringPayloadEmbedFooter
    {
        [JsonPropertyName("text")]
        public string Text { get; set; } = "";

        public MonitoringPayloadEmbedFooter()
        { }
    }

    public struct MonitoringChannelMessage
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;
        [JsonPropertyName("author")]
        public MonitoringAuthorMessage Author { get; set; } = default!;
        [JsonPropertyName("content")]
        public string Content { get; set; } = string.Empty;
        [JsonPropertyName("type")]
        public int Type { get; set; } = 0;
        [JsonPropertyName("application_id")]
        public string AppId { get; set; } = string.Empty;

        public MonitoringChannelMessage()
        { }
    }

    public struct MonitoringAuthorMessage
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;
        [JsonPropertyName("bot")]
        public bool? Bot { get; set; } = null;
        public MonitoringAuthorMessage()
        { }
    }

}


