using FluentAssertions;
using LemballEditor.Models;

namespace LemballEditor.Tests.SerializerTests.LevelTests.MapTests
{
    [TestClass]
    public class MapSerializerTests
    {

        private byte[] Serialize(IMap map)
        {
            var level = ServiceFactory.CreateLevel(1, 1);
            level.Map = map;

            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);
            ServiceFactory.CreateMapSerializer().Serialize(level, writer);


            return stream.ToArray();
        }

        [TestMethod]
        public void ShouldSerializeAndDeserializeTheMapOfTheGivenLevel()
        {
            var originalMap = ServiceFactory.CreateMap(2, 2);
            originalMap.SetTile(0, 0, ServiceFactory.CreateTile(0, 0));
            originalMap.SetTile(1, 0, ServiceFactory.CreateTile(1, 10));
            originalMap.SetTile(0, 1, ServiceFactory.CreateTile(2, 20));
            originalMap.SetTile(1, 1, ServiceFactory.CreateTile(3, 30));

            var data = Serialize(originalMap);

            using var stream = new MemoryStream(data);
            using var reader = new BinaryReader(stream);

            var level = ServiceFactory.CreateLevel(2, 2);
            var resultMap = ServiceFactory.CreateMapSerializer().Deserialize(reader, level).Map;

            _ = resultMap.XTiles.Should().Be(originalMap.XTiles);
            _ = resultMap.YTiles.Should().Be(originalMap.YTiles);

            _ = resultMap.GetTile(0, 0).TileRef.Should().Be(originalMap.GetTile(0, 0).TileRef);
            _ = resultMap.GetTile(1, 0).TileRef.Should().Be(originalMap.GetTile(1, 0).TileRef);
            _ = resultMap.GetTile(0, 1).TileRef.Should().Be(originalMap.GetTile(0, 1).TileRef);
            _ = resultMap.GetTile(1, 1).TileRef.Should().Be(originalMap.GetTile(1, 1).TileRef);
        }
    }
}

