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

            var levelGroupContext = new LevelDirectory() { LevelGroup = new LevelGroup(LevelGroupName.Fun) };
            var serializer = new LevelDirectorySerializer();

            var act = () => serializer.Deserialize(reader, levelGroupContext);
            _ = act.Should().Throw<InvalidDataException>().WithMessage("Invalid directory header");
        }

        [TestMethod]
        public void ShouldSerializeAndDeserializeTheLevelDirectory()
        {
            var originalLevelGroup = new LevelGroup(LevelGroupName.Fun);
            var originContext = new LevelDirectory() { LevelGroup = originalLevelGroup };

            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            var serializer = new LevelDirectorySerializer();

            var serializeAct = () => serializer.Serialize(originContext, writer);
            _ = serializeAct.Should().NotThrow();

            using var reader = new BinaryReader(stream);
            stream.Position = 0;

            var deserializedLevelGroup = new LevelGroup(LevelGroupName.Fun);
            var deserializedContext = new LevelDirectory() { LevelGroup = deserializedLevelGroup };

            var deserializeAct = () => serializer.Deserialize(reader, deserializedContext);
            _ = deserializeAct.Should().NotThrow();
        }
    }
}
