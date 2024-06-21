using _2048Game.Core;

namespace _2048Game.Services.Abstractions;

public interface IGameService
{
    bool Running { get; }
    void StartGame();
    ProcessStepResult ProcessStep(Direction direction);
    void SaveGame();
    void StopGame();
    void RestartGame();
}