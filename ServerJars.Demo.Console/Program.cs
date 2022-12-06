// See https://aka.ms/new-console-template for more information
using ServerJarsAPI;
using ServerJarsAPI.Events;
using ServerJarsAPI.Extensions;
using System.Text.Json;

var jsonOptions = new JsonSerializerOptions
{
    WriteIndented = true,
};


var serverJar = new ServerJars();


// GetTypes
SetConsoleColor(ConsoleColor.White, ConsoleColor.Red);
Console.WriteLine("\nAPI call - GetTypes:\n");
ResetConsoleColor();

var types = await serverJar.GetTypes();
Console.WriteLine(JsonSerializer.Serialize(types, jsonOptions));

// GetTypes.AsDictionary() extension
SetConsoleColor(ConsoleColor.White, ConsoleColor.Red);
Console.WriteLine("\nAPI call - GetTypes.AsDictionary() extension:\n");
ResetConsoleColor();

var dict = types.AsDictionary();
Console.WriteLine(string.Join(Environment.NewLine, dict.Select((kv) => $"{kv.Key}: {string.Join(", ", kv.Value)}")));


// GetDetails
SetConsoleColor(ConsoleColor.White, ConsoleColor.Red);
Console.WriteLine("\nAPI call - GetDetails:\n");
ResetConsoleColor();

var details = await serverJar.GetDetails("servers", "spigot", "1.19.1");
Console.WriteLine(JsonSerializer.Serialize(details, jsonOptions));


// GetLatest
SetConsoleColor(ConsoleColor.White, ConsoleColor.Red);
Console.WriteLine("\nAPI call - GetLatestDetails:\n");
ResetConsoleColor();

var latestDetails = await serverJar.GetLatest("servers", "spigot");
Console.WriteLine(JsonSerializer.Serialize(latestDetails, jsonOptions));


// GetAllDetails
SetConsoleColor(ConsoleColor.White, ConsoleColor.Red);
Console.WriteLine("\nAPI call - GetAllDetails:\n");
ResetConsoleColor();

var allDetails = await serverJar.GetAllDetails("servers", "spigot", 5u);
Console.WriteLine(JsonSerializer.Serialize(allDetails, jsonOptions));


// GetJar Method 1 (including progress)
SetConsoleColor(ConsoleColor.White, ConsoleColor.Red);
Console.WriteLine("\nAPI call - GetJar with method 1 (with progress):\n");
ResetConsoleColor();


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
SetConsoleColor(ConsoleColor.White, ConsoleColor.Red);
Console.WriteLine("\nAPI call - GetJar method 2:\n");
ResetConsoleColor();

using (var stream = await serverJar.GetJar("servers", "spigot", "1.19.1"))
{
    using var fileStream2 = File.Create("./server2.jar");
    await stream.CopyToAsync(fileStream2);
    Console.WriteLine($"Downloaded {fileStream2.Length / 1024 / 1024}MB to {fileStream2.Name}");
}


static void SetConsoleColor(ConsoleColor foregroundColor, ConsoleColor? backgroundColor = null)
{
    Console.ForegroundColor = foregroundColor;
    if (backgroundColor is not null)
    {
        Console.BackgroundColor = (ConsoleColor)backgroundColor;
    }
}

static void ResetConsoleColor()
{
    Console.ForegroundColor = ConsoleColor.Gray;
    Console.BackgroundColor = ConsoleColor.Black;
}