using FluentAssertions;
using LemballEditor.Models;
using LemballEditor.Serializers.Level;
using Moq;

namespace LemballEditor.Tests.SerializerTests.LevelTests
{
    [TestClass]
    public sealed class FlagsRequiredIndicatorTests
    {
        [TestMethod]
        public void Deserialize_ShouldThrowAnExceptionIfTheValueIsAboveTheByteMaxSize()
        {
            ushort invalidValue = byte.MaxValue + 1;

            using var stream = new MemoryStream(BitConverter.GetBytes(invalidValue));
            using var reader = new BinaryReader(stream);

            var level = new Level();
            var act = () => new FlagsRequiredIndicator().Deserialize(level, reader);
            _ = act.Should().Throw<InvalidDataException>().WithMessage($"FlagsRequiredIndicator out of byte range. {invalidValue} given");
        }

        [TestMethod]
        public void Deserialize_ShouldThrowAnExceptionIfTheValueIsInvalid()
        {
            ushort invalidValue = 5;
            using var stream = new MemoryStream(BitConverter.GetBytes(invalidValue));
            using var reader = new BinaryReader(stream);

            var mockLevel = new Mock<ILevel>();

            var fakeErrorMessage = "Some Error";
            _ = mockLevel.SetupSet(m => m.FlagsRequired = It.Is<byte>(x => x == invalidValue))
                    .Throws(() => new ArgumentException(fakeErrorMessage));

            var act = () => new FlagsRequiredIndicator().Deserialize(mockLevel.Object, reader);

            _ = act.Should().Throw<InvalidDataException>().WithMessage(fakeErrorMessage);
        }

        [TestMethod]
        public void Deserialize_ShouldSetTheFlagsRequiredToZero_WhenTheValueIsSeven()
        {
            using var stream = new MemoryStream(BitConverter.GetBytes(7));
            using var reader = new BinaryReader(stream);

            var level = new Level
            {
                FlagsRequired = 4
            };

            new FlagsRequiredIndicator().Deserialize(level, reader);

            _ = level.FlagsRequired.Should().Be(0);
        }

        [TestMethod]
        public void Serialize_ShouldWriteTheUShortToTheStream()
        {
            var level = new Level() { FlagsRequired = 2 };
            ushort expectedValue = 2;
            var serializer = new FlagsRequiredIndicator();

            TestHelper.AssertWrite(level, serializer, expectedValue);
        }

        [TestMethod]
        public void Serialize_ShouldWriteAUShortOfSevenToTheStream_WhenNoFlagsAreRequired()
        {
            var level = new Level() { FlagsRequired = 0 };
            ushort expectedValue = 7;
            var serializer = new FlagsRequiredIndicator();

            TestHelper.AssertWrite(level, serializer, expectedValue);
        }
    }
}
