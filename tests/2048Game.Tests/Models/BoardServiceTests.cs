using _2048Game.Core;
using _2048Game.Services;
using _2048Game.Services.Abstractions;

namespace _2048Game.Tests.Models;

public sealed class BoardServiceTests
{
    [Fact]
    public void Board_Initializes_With_Sixteen_Default_Tiles()
    {
        var board = new BoardService();
        board.Tiles.Length.ShouldBe(4);
    }

    [Fact]
    public void Board_Only_Two_Tiles_By_Default_Has_Values()
    {
        var board = new BoardService();
        ValidatePositives(board, 2);
    }

    [Theory]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    [InlineData(7)]
    [InlineData(8)]
    public void Board_Creates_With_Valid_Size(int size)
    {
        var board = new BoardService(size: size);
        board.BoardSize.ShouldBe(size);
        board.Tiles.Length.ShouldBe(size);
    }

    [Fact]
    public void AddRandomTile_Adds_A_Tile()
    {
        var board = new BoardService();
        board.AddRandomTile();
        ValidatePositives(board, 3);
    }

    [Theory]
    [InlineData(3)]
    [InlineData(9)]
    public void Board_Throws_Exception_For_Invalid_Size(int size)
    {
        var ex = Assert.Throws<ArgumentException>(() => new BoardService(size: size));
        ex.Message.ShouldBe("Board size must be between 4 and 8.");
    }

    [Fact]
    public void Reset_Recreate_Tiles_And_Resets_Size()
    {
        var board = new BoardService();
        board.Reset(6);
        board.Tiles.Length.ShouldBe(6);
        board.BoardSize.ShouldBe(6);
    }

    [Fact]
    public void CanMove_Returns_True_If_Empty_Space()
    {
        var board = new BoardService();
        board.AddRandomTile(); // Adding a third tile to ensure there are empty spaces
        board.CanMove().ShouldBeTrue();
    }

    [Fact]
    public void CanMove_Returns_True_If_Merge_Possible()
    {
        var board = new BoardService(
        [
            [2, 0, 1, 2],
            [0, 0, 3, 4],
            [0, 0, 3, 4],
            [0, 0, 3, 4]
        ]);

        board.CanMove().ShouldBeTrue();
    }

    [Fact]
    public void CanMove_Returns_False_If_No_Merge_Possible_And_No_Empty_Space()
    {
        var board = new BoardService();

        for (var row = 0; row < board.BoardSize; row++)
        {
            for (var col = 0; col < board.BoardSize; col++)
            {
                board.Tiles[row][col] = (row * 4 + col + 1) * 2;
            }
        }

        board.CanMove().ShouldBeFalse();
    }

    [Theory]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    [InlineData(7)]
    [InlineData(8)]
    public void MoveLeft_Moves_Tiles_Left(int boardSize)
    {
        var tiles = InitializeArray(boardSize);
        tiles[0][0] = 2;
        tiles[0][1] = 2;
        tiles[0][2] = 3;

        var boardService = new BoardService(tiles, size: boardSize);

        var moved = boardService.Move(Direction.Left);

        moved.ShouldBeTrue();

        boardService.Tiles[0][0].ShouldBe(4);
        boardService.Tiles[0][1].ShouldBe(3);
    }

    [Theory]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    [InlineData(7)]
    [InlineData(8)]
    public void MoveRight_Moves_Tiles_Right(int boardSize)
    {
        var tiles = InitializeArray(boardSize);
        tiles[0][0] = 3;
        tiles[0][1] = 2;
        tiles[0][2] = 2;

        var boardService = new BoardService(tiles, size: boardSize);

        var moved = boardService.Move(Direction.Right);

        moved.ShouldBeTrue();

        boardService.Tiles[0][boardSize - 1].ShouldBe(4);
        boardService.Tiles[0][boardSize - 2].ShouldBe(3);
    }

    [Fact]
    public void MoveRight_Moves_Tiles_Right_WithThreeValuesOnTheRow()
    {
        var tiles = InitializeArray(4);
        tiles[0][0] = 2;
        tiles[0][1] = 2;
        tiles[0][2] = 2;

        var boardService = new BoardService(tiles);

        var moved = boardService.Move(Direction.Right);

        moved.ShouldBeTrue();

        boardService.Tiles[0][3].ShouldBe(4);
        boardService.Tiles[0][2].ShouldBe(2);
    }

    [Theory]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    [InlineData(7)]
    [InlineData(8)]
    public void MoveUp_Moves_Tiles_Up(int boardSize)
    {
        var tiles = InitializeArray(boardSize);
        tiles[0][0] = 2;
        tiles[1][0] = 2;
        tiles[2][0] = 3;

        var boardService = new BoardService(tiles, size: boardSize);

        var moved = boardService.Move(Direction.Up);

        moved.ShouldBeTrue();

        boardService.Tiles[0][0].ShouldBe(4);
        boardService.Tiles[1][0].ShouldBe(3);
    }

    [Theory]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    [InlineData(7)]
    [InlineData(8)]
    public void MoveDown_Moves_Tiles_Down(int boardSize)
    {
        var tiles = InitializeArray(boardSize);
        tiles[0][0] = 3;
        tiles[1][0] = 2;
        tiles[2][0] = 2;

        var boardService = new BoardService(tiles, size: boardSize);

        var moved = boardService.Move(Direction.Down);

        moved.ShouldBeTrue();

        boardService.Tiles[boardSize - 1][0].ShouldBe(4);
        boardService.Tiles[boardSize - 2][0].ShouldBe(3);
    }

    private static void ValidatePositives(IBoardService board, int count)
    {
        var positive = new List<int>();
        for (var x = 0; x < board.BoardSize; x++)
        {
            for (var y = 0; y < board.BoardSize; y++)
            {
                if (board.Tiles[x][y] > 0)
                {
                    positive.Add(board.Tiles[x][y]);
                }
            }
        }

        positive.Count.ShouldBe(count);
    }

    private static int[][] InitializeArray(int i)
    {
        var array = new int[i][];
        for (var x = 0; x < i; x++)
        {
            array[x] = new int[i];
        }

        return array;
    }
}