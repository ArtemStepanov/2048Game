using _2048Game.Core;
using _2048Game.Models;
using _2048Game.Services.Abstractions;

namespace _2048Game.Services;
public sealed class BoardService : IBoardService
{
    private readonly Random _randomX = new();
    private readonly Random _randomY = new();
    private readonly Random _random = new();
    private List<(int, int)> _emptyTiles = [];

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public BoardService(int[,]? tiles = null, ScoreBoard? scoreBoard = null, int size = 4)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
        BoardSize = size;
        ScoreBoard = scoreBoard ?? new ScoreBoard();
        Tiles = tiles ?? new int[size, size];
        IsLoadedFromSave = tiles is not null;
        Initialize();
    }

    public int BoardSize { get; private set; }
    public ScoreBoard ScoreBoard { get; }
    public int[,] Tiles { get; private set; }
    private bool IsLoadedFromSave { get; init; }
    private int MergeScore { get; set; }

    public void AddRandomTile()
    {
        if (_emptyTiles.Count == 0)
        {
            return;
        }

        var (x, y) = _emptyTiles[_random.Next(_emptyTiles.Count)];
        Tiles[x, y] = _random.NextDouble() < 0.9 ? 2 : 4;
        _emptyTiles.Remove((x, y));
    }

    public void RemoveTile(int x, int y)
    {
        if (x > BoardSize || y > BoardSize)
        {
            throw new ArgumentOutOfRangeException("Coordinates out of bounds.");
        }

        if (Tiles[x, y] != 0)
        {
            Tiles[x, y] = 0;
            _emptyTiles.Add((x, y));
        }
    }

    public void Reset(int boardSize)
    {
        BoardSize = boardSize;
        ScoreBoard.Reset();
        Initialize(true);
    }

    public bool Move(Direction direction)
    {
        MergeScore = 0;
        bool moved = false;

        switch (direction)
        {
            case Direction.Left:
                for (int x = 0; x < BoardSize; x++)
                {
                    if (MoveLine(x, 0, 1, 0))
                    {
                        moved = true;
                    }
                }

                break;

            case Direction.Right:
                for (int y = 0; y < BoardSize; y++)
                {
                    if (MoveLine(y, BoardSize - 1, -1, 0))
                    {
                        moved = true;
                    }
                }

                break;

            case Direction.Up:
                for (int x = 0; x < BoardSize; x++)
                {
                    if (MoveLine(0, x, 0, 1))
                    {
                        moved = true;
                    }
                }

                break;

            case Direction.Down:
                for (int x = 0; x < BoardSize; x++)
                {
                    if (MoveLine(BoardSize - 1, x, 0, -1))
                    {
                        moved = true;
                    }
                }

                break;
        }

        if (moved)
        {
            AddRandomTile();
        }

        ScoreBoard.AddScore(MergeScore);

        return moved;
    }

    public bool CanMove()
    {
        for (int x = 0; x < BoardSize; x++)
        {
            for (int y = 0; y < BoardSize; y++)
            {
                // Check empty tile
                if (Tiles[x, y] == 0)
                {
                    return true;
                }

                // Left
                if (x > 0 && Tiles[x, y] == Tiles[x - 1, y])
                {
                    return true;
                }

                // Up
                if (y > 0 && Tiles[x, y] == Tiles[x, y - 1])
                {
                    return true;
                }

                // Right
                if (x < BoardSize - 1 && Tiles[x, y] == Tiles[x + 1, y])
                {
                    return true;
                }

                // Down
                if (y < BoardSize - 1 && Tiles[x, y] == Tiles[x, y + 1])
                {
                    return true;
                }
            }
        }

        return false;
    }

    public bool HasWon()
    {
        for (int x = 0; x < BoardSize; x++)
        {
            for (int y = 0; y < BoardSize; y++)
            {
                if (Tiles[x, y] == 2048)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private bool MoveLine(int startX, int startY, int dx, int dy)
    {
        bool moved = false;
        int[] line = new int[BoardSize];
        int[] merged = new int[BoardSize];

        for (int i = 0; i < BoardSize; i++)
        {
            int x = startX + i * dx;
            int y = startY + i * dy;
            line[i] = Tiles[x, y];
        }

        int target = 0;

        for (int i = 0; i < BoardSize; i++)
        {
            if (line[i] != 0)
            {
                if (merged[target] != 0 && merged[target] == line[i])
                {
                    merged[target] *= 2;
                    MergeScore += merged[target];
                    target++;
                }
                else if (merged[target] != 0)
                {
                    target++;
                    merged[target] = line[i];
                }
                else
                {
                    merged[target] = line[i];
                }
            }
        }

        for (int i = 0; i < BoardSize; i++)
        {
            int x = startX + i * dx;
            int y = startY + i * dy;
            if (Tiles[x, y] != merged[i])
            {
                moved = true;
                if (merged[i] == 0)
                {
                    _emptyTiles.Add((x, y));
                }
                else if (Tiles[x, y] == 0)
                {
                    _emptyTiles.Remove((x, y));
                }
            }

            Tiles[x, y] = merged[i];
        }

        return moved;
    }

    private void Initialize(bool reset = false)
    {
        // Note: for the possible difficulty feature implementation. Not used ATM
        if (BoardSize is < 4 or > 8)
        {
            throw new ArgumentException("Board size must be between 4 and 8.");
        }

        if (IsLoadedFromSave && !reset)
        {
            for (var x = 0; x < BoardSize; x++)
            {
                for (var y = 0; y < BoardSize; y++)
                {
                    if (Tiles[x, y] == 0)
                    {
                        _emptyTiles.Add((x, y));
                    }
                }
            }

            return;
        }

        var tmpTiles = new int[BoardSize, BoardSize];
        _emptyTiles = [];

        for (var x = 0; x < BoardSize; x++)
        {
            for (var y = 0; y < BoardSize; y++)
            {
                tmpTiles[x, y] = 0;
                _emptyTiles.Add((x, y));
            }
        }

        Tiles = tmpTiles;

        AddRandomTile();
        AddRandomTile();
    }
}
