using FluentAssertions;
using LemballEditor.Models;
using LemballEditor.Serializers.Level;

namespace LemballEditor.Tests.SerializerTests.LevelTests
{
    [TestClass]
    public sealed class TimeLimitTest
    {
        private static readonly IModelFactory modelFactory = new ModelFactory();

        [TestMethod]
        public void Deserialize_ShouldSetTimeLimitToNull_WhenValueIs600()
        {
            ushort value = 600;
            using var stream = new MemoryStream(BitConverter.GetBytes(value));
            using var reader = new BinaryReader(stream);

            var level = modelFactory.CreateLevel(1, 1);
            level.TimeLimitInSeconds = 100;

            _ = new TimeLimit().Deserialize(reader, level);

            _ = level.TimeLimitInSeconds.Should().BeNull();
        }

        [TestMethod]
        public void Deserialize_ShouldSetTimeLimit_WhenValueIsBelow600()
        {
            ushort value = 599;
            using var stream = new MemoryStream(BitConverter.GetBytes(value));
            using var reader = new BinaryReader(stream);

            var level = modelFactory.CreateLevel(1, 1);
            level.TimeLimitInSeconds = 100;

            _ = new TimeLimit().Deserialize(reader, level);

            _ = level.TimeLimitInSeconds.Should().Be(599);
        }

        [TestMethod]
        public void Serialize_ShouldWriteA600UShort_WhenThereIsNotTimeLimit()
        {
            var level = modelFactory.CreateLevel(1, 1);
            level.TimeLimitInSeconds = null;

            ushort expectedValue = 600;
            var serializer = new TimeLimit();

            TestHelper.AssertWrite(level, serializer, expectedValue);
        }

        [TestMethod]
        public void Serialize_ShouldWriteTheTimeLimitUShort_WhenThereIsATimeLimit()
        {
            var level = modelFactory.CreateLevel(1, 1);
            level.TimeLimitInSeconds = 599;

            ushort expectedValue = 599;
            var serializer = new TimeLimit();

            TestHelper.AssertWrite(level, serializer, expectedValue);
        }
    }
}
