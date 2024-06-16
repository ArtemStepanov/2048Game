using _2048Game.Core;

namespace _2048Game.Services.Abstractions;

public interface IGameService
{
    void StartGame();
    void ProcessStep(Direction direction);
    void StartNewGame();
    void SaveGame();
    void ProcessExit();
    void ProcessRestart();
}