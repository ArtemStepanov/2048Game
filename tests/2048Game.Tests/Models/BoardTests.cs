using _2048Game.Models;
using System.Reflection;

namespace _2048Game.Tests.Models;

public sealed class BoardTests
{
    [Fact]
    public void Board_Initializes_With_Two_Tiles()
    {
        var board = new Board();
        Assert.Equal(2, board.Tiles.Count);
    }

    [Theory]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    [InlineData(7)]
    [InlineData(8)]
    public void Board_Creates_With_Valid_Size(int size)
    {
        var board = new Board(size);
        Assert.Equal(size, board.Size);
        Assert.Equal(2, board.Tiles.Count);
    }

    [Fact]
    public void AddRandomTile_Adds_A_Tile()
    {
        var board = new Board();
        board.AddRandomTile();
        Assert.Equal(3, board.Tiles.Count);
    }

    [Fact]
    public void RemoveTile_Removes_A_Tile()
    {
        var board = new Board();
        var tile = board.Tiles.First();
        board.RemoveTile(tile);
        Assert.Single(board.Tiles);
    }

    [Theory]
    [InlineData(3)]
    [InlineData(9)]
    public void Board_Throws_Exception_For_Invalid_Size(int size)
    {
        var ex = Assert.Throws<ArgumentException>(() => new Board(size));
        Assert.Equal("Board size must be between 4 and 8.", ex.Message);
    }

    [Fact]
    public void Reset_Recreate_Tiles_And_Resets_Size()
    {
        var board = new Board();
        board.Reset(6);
        Assert.Equal(2, board.Tiles.Count);
        Assert.Equal(6, board.Size);
    }

    [Fact]
    public void CanMove_Returns_True_If_Empty_Space()
    {
        var board = new Board();
        board.AddRandomTile(); // Adding a third tile to ensure there are empty spaces
        Assert.True(board.CanMove());
    }

    [Fact]
    public void CanMove_Returns_True_If_Merge_Possible()
    {
        var board = new Board();
        board.Reset(4);
        board.RemoveTile(board.Tiles.First());

        var tile1 = new Tile(0, 0, 2);
        var tile2 = new Tile(0, 1, 2); // Create a mergeable tile

        AddTileUsingReflection(board, tile1);
        AddTileUsingReflection(board, tile2);

        Assert.True(board.CanMove());
    }

    [Fact]
    public void CanMove_Returns_False_If_No_Merge_Possible_And_No_Empty_Space()
    {
        var board = new Board();

        board.Reset(board.Size);
        for (var row = 0; row < board.Size; row++)
        {
            for (var col = 0; col < board.Size; col++)
            {
                var value = (row * 4 + col + 1) * 2;
                AddTileUsingReflection(board, new Tile(row, col, value));
            }
        }

        Assert.False(board.CanMove());
    }

    private static void AddTileUsingReflection(Board board, Tile tile)
    {
        var tilesField = typeof(Board).GetField("_tiles", BindingFlags.NonPublic | BindingFlags.Instance);
        if (tilesField == null)
        {
            return;
        }

        var tiles = (List<Tile>)tilesField.GetValue(board)!;
        tiles.Add(tile);
    }
}