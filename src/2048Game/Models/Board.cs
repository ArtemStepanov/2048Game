using _2048Game.Models.Abstractions;
using System.Text.Json.Serialization;

namespace _2048Game.Models;

// Interface introduced to allow for mocking in tests

public sealed class Board : IBoard
{
    private readonly Random _random = new();

    [JsonInclude]
    [JsonPropertyName("Tiles")]
    private List<Tile> _tiles = [];

    public Board(int size = 4)
    {
        // For the possible difficulty feature implementation. Not used ATM
        if (size is < 4 or > 8)
        {
            throw new ArgumentException("Board size must be between 4 and 8.");
        }

        Size = size;
        Initialize();
    }

    [JsonIgnore]
    public IReadOnlyCollection<Tile> Tiles => _tiles;

    public int Size { get; private set; }

    public void AddRandomTile()
    {
        int x, y;
        do
        {
            x = _random.Next(Size);
            y = _random.Next(Size);
        } while (Tiles.Any(t => t.Row == x && t.Column == y));

        _tiles.Add(new Tile(x, y, _random.NextDouble() < 0.9 ? 2 : 4));
    }

    public void RemoveTile(Tile tile)
    {
        _tiles.Remove(tile);
    }

    public void Reset(int boardSize)
    {
        _tiles.Clear();
        Size = boardSize;
        Initialize();
    }

    public bool CanMove()
    {
        return Tiles.Count < Size * Size || Tiles.Any(VerifyTileIsMergeable);
    }

    private bool VerifyTileIsMergeable(Tile tile)
    {
        // Check if we can merge with the tile above
        var upTile = Tiles.FirstOrDefault(t => t.Row == tile.Row - 1 && t.Column == tile.Column);
        if (upTile is not null && upTile.Value == tile.Value)
        {
            return true;
        }

        // Check if we can merge with the tile below
        var downTile = Tiles.FirstOrDefault(t => t.Row == tile.Row + 1 && t.Column == tile.Column);
        if (downTile is not null && downTile.Value == tile.Value)
        {
            return true;
        }

        // Check if we can merge with the tile to the left
        var leftTile = Tiles.FirstOrDefault(t => t.Row == tile.Row && t.Column == tile.Column - 1);
        if (leftTile is not null && leftTile.Value == tile.Value)
        {
            return true;
        }

        // Check if we can merge with the tile to the right
        var rightTile = Tiles.FirstOrDefault(t => t.Row == tile.Row && t.Column == tile.Column + 1);
        return rightTile is not null && rightTile.Value == tile.Value;
    }

    private void Initialize()
    {
        AddRandomTile();
        AddRandomTile();
    }
}