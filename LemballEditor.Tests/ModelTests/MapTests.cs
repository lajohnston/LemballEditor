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

            var map = ModelFactory.MapFactory(xTiles, yTiles);

            _ = map.XTiles.Should().Be(64);
            _ = map.YTiles.Should().Be(64);

            _ = map.TileCount.Should().Be(64 * 64);
        }

        [TestMethod]
        public void ShouldGetAndSetTheTileAtTheGivenCoordinate()
        {
            var map = ModelFactory.MapFactory(2, 4);
            var tile = new Tile(123);

            map.SetTile(1, 3, tile);

            _ = map.GetTile(1, 3).Should().Be(tile);
        }

        [TestMethod]
        public void SetTile_ShouldThrowAnExceptionIfTheXCoordinateIsOutOfBounds()
        {
            var map = ModelFactory.MapFactory(10, 10);
            var act = () => map.SetTile(10, 0, new Tile(123));

            _ = act.Should().Throw<IndexOutOfRangeException>().WithMessage("xTile 10 is out of bounds");
        }

        [TestMethod]
        public void SetTile_ShouldThrowAnExceptionIfTheYCoordinateIsOutOfBounds()
        {
            var map = ModelFactory.MapFactory(10, 10);
            var act = () => map.SetTile(0, 10, new Tile(123));

            _ = act.Should().Throw<IndexOutOfRangeException>().WithMessage("yTile 10 is out of bounds");
        }

        [TestMethod]
        public void GetTile_ShouldThrowAnExceptionIfTheXCoordinateIsOutOfBounds()
        {
            var map = ModelFactory.MapFactory(10, 10);
            var act = () => map.GetTile(10, 0);

            _ = act.Should().Throw<IndexOutOfRangeException>().WithMessage("xTile 10 is out of bounds");
        }

        [TestMethod]
        public void GetTile_ShouldThrowAnExceptionIfTheYCoordinateIsOutOfBounds()
        {
            var map = ModelFactory.MapFactory(10, 10);
            var act = () => map.GetTile(0, 10);

            _ = act.Should().Throw<IndexOutOfRangeException>().WithMessage("yTile 10 is out of bounds");
        }

        [TestMethod]
        public void SetTile_ShouldThrowAnException_WhenTheGivenTileIsNull()
        {
            var map = ModelFactory.MapFactory(2, 2);
            var act = () => map.SetTile(0, 0, null);

            _ = act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null.");
        }

        [TestMethod]
        public void GetTileIterator_ShouldIterateThroughEachTile()
        {
            var map = ModelFactory.MapFactory(2, 2);

            var tiles = new List<Tile>()
            {
                new(1,1),
                new(2,2),
                new(3,3),
                new(4,4),
            };

            map.SetTile(0, 0, tiles[0]);
            map.SetTile(1, 0, tiles[1]);
            map.SetTile(0, 1, tiles[2]);
            map.SetTile(1, 1, tiles[3]);

            var result = map.GetTileIterator().ToList();
            _ = result.Should().BeEquivalentTo(tiles);
        }

        [TestMethod]
        public void Constructor_ShouldFillMapWithCopiesOfTheDefaultTile()
        {
            var defaultTile = new Tile();

            var map = ModelFactory.MapFactory(2, 2);

            var tile1 = map.GetTile(0, 0);
            var tile2 = map.GetTile(1, 0);
            var tile3 = map.GetTile(0, 1);
            var tile4 = map.GetTile(1, 1);

            _ = tile1.TileRef.Should().Be(defaultTile.TileRef);
            _ = tile2.TileRef.Should().Be(defaultTile.TileRef);
            _ = tile3.TileRef.Should().Be(defaultTile.TileRef);
            _ = tile4.TileRef.Should().Be(defaultTile.TileRef);

            _ = tile1.Elevation.Should().Be(defaultTile.Elevation);
            _ = tile2.Elevation.Should().Be(defaultTile.Elevation);
            _ = tile3.Elevation.Should().Be(defaultTile.Elevation);
            _ = tile4.Elevation.Should().Be(defaultTile.Elevation);

            _ = tile1.Should().NotBe(tile2);
            _ = tile2.Should().NotBe(tile3);
            _ = tile3.Should().NotBe(tile4);
        }
    }
}
