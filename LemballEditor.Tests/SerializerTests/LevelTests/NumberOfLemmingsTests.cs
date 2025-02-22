using FluentAssertions;
using LemballEditor.Models;
using LemballEditor.Serializers.Level;
using Moq;

namespace LemballEditor.Tests.SerializerTests.LevelTests
{
    [TestClass]
    public sealed class NumberOfLemmingsTests
    {
        [TestMethod]
        public void Deserialize_ShouldThrowAnExceptionIfTheValueIsInvalid()
        {
            ushort invalidValue = 1;
            using var stream = new MemoryStream(BitConverter.GetBytes(invalidValue));
            using var reader = new BinaryReader(stream);

            // Mock a level that throws an exception when UnknownA is set
            var fakeErrorMessage = "Some Error";
            var mockLevel = new Mock<ILevel>();

            _ = mockLevel.SetupSet(m => m.NumberOfLemmings = It.Is<ushort>(x => x == invalidValue))
                    .Throws(() => new ArgumentException(fakeErrorMessage));

            var act = () => new UnusedNumberOfLemmings().Deserialize(mockLevel.Object, reader);

            _ = act.Should().Throw<InvalidDataException>().WithMessage(fakeErrorMessage);
        }

        [TestMethod]
        public void Serialize_ShouldWriteTheUShortToTheStream()
        {
            var level = new Level() { NumberOfLemmings = 2 };
            ushort expectedValue = 2;
            var serializer = new UnusedNumberOfLemmings();

            TestHelper.AssertWrite(level, serializer, expectedValue);
        }
    }
}
