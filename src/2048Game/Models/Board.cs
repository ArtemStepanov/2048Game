using _2048Game.Models.Abstractions;
using System.Text.Json.Serialization;

namespace _2048Game.Models;

public sealed class Board : IBoard
{
    private readonly Random _random = new();

    [JsonInclude]
    [JsonPropertyName("Tiles")]
    private List<Tile> _tiles = [];

    public Board(int size = 4)
    {
        // Note: for the possible difficulty feature implementation. Not used ATM
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
        return CanMergeWith(tile, tile.Row - 1, tile.Column) || // Validate tile above
               CanMergeWith(tile, tile.Row + 1, tile.Column) || // Validate tile below
               CanMergeWith(tile, tile.Row, tile.Column - 1) || // Validate tile to the left
               CanMergeWith(tile, tile.Row, tile.Column + 1); // Validate tile to the right
    }

    private bool CanMergeWith(Tile tile, int targetRow, int targetColumn)
    {
        var targetTile = Tiles.FirstOrDefault(t => t.Row == targetRow && t.Column == targetColumn);
        return targetTile is not null && targetTile.Value == tile.Value;
    }

    private void Initialize()
    {
        AddRandomTile();
        AddRandomTile();
    }
}