using FluentAssertions;
using LemballEditor.Models;
using LemballEditor.Serializers.Level.Map;

namespace LemballEditor.Tests.SerializerTests.LevelTests.MapTests
{
    [TestClass]
    public class SizeTests
    {
        [TestMethod]
        public void Deserialize_ShouldThrowANotImplementedException_IfAnExistingMapInstanceIsPassedToTheFunction()
        {
            var map = ServiceFactory.CreateMap(1, 1);
            var serializer = new Size(ServiceFactory.CreateMap);

            using var stream = new MemoryStream();
            using var reader = new BinaryReader(stream);

            var act = () => serializer.Deserialize(reader, map);

            _ = act.Should().Throw<NotImplementedException>().WithMessage("Map Size deserializer cannot be passed an existing Map instance as it creates its own");
        }

        [TestMethod]
        public void Deserialize_ShouldThrowAnException_IfTheMapSizeAndDataSizeDoNotMatch()
        {
            uint invalidDataSize = 1;
            ushort givenXSize = 64;
            ushort givenYSize = 64;

            var data = new List<byte>();
            data.AddRange(BitConverter.GetBytes(invalidDataSize));
            data.AddRange(BitConverter.GetBytes(givenXSize));
            data.AddRange(BitConverter.GetBytes(givenYSize));

            using var stream = new MemoryStream(data.ToArray());
            using var reader = new BinaryReader(stream);

            var serializer = new Size(ServiceFactory.CreateMap);

            var act = () => serializer.Deserialize(reader);

            _ = act.Should().Throw<InvalidDataException>().WithMessage($"Invalid map data size: {invalidDataSize} given for a {givenXSize}x{givenYSize} map");
        }

        [TestMethod]
        public void Deserialize_ShouldCreateAndReturnAMapWithTheReadDimensions()
        {
            var tileSizeBytes = 6;
            var headerSize = 12;

            ushort givenXSize = 8;
            ushort givenYSize = 12;
            var dataSize = (uint)((givenXSize * givenYSize * tileSizeBytes) + headerSize);

            var data = new List<byte>();
            data.AddRange(BitConverter.GetBytes(dataSize));
            data.AddRange(BitConverter.GetBytes(givenXSize));
            data.AddRange(BitConverter.GetBytes(givenYSize));

            using var stream = new MemoryStream(data.ToArray());
            using var reader = new BinaryReader(stream);

            var serializer = new Size(ServiceFactory.CreateMap);

            var map = serializer.Deserialize(reader);

            _ = map.XTiles.Should().Be(8);
            _ = map.YTiles.Should().Be(12);
        }

        [TestMethod]
        public void Serializer_ShouldWriteTheMapDataSize()
        {
            var tileSizeBytes = 6;
            var headerSize = 12;

            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);
            using var reader = new BinaryReader(stream);

            var serializer = new Size(ServiceFactory.CreateMap);

            var map = ServiceFactory.CreateMap(8, 12);

            serializer.Serialize(map, writer);

            _ = stream.Seek(0, SeekOrigin.Begin);
            var dataSize = reader.ReadUInt32();
            _ = dataSize.Should().Be((uint)((map.TileCount * tileSizeBytes) + headerSize));
        }

        [TestMethod]
        public void Serializer_ShouldWriteTheMapSizeInTiles()
        {
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);
            using var reader = new BinaryReader(stream);

            var serializer = new Size(ServiceFactory.CreateMap);

            var map = ServiceFactory.CreateMap(12, 24);

            serializer.Serialize(map, writer);

            _ = stream.Seek(0, SeekOrigin.Begin);
            _ = reader.ReadUInt32();

            var xSize = reader.ReadUInt16();
            var ySize = reader.ReadUInt16();

            _ = xSize.Should().Be(map.XTiles);
            _ = ySize.Should().Be(map.YTiles);
        }
    }
}
