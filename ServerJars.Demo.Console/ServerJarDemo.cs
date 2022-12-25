using ServerJarsAPI.Events;
using ServerJarsAPI.Extensions;

namespace ServerJarsAPI.Demo.Console;

internal static class ServerJarDemo
{
    public static async Task Demo()
    {
        var serverJar = new ServerJars();

        // GetTypes
        ConsoleWriter.WriteLine("\nAPI call - GetTypes:\n", ConsoleColor.White, ConsoleColor.Red);

        var types = await serverJar.GetTypes();
        ConsoleWriter.WriteObject(types);

        // GetTypes.AsDictionary() extension
        ConsoleWriter.WriteLine("\nAPI call - GetTypes.AsDictionary() extension:\n", ConsoleColor.White, ConsoleColor.Red);

        var dict = types.AsDictionary();
        ConsoleWriter.WriteLine(string.Join(Environment.NewLine, dict.Select((kv) => $"{kv.Key}: {string.Join(", ", kv.Value)}")));


        // GetDetails
        ConsoleWriter.WriteLine("\nAPI call - GetDetails:\n", ConsoleColor.White, ConsoleColor.Red);

        var details = await serverJar.GetDetails("servers", "spigot", "1.19.1");
        ConsoleWriter.WriteObject(details);


        // GetLatest
        ConsoleWriter.WriteLine("\nAPI call - GetLatestDetails:\n", ConsoleColor.White, ConsoleColor.Red);

        var latestDetails = await serverJar.GetLatest("servers", "spigot");
        ConsoleWriter.WriteObject(latestDetails);


        // GetAllDetails
        ConsoleWriter.WriteLine("\nAPI call - GetAllDetails:\n", ConsoleColor.White, ConsoleColor.Red);

        var allDetails = await serverJar.GetAllDetails("servers", "spigot", 5u);
        ConsoleWriter.WriteObject(allDetails);


        // GetJar Method 1 (including progress)
        ConsoleWriter.WriteLine("\nAPI call - GetJar with method 1 (with progress):\n", ConsoleColor.White, ConsoleColor.Red);

        using var fileStream1 = File.Create("./server1.jar");
        Progress<ProgressEventArgs> progress = new();
        progress.ProgressChanged += (_, e) =>
        {
            ConsoleWriter.Write($"\rProgress: {e.ProgressPercentage}% ({e.BytesTransferred / 1024 / 1024}MB / {e.TotalBytes / 1024 / 1024}MB)          ");
        };
        await serverJar.GetJar(fileStream1, "servers", "spigot", progress: progress);
        await fileStream1.FlushAsync();
        ConsoleWriter.WriteLine($"\nDownloaded {fileStream1.Length / 1024 / 1024}MB to {fileStream1.Name}");

        // GetJar Method 2
        ConsoleWriter.WriteLine("\nAPI call - GetJar method 2:\n", ConsoleColor.White, ConsoleColor.Red);

        using (var stream = await serverJar.GetJar("servers", "spigot", "1.19.1"))
        {
            using var fileStream2 = File.Create("./server2.jar");
            await stream.CopyToAsync(fileStream2);
            ConsoleWriter.WriteLine($"Downloaded {fileStream2.Length / 1024 / 1024}MB to {fileStream2.Name}");
        }
    }
}
