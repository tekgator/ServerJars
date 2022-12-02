using ServerJarsAPI.Events;
using ServerJarsAPI.Models;

namespace ServerJarsAPI;

/// <summary>
/// Implementation of the serverjars.com API
/// <br/>Documentation <seealso cref="https://serverjars.com/documentation"/>
/// </summary>
public class ServerJars : ClientApi
{
    public ServerJars() : base("https://serverjars.com/api/")
    {

    }

    public Task<JarTypes> GetTypes(
        string type = "",
        CancellationToken cancellationToken = default)
    {
        return GetAsync<JarTypes>($"fetchTypes/{type}", cancellationToken);
    }

    public Task<JarDetails> GetDetails(
        string type,
        string category,
        string version = "",
        CancellationToken cancellationToken = default)
    {
        return GetAsync<JarDetails>($"fetchDetails/{type}/{category}/{version}", cancellationToken);
    }

    public Task<JarDetails> GetLatest(
        string type,
        string category,
        CancellationToken cancellationToken = default)
    {
        return GetAsync<JarDetails>($"fetchLatest/{type}/{category}", cancellationToken);
    }

    public Task<IEnumerable<JarDetails>> GetAllDetails(
        string type,
        string category,
        uint? max = null,
        CancellationToken cancellationToken = default)
    {
        string maxResult = max is null ? string.Empty : max.ToString()!;
        return GetAsync<IEnumerable<JarDetails>>($"fetchAll/{type}/{category}/{maxResult}", cancellationToken);
    }

    public Task<Stream> GetJar(
        string type,
        string category,
        string version = "",
        CancellationToken cancellationToken = default)
    {
        return StreamAsync($"fetchJar/{type}/{category}/{version}", cancellationToken);
    }

    public Task GetJar(
        Stream stream,
        string type,
        string category,
        string version = "",
        IProgress<ProgressEventArgs>? progress = null,
        CancellationToken cancellationToken = default)
    {
        return DownloadAsync(stream, $"fetchJar/{type}/{category}/{version}", progress, cancellationToken);
    }

}