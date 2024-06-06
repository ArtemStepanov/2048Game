using _2048Game.Models;

namespace _2048Game.Tests.Models;

public sealed class ScoreBoardTests
{
    [Fact]
    public void AddScore_Updates_Score_And_BestScore()
    {
        var scoreBoard = new ScoreBoard();
        scoreBoard.AddScore(10);
        Assert.Equal(10, scoreBoard.Score);
        Assert.Equal(10, scoreBoard.BestScore);

        scoreBoard.AddScore(5);
        Assert.Equal(15, scoreBoard.Score);
        Assert.Equal(15, scoreBoard.BestScore);
    }

    [Fact]
    public void ResetScore_Resets_Score_To_Zero()
    {
        var scoreBoard = new ScoreBoard();
        scoreBoard.AddScore(10);
        scoreBoard.Reset();
        Assert.Equal(0, scoreBoard.Score);
    }

    [Fact]
    public void ResetScore_Keep_BestScore_After_Reset()
    {
        var scoreBoard = new ScoreBoard();
        scoreBoard.AddScore(10);
        scoreBoard.Reset();
        Assert.Equal(0, scoreBoard.Score);
        Assert.Equal(10, scoreBoard.BestScore);
    }
}