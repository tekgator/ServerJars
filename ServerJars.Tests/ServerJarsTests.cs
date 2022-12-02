using ServerJarsAPI.Events;
using System.Text.Json;

namespace ServerJarsAPI.Tests;

public class ServerJarsTests
{
    private readonly ServerJars _serverJars = new();

    [SetUp]
    public void Setup()
    {

    }

    [TestCase("")]
    [TestCase("servers")]
    public async Task GetTypes_Success(string type)
    {
        var types = await _serverJars.GetTypes(type);
        Assert.That(types.Servers, Is.Not.Empty);
    }

    [TestCase("abc")]
    public void GetTypes_InvalidType(string type)
    {
        Assert.ThrowsAsync<JsonException>(async () => await _serverJars.GetTypes(type));
    }

    [TestCase("servers", "spigot", "")]
    [TestCase("servers", "spigot", "1.19.2")]
    public async Task GetDetails_Success(string type, string category, string version)
    {
        var details = await _serverJars.GetDetails(type, category, version);
        Assert.That(details, Is.Not.Null);
    }

    [TestCase("servers", "spigot", "1.19.1")]
    public async Task GetDetails_SuccessCorrectVersion(string type, string category, string version)
    {
        var details = await _serverJars.GetDetails(type, category, version);
        Assert.That(details.Version, Is.EqualTo("1.19.1"));
    }

    [TestCase("abc", "spigot", "")]
    [TestCase("servers", "abc", "")]
    [TestCase("servers", "spigot", "1.19.110")]
    public void GetDetails_InvalidTypeCategoryVersion(string type, string category, string version)
    {
        Assert.ThrowsAsync<HttpRequestException>(async () => await _serverJars.GetDetails(type, category, version));
    }

    [TestCase("servers", "spigot")]
    [TestCase("vanilla", "vanilla")]
    public async Task GetLatest_Success(string type, string category)
    {
        var details = await _serverJars.GetLatest(type, category);
        Assert.That(details, Is.Not.Null);
    }

    [TestCase("abc", "spigot")]
    [TestCase("servers", "abc")]
    public void GetLatest_InvalidTypeCategory(string type, string category)
    {
        Assert.ThrowsAsync<HttpRequestException>(async () => await _serverJars.GetLatest(type, category));
    }

    [TestCase("servers", "spigot", "5")]
    [TestCase("vanilla", "vanilla", "3")]
    public async Task GetAllDetails_Success(string type, string category, uint? max)
    {
        var details = await _serverJars.GetAllDetails(type, category, max);
        Assert.That(details, Is.Not.Null);
        Assert.That(details.ToList(), Has.Count.AtLeast(3));
    }

    [TestCase("abc", "spigot")]
    [TestCase("servers", "abc")]
    public void GetAllDetails_InvalidTypeCategory(string type, string category)
    {
        Assert.ThrowsAsync<HttpRequestException>(async () => await _serverJars.GetAllDetails(type, category));
    }

    [TestCase("servers", "spigot", "")]
    [TestCase("servers", "spigot", "1.19.1")]
    public async Task GetJar_Success(string type, string category, string version)
    {
        using var stream = await _serverJars.GetJar(type, category, version);
        Assert.That(stream.CanRead, Is.True);

        using var memStream = new MemoryStream();
        await stream.CopyToAsync(memStream);

        Assert.That(memStream.Length, Is.GreaterThan(0));
    }

    [TestCase("abc", "spigot", "")]
    [TestCase("servers", "abc", "")]
    [TestCase("servers", "spigot", "1.19.110")]
    public void GetJar_InvalidTypeCategoryVersion(string type, string category, string version)
    {
        Assert.ThrowsAsync<HttpRequestException>(async () => await _serverJars.GetJar(type, category, version));
    }


    [TestCase("servers", "spigot", "")]
    [TestCase("servers", "spigot", "1.19.1")]
    public async Task GetJarWithProgress_Success(string type, string category, string version)
    {
        bool progressChanged = false;
        using var memStream = new MemoryStream();
        Progress<ProgressEventArgs> progress = new();
        progress.ProgressChanged += (_, progress) =>
        {
            Assert.Multiple(() =>
            {
                Assert.That(progress.ProgressPercentage, Is.AtLeast(0));
                Assert.That(progress.ProgressPercentage, Is.AtMost(100));
            });
            progressChanged = true;
        };

        await _serverJars.GetJar(memStream, type, category, version, progress);
        Assert.That(progressChanged, Is.True);
    }

    [TestCase("abc", "spigot", "")]
    [TestCase("servers", "abc", "")]
    [TestCase("servers", "spigot", "1.19.110")]
    public void GetJarWithProgress_InvalidTypeCategoryVersion(string type, string category, string version)
    {
        using var memStream = new MemoryStream();
        Progress<ProgressEventArgs> progress = new();
        progress.ProgressChanged += (_, progress) =>
        {
            Assert.Multiple(() =>
            {
                Assert.That(progress.ProgressPercentage, Is.AtLeast(0));
                Assert.That(progress.ProgressPercentage, Is.AtMost(100));
            });
        };

        Assert.ThrowsAsync<HttpRequestException>(async () => await _serverJars.GetJar(memStream, type, category, version, progress));
    }

    [TestCase("servers", "spigot", "")]
    [TestCase("servers", "spigot", "1.19.1")]
    public async Task GetJarWithNullProgress_Success(string type, string category, string version)
    {
        using var memStream = new MemoryStream();
        await _serverJars.GetJar(memStream, type, category, version);
        Assert.That(memStream.Length, Is.GreaterThan(0));
    }
}