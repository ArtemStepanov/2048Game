namespace _2048Game.Services.Abstractions;

public interface IInputService
{
    void StartGameAndListenInput();
    ConsoleKey ListenKey();
    bool ConfirmAction(string message);
}