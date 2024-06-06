using _2048Game.Core;
using _2048Game.Models;
using _2048Game.Models.Abstractions;
using _2048Game.Services.Abstractions;

namespace _2048Game.Services;

public sealed class TileControlService(IBoard board) : ITileControlService
{
    public int MergeScore { get; private set; }

    public bool Move(Direction direction)
    {
        MergeScore = 0; // Reset merge score for each move
        return direction switch
        {
            Direction.Left => MoveTiles(t => t.Row, t => t.Column, 0, 1, board.Size),
            Direction.Right => MoveTiles(t => t.Row, t => t.Column, board.Size - 1, -1, board.Size),
            Direction.Up => MoveTiles(t => t.Column, t => t.Row, 0, 1, board.Size),
            Direction.Down => MoveTiles(t => t.Column, t => t.Row, board.Size - 1, -1, board.Size),
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }

    private bool MoveTiles(Func<Tile, int> primarySelector, Func<Tile, int> secondarySelector, int start, int step, int size)
    {
        var moved = false;

        for (var primaryIndex = 0; primaryIndex < size; primaryIndex++)
        {
            var tiles = GetTilesInOrder(primarySelector, secondarySelector, primaryIndex, step);
            var pos = start;
            Tile? previousTile = null;

            foreach (var tile in tiles)
            {
                if (ShouldMergeTiles(tile, previousTile, secondarySelector, pos, step))
                {
                    MergeTiles(tile, previousTile);
                    moved = true;
                    continue;
                }

                if (secondarySelector(tile) != pos)
                {
                    moved = true;
                    MoveTile(tile, primarySelector, pos);
                }

                pos += step;
                previousTile = tile;
            }
        }

        return moved;
    }

    private List<Tile> GetTilesInOrder(Func<Tile, int> primarySelector, Func<Tile, int> secondarySelector, int primaryIndex, int step)
    {
        return board.Tiles.Where(t => primarySelector(t) == primaryIndex)
            .OrderBy(t => secondarySelector(t) * step)
            .ToList();
    }

    private void MergeTiles(Tile tile, Tile? previousTile)
    {
        if (previousTile == null)
        {
            return;
        }

        previousTile.Value *= 2;
        MergeScore += previousTile.Value;
        board.RemoveTile(tile);
    }

    private static bool ShouldMergeTiles(Tile tile, Tile? previousTile, Func<Tile, int> secondarySelector, int pos, int step)
    {
        return previousTile?.Value == tile.Value && secondarySelector(previousTile) == pos - step;
    }

    private static void MoveTile(Tile tile, Func<Tile, int> primarySelector, int pos)
    {
        if (primarySelector(tile) == tile.Row)
        {
            tile.Column = pos;
        }
        else
        {
            tile.Row = pos;
        }
    }
}