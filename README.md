![ServerJars](Resources/ServerJars-Logo-64px.png "ServerJars") 
ServerJars .NET API
======

Implementation of the ServerJars.com API as a .NET library.

Please see [API Documentation](https://serverjars.com/documentation) for further details


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

var serverJar = new ServerJars();

// GetTypes
var types = await serverJar.GetTypes();
Console.WriteLine(JsonSerializer.Serialize(types, jsonOptions));

// GetDetails
var details = await serverJar.GetDetails("servers", "spigot", "1.19.1");
Console.WriteLine(JsonSerializer.Serialize(details, jsonOptions));

// GetLatest
var latestDetails = await serverJar.GetLatest("servers", "spigot");
Console.WriteLine(JsonSerializer.Serialize(latestDetails, jsonOptions));

// GetAllDetails
var allDetails = await serverJar.GetAllDetails("servers", "spigot", 5u);
Console.WriteLine(JsonSerializer.Serialize(allDetails, jsonOptions));

// GetJar
using (var stream = await serverJar.GetJar("servers", "spigot"))
{
    stream.Seek(0, SeekOrigin.Begin);
    Console.WriteLine($"Downloaded {stream.Length / 1024 / 1024} MB.");
    using var fileStream = File.Create("./server.jar");
    stream.CopyTo(fileStream);
}
```

## Demo application

Have a look at the [Console Demo](ServerJars/ServerJars.Demo.Console) within the repository. 
It will run straight out of the box to give you a hint what the library can do for you.


## Support

I try to be responsive to [Stack Overflow questions in the `serverjars-net` tag](https://stackoverflow.com/questions/tagged/serverjars-net) and [issues logged on this GitHub repository](https://github.com/tekgator/ServerJars/issues). 

If I've helped you, feel free to buy me a coffee or see the Sponsor link [at the top right of the GitHub page](https://github.com/tekgator/ServerJars).

<a href="https://www.buymeacoffee.com/tekgator" target="_blank"><img src="https://www.buymeacoffee.com/assets/img/custom_images/orange_img.png" alt="Buy Me A Coffee" style="height: 41px !important;width: 174px !important;box-shadow: 0px 3px 2px 0px rgba(190, 190, 190, 0.5) !important;-webkit-box-shadow: 0px 3px 2px 0px rgba(190, 190, 190, 0.5) !important;" ></a>


## Dependencies and Credits

The project has no special dependencies except to the awesome API from the [ServerJars.com](https://serverjars.com) website. 