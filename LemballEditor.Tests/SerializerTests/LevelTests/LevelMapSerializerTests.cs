using FluentAssertions;
using LemballEditor.Models;
using LemballEditor.Serializers;
using LemballEditor.Serializers.Level;
using Moq;
namespace LemballEditor.Tests.SerializerTests.LevelTests
{
    [TestClass]
    public class LevelMapSerializerTests
    {
        private (LevelMapSerializer, Mock<ISerializer<IMap>>) CreateLevelMapSerializer()
        {
            var mockMapSerializer = new Mock<ISerializer<IMap>>();
            var levelMapSerializer = new LevelMapSerializer(mockMapSerializer.Object);

            return (levelMapSerializer, mockMapSerializer);
        }

        [TestMethod]
        public void Deserialize_ShouldCallTheMapSerializerWithTheWriterButNoMap()
        {
            var (serializer, mockMapSerializer) = this.CreateLevelMapSerializer();
            var level = new Mock<ILevel>().Object;

            using var reader = new BinaryReader(new MemoryStream());
            _ = serializer.Deserialize(reader, level);

            mockMapSerializer.Verify(s => s.Deserialize(It.Is<BinaryReader>(r => r == reader), It.Is<IMap>(map => map == null)));
        }

        [TestMethod]
        public void Deserialize_ShouldSetTheMapToTheModel()
        {
            var (serializer, mockMapSerializer) = this.CreateLevelMapSerializer();
            var mockLevel = new Mock<ILevel>();

            var map = new Mock<IMap>().Object;
            _ = mockMapSerializer.Setup(s => s.Deserialize(It.IsAny<BinaryReader>(), It.IsAny<IMap>())).Returns(map);

            using var reader = new BinaryReader(new MemoryStream());
            var result = serializer.Deserialize(reader, mockLevel.Object);

            mockLevel.VerifySet(l => l.Map = map, Times.Once);
        }

        [TestMethod]
        public void Deserialize_ShouldReturnTheLevelModel()
        {
            var (serializer, _) = this.CreateLevelMapSerializer();
            var level = new Mock<ILevel>().Object;

            using var reader = new BinaryReader(new MemoryStream());
            var result = serializer.Deserialize(reader, level);

            _ = result.Should().Be(level, "The deserialized level should be the same as the input level");
        }

        [TestMethod]
        public void Serialize_ShouldCallTheMapSerializerWithTheMapFromTheLevel()
        {
            var (serializer, mockMapSerializer) = this.CreateLevelMapSerializer();
            var level = new Mock<ILevel>();

            var map = new Mock<IMap>().Object;
            _ = level.Setup(l => l.Map).Returns(map);

            using var writer = BinaryWriter.Null;
            serializer.Serialize(level.Object, writer);
            mockMapSerializer.Verify(s => s.Serialize(It.Is<IMap>(m => m == map), It.Is<BinaryWriter>(w => w == writer)));
        }
    }
}
