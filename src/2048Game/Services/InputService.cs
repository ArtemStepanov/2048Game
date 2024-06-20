using _2048Game.Core;
using _2048Game.Services.Abstractions;

namespace _2048Game.Services;

public sealed class InputService(IGameService gameService, IConsoleService consoleService) : IInputService
{

    public void StartGameAndListenInput()
    {
        gameService.StartGame();
        while (gameService.Running)
        {
            var input = ListenKey();
            switch (input)
            {
                case ConsoleKey.UpArrow:
                case ConsoleKey.DownArrow:
                case ConsoleKey.RightArrow:
                case ConsoleKey.LeftArrow:
                    var result = gameService.ProcessStep(Mapping.KeyToDirection[input]);
                    if (result is ProcessStepResult.GameOver && !ConfirmAction("Start a new game?"))
                    {
                        gameService.StopGame();
                        return;
                    }

                    if (result is ProcessStepResult.Win && !ConfirmAction("Continue playing?"))
                    {
                        gameService.StopGame();
                        return;
                    }

                    break;

                case ConsoleKey.R:
                    if (ConfirmAction("Restart game?"))
                    {
                        gameService.RestartGame();
                    }

                    break;

                case ConsoleKey.Q:
                    if (ConfirmAction("Quit game?"))
                    {
                        gameService.StopGame();
                    }

                    break;

                default:
                    consoleService.WriteLine("Invalid input. Use arrow keys to move, R to restart, Q to quit.");
                    break;
            }
        }
    }

    public ConsoleKey ListenKey()
    {
        return consoleService.ReadKey(true).Key;
    }

    public bool ConfirmAction(string message)
    {
        consoleService.Write(message + " (Y/N)");

        ConsoleKey key = default;
        while (key is not (ConsoleKey.Y or ConsoleKey.N or ConsoleKey.Enter))
        {
            key = ListenKey();
        }

        // Clear the line
        consoleService.SetCursorPosition(0, consoleService.CursorTop);
        consoleService.Write(new string(' ', consoleService.WindowWidth));
        consoleService.SetCursorPosition(0, consoleService.CursorTop);

        return key is ConsoleKey.Y or ConsoleKey.Enter;
    }
}