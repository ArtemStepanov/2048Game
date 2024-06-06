using _2048Game.Core;
using _2048Game.Core.Exceptions;
using _2048Game.Models;
using _2048Game.Models.Abstractions;
using _2048Game.Services;
using _2048Game.Services.Abstractions;

namespace _2048Game.Tests.Services;

public sealed class GameServiceTests
{
    private readonly GameService _gameService;
    private readonly Mock<IBoard> _mockBoard;
    private readonly Mock<IRenderService> _mockRenderService;
    private readonly Mock<IStorageService> _mockStorageService;
    private readonly Mock<ITileControlService> _mockTileControlService;
    private readonly ScoreBoard _scoreBoard;

    public GameServiceTests()
    {
        _mockBoard = new Mock<IBoard>();
        _scoreBoard = new ScoreBoard();
        _mockTileControlService = new Mock<ITileControlService>();
        _mockStorageService = new Mock<IStorageService>();
        _mockRenderService = new Mock<IRenderService>();

        _mockBoard.Setup(b => b.CanMove()).Returns(true);

        _gameService = new GameService(
            _mockBoard.Object,
            _scoreBoard,
            _mockTileControlService.Object,
            _mockStorageService.Object,
            _mockRenderService.Object
        );
    }

    [Fact]
    public void StartGame_Renders_Board()
    {
        _gameService.StartGame();
        _mockRenderService.Verify(r => r.RenderBoard(), Times.Once);
    }

    [Fact]
    public void ProcessStep_Moves_Tile_And_Adds_Random_Tile_When_Move_Is_Valid()
    {
        _mockTileControlService.Setup(t => t.Move(It.IsAny<Direction>())).Returns(true);

        _gameService.ProcessStep(Direction.Left);

        _mockTileControlService.Verify(t => t.Move(Direction.Left), Times.Once);
        _mockBoard.Verify(b => b.AddRandomTile(), Times.Once);
        Assert.Equal(0, _scoreBoard.Score);
        _mockStorageService.Verify(s => s.SaveGame(_mockBoard.Object, _scoreBoard), Times.Once);
        _mockRenderService.Verify(r => r.RenderBoard(), Times.Once);
    }

    [Fact]
    public void ProcessStep_Does_Not_Move_When_Direction_Is_Null()
    {
        _gameService.ProcessStep(null);

        _mockTileControlService.Verify(t => t.Move(It.IsAny<Direction>()), Times.Never);
        _mockBoard.Verify(b => b.AddRandomTile(), Times.Never);
        Assert.Equal(0, _scoreBoard.Score);
        _mockStorageService.Verify(s => s.SaveGame(It.IsAny<IBoard>(), It.IsAny<ScoreBoard>()), Times.Never);
        _mockRenderService.Verify(r => r.RenderBoard(), Times.Once);
    }

    [Fact]
    public void ProcessStep_Handles_Game_Over_When_No_Moves_Left()
    {
        _mockTileControlService.Setup(t => t.Move(It.IsAny<Direction>())).Returns(false);
        _mockBoard.Setup(b => b.CanMove()).Returns(false);
        _mockRenderService.Setup(r => r.ConfirmAction(It.IsAny<string>())).Returns(false);

        Assert.Throws<GameExitException>(() => _gameService.ProcessStep(Direction.Left));

        _mockRenderService.Verify(r => r.RenderGameOver(), Times.Once);
        _mockStorageService.Verify(s => s.ResetGameSave(), Times.Once);
    }

    [Fact]
    public void ProcessStep_Handles_Game_Over_Start_New_Game()
    {
        _mockTileControlService.Setup(t => t.Move(It.IsAny<Direction>())).Returns(false);
        _mockBoard.Setup(b => b.CanMove()).Returns(false);
        _mockRenderService.Setup(r => r.ConfirmAction(It.IsAny<string>())).Returns(true);

        _gameService.ProcessStep(Direction.Left);

        _mockBoard.Verify(b => b.Reset(It.IsAny<int>()), Times.Once);
        _mockRenderService.Verify(r => r.RenderGameOver(), Times.Once);
        _mockRenderService.Verify(r => r.RenderBoard(), Times.Exactly(2));
        _mockStorageService.Verify(s => s.SaveGame(_mockBoard.Object, _scoreBoard), Times.Once);
    }

    [Fact]
    public void StartNewGame_Resets_Board_And_Score()
    {
        _gameService.StartNewGame();

        _mockBoard.Verify(b => b.Reset(4), Times.Once);
        Assert.Equal(0, _scoreBoard.Score);
        _mockRenderService.Verify(r => r.RenderBoard(), Times.Once);
        _mockStorageService.Verify(s => s.SaveGame(_mockBoard.Object, _scoreBoard), Times.Once);
    }

    [Fact]
    public void SaveGame_Saves_Current_Game_State()
    {
        _gameService.SaveGame();

        _mockStorageService.Verify(s => s.SaveGame(_mockBoard.Object, _scoreBoard), Times.Once);
    }
}