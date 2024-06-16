using _2048Game.Core;
using _2048Game.Services.Abstractions;

namespace _2048Game.Services;

public sealed class GameService(
    IBoardService boardService,
    IStorageService storageService,
    IRenderService renderService,
    IInputService inputService
) : IGameService
{
    public bool Running { get; private set; }

    public void StartGame()
    {
        Running = true;
        while (Running)
        {
            inputService.ListenKey();
            RenderBoard();
        }
    }

    public void ProcessStep(Direction direction)
    {
        var moved = boardService.Move(direction);

        switch (moved)
        {
            case true: // Process regular move
                HandleMove();
                break;

            case false when !boardService.CanMove(): // Game Over
            {
                HandleGameOver();
                break;
            }
        }

        RenderBoard();

        return;

        void HandleMove()
        {
            if (boardService.HasWon())
            {
                HandleWin();
                return;
            }

            boardService.AddRandomTile();
            SaveGame();
        }

        void HandleWin()
        {
            renderService.RenderWin();
            SaveGame();
            Running = false;
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
            Running = false;
        }
    }

    public void StartNewGame()
    {
        boardService.Reset();
        RenderBoard();
        SaveGame();
    }

    public void SaveGame()
    {
        storageService.SaveGame(boardService.Tiles, boardService.ScoreBoard);
    }

    public void ProcessExit()
    {
        // move logic to GameService
        if (renderService.ConfirmAction("Quit game?"))
        {
            SaveGame();
            Running = false;
        }
    }

    public void ProcessRestart()
    {
        if (renderService.ConfirmAction("Restart game?"))
        {
            StartNewGame();
        }
    }

    private void RenderBoard()
    {
        renderService.RenderBoard(boardService.Tiles, boardService.ScoreBoard, boardService.BoardSize);
    }
}