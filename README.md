![ServerJars](Resources/ServerJars-Logo-64px.png "ServerJars") 
ServerJars .NET API
======

<p>
  <a href="https://github.com/tekgator/ServerJars/blob/main/LICENSE" alt="License">
    <img src="https://img.shields.io/github/license/tekgator/ServerJars" />
  </a>
  <img src="https://img.shields.io/github/languages/top/tekgator/ServerJars" />
  <a href="https://www.nuget.org/packages/ServerJars" alt="Nuget">
    <img alt="Nuget" src="https://img.shields.io/nuget/dt/ServerJars">
  </a>
  <a href="https://github.com/tekgator/ServerJars/actions/workflows/build-on-push.yml" alt="BuildStatus">
    <img src="https://img.shields.io/github/actions/workflow/status/tekgator/ServerJars/build-on-push.yml?branch=main" />
  </a>
  <a href="https://github.com/tekgator/ServerJars/releases" alt="Releases">
    <img src="https://img.shields.io/github/v/release/tekgator/ServerJars" />
  </a>
  <a href="https://github.com/tekgator/ServerJars/releases" alt="Releases">
    <img alt="GitHub Release Date" src="https://img.shields.io/github/release-date/tekgator/ServerJars">
  </a>
  <a href="https://github.com/tekgator/ServerJars/commit" alt="Commit">
    <img alt="GitHub last commit" src="https://img.shields.io/github/last-commit/tekgator/ServerJars">
  </a>
</p>

Implementation of the ServerJars.com API as a .NET library.

Please see [API Documentation](https://serverjars.com/documentation) for further details

## Support

I try to be responsive to [Stack Overflow questions in the `serverjars-net` tag](https://stackoverflow.com/questions/tagged/serverjars-net) and [issues logged on this GitHub repository](https://github.com/tekgator/ServerJars/issues).

If I've helped you and you like some of my work, feel free to buy me a coffee ☕ (or more likely a beer 🍺)

[![ko-fi](https://ko-fi.com/img/githubbutton_sm.svg)](https://ko-fi.com/C0C7LO3V1)


## Installing

Multiple options are available to install within your project:

1. Install, using the [Nuget Gallery](https://www.nuget.org/packages/ServerJars)

2. Install using the Package Manager Console: 
   ```ps
   Install-Package ServerJars 
   ```
3. Install using .NET CLI
   ```cmd
   dotnet add package ServerJars
   ```


## Usage

Simply instantiate the `ServerJars` class and use it's methods to gather information from the API.

```CSharp
using ServerJarsAPI;
using ServerJarsAPI.Events;
using ServerJarsAPI.Extensions;
using System.Text.Json;

var serverJar = new ServerJars();

// GetTypes
var types = await serverJar.GetTypes();
Console.WriteLine(JsonSerializer.Serialize(types, jsonOptions));

// GetTypes.AsDictionary() extension
var dict = types.AsDictionary();
Console.WriteLine(string.Join(Environment.NewLine, dict.Select((kv) => $"{kv.Key}: {string.Join(", ", kv.Value)}")));

// GetDetails
var details = await serverJar.GetDetails("servers", "spigot", "1.19.1");
Console.WriteLine(JsonSerializer.Serialize(details, jsonOptions));

// GetLatest
var latestDetails = await serverJar.GetLatest("servers", "spigot");
Console.WriteLine(JsonSerializer.Serialize(latestDetails, jsonOptions));

// GetAllDetails
var allDetails = await serverJar.GetAllDetails("servers", "spigot", 5u);
Console.WriteLine(JsonSerializer.Serialize(allDetails, jsonOptions));

// GetJar Method 1 (including progress)
using var fileStream1 = File.Create("./server1.jar");
Progress<ProgressEventArgs> progress = new();
progress.ProgressChanged += (_, e) =>
{
    Console.Write($"\rProgress: {e.ProgressPercentage}% ({e.BytesTransferred / 1024 / 1024}MB / {e.TotalBytes / 1024 / 1024}MB)          ");
};
await serverJar.GetJar(fileStream1, "servers", "spigot", progress: progress);
await fileStream1.FlushAsync();
Console.WriteLine($"\nDownloaded {fileStream1.Length / 1024 / 1024}MB to {fileStream1.Name}");

// GetJar Method 2
using (var stream = await serverJar.GetJar("servers", "spigot", "1.19.1"))
{
    using var fileStream2 = File.Create("./server2.jar");
    await stream.CopyToAsync(fileStream2);
    Console.WriteLine($"Downloaded {fileStream2.Length / 1024 / 1024}MB to {fileStream2.Name}");
}
```

### ServerJarExt

`ServerJarExt` is an extended version of the API e.g. providing the `JarTypes` directly as Dictionary and adds a few more information to the [JarDetails](ServerJars/Models/JarDetails.cs) in an [JarDetailsExt](ServerJars/Models/JarDetailsExt.cs) Version.


## Demo application

Have a look at the [Console Demo](ServerJars.Demo.Console/Program.cs) within the repository. 
It will run straight out of the box to give you a hint what the library can do for you.


## Dependencies and Credits

The project has no special dependencies except to the awesome API from the [ServerJars.com](https://serverjars.com) website. 