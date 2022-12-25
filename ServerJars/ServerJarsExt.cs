using ServerJarsAPI.Extensions;
using ServerJarsAPI.Models;

namespace ServerJarsAPI;

/// <summary>
/// Implementation of the serverjars.com API
/// <br/>Documentation <seealso cref="https://serverjars.com/documentation"/>
/// </summary>
public class ServerJarsExt : ServerJars
{
    public ServerJarsExt() :
        this(new HttpClient())
    { }

    public ServerJarsExt(HttpClient httpClient, bool disposeClient = true) :
        base(httpClient, disposeClient)
    { }

    public new async Task<Dictionary<string, IEnumerable<string>>> GetTypes(
        string type = "",
        CancellationToken cancellationToken = default)
    {
        return (await base.GetTypes(type, cancellationToken)).AsDictionary();
    }

    public new async Task<JarDetailsExt> GetDetails(
        string type,
        string category,
        string version = "",
        CancellationToken cancellationToken = default)
    {
        return GetJarDetailsExt(await base.GetDetails(type, category, version, cancellationToken), type, category);
    }

    public new async Task<JarDetailsExt> GetLatest(
        string type,
        string category,
        CancellationToken cancellationToken = default)
    {
        return GetJarDetailsExt(await base.GetLatest(type, category, cancellationToken), type, category);
    }

    public new async Task<IEnumerable<JarDetailsExt>> GetAllDetails(
        string type,
        string category,
        uint? max = null,
        CancellationToken cancellationToken = default)
    {
        var jarDetails = await base.GetAllDetails(type, category, max, cancellationToken);
        return jarDetails.Select(jarDetail => GetJarDetailsExt(jarDetail, type, category));
    }

    private JarDetailsExt GetJarDetailsExt(
        JarDetails jarDetail,
        string type,
        string category)
    {
        return new(jarDetail)
        {
            Type = type,
            Category = category,
            DownloadUrl = $"{_httpClient.BaseAddress}fetchJar/{type}/{category}/{jarDetail.Version}"
        };
    }
}