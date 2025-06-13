using FluentAssertions;
using LemballEditor.Models;
using LemballEditor.Serializers.Level;

namespace LemballEditor.Tests.SerializerTests.LevelTests
{
    [TestClass]
    public sealed class UnknownBSerializerTests
    {
        [TestMethod]
        public void Deserialize_ShouldSetTheValueToTheModel()
        {
            ushort value = 33425;
            using var stream = new MemoryStream(BitConverter.GetBytes(value));
            using var reader = new BinaryReader(stream);

            var level = ServiceFactory.CreateLevel(1, 1);
            level.UnknownB = 0;

            _ = new UnknownBSerializer().Deserialize(reader, level);

            _ = level.UnknownB.Should().Be(33425);
        }

        [TestMethod]
        public void Serialize_ShouldWriteTheUShortToTheStream()
        {
            var level = ServiceFactory.CreateLevel(1, 1);
            level.UnknownB = 10;

            ushort expectedValue = 10;
            var serializer = new UnknownBSerializer();

            TestHelper.AssertWrite(level, serializer, expectedValue);
        }
    }
}
