using FluentAssertions;
using LemballEditor.Models;
namespace LemballEditor.Tests.ModelTests
{
    [TestClass]
    public class ModelFactoryTests
    {
        [TestMethod]
        public void CreateLevel_ShouldCreateALevelWithAMap()
        {
            var factory = new ModelFactory();
            var level = factory.CreateLevel(1, 2);
            _ = level.Should().BeAssignableTo<ILevel>();

            var map = level.Map;
            _ = map.Should().NotBeNull();
            _ = map.XTiles.Should().Be(1);
            _ = map.YTiles.Should().Be(2);
        }

        [TestMethod]
        public void CreateMap_ShouldCreateAMapWithTheGivenSize()
        {
            var factory = new ModelFactory();
            var map = factory.CreateMap(1, 2);
            _ = map.XTiles.Should().Be(1);
            _ = map.YTiles.Should().Be(2);
        }

        [TestMethod]
        public void CreateTile_ShouldCreateAMapTileWithTheDefaultGroundTileAndElevationOfZero()
        {
            var factory = new ModelFactory();
            var tile = factory.CreateTile();

            _ = tile.TileRef.Should().Be(521);
            _ = tile.Elevation.Should().Be(0);
        }

        [TestMethod]
        public void CreateTile_ShouldCreateAMapTileWithTheGivenTileRef()
        {
            var factory = new ModelFactory();
            var tile = factory.CreateTile(123);

            _ = tile.TileRef.Should().Be(123);
            _ = tile.Elevation.Should().Be(0);
        }

        [TestMethod]
        public void CreateTile_ShouldCreateAMapTileWithTheGivenTileRefAndElevation()
        {
            var factory = new ModelFactory();
            var tile = factory.CreateTile(123, 10);

            _ = tile.TileRef.Should().Be(123);
            _ = tile.Elevation.Should().Be(10);
        }
    }
}
