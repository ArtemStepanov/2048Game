namespace _2048Game.Models;

public sealed class ScoreBoard
{
    public int Score { get; set; }
    public int BestScore { get; set; }

    public void AddScore(int score)
    {
        Score += score;
        if (Score > BestScore)
        {
            BestScore = Score;
        }
    }

    public void Reset()
    {
        Score = 0;
    }
}