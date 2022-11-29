using System.Text.Json.Serialization;

namespace ServerJarsAPI.Models;

public class JarTypes
{
    [JsonPropertyName("bedrock")]
    public List<string> Bedrock { get; set; } = new();

    [JsonPropertyName("modded")]
    public List<string> Modded { get; set; } = new();

    [JsonPropertyName("proxies")]
    public List<string> Proxies { get; set; } = new();

    [JsonPropertyName("servers")]
    public List<string> Servers { get; set; } = new();

    [JsonPropertyName("vanilla")]
    public List<string> Vanilla { get; set; } = new();
}