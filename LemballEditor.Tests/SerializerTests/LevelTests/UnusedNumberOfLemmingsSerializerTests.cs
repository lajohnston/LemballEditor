using FluentAssertions;
using LemballEditor.Models;
using LemballEditor.Serializers.Level;
using Moq;

namespace LemballEditor.Tests.SerializerTests.LevelTests
{
    [TestClass]
    public sealed class UnusedNumberOfLemmingsSerializerTests
    {
        [TestMethod]
        public void Deserialize_ShouldThrowAnExceptionIfTheValueIsAboveTheByteMaxSize()
        {
            ushort invalidValue = byte.MaxValue + 1;

            using var stream = new MemoryStream(BitConverter.GetBytes(invalidValue));
            using var reader = new BinaryReader(stream);

            var level = ServiceFactory.CreateLevel(1, 1);
            var act = () => new UnusedNumberOfLemmingsSerializer().Deserialize(reader, level);
            _ = act.Should().Throw<InvalidDataException>().WithMessage($"NumberOfLemmings out of byte range. {invalidValue} given");
        }

        [TestMethod]
        public void Deserialize_ShouldThrowAnExceptionIfTheValueIsInvalid()
        {
            ushort invalidValue = 1;
            using var stream = new MemoryStream(BitConverter.GetBytes(invalidValue));
            using var reader = new BinaryReader(stream);

            // Mock a level that throws an exception when UnknownA is set
            var fakeErrorMessage = "Some Error";
            var mockLevel = new Mock<ILevel>();

            _ = mockLevel.SetupSet(m => m.NumberOfLemmings = It.Is<byte>(x => x == invalidValue))
                    .Throws(() => new ArgumentException(fakeErrorMessage));

            var act = () => new UnusedNumberOfLemmingsSerializer().Deserialize(reader, mockLevel.Object);

            _ = act.Should().Throw<InvalidDataException>().WithMessage(fakeErrorMessage);
        }

        [TestMethod]
        public void Serialize_ShouldWriteTheUShortToTheStream()
        {
            var level = ServiceFactory.CreateLevel(1, 1);
            level.NumberOfLemmings = 2;
            ushort expectedValue = 2;
            var serializer = new UnusedNumberOfLemmingsSerializer();

            TestHelper.AssertWrite(level, serializer, expectedValue);
        }
    }
}
