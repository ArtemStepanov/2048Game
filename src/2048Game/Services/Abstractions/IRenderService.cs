namespace _2048Game.Services.Abstractions;

public interface IRenderService
{
    void RenderBoard();
    void RenderGameOver();
    bool ConfirmAction(string message);
}