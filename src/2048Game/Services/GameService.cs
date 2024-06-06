using _2048Game.Core;
using _2048Game.Core.Exceptions;
using _2048Game.Models;
using _2048Game.Models.Abstractions;
using _2048Game.Services.Abstractions;

namespace _2048Game.Services;

public sealed class GameService(
    IBoard board,
    ScoreBoard scoreBoard,
    ITileControlService tileControlService,
    IStorageService storageService,
    IRenderService renderService
) : IGameService
{
    public void StartGame()
    {
        renderService.RenderBoard();
    }

    public void ProcessStep(Direction? direction)
    {
        var moved = false;

        if (direction.HasValue)
        {
            moved = tileControlService.Move(direction.Value);
        }

        switch (moved)
        {
            case true: // Process regular move
                HandleMove();
                break;

            case false when !board.CanMove(): // Game Over
            {
                HandleGameOver();
                break;
            }
        }

        renderService.RenderBoard();

        return;

        void HandleMove()
        {
            board.AddRandomTile();
            scoreBoard.AddScore(tileControlService.MergeScore);
            SaveGame();
        }

        void HandleGameOver()
        {
            renderService.RenderGameOver();
            if (renderService.ConfirmAction("Start a new game?"))
            {
                StartNewGame();
                return;
            }

            // Save empty board and score before exit
            storageService.ResetGameSave();
            throw new GameExitException();
        }
    }

    public void StartNewGame()
    {
        board.Reset();
        scoreBoard.Reset();
        renderService.RenderBoard();
        SaveGame();
    }

    public void SaveGame()
    {
        storageService.SaveGame(board, scoreBoard);
    }
}