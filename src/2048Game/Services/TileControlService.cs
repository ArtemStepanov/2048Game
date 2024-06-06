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
            Direction.Left => MoveLeft(),
            Direction.Right => MoveRight(),
            Direction.Up => MoveUp(),
            Direction.Down => MoveDown(),
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }

    private bool MoveLeft()
    {
        var moved = false;
        for (var row = 0; row < board.Size; row++)
        {
            var tiles = board.Tiles.Where(t => t.Row == row).OrderBy(t => t.Column).ToList();
            var posVertical = 0;
            Tile? previousTile = null;
            for (var i = 0; i < tiles.Count; i++)
            {
                var tile = tiles[i];
                if (i > 0 && previousTile?.Value == tile.Value && previousTile.Column == posVertical - 1)
                {
                    previousTile.Value *= 2;
                    MergeScore += previousTile.Value;
                    board.RemoveTile(tile);
                    moved = true;
                    continue;
                }

                if (tile.Column != posVertical)
                {
                    moved = true;
                    tile.Column = posVertical;
                }

                posVertical++;
                previousTile = tile;
            }
        }

        return moved;
    }

    private bool MoveRight()
    {
        var moved = false;
        for (var row = 0; row < board.Size; row++)
        {
            var tiles = board.Tiles.Where(t => t.Row == row).OrderByDescending(t => t.Column).ToList();
            var posVertical = board.Size - 1;
            Tile? previousTile = null;
            for (var i = 0; i < tiles.Count; i++)
            {
                var tile = tiles[i];
                if (i > 0 && previousTile?.Value == tile.Value && previousTile.Column == posVertical + 1)
                {
                    previousTile.Value *= 2;
                    MergeScore += previousTile.Value;
                    board.RemoveTile(tile);
                    moved = true;
                    continue;
                }

                if (tile.Column != posVertical)
                {
                    moved = true;
                    tile.Column = posVertical;
                }

                posVertical--;
                previousTile = tile;
            }
        }

        return moved;
    }

    private bool MoveUp()
    {
        var moved = false;
        for (var column = 0; column < board.Size; column++)
        {
            var tiles = board.Tiles.Where(t => t.Column == column).OrderBy(t => t.Row).ToList();
            var posHorizontal = 0;
            Tile? previousTile = null;
            for (var i = 0; i < tiles.Count; i++)
            {
                var tile = tiles[i];
                if (i > 0 && previousTile?.Value == tile.Value && previousTile.Row == posHorizontal - 1)
                {
                    previousTile.Value *= 2;
                    MergeScore += previousTile.Value;
                    board.RemoveTile(tile);
                    moved = true;
                    continue;
                }

                if (tile.Row != posHorizontal)
                {
                    moved = true;
                    tile.Row = posHorizontal;
                }

                posHorizontal++;
                previousTile = tile;
            }
        }

        return moved;
    }

    private bool MoveDown()
    {
        var moved = false;
        for (var column = 0; column < board.Size; column++)
        {
            var tiles = board.Tiles.Where(t => t.Column == column).OrderByDescending(t => t.Row).ToList();
            var posHorizontal = board.Size - 1;
            Tile? previousTile = null;
            for (var x = 0; x < tiles.Count; x++)
            {
                var tile = tiles[x];
                if (x > 0 && previousTile?.Value == tile.Value && previousTile.Row == posHorizontal + 1)
                {
                    previousTile.Value *= 2;
                    MergeScore += previousTile.Value;
                    board.RemoveTile(tile);
                    moved = true;
                    continue;
                }

                if (tile.Row != posHorizontal)
                {
                    moved = true;
                    tile.Row = posHorizontal;
                }

                posHorizontal--;
                previousTile = tile;
            }
        }

        return moved;
    }
}