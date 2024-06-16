using _2048Game.Core;
using _2048Game.Services.Abstractions;

namespace _2048Game.Services;

public sealed class InputService(IGameService gameService, IConsoleService consoleService) : IInputService
{

    public void HandleInput()
    {
        var input = ListenKey();
        switch (input)
        {
            case ConsoleKey.UpArrow:
            case ConsoleKey.DownArrow:
            case ConsoleKey.RightArrow:
            case ConsoleKey.LeftArrow:
                gameService.ProcessStep(Mapping.KeyToDirection[input]);
                break;

            case ConsoleKey.R:
                gameService.ProcessRestart();
                break;

            case ConsoleKey.Q:
                gameService.ProcessExit();
                break;

            default:
                consoleService.WriteLine("Invalid input. Use arrow keys to move, R to restart, Q to quit.");
                break;
        }
    }

    public ConsoleKey ListenKey()
    {
        return consoleService.ReadKey(true).Key;
    }
}