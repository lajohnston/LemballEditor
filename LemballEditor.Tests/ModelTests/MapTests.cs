using FluentAssertions;
using LemballEditor.Models;

namespace LemballEditor.Tests.ModelTests
{
    [TestClass]
    public class MapTests
    {
        [TestMethod]
        public void ShouldReturnTheNumberOfTiles()
        {
            byte xTiles = 64;
            byte yTiles = 64;

            var map = new Map(xTiles, yTiles);

            _ = map.XTiles.Should().Be(64);
            _ = map.YTiles.Should().Be(64);

            _ = map.TileCount.Should().Be(64 * 64);
        }

        [TestMethod]
        public void ShouldGetAndSetTheTileAtTheGivenCoordinate()
        {
            var map = new Map(10, 10);
            var tile = new Tile(123);

            map.SetTile(tile, 9, 9);

            _ = map.GetTile(9, 9).Should().Be(tile);
        }

        [TestMethod]
        public void SetTile_ShouldThrowAnExceptionIfTheXCoordinateIsOutOfBounds()
        {
            var map = new Map(10, 10);
            var act = () => map.SetTile(new Tile(123), 10, 0);

            _ = act.Should().Throw<IndexOutOfRangeException>().WithMessage("xTile 10 is out of bounds");
        }

        [TestMethod]
        public void SetTile_ShouldThrowAnExceptionIfTheYCoordinateIsOutOfBounds()
        {
            var map = new Map(10, 10);
            var act = () => map.SetTile(new Tile(123), 0, 10);

            _ = act.Should().Throw<IndexOutOfRangeException>().WithMessage("yTile 10 is out of bounds");
        }

        [TestMethod]
        public void GetTile_ShouldThrowAnExceptionIfTheXCoordinateIsOutOfBounds()
        {
            var map = new Map(10, 10);
            var act = () => map.GetTile(10, 0);

            _ = act.Should().Throw<IndexOutOfRangeException>().WithMessage("xTile 10 is out of bounds");
        }

        [TestMethod]
        public void GetTile_ShouldThrowAnExceptionIfTheYCoordinateIsOutOfBounds()
        {
            var map = new Map(10, 10);
            var act = () => map.GetTile(0, 10);

            _ = act.Should().Throw<IndexOutOfRangeException>().WithMessage("yTile 10 is out of bounds");
        }

        [TestMethod]
        public void GetTile_ShouldReturnNull_IfATileHasNotBeenSetAtTheGivenCoorindate()
        {
            var map = new Map(10, 10);

            _ = map.GetTile(0, 0).Should().BeNull();
        }

        [TestMethod]
        public void GetTileIterator_ShouldIterateThroughEachTile()
        {
            var map = new Map(2, 2);

            var tiles = new List<Tile>()
            {
                new(1,1),
                new(2,2),
                new(3,3),
                new(4,4),
            };

            map.SetTile(tiles[0], 0, 0);
            map.SetTile(tiles[1], 0, 1);
            map.SetTile(tiles[2], 1, 0);
            map.SetTile(tiles[3], 1, 1);

            var result = map.GetTileIterator().ToList();
            _ = result.Should().BeEquivalentTo(tiles);
        }
    }
}
