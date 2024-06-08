using _2048Game.Core;
using _2048Game.Services;
using _2048Game.Services.Abstractions;

namespace _2048Game.Tests.Services;

public sealed class InputServiceTests : IDisposable
{
    private readonly ConsoleOutput _consoleOutput;
    private readonly InputService _inputService;
    private readonly Mock<IConsoleService> _mockConsoleService;
    private readonly Mock<IGameService> _mockGameService;
    private readonly Mock<IRenderService> _mockRenderService;

    public InputServiceTests()
    {
        _mockGameService = new Mock<IGameService>();
        _mockRenderService = new Mock<IRenderService>();
        _mockConsoleService = new Mock<IConsoleService>();
        _inputService = new InputService(_mockGameService.Object, _mockRenderService.Object, _mockConsoleService.Object);
        _consoleOutput = new ConsoleOutput();
        _mockConsoleService.Setup(c => c.WriteLine(It.IsAny<string>())).Callback<string>(s => _consoleOutput.WriteLine(s));
    }

    public void Dispose()
    {
        _consoleOutput.Dispose();
    }

    [Theory]
    [InlineData(ConsoleKey.UpArrow, Direction.Up)]
    [InlineData(ConsoleKey.DownArrow, Direction.Down)]
    [InlineData(ConsoleKey.LeftArrow, Direction.Left)]
    [InlineData(ConsoleKey.RightArrow, Direction.Right)]
    public void HandleInput_Processes_Arrow_Keys(ConsoleKey key, Direction direction)
    {
        _mockConsoleService.SetupSequence(c => c.ReadKey(true))
            .Returns(new ConsoleKeyInfo('\0', key, false, false, false));

        _inputService.HandleInput().ShouldBeTrue();
        _consoleOutput.GetOutput().ShouldBeEmpty();

        _mockGameService.Verify(g => g.ProcessStep(direction), Times.Once);
    }

    [Fact]
    public void HandleInput_Restarts_Game_On_R_Key()
    {
        _mockRenderService.Setup(r => r.ConfirmAction("Restart game?")).Returns(true);
        _mockConsoleService.SetupSequence(c => c.ReadKey(true))
            .Returns(new ConsoleKeyInfo('R', ConsoleKey.R, false, false, false));

        _inputService.HandleInput().ShouldBeTrue();

        _mockGameService.Verify(g => g.StartNewGame(), Times.Once);
    }

    [Fact]
    public void HandleInput_Quits_Game_On_Q_Key()
    {
        _mockRenderService.Setup(r => r.ConfirmAction("Quit game?")).Returns(true);
        _mockConsoleService.SetupSequence(c => c.ReadKey(true))
            .Returns(new ConsoleKeyInfo('Q', ConsoleKey.Q, false, false, false));

        _inputService.HandleInput().ShouldBeFalse();
        _consoleOutput.GetOutput().ShouldBe("Goodbye!" + Environment.NewLine);

        _mockGameService.Verify(g => g.SaveGame(), Times.Once);
    }

    [Fact]
    public void ListenInput_Invalid_Key_Displays_Message()
    {
        const ConsoleKey invalidKey = ConsoleKey.A;
        _mockConsoleService.SetupSequence(c => c.ReadKey(true))
            .Returns(new ConsoleKeyInfo('A', invalidKey, false, false, false));

        _inputService.HandleInput().ShouldBeTrue();

        _consoleOutput.GetOutput()
            .ShouldBe("Invalid input. Use arrow keys to move, R to restart, Q to quit." + Environment.NewLine);
    }
}