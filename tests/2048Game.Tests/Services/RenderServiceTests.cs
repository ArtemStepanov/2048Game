using _2048Game.Models;
using _2048Game.Services;
using _2048Game.Services.Abstractions;
using System.Text;

namespace _2048Game.Tests.Services;

public sealed class RenderServiceTests
{
    private readonly StringBuilder _stringBuilder;
    private readonly Mock<IBoardService> _mockBoard;
    private readonly Mock<IConsoleService> _mockConsoleService;
    private readonly RenderService _renderService;

    public RenderServiceTests()
    {
        _mockBoard = new Mock<IBoardService>();
        _mockConsoleService = new Mock<IConsoleService>();
        _renderService = new RenderService(_mockConsoleService.Object);
        _stringBuilder = new StringBuilder();
        MockConsole();
    }

    private void MockConsole()
    {
        _mockConsoleService.Setup(c => c.Clear()).Callback(() => _stringBuilder.Clear());
        _mockConsoleService.Setup(c => c.WriteLine(It.IsAny<string>())).Callback<string>(s => _stringBuilder.AppendLine(s));
        _mockConsoleService.Setup(c => c.Write(It.IsAny<string>())).Callback<string>(s => _stringBuilder.Append(s));
    }

    [Fact]
    public void RenderBoard_Displays_Correct_Output()
    {
        _mockBoard.Setup(b => b.BoardSize).Returns(4);
        _mockBoard.Setup(b => b.Tiles).Returns(
        [
            [ 2, 0, 0, 0 ],
            [ 0, 4, 0, 0 ],
            [ 0, 0, 8, 0 ],
            [ 0, 0, 0, 16]
        ]);

        _mockBoard.Setup(b => b.ScoreBoard).Returns(new ScoreBoard());

        _renderService.RenderBoard(_mockBoard.Object.Tiles, _mockBoard.Object.ScoreBoard, _mockBoard.Object.BoardSize);
        var output = _stringBuilder.ToString();

        const string expected = """
                                Score: 0
                                Best: 0
                                -----------------------------
                                |     2|      |      |      |
                                -----------------------------
                                |      |     4|      |      |
                                -----------------------------
                                |      |      |     8|      |
                                -----------------------------
                                |      |      |      |    16|
                                -----------------------------

                                """;

        output.ShouldBe(expected);
        _mockConsoleService.Verify(c => c.Clear(), Times.Once);
        _mockConsoleService.Verify(c => c.WriteLine(It.IsAny<string>()), Times.Exactly(11));
        _mockConsoleService.Verify(c => c.Write(It.IsAny<string>()), Times.Exactly(20));
    }

    [Fact]
    public void RenderGameOver_Displays_Game_Over_Message()
    {
        _renderService.RenderGameOver();
        var output = _stringBuilder.ToString();

        output.ShouldBe("Game Over!" + Environment.NewLine);
        _mockConsoleService.Verify(c => c.WriteLine(It.IsAny<string>()), Times.Once);
    }
}