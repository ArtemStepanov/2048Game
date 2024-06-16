using _2048Game.Models;
using _2048Game.Services;
using _2048Game.Services.Abstractions;
using System.Text.Json;

namespace _2048Game.Tests.Services;

public sealed class StorageServiceTests : IDisposable
{
    private readonly string _saveFilePath;
    private readonly StorageService _storageService;
    private readonly string _tempAppDataPath;

    public StorageServiceTests()
    {
        _tempAppDataPath = Path.Combine(Path.GetTempPath(), "2048Game");
        _saveFilePath = Path.Combine(_tempAppDataPath, "savegame.json");
        _storageService = new StorageService(_tempAppDataPath);
    }

    [Fact]
    public void Constructor_Creates_AppData_Directory()
    {
        Directory.Exists(_tempAppDataPath).ShouldBeTrue();
    }

    [Fact]
    public void Constructor_If_Empty_AppDataPath_Then_AppData_Is_Used()
    {
        var appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "2048Game");
        _ = new StorageService(string.Empty);
        Directory.Exists(appDataPath).ShouldBeTrue();
    }

    [Fact]
    public void SaveGame_Creates_Save_Files()
    {
        var mockBoardService = new Mock<IBoardService>();

        mockBoardService.Setup(b => b.Tiles).Returns(new int[4,4]);
        mockBoardService.Setup(b => b.BoardSize).Returns(4);
        mockBoardService.Setup(b => b.ScoreBoard).Returns(new ScoreBoard());

        _storageService.SaveGame(mockBoardService.Object.Tiles, mockBoardService.Object.ScoreBoard);

        File.Exists(_saveFilePath).ShouldBeTrue();
    }

    [Fact]
    public void LoadGame_Loads_Saved_Files()
    {
        var board = new BoardService(scoreBoard: new ScoreBoard
        {
            Score = 100,
            BestScore = 200
        });

        _storageService.SaveGame(board.Tiles, board.ScoreBoard);

        var (loadedTiles, loadedScoreBoard) = _storageService.LoadGame();

        loadedTiles.ShouldNotBeNull();
        loadedScoreBoard.ShouldNotBeNull();
        loadedTiles.Length.ShouldBe(board.Tiles.Length);
        loadedScoreBoard.Score.ShouldBe(board.ScoreBoard.Score);
        loadedScoreBoard.BestScore.ShouldBe(board.ScoreBoard.BestScore);
        loadedTiles.ShouldBe(board.Tiles);
    }

    [Fact]
    public void ResetGameSave_Deletes_Save_File()
    {
        var scoreBoard = new ScoreBoard
        {
            Score = 100,
            BestScore = 200
        };

        _storageService.SaveGame(new int[4,4], scoreBoard);
        _storageService.ResetGameSave();

        File.Exists(_saveFilePath).ShouldBeFalse();

        var loadedScoreBoard = JsonSerializer.Deserialize<StorageService.GameSave>(File.ReadAllText(_saveFilePath));
        loadedScoreBoard.ShouldNotBeNull();
        loadedScoreBoard.ScoreBoard.Score.ShouldBe(0);
        loadedScoreBoard.ScoreBoard.BestScore.ShouldBe(scoreBoard.BestScore);
    }

    [Fact]
    public void ResetGameSave_If_ScoreBoard_NotExist_Returns()
    {
        File.WriteAllText(_saveFilePath, JsonSerializer.Serialize(new StorageService.GameSave(new int[4,4], new ScoreBoard())));

        _storageService.ResetGameSave();

        File.Exists(_saveFilePath).ShouldBeTrue();
    }

    public void Dispose()
    {
        if (Directory.Exists(_tempAppDataPath))
        {
            Directory.Delete(_tempAppDataPath, true);
        }
    }
}