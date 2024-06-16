namespace _2048Game.Services.Abstractions;

public interface IInputService
{
    void HandleInput();
    ConsoleKey ListenKey();
}