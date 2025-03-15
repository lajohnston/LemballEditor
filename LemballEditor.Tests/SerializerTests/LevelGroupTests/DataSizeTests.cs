using FluentAssertions;
using LemballEditor.Serializers.LevelGroup;

namespace LemballEditor.Tests.SerializerTests.LevelGroupTests
{
    [TestClass]
    public class DataSizeTests
    {
        [TestMethod]
        public void Deserialize_ShouldSkipTheDirectorySize()
        {
            using var stream = new MemoryStream(new byte[10]);
            using var reader = new BinaryReader(stream);

            var model = new LevelGroupContext();

            var serializer = new DataSize();
            _ = serializer.Deserialize(reader, model);

            _ = stream.Position.Should().Be(4);
        }

        [TestMethod]
        public void Deserialize_ShouldReturnTheGivenModel()
        {
            using var stream = new MemoryStream(new byte[10]);
            using var reader = new BinaryReader(stream);

            var model = new LevelGroupContext();

            var serializer = new DataSize();
            _ = serializer.Deserialize(reader, model).Should().Be(model);
        }
    }
}
