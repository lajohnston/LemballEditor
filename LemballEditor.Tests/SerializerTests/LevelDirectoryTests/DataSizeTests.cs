using FluentAssertions;
using LemballEditor.Serializers.LevelDirectory;

namespace LemballEditor.Tests.SerializerTests.LevelDirectoryTests
{
    [TestClass]
    public class DataSizeTests
    {
        [TestMethod]
        public void Deserialize_ShouldSkipTheDirectorySize()
        {
            using var stream = new MemoryStream(new byte[10]);
            using var reader = new BinaryReader(stream);

            var model = new PendingLevelGroup();

            var serializer = new DataSizeSerializer();
            _ = serializer.Deserialize(reader, model);

            _ = stream.Position.Should().Be(4);
        }

        [TestMethod]
        public void Deserialize_ShouldReturnTheGivenModel()
        {
            using var stream = new MemoryStream(new byte[10]);
            using var reader = new BinaryReader(stream);

            var model = new PendingLevelGroup();

            var serializer = new DataSizeSerializer();
            _ = serializer.Deserialize(reader, model).Should().Be(model);
        }

        [TestMethod]
        public void Serialize_ShouldWriteAnEmptyUintForNow()
        {
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            var model = new PendingLevelGroup();

            var serializer = new DataSizeSerializer();
            serializer.Serialize(model, writer);

            _ = stream.ToArray().Should().BeEquivalentTo(new byte[] { 0, 0, 0, 0 });
        }
    }
}
