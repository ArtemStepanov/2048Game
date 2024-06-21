using _2048Game.Core;
using _2048Game.Models;

namespace _2048Game.Services.Abstractions;
public interface IBoardService
{
    int[][] Tiles { get; }
    int BoardSize { get; }
    ScoreBoard ScoreBoard { get; }
    void AddRandomTile();
    bool Move(Direction direction);
    void Reset(int boardSize = 4);
    bool CanMove();
    bool HasWon();
}
