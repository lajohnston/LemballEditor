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
            var map = new Map();
            _ = map.XTileSize.Should().Be(64);
            _ = map.YTileSize.Should().Be(64);

            _ = map.TileCount.Should().Be(64 * 64);
        }
    }
}
