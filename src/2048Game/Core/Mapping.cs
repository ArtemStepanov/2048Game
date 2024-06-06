using System.Collections.ObjectModel;

namespace _2048Game.Core;

public static class Mapping
{
    public static readonly ReadOnlyDictionary<ConsoleKey, Direction> KeyToDirection =
        new Dictionary<ConsoleKey, Direction>
        {
            { ConsoleKey.UpArrow, Direction.Up },
            { ConsoleKey.LeftArrow, Direction.Left },
            { ConsoleKey.DownArrow, Direction.Down },
            { ConsoleKey.RightArrow, Direction.Right }
        }.AsReadOnly();

    public static readonly ReadOnlyDictionary<int, ConsoleColor> ValueToBackgroundColor =
        new Dictionary<int, ConsoleColor>
        {
            { 2, ConsoleColor.Gray },
            { 4, ConsoleColor.DarkGray },
            { 8, ConsoleColor.DarkCyan },
            { 16, ConsoleColor.DarkYellow },
            { 32, ConsoleColor.DarkRed },
            { 64, ConsoleColor.Red },
            { 128, ConsoleColor.DarkMagenta },
            { 256, ConsoleColor.Magenta },
            { 512, ConsoleColor.DarkBlue },
            { 1024, ConsoleColor.Blue },
            { 2048, ConsoleColor.DarkGreen }
        }.AsReadOnly();

    public static readonly ReadOnlyDictionary<int, ConsoleColor> ValueToForegroundColor =
        new Dictionary<int, ConsoleColor>
        {
            { 2, ConsoleColor.Black },
            { 4, ConsoleColor.Black },
            { 8, ConsoleColor.Black },
            { 16, ConsoleColor.Black },
            { 32, ConsoleColor.White },
            { 64, ConsoleColor.White },
            { 128, ConsoleColor.White },
            { 256, ConsoleColor.White },
            { 512, ConsoleColor.White },
            { 1024, ConsoleColor.White },
            { 2048, ConsoleColor.White }
        }.AsReadOnly();
}