using _2048Game.Models;
using _2048Game.Models.Abstractions;
using _2048Game.Services.Abstractions;
using System.Text.Json;

namespace _2048Game.Services;

public sealed class StorageService : IStorageService
{
    private readonly string _saveFilePath;
    private readonly string _scoreBoardPath;

    public StorageService(string appDataPath = "")
    {
        appDataPath = string.IsNullOrEmpty(appDataPath)
            ? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "2048Game")
            : appDataPath;

        _saveFilePath = Path.Combine(appDataPath, "savegame.json");
        _scoreBoardPath = Path.Combine(appDataPath, "scoreboard.json");

        if (!Directory.Exists(appDataPath))
        {
            Directory.CreateDirectory(appDataPath);
        }
    }

    public void SaveGame(IBoard board, ScoreBoard scoreBoard)
    {
        var gameData = JsonSerializer.Serialize(board);
        var scoreData = JsonSerializer.Serialize(scoreBoard);
        File.WriteAllText(_saveFilePath, gameData);
        File.WriteAllText(_scoreBoardPath, scoreData);
    }

    public (IBoard?, ScoreBoard?) LoadGame()
    {
        Board? boardSavedData = null;
        if (File.Exists(_saveFilePath))
        {
            boardSavedData = ReadJsonFile<Board>(_saveFilePath);
        }

        ScoreBoard? scoreBoardSavedData = null;
        if (File.Exists(_scoreBoardPath))
        {
            scoreBoardSavedData = ReadJsonFile<ScoreBoard>(_scoreBoardPath);
        }

        return (boardSavedData, scoreBoardSavedData);
    }

    public void ResetGameSave()
    {
        if (File.Exists(_saveFilePath))
        {
            File.Delete(_saveFilePath);
        }

        if (!File.Exists(_scoreBoardPath))
        {
            return;
        }

        // Reset score, but not the record score, in the saved file
        var scoreBoard = ReadJsonFile<ScoreBoard>(_scoreBoardPath)!;

        scoreBoard.Reset();
        File.WriteAllText(_scoreBoardPath, JsonSerializer.Serialize(scoreBoard));
    }

    private static T? ReadJsonFile<T>(string path) where T : class
    {
        return JsonSerializer.Deserialize<T>(File.ReadAllText(path));
    }
}