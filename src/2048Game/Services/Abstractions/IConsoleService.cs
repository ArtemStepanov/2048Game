namespace _2048Game.Services.Abstractions;

public interface IConsoleService
{
    int CursorTop { get; }
    int WindowWidth { get; }
    ConsoleColor BackgroundColor { set; }
    ConsoleColor ForegroundColor { set; }

    ConsoleKeyInfo ReadKey(bool intercept);
    void WriteLine(string message);
    void Clear();
    void SetCursorPosition(int left, int top);
    void Write(string message);
    void ResetColor();
}