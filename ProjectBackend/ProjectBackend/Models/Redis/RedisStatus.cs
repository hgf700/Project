using System.Text.Json.Serialization;

namespace ProjectBackend.Models.Redis;

public class RedisStatus
{
    [JsonPropertyName("isConnected")]
    public bool IsConnected { get; set; }

    [JsonPropertyName("lastPing")]
    public DateTime LastPing { get; set; } = DateTime.UtcNow;

    [JsonPropertyName("streamLength")]
    public long StreamLength { get; set; }

    [JsonPropertyName("lastMessageId")]
    public string? LastMessageId { get; set; }
}