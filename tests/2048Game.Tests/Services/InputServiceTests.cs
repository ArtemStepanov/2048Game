using _2048Game.Core;
using _2048Game.Services;
using _2048Game.Services.Abstractions;
using System.Text;

namespace _2048Game.Tests.Services;

public sealed class InputServiceTests
{
    private readonly StringBuilder _stringBuilder;
    private readonly InputService _inputService;
    private readonly Mock<IConsoleService> _mockConsoleService;
    private readonly Mock<IGameService> _mockGameService;

    public InputServiceTests()
    {
        _mockGameService = new Mock<IGameService>();
        _mockConsoleService = new Mock<IConsoleService>();
        _inputService = new InputService(_mockGameService.Object, _mockConsoleService.Object);
        _stringBuilder = new StringBuilder();
        MockConsoleService();
        MockGameService();
    }

    private void MockGameService()
    {
        _mockGameService.Setup(g => g.StartGame()).Callback(() => _mockGameService.Setup(gg => gg.Running).Returns(true));
    }

    private void MockConsoleService()
    {
        _mockConsoleService.Setup(c => c.WriteLine(It.IsAny<string>())).Callback<string>(s => _stringBuilder.AppendLine(s));
        _mockConsoleService.Setup(c => c.Write(It.IsAny<string>())).Callback<string>(s => _stringBuilder.Append(s));
        _mockConsoleService.Setup(c => c.ReadKey(true)).Returns(new ConsoleKeyInfo());
    }

    [Theory]
    [InlineData(ConsoleKey.UpArrow, Direction.Up)]
    [InlineData(ConsoleKey.DownArrow, Direction.Down)]
    [InlineData(ConsoleKey.LeftArrow, Direction.Left)]
    [InlineData(ConsoleKey.RightArrow, Direction.Right)]
    public void HandleInput_Processes_Arrow_Keys(ConsoleKey key, Direction direction)
    {
        _mockConsoleService.Setup(c => c.ReadKey(true))
            .Returns(new ConsoleKeyInfo('\0', key, false, false, false));

        _mockGameService.Setup(g => g.ProcessStep(direction))
            .Returns(ProcessStepResult.RegularMove)
            .Callback(() => _mockGameService.Setup(gg => gg.Running).Returns(false));

        _inputService.StartGameAndListenInput();
        _stringBuilder.Length.ShouldBe(0);

        _mockGameService.Verify(g => g.StartGame(), Times.Once);
        _mockGameService.Verify(g => g.ProcessStep(direction), Times.Once);
    }

    [Fact]
    public void HandleInput_Restarts_Game_On_R_Key()
    {
        _mockConsoleService.SetupSequence(c => c.ReadKey(true))
            .Returns(new ConsoleKeyInfo('R', ConsoleKey.R, false, false, false))
            .Returns(new ConsoleKeyInfo('Y', ConsoleKey.Y, false, false, false));

        _mockGameService.Setup(g => g.RestartGame())
            .Callback(() => _mockGameService.Setup(gg => gg.Running).Returns(false));

        _inputService.StartGameAndListenInput();

        _mockGameService.Verify(g => g.StartGame(), Times.Once);
        _mockGameService.Verify(g => g.RestartGame(), Times.Once);
    }

    [Fact]
    public void HandleInput_Quits_Game_On_Q_Key()
    {
        _mockConsoleService.SetupSequence(c => c.ReadKey(true))
            .Returns(new ConsoleKeyInfo('Q', ConsoleKey.Q, false, false, false))
            .Returns(new ConsoleKeyInfo('Y', ConsoleKey.Y, false, false, false));

        _mockGameService.Setup(g => g.StopGame())
            .Callback(() => _mockGameService.Setup(gg => gg.Running).Returns(false));

        _inputService.StartGameAndListenInput();

        _mockGameService.Verify(g => g.StartGame(), Times.Once);
        _mockGameService.Verify(g => g.StopGame(), Times.Once);
    }

    [Fact]
    public void ListenInput_Invalid_Key_Displays_Message()
    {
        const ConsoleKey invalidKey = ConsoleKey.A;
        _mockConsoleService.Setup(c => c.ReadKey(true))
            .Returns(new ConsoleKeyInfo('A', invalidKey, false, false, false))
            .Callback(() => _mockGameService.Setup(gg => gg.Running).Returns(false));

        _inputService.StartGameAndListenInput();

        _stringBuilder.ToString()
            .ShouldBe("Invalid input. Use arrow keys to move, R to restart, Q to quit." + Environment.NewLine);
        _mockGameService.Verify(g => g.StartGame(), Times.Once);
    }

    [Theory]
    [InlineData(ConsoleKey.Y, true)]
    [InlineData(ConsoleKey.N, false)]
    public void ConfirmAction_Returns_Correct_Result(ConsoleKey key, bool expected)
    {
        _mockConsoleService.Setup(c => c.ReadKey(true)).Returns(new ConsoleKeyInfo('\0', key, false, false, false));

        var result = _inputService.ConfirmAction("Restart game?");

        result.ShouldBe(expected);
        _mockConsoleService.Verify(c => c.ReadKey(true), Times.Once);
        _mockConsoleService.Verify(c => c.SetCursorPosition(It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(2));
        _mockConsoleService.Verify(c => c.Write(It.IsAny<string>()), Times.Exactly(2));
    }
}