using System.Text.Json.Serialization;

namespace ServerJarsAPI.Models;

internal class ApiError
{
    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;
}
