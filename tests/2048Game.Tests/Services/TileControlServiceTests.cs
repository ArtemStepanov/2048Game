using _2048Game.Core;
using _2048Game.Models;
using _2048Game.Models.Abstractions;
using _2048Game.Services;

namespace _2048Game.Tests.Services;

public sealed class TileControlServiceTests
{
    private readonly Mock<IBoard> _boardMock;
    private readonly TileControlService _service;

    public TileControlServiceTests()
    {
        _boardMock = new Mock<IBoard>();
        _service = new TileControlService(_boardMock.Object);
    }

    [Theory]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    [InlineData(7)]
    [InlineData(8)]
    public void MoveLeft_Moves_Tiles_Left(int boardSize)
    {
        var tiles = new List<Tile>
        {
            new(0, 1, 2),
            new(0, 2, 2)
        };

        _boardMock.Setup(b => b.Tiles).Returns(tiles);
        _boardMock.Setup(b => b.Size).Returns(boardSize);

        var moved = _service.Move(Direction.Left);

        moved.ShouldBeTrue();
        tiles[0].Value.ShouldBe(4);
        tiles[0].Column.ShouldBe(0);
        _boardMock.Verify(b => b.RemoveTile(It.IsAny<Tile>()), Times.Once);
    }

    [Theory]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    [InlineData(7)]
    [InlineData(8)]
    public void MoveRight_Moves_Tiles_Right(int boardSize)
    {
        var tiles = new List<Tile>
        {
            new(0, 1, 2),
            new(0, 2, 2)
        };

        _boardMock.Setup(b => b.Tiles).Returns(tiles);
        _boardMock.Setup(b => b.Size).Returns(boardSize);

        var moved = _service.Move(Direction.Right);

        moved.ShouldBeTrue();
        tiles[1].Value.ShouldBe(4);
        tiles[1].Column.ShouldBe(boardSize - 1);
        _boardMock.Verify(b => b.RemoveTile(It.IsAny<Tile>()), Times.Once);
    }

    [Theory]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    [InlineData(7)]
    [InlineData(8)]
    public void MoveUp_Moves_Tiles_Up(int size)
    {
        var tiles = new List<Tile>
        {
            new(1, 0, 2),
            new(2, 0, 2)
        };

        _boardMock.Setup(b => b.Tiles).Returns(tiles);
        _boardMock.Setup(b => b.Size).Returns(size);

        var moved = _service.Move(Direction.Up);

        moved.ShouldBeTrue();
        tiles[0].Value.ShouldBe(4);
        tiles[0].Row.ShouldBe(0);
        _boardMock.Verify(b => b.RemoveTile(It.IsAny<Tile>()), Times.Once);
    }

    [Theory]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    [InlineData(7)]
    [InlineData(8)]
    public void MoveDown_Moves_Tiles_Down(int size)
    {
        var tiles = new List<Tile>
        {
            new(1, 0, 2),
            new(2, 0, 2)
        };

        _boardMock.Setup(b => b.Tiles).Returns(tiles);
        _boardMock.Setup(b => b.Size).Returns(size);

        var moved = _service.Move(Direction.Down);

        moved.ShouldBeTrue();
        tiles[1].Value.ShouldBe(4);
        tiles[1].Row.ShouldBe(size - 1);
        _boardMock.Verify(b => b.RemoveTile(It.IsAny<Tile>()), Times.Once);
    }
}