using _2048Game.Core;
using _2048Game.Models;
using _2048Game.Services.Abstractions;

namespace _2048Game.Services;

// Should not be called from anywhere. Should work independently.
// Input -> Logic -> Render
public sealed class RenderService(
    IConsoleService consoleService,
    IInputService inputService
) : IRenderService
{
    private const int TileWidth = 7;

    public void RenderBoard(int[,] tiles, ScoreBoard scoreBoard, int boardSize)
    {
        consoleService.Clear();
        consoleService.WriteLine($"Score: {scoreBoard.Score}");
        consoleService.WriteLine($"Best: {scoreBoard.BestScore}");
        consoleService.WriteLine(new string('-', boardSize * TileWidth + 1));

        for (var x = 0; x < boardSize; x++)
        {
            for (var y = 0; y < boardSize; y++)
            {
                var tileValue = tiles[x,y];
                PrintTileValue(tileValue);
            }

            consoleService.WriteLine("|");
            consoleService.WriteLine(new string('-', boardSize * TileWidth + 1));
        }
    }

    public void RenderGameOver()
    {
        consoleService.WriteLine("Game Over!");
    }

    public void RenderWin()
    {
        consoleService.WriteLine("Congratulations! You've reached 2048!");
    }

    public bool ConfirmAction(string message)
    {
        consoleService.Write(message + " (Y/N)");

        ConsoleKey key = default;
        while (key is not (ConsoleKey.Y or ConsoleKey.N or ConsoleKey.Enter))
        {
            key = inputService.ListenKey();
        }

        // Clear the line
        consoleService.SetCursorPosition(0, consoleService.CursorTop);
        consoleService.Write(new string(' ', consoleService.WindowWidth));
        consoleService.SetCursorPosition(0, consoleService.CursorTop);

        return key is ConsoleKey.Y or ConsoleKey.Enter;
    }

    private void PrintTileValue(int tileValue)
    {
        if (tileValue == 0)
        {
            consoleService.Write("|" + new string(' ', TileWidth - 1));
            return;
        }

        var valueString = tileValue.ToString();
        var paddedValue = valueString.PadLeft(TileWidth - 1);

        consoleService.Write("|");
        consoleService.BackgroundColor = Mapping.ValueToBackgroundColor[tileValue];
        consoleService.ForegroundColor = Mapping.ValueToForegroundColor[tileValue];
        consoleService.Write($"{paddedValue}");
        consoleService.ResetColor();
    }
}