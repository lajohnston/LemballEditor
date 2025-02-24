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
    }
}
