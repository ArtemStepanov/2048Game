namespace _2048Game.Models.Abstractions;

public interface IBoard
{
    IReadOnlyCollection<Tile> Tiles { get; }
    int Size { get; }
    void AddRandomTile();
    void RemoveTile(Tile tile);
    void Reset(int boardSize = 4);
    bool CanMove();
}