using System.Text.Json;

namespace ServerJarsAPI.Demo.Console;

internal static class ConsoleWriter
{
    private static JsonSerializerOptions _jsonOptions = new()
    {
        WriteIndented = true,
    };

    public static void WriteLine(string text, ConsoleColor? foregroundColor = null, ConsoleColor? backgroundColor = null)
    {
        if (foregroundColor != null)
            SetConsoleColor((ConsoleColor)foregroundColor, backgroundColor);

        System.Console.WriteLine(text);

        if (foregroundColor != null)
            ResetConsoleColor();
    }

    public static void WriteObject(object obj, ConsoleColor? foregroundColor = null, ConsoleColor? backgroundColor = null)
    {
        WriteLine(JsonSerializer.Serialize(obj, _jsonOptions), foregroundColor, backgroundColor);
    }

    public static void Write(string text, ConsoleColor? foregroundColor = null, ConsoleColor? backgroundColor = null)
    {
        if (foregroundColor != null)
            SetConsoleColor((ConsoleColor)foregroundColor, backgroundColor);

        System.Console.Write(text);

        if (foregroundColor != null)
            ResetConsoleColor();
    }

    private static void SetConsoleColor(ConsoleColor foregroundColor, ConsoleColor? backgroundColor = null)
    {
        System.Console.ForegroundColor = foregroundColor;
        if (backgroundColor is not null)
        {
            System.Console.BackgroundColor = (ConsoleColor)backgroundColor;
        }
    }

    private static void ResetConsoleColor()
    {
        System.Console.ForegroundColor = ConsoleColor.Gray;
        System.Console.BackgroundColor = ConsoleColor.Black;
    }
}
