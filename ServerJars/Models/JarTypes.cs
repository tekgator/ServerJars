namespace ServerJarsAPI.Models;

public class JarTypes
{
    public List<string> Bedrock { get; set; } = new();

    public List<string> Modded { get; set; } = new();

    public List<string> Proxies { get; set; } = new();

    public List<string> Servers { get; set; } = new();

    public List<string> Vanilla { get; set; } = new();
}