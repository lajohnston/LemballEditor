using FluentAssertions;
using LemballEditor.Models;
using LemballEditor.Serializers.Level;

namespace LemballEditor.Tests.SerializerTests.LevelTests
{
    [TestClass]
    public sealed class TimeLimitTest
    {
        [TestMethod]
        public void Deserialize_ShouldSetTimeLimitToNull_WhenValueIs600()
        {
            ushort value = 600;
            using var stream = new MemoryStream(BitConverter.GetBytes(value));
            using var reader = new BinaryReader(stream);

            var level = new Level
            {
                TimeLimitInSeconds = 100
            };

            new TimeLimit().Deserialize(level, reader);

            _ = level.TimeLimitInSeconds.Should().BeNull();
        }

        [TestMethod]
        public void Deserialize_ShouldSetTimeLimit_WhenValueIsBelow600()
        {
            ushort value = 599;
            using var stream = new MemoryStream(BitConverter.GetBytes(value));
            using var reader = new BinaryReader(stream);

            var level = new Level
            {
                TimeLimitInSeconds = 100
            };

            new TimeLimit().Deserialize(level, reader);

            _ = level.TimeLimitInSeconds.Should().Be(599);
        }

        [TestMethod]
        public void Serialize_ShouldWriteA600UShort_WhenThereIsNotTimeLimit()
        {
            var level = new Level
            {
                TimeLimitInSeconds = null
            };

            ushort expectedValue = 600;
            var serializer = new TimeLimit();

            TestHelper.AssertWrite(level, serializer, expectedValue);
        }

        [TestMethod]
        public void Serialize_ShouldWriteTheTimeLimitUShort_WhenThereIsATimeLimit()
        {
            var level = new Level
            {
                TimeLimitInSeconds = 599
            };

            ushort expectedValue = 599;
            var serializer = new TimeLimit();

            TestHelper.AssertWrite(level, serializer, expectedValue);
        }
    }
}
