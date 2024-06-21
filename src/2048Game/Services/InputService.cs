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
                    ProcessMove(input, gameService);
                    break;

                case ConsoleKey.R:
                    ProcessRestart();
                    break;

                case ConsoleKey.Q:
                    ProcessQuit();
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

    private void ProcessMove(ConsoleKey input, IGameService gameService)
    {
        var result = gameService.ProcessStep(Mapping.KeyToDirection[input]);
        switch (result)
        {
            // todo: починить сообщение гейм овер
            case ProcessStepResult.GameOver when !ConfirmAction("Start a new game?"):
                gameService.StopGame();
                return;
            case ProcessStepResult.Win when !ConfirmAction("Continue playing?"):
                gameService.StopGame();
                return;
        }
    }

    private void ProcessQuit()
    {
        if (ConfirmAction("Quit game?"))
        {
            gameService.StopGame();
        }
    }

    private void ProcessRestart()
    {
        if (ConfirmAction("Restart game?"))
        {
            gameService.RestartGame();
        }
    }
}