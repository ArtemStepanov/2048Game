﻿using _2048Game.Models;

namespace _2048Game.Services.Abstractions;

public interface IStorageService
{
    void SaveGame(int[,] tiles, ScoreBoard scoreBoard);
    (int[,]? Tiles, ScoreBoard? ScoreBoard) LoadGame();
    void ResetGameSave();
}