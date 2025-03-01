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

            var level = ModelFactory.LevelFactory(1, 1);
            var act = () => new FlagsRequiredIndicator().Deserialize(reader, level);
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

            var act = () => new FlagsRequiredIndicator().Deserialize(reader, mockLevel.Object);

            _ = act.Should().Throw<InvalidDataException>().WithMessage(fakeErrorMessage);
        }

        [TestMethod]
        public void Deserialize_ShouldSetTheFlagsRequiredToZero_WhenTheValueIsSeven()
        {
            using var stream = new MemoryStream(BitConverter.GetBytes(7));
            using var reader = new BinaryReader(stream);

            var level = ModelFactory.LevelFactory(1, 1);
            level.FlagsRequired = 4;

            _ = new FlagsRequiredIndicator().Deserialize(reader, level);

            _ = level.FlagsRequired.Should().Be(0);
        }

        [TestMethod]
        public void Serialize_ShouldWriteTheUShortToTheStream()
        {
            var level = ModelFactory.LevelFactory(1, 1);
            level.FlagsRequired = 2;

            var serializer = new FlagsRequiredIndicator();

            TestHelper.AssertWrite(level, serializer, 2);
        }

        [TestMethod]
        public void Serialize_ShouldWriteAUShortOfSevenToTheStream_WhenNoFlagsAreRequired()
        {
            var level = ModelFactory.LevelFactory(1, 1);
            level.FlagsRequired = 0;

            ushort expectedValue = 7;
            var serializer = new FlagsRequiredIndicator();

            TestHelper.AssertWrite(level, serializer, expectedValue);
        }
    }
}
