using FluentAssertions;
using LemballEditor.Serializers.LevelDirectory;

namespace LemballEditor.Tests.SerializerTests.LevelGroupTests
{
    [TestClass]
    public class LevelCountTests
    {
        [TestMethod]
        public void Deserialize_ShouldStoreTheNumberOfLevelsInThePendingLevelGroup()
        {
            uint numberOfFiles = 29;
            var data = BitConverter.GetBytes(numberOfFiles);

            using var stream = new MemoryStream(data);
            using var reader = new BinaryReader(stream);

            var model = new LevelDirectory();

            var serializer = new LevelCount();
            _ = serializer.Deserialize(reader, model);

            _ = model.LevelCount.Should().Be((byte)numberOfFiles);
            _ = stream.Position.Should().Be(4);
        }

        [TestMethod]
        public void Deserialize_ShouldThrowAnInvalidDataExceptionIfTheNumberOfLevelsAreAbove29()
        {
            uint numberOfFiles = 30;
            var data = BitConverter.GetBytes(numberOfFiles);

            using var stream = new MemoryStream(data);
            using var reader = new BinaryReader(stream);

            var model = new LevelDirectory();

            var serializer = new LevelCount();
            var act = () => serializer.Deserialize(reader, model);

            _ = act.Should().Throw<InvalidDataException>().WithMessage($"Number of levels in level group higher than 29 maximum: {numberOfFiles}");
        }

        [TestMethod]
        public void Deserialize_ShouldReturnTheGivenModel()
        {
            using var stream = new MemoryStream(new byte[10]);
            using var reader = new BinaryReader(stream);

            var model = new LevelDirectory();

            var serializer = new LevelCount();
            _ = serializer.Deserialize(reader, model).Should().Be(model);
        }
    }
}
