using _2048Game.Models;
using _2048Game.Models.Abstractions;
using _2048Game.Services;
using _2048Game.Services.Abstractions;

namespace _2048Game.Tests.Services;

public sealed class RenderServiceTests : IDisposable
{
    private readonly ConsoleOutput _consoleOutput;
    private readonly Mock<IBoard> _mockBoard;
    private readonly Mock<IConsoleService> _mockConsoleService;
    private readonly RenderService _renderService;

    public RenderServiceTests()
    {
        _mockBoard = new Mock<IBoard>();
        _mockConsoleService = new Mock<IConsoleService>();
        _renderService = new RenderService(_mockBoard.Object, new ScoreBoard(), _mockConsoleService.Object);
        _consoleOutput = new ConsoleOutput();
    }

    public void Dispose()
    {
        _consoleOutput.Dispose();
    }

    [Fact]
    public void RenderBoard_Displays_Correct_Output()
    {
        _mockBoard.Setup(b => b.Size).Returns(4);
        _mockBoard.Setup(b => b.Tiles).Returns(new List<Tile>
        {
            new(0, 0, 2),
            new(1, 1, 4),
            new(2, 2, 8),
            new(3, 3, 16)
        });

        _mockConsoleService.Setup(c => c.Clear()).Callback(() => _consoleOutput.Clear());
        _mockConsoleService.Setup(c => c.WriteLine(It.IsAny<string>())).Callback<string>(s => _consoleOutput.WriteLine(s));
        _mockConsoleService.Setup(c => c.Write(It.IsAny<string>())).Callback<string>(s => _consoleOutput.Write(s));

        _renderService.RenderBoard();
        var output = _consoleOutput.GetOutput();

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

        Assert.Equal(expected, output);
        _mockConsoleService.Verify(c => c.Clear(), Times.Once);
        _mockConsoleService.Verify(c => c.WriteLine(It.IsAny<string>()), Times.Exactly(11));
        _mockConsoleService.Verify(c => c.Write(It.IsAny<string>()), Times.Exactly(20));
    }

    [Fact]
    public void RenderGameOver_Displays_Game_Over_Message()
    {
        _mockConsoleService.Setup(c => c.WriteLine(It.IsAny<string>())).Callback<string>(s => _consoleOutput.WriteLine(s));

        _renderService.RenderGameOver();
        var output = _consoleOutput.GetOutput();

        Assert.Equal("Game Over!" + Environment.NewLine, output);
        _mockConsoleService.Verify(c => c.WriteLine(It.IsAny<string>()), Times.Once);
    }

    [Theory]
    [InlineData(ConsoleKey.Y, true)]
    [InlineData(ConsoleKey.N, false)]
    public void ConfirmAction_Returns_Correct_Result(ConsoleKey key, bool expected)
    {
        _mockConsoleService.Setup(c => c.ReadKey(true)).Returns(new ConsoleKeyInfo('\0', key, false, false, false));
        _mockConsoleService.Setup(c => c.SetCursorPosition(It.IsAny<int>(), It.IsAny<int>())).Verifiable();
        _mockConsoleService.Setup(c => c.Write(It.IsAny<string>())).Verifiable();

        var result = _renderService.ConfirmAction("Restart game?");

        Assert.Equal(expected, result);
        _mockConsoleService.Verify(c => c.ReadKey(true), Times.AtLeastOnce);
        _mockConsoleService.Verify(c => c.SetCursorPosition(It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(2));
        _mockConsoleService.Verify(c => c.Write(It.IsAny<string>()), Times.Exactly(2));
    }
}