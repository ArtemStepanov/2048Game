using _2048Game.Core;

namespace _2048Game.Services.Abstractions;

public interface ITileControlService
{
    int MergeScore { get; }
    bool Move(Direction direction);
}