using FluentAssertions;
using LemballEditor.Models;

namespace LemballEditor.Tests.ModelTests
{
    [TestClass]
    public class TileTests
    {
        [TestMethod]
        public void Constructor_ShouldInitialiseWithTheDefaultTileRefAndZeroElevation()
        {
            var tile = new Tile();
            _ = tile.TileRef.Should().Be(521);
            _ = tile.Elevation.Should().Be(0);
        }

        [TestMethod]
        public void Constructor_ShouldInitialiseWithTheGivenTileRefAndZeroElevation()
        {
            ushort tileRef = 123;
            var tile = new Tile(tileRef);
            _ = tile.TileRef.Should().Be(123);
            _ = tile.Elevation.Should().Be(0);
        }

        [TestMethod]
        public void Constructor_ShouldInitialiseWithTheTileRefAndElevation()
        {
            ushort tileRef = 123;
            var tile = new Tile(tileRef, 10);
            _ = tile.TileRef.Should().Be(123);
            _ = tile.Elevation.Should().Be(10);
        }

        [TestMethod]
        public void Constructor_ShouldThrowAnException_WhenTheElevationValueIsOver88()
        {
            var act = () => new Tile(0, 89);
            _ = act.Should().Throw<ArgumentException>().WithMessage("Max elevation is 88");
        }

        [TestMethod]
        public void ShouldGetAndSetTheTileRef()
        {
            var tile = new Tile(0)
            {
                TileRef = 123
            };
            _ = tile.TileRef.Should().Be(123);
        }

        [TestMethod]
        public void ShouldGetAndSetTheElevation()
        {
            var tile = new Tile(0)
            {
                Elevation = 88
            };
            _ = tile.Elevation.Should().Be(88);
        }

        [TestMethod]
        public void Elevation_ShouldThrowAnException_WhenTheValueIsOver88()
        {
            var tile = new Tile(0);

            var act = () => tile.Elevation = 89;

            _ = act.Should().Throw<ArgumentException>().WithMessage("Max elevation is 88");
        }
    }
}
