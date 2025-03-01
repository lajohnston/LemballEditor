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
            var level = ModelFactory.LevelFactory(1, 2);
            _ = level.Should().BeAssignableTo<ILevel>();

            var map = level.Map;
            _ = map.Should().NotBeNull();
            _ = map.XTiles.Should().Be(1);
            _ = map.YTiles.Should().Be(2);
        }

        [TestMethod]
        public void CreateMap_ShouldCreateAMapWithTheGivenSize()
        {
            var map = ModelFactory.MapFactory(1, 2);
            _ = map.XTiles.Should().Be(1);
            _ = map.YTiles.Should().Be(2);
        }

        [TestMethod]
        public void CreateTile_ShouldCreateAMapTileWithTheGivenTileRefAndElevation()
        {
            var tile = ModelFactory.TileFactory(123, 10);

            _ = tile.TileRef.Should().Be(123);
            _ = tile.Elevation.Should().Be(10);
        }
    }
}
