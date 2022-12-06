namespace ServerJarsAPI.Models;

public class JarDetails
{
    public string Version { get; set; } = string.Empty;

    public string File { get; set; } = string.Empty;

    public Size Size { get; set; } = new();

    public string Md5 { get; set; } = string.Empty;

    public DateTime Built { get; set; } = DateTime.MinValue;

    public string Stability { get; set; } = string.Empty;
}

public class Size
{
    public string Display { get; set; } = string.Empty;

    public uint Bytes { get; set; } = 0;
}
