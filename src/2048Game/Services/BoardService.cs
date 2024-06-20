using _2048Game.Core;
using _2048Game.Models;
using _2048Game.Services.Abstractions;

namespace _2048Game.Services;

public sealed class BoardService : IBoardService
{
    private readonly Random _random = new();
    private List<(int, int)> _emptyTiles = [];

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public BoardService(int[][]? tiles = null, ScoreBoard? scoreBoard = null, int size = 4)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
        BoardSize = size;
        ScoreBoard = scoreBoard ?? new ScoreBoard();
        Tiles = tiles ?? new int[size][];
        IsLoadedFromSave = tiles is not null;
        Initialize();
    }

    public int BoardSize { get; private set; }
    public ScoreBoard ScoreBoard { get; }
    public int[][] Tiles { get; private set; }
    private bool IsLoadedFromSave { get; }
    private int MergeScore { get; set; }

    public void AddRandomTile()
    {
        if (_emptyTiles.Count == 0)
        {
            return;
        }

        var (x, y) = _emptyTiles[_random.Next(_emptyTiles.Count)];
        Tiles[x][y] = _random.NextDouble() < 0.9 ? 2 : 4;
        _emptyTiles.Remove((y, x));
    }

    public void Reset(int boardSize)
    {
        BoardSize = boardSize;
        ScoreBoard.Reset();
        Initialize(true);
    }

    // TODO: There are still issues with moving. Sometimes tiles disappear, sometimes they moved weirdly.
    public bool Move(Direction direction)
    {
        var moved = direction switch
        {
            Direction.Up => MoveUp(),
            Direction.Down => MoveDown(),
            Direction.Left => MoveLeft(),
            Direction.Right => MoveRight(),
            _ => false
        };

        if (moved)
        {
            AddRandomTile();
        }

        RecalculateEmptyTiles();

        return moved;
    }

    public bool CanMove()
    {
        for (var x = 0; x < BoardSize; x++)
        {
            for (var y = 0; y < BoardSize; y++)
            {
                if (CheckTiles(x, y))
                {
                    return true;
                }
            }
        }

        return false;

        bool CheckTiles(int x, int y)
        {
            // Check empty tile
            if (Tiles[x][y] == 0)
            {
                return true;
            }

            // Left
            if (x > 0 && Tiles[x][y] == Tiles[x - 1][y])
            {
                return true;
            }

            // Up
            if (y > 0 && Tiles[x][y] == Tiles[x][y - 1])
            {
                return true;
            }

            // Right
            if (x < BoardSize - 1 && Tiles[x][y] == Tiles[x + 1][y])
            {
                return true;
            }

            // Down
            if (y < BoardSize - 1 && Tiles[x][y] == Tiles[x][y + 1])
            {
                return true;
            }

            return false;
        }
    }

    public bool HasWon()
    {
        for (var x = 0; x < BoardSize; x++)
        {
            for (var y = 0; y < BoardSize; y++)
            {
                if (Tiles[x][y] == 2048)
                {
                    return true;
                }
            }
        }

        return false;
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
            RecalculateEmptyTiles();
            return;
        }

        var tmpTiles = new int[BoardSize][];
        for (var i = 0; i < BoardSize; i++)
        {
            tmpTiles[i] = new int[BoardSize];
            for (var j = 0; j < BoardSize; j++)
            {
                tmpTiles[i][j] = 0;
                _emptyTiles.Add((i, j));
            }
        }

        Tiles = tmpTiles;

        AddRandomTile();
        AddRandomTile();
        RecalculateEmptyTiles();
    }

    private bool CoverUp()
    {
        var newMatrix = new int[BoardSize][];
        for (var i = 0; i < BoardSize; i++)
        {
            newMatrix[i] = new int[BoardSize];
        }

        var done = false;
        for (var i = 0; i < BoardSize; i++)
        {
            var count = 0;
            for (var j = 0; j < BoardSize; j++)
            {
                if (Tiles[i][j] != 0)
                {
                    newMatrix[i][count] = Tiles[i][j];
                    if (j != count)
                    {
                        done = true;
                    }

                    count++;
                }
            }
        }

        Tiles = newMatrix;

        return done;
    }

    private bool Merge(bool done)
    {
        for (var i = 0; i < BoardSize; i++)
        {
            for (var j = 0; j < BoardSize - 1; j++)
            {
                if (Tiles[i][j] == Tiles[i][j + 1] && Tiles[i][j] != 0)
                {
                    Tiles[i][j] *= 2;
                    Tiles[i][j + 1] = 0;
                    done = true;
                    MergeScore += Tiles[i][j]; // Increase the score
                }
            }
        }

        return done;
    }

    private void Reverse()
    {
        var newMat = new int[BoardSize][];
        for (var i = 0; i < BoardSize; i++)
        {
            newMat[i] = new int[BoardSize];
            for (var j = 0; j < BoardSize; j++)
            {
                newMat[i][j] = Tiles[i][BoardSize - j - 1];
            }
        }

        Tiles = newMat;
    }

    private void Transpose()
    {
        var newMat = new int[BoardSize][];
        for (var i = 0; i < BoardSize; i++)
        {
            newMat[i] = new int[BoardSize];
            for (var j = 0; j < BoardSize; j++)
            {
                newMat[i][j] = Tiles[j][i];
            }
        }

        Tiles = newMat;
    }

    private bool MoveUp()
    {
        Transpose();
        var done = Merge(CoverUp());
        CoverUp();
        Transpose();
        return done;
    }

    private bool MoveDown()
    {
        Transpose();
        Reverse();
        var done = Merge(CoverUp());
        CoverUp();
        Reverse();
        Transpose();
        return done;
    }

    private bool MoveLeft()
    {
        var done = Merge(CoverUp());
        CoverUp();
        return done;
    }

    private bool MoveRight()
    {
        Reverse();
        var done = Merge(CoverUp());
        CoverUp();
        Reverse();
        return done;
    }

    private void RecalculateEmptyTiles()
    {
        _emptyTiles.Clear();
        for (var x = 0; x < BoardSize; x++)
        {
            for (var y = 0; y < BoardSize; y++)
            {
                if (Tiles[y][x] == 0)
                {
                    _emptyTiles.Add((x, y));
                }
            }
        }
    }
}
