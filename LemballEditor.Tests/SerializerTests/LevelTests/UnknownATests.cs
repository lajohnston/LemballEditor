using FluentAssertions;
using LemballEditor.Models;
using LemballEditor.Serializers.Level;
using Moq;

namespace LemballEditor.Tests.SerializerTests.LevelTests
{
    [TestClass]
    public sealed class UnknownATests
    {
        [TestMethod]
        public void Deserialize_ShouldThrowAnExceptionIfTheUnknownAValueIsInvalid()
        {
            ushort invalidValue = 1;
            using var stream = new MemoryStream(BitConverter.GetBytes(invalidValue));
            using var reader = new BinaryReader(stream);

            // Mock a level that throws an exception when UnknownA is set
            var fakeErrorMessage = "Some Error";
            var mockLevel = new Mock<ILevel>();

            _ = mockLevel.SetupSet(m => m.UnknownA = It.Is<ushort>(x => x == invalidValue))
                    .Throws(() => new ArgumentException(fakeErrorMessage));

            var act = () => new UnknownA().Deserialize(mockLevel.Object, reader);

            _ = act.Should().Throw<InvalidDataException>().WithMessage(fakeErrorMessage);
        }

        [TestMethod]
        public void Serialize_ShouldWriteTheUShortToTheStream()
        {
            var level = new Level() { UnknownA = 10 };
            ushort expectedValue = 10;
            var serializer = new UnknownA();

            TestHelper.AssertWrite(level, serializer, expectedValue);
        }
    }
}
