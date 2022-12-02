// See https://aka.ms/new-console-template for more information
using ServerJarsAPI;
using ServerJarsAPI.Events;
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
progress.ProgressChanged += (_, progress) => Console.Write($"\rProgress: {progress.ProgressPercentage}% ({progress.BytesTransferred / 1024 / 1024}MB / {progress.TotalBytes / 1024 / 1024}MB)          ");
await serverJar.GetJar(fileStream1, "servers", "spigot", progress: progress);


// GetJar Method 2
SetConsoleColor(ConsoleColor.White, ConsoleColor.Red);
Console.WriteLine("\n\nAPI call - GetJar method 2:\n");
ResetConsoleColor();

using (var stream = await serverJar.GetJar("servers", "spigot", "1.19.1"))
{
    using var fileStream2 = File.Create("./server2.jar");
    await stream.CopyToAsync(fileStream2);
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