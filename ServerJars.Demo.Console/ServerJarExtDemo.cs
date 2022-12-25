namespace ServerJarsAPI.Demo.Console;

internal static class ServerJarExtDemo
{
    public static async Task Demo()
    {
        var serverJarExt = new ServerJarsExt();

        // GetTypes
        ConsoleWriter.WriteLine("\nAPI call - GetTypes:\n", ConsoleColor.White, ConsoleColor.Red);

        var types = await serverJarExt.GetTypes();
        ConsoleWriter.WriteLine(string.Join(Environment.NewLine, types.Select((kv) => $"{kv.Key}: {string.Join(", ", kv.Value)}")));


        // GetDetails
        ConsoleWriter.WriteLine("\nAPI call - GetDetails:\n", ConsoleColor.White, ConsoleColor.Red);

        var details = await serverJarExt.GetDetails("servers", "spigot", "1.19.1");
        ConsoleWriter.WriteObject(details);


        // GetLatest
        ConsoleWriter.WriteLine("\nAPI call - GetLatestDetails:\n", ConsoleColor.White, ConsoleColor.Red);

        var latestDetails = await serverJarExt.GetLatest("servers", "spigot");
        ConsoleWriter.WriteObject(latestDetails);


        // GetAllDetails
        ConsoleWriter.WriteLine("\nAPI call - GetAllDetails:\n", ConsoleColor.White, ConsoleColor.Red);

        var allDetails = await serverJarExt.GetAllDetails("servers", "spigot", 5u);
        ConsoleWriter.WriteObject(allDetails);
    }
}
