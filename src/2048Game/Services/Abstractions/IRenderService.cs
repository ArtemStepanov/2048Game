using _2048Game.Models;

namespace _2048Game.Services.Abstractions;

public interface IRenderService
{
    void RenderBoard(int[,] tiles, ScoreBoard scoreBoard, int boardSize);
    void RenderGameOver();
    void RenderWin();
}