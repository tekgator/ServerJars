namespace ServerJarsAPI.Models;

internal class ApiResponse<T>
{
    public string Status { get; set; } = string.Empty;

    public T? Response { get; set; }

    public bool IsSuccess => Status == "success" && Response is not null;
}
