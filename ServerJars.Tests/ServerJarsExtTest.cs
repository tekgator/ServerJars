using System.Text.Json;

namespace ServerJarsAPI.Tests;

public class ServerJarsExtTest
{
    private readonly ServerJarsExt _serverJarsExt = new();

    [SetUp]
    public void Setup()
    { }

    [TestCase("")]
    [TestCase("servers")]
    public async Task GetTypes_Success(string type)
    {
        var types = await _serverJarsExt.GetTypes(type);
        Assert.That(types.ContainsKey("servers"), Is.True);
    }

    [TestCase("abc")]
    public void GetTypes_InvalidType(string type)
    {
        Assert.ThrowsAsync<JsonException>(async () => await _serverJarsExt.GetTypes(type));
    }

    [TestCase("servers", "spigot", "")]
    [TestCase("servers", "spigot", "1.19.2")]
    public async Task GetDetails_Success(string type, string category, string version)
    {
        var details = await _serverJarsExt.GetDetails(type, category, version);
        Assert.That(details, Is.Not.Null);
        Assert.That(details.DownloadUrl, Is.Not.Empty);
    }

    [TestCase("servers", "spigot")]
    [TestCase("vanilla", "vanilla")]
    public async Task GetLatest_Success(string type, string category)
    {
        var details = await _serverJarsExt.GetLatest(type, category);
        Assert.That(details, Is.Not.Null);
        Assert.That(details.DownloadUrl, Is.Not.Empty);
    }

    [TestCase("servers", "spigot", "5")]
    [TestCase("vanilla", "vanilla", "3")]
    public async Task GetAllDetails_Success(string type, string category, uint? max)
    {
        var details = await _serverJarsExt.GetAllDetails(type, category, max);
        Assert.That(details, Is.Not.Null);
        Assert.That(details.ToList(), Has.Count.AtLeast(3));
        foreach (var detail in details)
        {
            Assert.That(detail.DownloadUrl, Is.Not.Empty);
        }
    }
}