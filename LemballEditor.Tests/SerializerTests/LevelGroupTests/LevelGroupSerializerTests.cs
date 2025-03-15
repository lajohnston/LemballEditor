using FluentAssertions;
using LemballEditor.Models;
using LemballEditor.Serializers.LevelGroup;

namespace LemballEditor.Tests.SerializerTests.LevelGroupTests
{
    [TestClass]
    public class LevelGroupSerializerTests
    {
        [TestMethod]
        public void Deserialize_ShouldThrowAnInvalidDataException_WhenTheDataDoesNotBeginWithValidHeader()
        {
            byte[] data = [1, 2, 3];

            using var stream = new MemoryStream(data.ToArray());
            using var reader = new BinaryReader(stream);

            var levelGroup = new LevelGroup();
            var levelGroupContext = new LevelGroupContext();
            var serializer = new LevelGroupSerializer();

            var act = () => serializer.Deserialize(reader, (levelGroup, levelGroupContext));
            _ = act.Should().Throw<InvalidDataException>().WithMessage("Invalid directory header");
        }
    }
}
