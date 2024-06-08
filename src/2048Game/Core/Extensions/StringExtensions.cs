using System.Text.Json;

namespace _2048Game.Core.Extensions;

public static class StringExtensions
{
    public static T? ReadJsonFile<T>(this string path) where T : class
    {
        return JsonSerializer.Deserialize<T>(File.ReadAllText(path));
    }
}