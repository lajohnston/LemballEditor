using FluentAssertions;
using LemballEditor.Models;
using LemballEditor.Serializers;
using Moq;
using System.Text;

namespace LemballEditor.Tests.Serializers
{
    [TestClass]
    public sealed class LevelSerializerTests
    {
        [TestMethod]
        public void Deserialize_ShouldThrowAnInvalidDataException_WhenHeaderIsNotValid()
        {
            var invalidHeader = Encoding.UTF8.GetBytes("UNEXPECTED HEADER BYTES");

            using var stream = new MemoryStream(invalidHeader);
            using var reader = new BinaryReader(stream);

            var level = new Level();
            var act = () => LevelSerializer.Deserialize(level, reader);

            _ = act.Should().Throw<InvalidDataException>("Invalid header");
        }

        [TestMethod]
        public void Deserialize_ShouldThrowAnExceptionIfTheUnknownAValueIsInvalid()
        {
            var fakeErrorMessage = "Some Error";

            // Mock a level that throws an exception when UnknownA is set
            var mockLevel = new Mock<ILevel>();
            _ = mockLevel.SetupSet(m => m.UnknownA = It.IsAny<ushort>()).Throws<ArgumentException>();

            using var stream = new MemoryStream();
            using var reader = new BinaryReader(stream);

            // Serialize valid data
            using var writer = new BinaryWriter(stream);
            LevelSerializer.Serialize(mockLevel.Object, writer);

            // Deserialize the data
            _ = stream.Seek(0, SeekOrigin.Begin);
            var act = () => LevelSerializer.Deserialize(mockLevel.Object, reader);

            _ = act.Should().Throw<InvalidDataException>(fakeErrorMessage);
        }

        [TestMethod]
        public void ShouldGetAndSetUnknownA()
        {
            using var stream = new MemoryStream();
            using var reader = new BinaryReader(stream);

            // Serialize data
            using var writer = new BinaryWriter(stream);
            LevelSerializer.Serialize(new Level
            {
                UnknownA = 10
            }, writer);

            // Deserialize the data
            _ = stream.Seek(0, SeekOrigin.Begin);
            var resultLevel = new Level();
            LevelSerializer.Deserialize(resultLevel, reader);

            _ = resultLevel.UnknownA.Should().Be(10);
        }

        [TestMethod]
        public void Serialize_ShouldWriteTheCorrectHeaderBytes()
        {
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);
            using var reader = new BinaryReader(stream);

            var level = new Level();
            LevelSerializer.Serialize(level, writer);

            byte[] validHeader = { 0x20, 0x20, 0x49, 0x41, 0x12, 0x0, 0x0, 0x0 };

            _ = stream.Seek(0, SeekOrigin.Begin);
            var result = reader.ReadBytes(validHeader.Length);
            _ = result.Should().BeEquivalentTo(validHeader);
        }
    }
}