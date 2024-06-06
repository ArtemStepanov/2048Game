using _2048Game.Models;
using _2048Game.Models.Abstractions;

namespace _2048Game.Services.Abstractions;

public interface IStorageService
{
    void SaveGame(IBoard board, ScoreBoard scoreBoard);
    (IBoard?, ScoreBoard?) LoadGame();
    void ResetGameSave();
}