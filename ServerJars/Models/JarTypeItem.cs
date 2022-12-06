namespace ServerJarsAPI.Models;

public class JarTypeItem
{
    public string Category { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;

    public override string ToString() => $"{Category}:{Type}";
}
