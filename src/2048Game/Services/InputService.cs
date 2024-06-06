using _2048Game.Core;
using _2048Game.Core.Exceptions;
using _2048Game.Services.Abstractions;

namespace _2048Game.Services;

public class InputService(IGameService gameService, IRenderService renderService, IConsoleService consoleService)
{
    public bool HandleInput()
    {
        try
        {
            var input = consoleService.ReadKey(true).Key;
            HandleInput(input);
            return true;
        }
        catch (GameExitException)
        {
            consoleService.WriteLine("Goodbye!");
            return false;
        }
    }

    private void HandleInput(ConsoleKey input)
    {
        switch (input)
        {
            case ConsoleKey.UpArrow:
            case ConsoleKey.DownArrow:
            case ConsoleKey.RightArrow:
            case ConsoleKey.LeftArrow:
                gameService.ProcessStep(Mapping.KeyToDirection[input]);
                break;

            case ConsoleKey.R:
                if (renderService.ConfirmAction("Restart game?"))
                {
                    gameService.StartNewGame();
                }

                break;

            case ConsoleKey.Q:
                if (renderService.ConfirmAction("Quit game?"))
                {
                    gameService.SaveGame();
                    throw new GameExitException();
                }

                break;

            default:
                consoleService.WriteLine("Invalid input. Use arrow keys to move, R to restart, Q to quit.");
                break;
        }
    }
}