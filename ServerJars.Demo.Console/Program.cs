// See https://aka.ms/new-console-template for more information
using ServerJarsAPI;
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


// GetJar
SetConsoleColor(ConsoleColor.White, ConsoleColor.Red);
Console.WriteLine("\nAPI call - GetJar:\n");
ResetConsoleColor();

using (var stream = await serverJar.GetJar("servers", "spigot"))
{
    stream.Seek(0, SeekOrigin.Begin);
    Console.WriteLine($"Downloaded {stream.Length / 1024 / 1024} MB.");
    using var fileStream = File.Create("./server.jar");
    stream.CopyTo(fileStream);
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