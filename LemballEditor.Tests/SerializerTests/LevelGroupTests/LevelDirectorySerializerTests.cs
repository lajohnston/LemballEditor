using FluentAssertions;
using LemballEditor.Models;
using LemballEditor.Serializers.LevelDirectory;

namespace LemballEditor.Tests.SerializerTests.LevelGroupTests
{
    [TestClass]
    public class LevelDirectorySerializerTests
    {
        [TestMethod]
        public void Deserialize_ShouldThrowAnInvalidDataException_WhenTheDataDoesNotBeginWithValidHeader()
        {
            byte[] data = [1, 2, 3];

            using var stream = new MemoryStream(data);
            using var reader = new BinaryReader(stream);

            var levelGroupContext = new LevelDirectory() { LevelGroup = new LevelGroup() };
            var serializer = new LevelDirectorySerializer();

            var act = () => serializer.Deserialize(reader, levelGroupContext);
            _ = act.Should().Throw<InvalidDataException>().WithMessage("Invalid directory header");
        }
    }
}
