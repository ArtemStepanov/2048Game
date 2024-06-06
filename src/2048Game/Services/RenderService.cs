using _2048Game.Models;
using _2048Game.Models.Abstractions;
using _2048Game.Services.Abstractions;

namespace _2048Game.Services;

public sealed class RenderService(IBoard board, ScoreBoard scoreBoard, IConsoleService consoleService) : IRenderService
{
    private const int TileWidth = 7;

    public void RenderBoard()
    {
        consoleService.Clear();
        consoleService.WriteLine($"Score: {scoreBoard.Score}");
        consoleService.WriteLine($"Best: {scoreBoard.BestScore}");
        consoleService.WriteLine(new string('-', board.Size * TileWidth + 1));

        for (var x = 0; x < board.Size; x++)
        {
            for (var y = 0; y < board.Size; y++)
            {
                var tile = board.Tiles.FirstOrDefault(t => t.Row == x && t.Column == y);
                PrintTile(tile);
            }

            consoleService.WriteLine("|");
            consoleService.WriteLine(new string('-', board.Size * TileWidth + 1));
        }
    }

    public void RenderGameOver()
    {
        consoleService.WriteLine("Game Over!");
    }

    public bool ConfirmAction(string message)
    {
        consoleService.Write(message + " (Y/N)");

        ConsoleKey key = default;
        while (key is not (ConsoleKey.Y or ConsoleKey.N or ConsoleKey.Enter))
        {
            key = consoleService.ReadKey(true).Key;
        }

        // Clear the line
        consoleService.SetCursorPosition(0, consoleService.CursorTop);
        consoleService.Write(new string(' ', consoleService.WindowWidth));
        consoleService.SetCursorPosition(0, consoleService.CursorTop);

        return key is ConsoleKey.Y or ConsoleKey.Enter;
    }

    private void PrintTile(Tile? tile)
    {
        if (tile is null)
        {
            consoleService.Write("|" + new string(' ', TileWidth - 1));
            return;
        }

        var valueString = tile.Value.ToString();
        var paddedValue = valueString.PadLeft(TileWidth - 1);

        consoleService.Write("|");
        consoleService.BackgroundColor = tile.BackgroundColor;
        consoleService.ForegroundColor = tile.Color;
        consoleService.Write($"{paddedValue}");
        consoleService.ResetColor();
    }
}