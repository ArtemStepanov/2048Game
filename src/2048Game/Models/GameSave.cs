namespace _2048Game.Models;

public record GameSave(List<GameSave.Tile> Tiles, ScoreBoard ScoreBoard, int BoardSize = 4)
{
    public record struct Tile(int Column, int Row, int Value);

    public static GameSave Create(int[][] tiles, int boardSize, ScoreBoard scoreBoard)
    {
        var convertedTiles = new List<Tile>();
        for (var row = 0; row < boardSize; row++)
        {
            for (var column = 0; column < boardSize; column++)
            {
                convertedTiles.Add(new Tile(column, row, tiles[row][column]));
            }
        }

        return new GameSave(convertedTiles, scoreBoard, boardSize);
    }

    public (int[][] tiles, ScoreBoard scoreBoard) ToRawTilesAndScoreBoard()
    {
        var tiles = new int[BoardSize][];
        foreach (var tile in Tiles)
        {
            // ReSharper disable once NullCoalescingConditionIsAlwaysNotNullAccordingToAPIContract
            tiles[tile.Row] ??= new int[BoardSize];
            tiles[tile.Row][tile.Column] = tile.Value;
        }

        return (tiles, ScoreBoard);
    }

    internal void ResetSave()
    {
        ScoreBoard.Reset();
    }
}