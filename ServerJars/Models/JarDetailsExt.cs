namespace ServerJarsAPI.Models;

public class JarDetailsExt : JarDetails
{
    public JarDetailsExt(JarDetails jarDetails)
    {
        Version = jarDetails.Version;
        File = jarDetails.File;
        Size = jarDetails.Size;
        Md5 = jarDetails.Md5;
        Built = jarDetails.Built;
        Stability = jarDetails.Stability;
    }

    public string Type { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string DownloadUrl { get; set; } = string.Empty;
}