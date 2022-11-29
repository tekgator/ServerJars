using System.Text.Json.Serialization;

namespace ServerJarsAPI.Models;

public class JarDetails
{
    [JsonPropertyName("version")]
    public string Version { get; set; } = string.Empty;

    [JsonPropertyName("file")]
    public string File { get; set; } = string.Empty;

    [JsonPropertyName("size")]
    public Size Size { get; set; } = new();

    [JsonPropertyName("md5")]
    public string Md5 { get; set; } = string.Empty;

    [JsonPropertyName("built")]
    //public long Built { get; set; } = 0;
    public DateTime Built { get; set; } = DateTime.MinValue;

    [JsonPropertyName("stability")]
    public string Stability { get; set; } = string.Empty;
}

public class Size
{
    [JsonPropertyName("display")]
    public string Display { get; set; } = string.Empty;

    [JsonPropertyName("bytes")]
    public uint Bytes { get; set; } = 0;
}
