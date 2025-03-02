using FluentAssertions;
using LemballEditor.Serializers.Level.Map;

namespace LemballEditor.Tests.SerializerTests.LevelTests.MapTests
{
    [TestClass]
    public class TileMapTests
    {
        [TestMethod]
        public void Deserialize_ShouldThrowAnException_WhenATileElevationIsAbove88()
        {
            var data = new List<byte>();
            data.AddRange(BitConverter.GetBytes((int)100));
            data.AddRange(BitConverter.GetBytes((ushort)89));

            using var stream = new MemoryStream(data.ToArray());
            using var reader = new BinaryReader(stream);

            var serializer = new TileMap(ServiceFactory.CreateTile);

            var map = ServiceFactory.CreateMap(1, 1);
            var act = () => serializer.Deserialize(reader, map);
            _ = act.Should().Throw<InvalidDataException>().WithMessage($"Max elevation for tile is 88. Tile 1x1 has 89");
        }

        [TestMethod]
        public void Deserialize_ShouldSetTheTileAndElevationForEachTile()
        {
            var data = new List<byte>();
            data.AddRange(BitConverter.GetBytes((int)1));
            data.AddRange(BitConverter.GetBytes((ushort)1));

            data.AddRange(BitConverter.GetBytes((int)2));
            data.AddRange(BitConverter.GetBytes((ushort)2));

            data.AddRange(BitConverter.GetBytes((int)3));
            data.AddRange(BitConverter.GetBytes((ushort)3));

            data.AddRange(BitConverter.GetBytes((int)4));
            data.AddRange(BitConverter.GetBytes((ushort)4));

            using var stream = new MemoryStream(data.ToArray());
            using var reader = new BinaryReader(stream);

            var map = ServiceFactory.CreateMap(2, 2);
            var serializer = new TileMap(ServiceFactory.CreateTile);

            _ = serializer.Deserialize(reader, map);

            _ = map.GetTile(0, 0).TileRef.Should().Be(1);
            _ = map.GetTile(0, 0).Elevation.Should().Be(1);

            _ = map.GetTile(1, 0).TileRef.Should().Be(2);
            _ = map.GetTile(1, 0).Elevation.Should().Be(2);

            _ = map.GetTile(0, 1).TileRef.Should().Be(3);
            _ = map.GetTile(0, 1).Elevation.Should().Be(3);

            _ = map.GetTile(1, 1).TileRef.Should().Be(4);
            _ = map.GetTile(1, 1).Elevation.Should().Be(4);
        }

        [TestMethod]
        public void Serialize_ShouldWriteEachTileToTheStream()
        {
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);
            using var reader = new BinaryReader(stream);

            var map = ServiceFactory.CreateMap(2, 2);
            map.SetTile(0, 0, ServiceFactory.CreateTile(0, 0));
            map.SetTile(1, 0, ServiceFactory.CreateTile(1, 10));
            map.SetTile(0, 1, ServiceFactory.CreateTile(2, 20));
            map.SetTile(1, 1, ServiceFactory.CreateTile(3, 30));

            var serializer = new TileMap(ServiceFactory.CreateTile);
            serializer.Serialize(map, writer);

            _ = stream.Seek(0, SeekOrigin.Begin);

            _ = reader.ReadUInt32().Should().Be(0);
            _ = reader.ReadUInt16().Should().Be(0);

            _ = reader.ReadUInt32().Should().Be(1);
            _ = reader.ReadUInt16().Should().Be(10);

            _ = reader.ReadUInt32().Should().Be(2);
            _ = reader.ReadUInt16().Should().Be(20);

            _ = reader.ReadUInt32().Should().Be(3);
            _ = reader.ReadUInt16().Should().Be(30);
        }
    }
}
