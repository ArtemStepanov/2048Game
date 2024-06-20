using _2048Game.Core;
using _2048Game.Models;
using _2048Game.Services;
using _2048Game.Services.Abstractions;

namespace _2048Game.Tests.Services;

public sealed class GameServiceTests
{
    private readonly GameService _gameService;
    private readonly Mock<IBoardService> _mockBoardService;
    private readonly Mock<IRenderService> _mockRenderService;
    private readonly Mock<IStorageService> _mockStorageService;

    public GameServiceTests()
    {
        _mockBoardService = new Mock<IBoardService>();
        _mockStorageService = new Mock<IStorageService>();
        _mockRenderService = new Mock<IRenderService>();

        _mockBoardService.Setup(b => b.CanMove()).Returns(true);

        _gameService = new GameService(
            _mockBoardService.Object,
            _mockStorageService.Object,
            _mockRenderService.Object
        );
    }

    [Fact]
    public void StartGame_Renders_Board()
    {
        _mockRenderService.Setup(r => r.RenderBoard(It.IsAny<int[,]>(), It.IsAny<ScoreBoard>(), It.IsAny<int>())).Throws<Exception>();
        Should.Throw<Exception>(_gameService.StartGame);
        _mockRenderService.Verify(r => r.RenderBoard(_mockBoardService.Object.Tiles, _mockBoardService.Object.ScoreBoard, _mockBoardService.Object.BoardSize), Times.Once);
    }

    [Fact]
    public void ProcessStep_Moves_Tile_And_Adds_Random_Tile_When_Move_Is_Valid()
    {
        _mockBoardService.Setup(t => t.Move(It.IsAny<Direction>())).Returns(true);

        var result = _gameService.ProcessStep(Direction.Left);
        result.ShouldBe(ProcessStepResult.RegularMove);

        _mockBoardService.Verify(t => t.Move(Direction.Left), Times.Once);
        _mockBoardService.Verify(b => b.AddRandomTile(), Times.Once);
        _mockStorageService.Verify(s => s.SaveGame(_mockBoardService.Object.Tiles, It.IsAny<int>(), _mockBoardService.Object.ScoreBoard), Times.Once);
        _mockRenderService.Verify(r => r.RenderBoard(_mockBoardService.Object.Tiles, _mockBoardService.Object.ScoreBoard, _mockBoardService.Object.BoardSize), Times.Once);
    }

    [Fact]
    public void ProcessStep_Handles_Game_Over_When_No_Moves_Left()
    {
        _mockBoardService.Setup(t => t.Move(It.IsAny<Direction>())).Returns(false);
        _mockBoardService.Setup(b => b.CanMove()).Returns(false);

        var result = _gameService.ProcessStep(Direction.Left);
        result.ShouldBe(ProcessStepResult.GameOver);

        _mockRenderService.Verify(r => r.RenderGameOver(), Times.Once);
    }

    [Fact]
    public void ProcessStep_Handles_Game_Over_Start_New_Game()
    {
        _mockBoardService.Setup(t => t.Move(It.IsAny<Direction>())).Returns(false);
        _mockBoardService.Setup(b => b.CanMove()).Returns(false);

        var result = _gameService.ProcessStep(Direction.Left);

        result.ShouldBe(ProcessStepResult.GameOver);

        _mockStorageService.Verify(s => s.SaveGame(_mockBoardService.Object.Tiles, It.IsAny<int>(), _mockBoardService.Object.ScoreBoard), Times.Once);
        _mockRenderService.Verify(r => r.RenderGameOver(), Times.Once);
    }

    [Fact]
    public void StartNewGame_Resets_Board_And_Score()
    {
        _gameService.RestartGame();

        _mockBoardService.Verify(b => b.Reset(4), Times.Once);
        _mockRenderService.Verify(r => r.RenderBoard(_mockBoardService.Object.Tiles, _mockBoardService.Object.ScoreBoard, _mockBoardService.Object.BoardSize), Times.Once);
        _mockStorageService.Verify(s => s.SaveGame(_mockBoardService.Object.Tiles, _mockBoardService.Object.BoardSize, _mockBoardService.Object.ScoreBoard), Times.Once);
    }

    [Fact]
    public void SaveGame_Saves_Current_Game_State()
    {
        _gameService.SaveGame();

        _mockStorageService.Verify(s => s.SaveGame(_mockBoardService.Object.Tiles, It.IsAny<int>(), _mockBoardService.Object.ScoreBoard), Times.Once);
    }
}