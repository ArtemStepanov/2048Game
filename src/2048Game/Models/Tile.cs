using _2048Game.Core;
using System.Text.Json.Serialization;

namespace _2048Game.Models;

public class Tile(int row, int column, int value)
{
    public int Row { get; set; } = row;
    public int Column { get; set; } = column;
    public int Value { get; set; } = value;

    [JsonIgnore]
    public ConsoleColor Color =>
        Mapping.ValueToForegroundColor.GetValueOrDefault(Value, ConsoleColor.White);

    [JsonIgnore]
    public ConsoleColor BackgroundColor =>
        Mapping.ValueToBackgroundColor.GetValueOrDefault(Value, ConsoleColor.Black);
}