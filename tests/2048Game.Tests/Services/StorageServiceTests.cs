using _2048Game.Models;
using _2048Game.Models.Abstractions;
using _2048Game.Services;
using System.Text.Json;

namespace _2048Game.Tests.Services;

public sealed class StorageServiceTests : IDisposable
{
    private readonly string _saveFilePath;
    private readonly string _scoreBoardPath;
    private readonly StorageService _storageService;
    private readonly string _tempAppDataPath;

    public StorageServiceTests()
    {
        _tempAppDataPath = Path.Combine(Path.GetTempPath(), "2048Game");
        _saveFilePath = Path.Combine(_tempAppDataPath, "savegame.json");
        _scoreBoardPath = Path.Combine(_tempAppDataPath, "scoreboard.json");
        _storageService = new StorageService(_tempAppDataPath);
    }

    [Fact]
    public void Constructor_Creates_AppData_Directory()
    {
        Assert.True(Directory.Exists(_tempAppDataPath));
    }

    [Fact]
    public void Constructor_If_Empty_AppDataPath_Then_AppData_Is_Used()
    {
        var appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "2048Game");
        _ = new StorageService(string.Empty);
        Assert.True(Directory.Exists(appDataPath));
    }

    [Fact]
    public void SaveGame_Creates_Save_Files()
    {
        var mockBoard = new Mock<IBoard>();
        var scoreBoard = new ScoreBoard();

        mockBoard.Setup(b => b.Tiles).Returns(new List<Tile>());
        mockBoard.Setup(b => b.Size).Returns(4);

        _storageService.SaveGame(mockBoard.Object, scoreBoard);

        Assert.True(File.Exists(_saveFilePath));
        Assert.True(File.Exists(_scoreBoardPath));
    }

    [Fact]
    public void LoadGame_Loads_Saved_Files()
    {
        var board = new Board();
        var scoreBoard = new ScoreBoard
        {
            Score = 100,
            BestScore = 200
        };

        File.WriteAllText(_saveFilePath, JsonSerializer.Serialize(board));
        File.WriteAllText(_scoreBoardPath, JsonSerializer.Serialize(scoreBoard));

        var (loadedBoard, loadedScoreBoard) = _storageService.LoadGame();

        Assert.NotNull(loadedBoard);
        Assert.NotNull(loadedScoreBoard);
        Assert.Equal(board.Size, loadedBoard.Size);
        Assert.Equal(scoreBoard.Score, loadedScoreBoard.Score);
        Assert.Equal(scoreBoard.BestScore, loadedScoreBoard.BestScore);
        Assert.Equal(
            board.Tiles.Select(t => new { t.Row, t.Column, t.Value }),
            loadedBoard.Tiles.Select(t => new { t.Row, t.Column, t.Value })
        );
    }

    [Fact]
    public void ResetGameSave_Deletes_Save_File()
    {
        var scoreBoard = new ScoreBoard
        {
            Score = 100,
            BestScore = 200
        };

        File.WriteAllText(_saveFilePath, JsonSerializer.Serialize(new Board()));
        File.WriteAllText(_scoreBoardPath, JsonSerializer.Serialize(scoreBoard));

        _storageService.ResetGameSave();

        Assert.False(File.Exists(_saveFilePath));
        Assert.True(File.Exists(_scoreBoardPath));

        var loadedScoreBoard = JsonSerializer.Deserialize<ScoreBoard>(File.ReadAllText(_scoreBoardPath));
        Assert.NotNull(loadedScoreBoard);
        Assert.Equal(0, loadedScoreBoard.Score);
        Assert.Equal(scoreBoard.BestScore, loadedScoreBoard.BestScore);
    }

    [Fact]
    public void ResetGameSave_If_ScoreBoard_NotExist_Returns()
    {
        File.WriteAllText(_saveFilePath, JsonSerializer.Serialize(new Board()));
        File.Delete(_scoreBoardPath);

        _storageService.ResetGameSave();

        Assert.False(File.Exists(_saveFilePath));
        Assert.False(File.Exists(_scoreBoardPath));
    }

    public void Dispose()
    {
        if (Directory.Exists(_tempAppDataPath))
        {
            Directory.Delete(_tempAppDataPath, true);
        }
    }
}