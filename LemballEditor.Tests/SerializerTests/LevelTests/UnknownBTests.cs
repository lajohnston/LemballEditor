using FluentAssertions;
using LemballEditor.Models;
using LemballEditor.Serializers.Level;

namespace LemballEditor.Tests.SerializerTests.LevelTests
{
    [TestClass]
    public sealed class UnknownBTests
    {
        [TestMethod]
        public void Deserialize_ShouldSetTheValueToTheModel()
        {
            ushort value = 33425;
            using var stream = new MemoryStream(BitConverter.GetBytes(value));
            using var reader = new BinaryReader(stream);

            var level = new Level() { UnknownB = 0 };

            new UnknownB().Deserialize(level, reader);

            _ = level.UnknownB.Should().Be(33425);
        }

        [TestMethod]
        public void Serialize_ShouldWriteTheUShortToTheStream()
        {
            var level = new Level() { UnknownB = 10 };
            ushort expectedValue = 10;
            var serializer = new UnknownB();

            TestHelper.AssertWrite(level, serializer, expectedValue);
        }
    }
}
