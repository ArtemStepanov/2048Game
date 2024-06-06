using _2048Game.Services.Abstractions;
using System.Diagnostics.CodeAnalysis;

namespace _2048Game.Services;

[ExcludeFromCodeCoverage]
public sealed class ConsoleService : IConsoleService
{
    public int CursorTop => Console.CursorTop;
    public int WindowWidth => Console.WindowWidth;

    public ConsoleColor BackgroundColor
    {
        set => Console.BackgroundColor = value;
    }

    public ConsoleColor ForegroundColor
    {
        set => Console.ForegroundColor = value;
    }

    public ConsoleKeyInfo ReadKey(bool intercept)
    {
        return Console.ReadKey(intercept);
    }

    public void WriteLine(string message)
    {
        Console.WriteLine(message);
    }

    public void Clear()
    {
        Console.Clear();
    }

    public void SetCursorPosition(int left, int top)
    {
        Console.SetCursorPosition(left, top);
    }

    public void Write(string message)
    {
        Console.Write(message);
    }

    public void ResetColor()
    {
        Console.ResetColor();
    }
}