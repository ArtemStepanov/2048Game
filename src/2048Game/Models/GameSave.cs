namespace _2048Game.Models;

public record GameSave(GameSave.Tile[] Tiles, ScoreBoard ScoreBoard, int BoardSize = 4)
{
    public record struct Tile(int X, int Y, int Value);

    public static GameSave Create(int[,] tiles, int boardSize, ScoreBoard scoreBoard)
    {
        int rows = tiles.GetLength(0);
        int cols = tiles.GetLength(1);
        var convertedTiles = new Tile[tiles.Length];
        var index = 0;
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                convertedTiles[index++] = new Tile(i, j, tiles[i, j]);
            }
        }

        return new GameSave(convertedTiles, scoreBoard, boardSize);
    }

    public (int[,] tiles, ScoreBoard scoreBoard) ToRawTilesAndScoreBoard()
    {
        var tiles = new int[BoardSize, BoardSize];
        foreach (var tile in Tiles)
        {
            tiles[tile.X, tile.Y] = tile.Value;
        }

        return (tiles, ScoreBoard);
    }

    internal void ResetSave()
    {
        ScoreBoard.Reset();
    }
}