using System.Text.Json.Serialization;

namespace ServerJarsAPI.Models;

internal class ApiResponse<T>
{
    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("response")]
    public T? Response { get; set; }

    public bool IsSuccess => Status == "success" && Response is not null;
}
