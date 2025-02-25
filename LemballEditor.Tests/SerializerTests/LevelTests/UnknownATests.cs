using FluentAssertions;
using LemballEditor.Models;
using LemballEditor.Serializers.Level;
using Moq;

namespace LemballEditor.Tests.SerializerTests.LevelTests
{
    [TestClass]
    public sealed class UnknownATests
    {
        private static readonly IModelFactory modelFactory = new ModelFactory();

        [TestMethod]
        public void Deserialize_ShouldThrowAnExceptionIfTheValueIsAboveTheByteMaxSize()
        {
            ushort invalidValue = byte.MaxValue + 1;

            using var stream = new MemoryStream(BitConverter.GetBytes(invalidValue));
            using var reader = new BinaryReader(stream);

            var level = modelFactory.CreateLevel(1, 1);
            var act = () => new UnknownA().Deserialize(reader, level);
            _ = act.Should().Throw<InvalidDataException>().WithMessage($"UnknownA out of byte range. {invalidValue} given");
        }

        [TestMethod]
        public void Deserialize_ShouldThrowAnExceptionIfTheUnknownAValueIsInvalid()
        {
            byte invalidValue = 1;
            using var stream = new MemoryStream(BitConverter.GetBytes(invalidValue));
            using var reader = new BinaryReader(stream);

            // Mock a level that throws an exception when UnknownA is set
            var fakeErrorMessage = "Some Error";
            var mockLevel = new Mock<ILevel>();

            _ = mockLevel.SetupSet(m => m.UnknownA = It.Is<byte>(x => x == invalidValue))
                    .Throws(() => new ArgumentException(fakeErrorMessage));

            var act = () => new UnknownA().Deserialize(reader, mockLevel.Object);

            _ = act.Should().Throw<InvalidDataException>().WithMessage(fakeErrorMessage);
        }

        [TestMethod]
        public void Serialize_ShouldWriteTheUShortToTheStream()
        {
            var level = modelFactory.CreateLevel(1, 1);
            level.UnknownA = 10;

            ushort expectedValue = 10;
            var serializer = new UnknownA();

            TestHelper.AssertWrite(level, serializer, expectedValue);
        }
    }
}
