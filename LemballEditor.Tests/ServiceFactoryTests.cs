using FluentAssertions;
using LemballEditor.Models;
namespace LemballEditor.Tests
{
    [TestClass]
    public class ServiceFactoryTests
    {
        [TestMethod]
        public void CreateLevel_ShouldCreateALevelWithAMap()
        {
            var level = ServiceFactory.CreateLevel(1, 2);
            _ = level.Should().BeAssignableTo<ILevel>();

            var map = level.Map;
            _ = map.Should().NotBeNull();
            _ = map.XTiles.Should().Be(1);
            _ = map.YTiles.Should().Be(2);
        }

        [TestMethod]
        public void CreateMap_ShouldCreateAMapWithTheGivenSize()
        {
            var map = ServiceFactory.CreateMap(1, 2);
            _ = map.XTiles.Should().Be(1);
            _ = map.YTiles.Should().Be(2);
        }

        [TestMethod]
        public void CreateTile_ShouldCreateAMapTileWithTheGivenTileRefAndElevation()
        {
            var tile = ServiceFactory.CreateTile(123, 10);

            _ = tile.TileRef.Should().Be(123);
            _ = tile.Elevation.Should().Be(10);
        }
    }
}
