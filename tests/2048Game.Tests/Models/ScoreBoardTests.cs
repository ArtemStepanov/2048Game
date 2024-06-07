using _2048Game.Models;

namespace _2048Game.Tests.Models;

public sealed class ScoreBoardTests
{
    [Fact]
    public void AddScore_Updates_Score_And_BestScore()
    {
        var scoreBoard = new ScoreBoard();
        scoreBoard.AddScore(10);
        scoreBoard.Score.ShouldBe(10);
        scoreBoard.BestScore.ShouldBe(10);

        scoreBoard.AddScore(5);
        scoreBoard.Score.ShouldBe(15);
        scoreBoard.BestScore.ShouldBe(15);
    }

    [Fact]
    public void ResetScore_Resets_Score_To_Zero()
    {
        var scoreBoard = new ScoreBoard();
        scoreBoard.AddScore(10);
        scoreBoard.Reset();
        scoreBoard.Score.ShouldBe(0);
    }

    [Fact]
    public void ResetScore_Keep_BestScore_After_Reset()
    {
        var scoreBoard = new ScoreBoard();
        scoreBoard.AddScore(10);
        scoreBoard.Reset();
        scoreBoard.Score.ShouldBe(0);
        scoreBoard.BestScore.ShouldBe(10);
    }
}