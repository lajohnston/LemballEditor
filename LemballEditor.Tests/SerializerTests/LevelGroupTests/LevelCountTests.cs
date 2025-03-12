using FluentAssertions;
using LemballEditor.Serializers.LevelGroup;

namespace LemballEditor.Tests.SerializerTests.LevelGroupTests
{
    [TestClass]
    public class LevelCountTests
    {
        [TestMethod]
        public void Deserialize_ShouldStoreTheNumberOfLevelsInThePendingLevelGroup()
        {
            uint numberOfFiles = 28;
            var data = BitConverter.GetBytes(numberOfFiles);

            using var stream = new MemoryStream(data);
            using var reader = new BinaryReader(stream);

            var model = new PendingLevelGroup();

            var serializer = new LevelCount();
            _ = serializer.Deserialize(reader, model);

            _ = model.LevelCount.Should().Be(numberOfFiles);
            _ = stream.Position.Should().Be(4);
        }

        [TestMethod]
        public void Deserialize_ShouldReturnTheGivenModel()
        {
            using var stream = new MemoryStream(new byte[10]);
            using var reader = new BinaryReader(stream);

            var model = new PendingLevelGroup();

            var serializer = new LevelCount();
            _ = serializer.Deserialize(reader, model).Should().Be(model);
        }
    }
}
