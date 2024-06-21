using _2048Game.Core;
using _2048Game.Services.Abstractions;

namespace _2048Game.Services;

public sealed class GameService(
    IBoardService boardService,
    IStorageService storageService,
    IRenderService renderService
) : IGameService
{
    public bool Running { get; private set; }

    public void StartGame()
    {
        Running = true;
        RenderBoard();
    }

    public ProcessStepResult ProcessStep(Direction direction)
    {
        var moved = boardService.Move(direction);

        SaveGame();

        switch (moved)
        {
            case true: // Process regular move
                if (boardService.HasWon())
                {
                    renderService.RenderWin();
                    return ProcessStepResult.Win;
                }

                RenderBoard();
                return ProcessStepResult.RegularMove;

            case false when !boardService.CanMove(): // Game Over
            {
                renderService.RenderGameOver();
                return ProcessStepResult.GameOver;
            }

            default:
                return ProcessStepResult.RegularMove;
        }
    }

    public void SaveGame()
    {
        storageService.SaveGame(boardService.Tiles, boardService.BoardSize, boardService.ScoreBoard);
    }

    public void StopGame()
    {
        SaveGame();
        Running = false;
    }

    public void RestartGame()
    {
        boardService.Reset();
        RenderBoard();
        SaveGame();
    }

    private void RenderBoard()
    {
        renderService.RenderBoard(boardService.Tiles, boardService.ScoreBoard, boardService.BoardSize);
    }
}