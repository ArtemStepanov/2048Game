using _2048Game.Models;
using _2048Game.Services.Abstractions;
using System.Text.Json;

namespace _2048Game.Services;

public sealed class StorageService : IStorageService
{
    private readonly string _saveFilePath;

    public StorageService(string appDataPath = "")
    {
        appDataPath = string.IsNullOrEmpty(appDataPath)
            ? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "2048Game")
            : appDataPath;

        _saveFilePath = Path.Combine(appDataPath, "savegame.json");

        if (!Directory.Exists(appDataPath))
        {
            Directory.CreateDirectory(appDataPath);
        }
    }

    public void SaveGame(int[,] tiles, ScoreBoard scoreBoard)
    {
        var gameSave = new GameSave(tiles, scoreBoard);
        var saveData = JsonSerializer.Serialize(gameSave);
        File.WriteAllText(_saveFilePath, saveData);
    }

    public (int[,]? Tiles, ScoreBoard? ScoreBoard) LoadGame()
    {
        GameSave? gameSave = null;
        if (File.Exists(_saveFilePath))
        {
            gameSave = ReadJsonFile<GameSave>(_saveFilePath);
        }

        return (gameSave?.Tiles, gameSave?.ScoreBoard);
    }

    public void ResetGameSave()
    {
        if (!File.Exists(_saveFilePath))
        {
            return;
        }

        // Reset score, but not the record score, in the saved file
        var gameSave = ReadJsonFile<GameSave>(_saveFilePath);
        gameSave!.ResetSave();
        File.WriteAllText(_saveFilePath, JsonSerializer.Serialize(gameSave));
    }

    private static T? ReadJsonFile<T>(string path) where T : class
    {
        // Move read logic to StorageService
        return JsonSerializer.Deserialize<T>(File.ReadAllText(path));
    }

    public record GameSave(int[,] Tiles, ScoreBoard ScoreBoard)
    {
        internal void ResetSave()
        {
            ScoreBoard.Reset();
        }
    }
}