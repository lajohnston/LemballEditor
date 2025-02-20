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
        private ILevel serializeAndDeserialize(ILevel level)
        {
            using var stream = new MemoryStream();
            using var reader = new BinaryReader(stream);

            // Serialize data
            using var writer = new BinaryWriter(stream);
            LevelSerializer.Serialize(level, writer);

            // Deserialize the data
            var result = new Level();
            _ = stream.Seek(0, SeekOrigin.Begin);
            LevelSerializer.Deserialize(result, reader);

            return result;
        }

        private byte[] getSerializedData(ILevel level)
        {
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            LevelSerializer.Serialize(level, writer);

            return stream.ToArray();
        }

        private void deserialize(byte[] data, ILevel level)
        {
            using var stream = new MemoryStream(data);
            using var reader = new BinaryReader(stream);

            LevelSerializer.Deserialize(level, reader);
        }

        [TestMethod]
        public void Deserialize_ShouldThrowAnInvalidDataException_WhenHeaderIsNotValid()
        {
            var invalidHeader = Encoding.UTF8.GetBytes("UNEXPECTED HEADER BYTES");
            var act = () => deserialize(invalidHeader, new Level());
            _ = act.Should().Throw<InvalidDataException>().WithMessage("Invalid header value");
        }

        [TestMethod]
        public void Deserialize_ShouldThrowAnExceptionIfTheUnknownAValueIsInvalid()
        {
            var level = new Level();
            var data = getSerializedData(level);

            // Mock a level that throws an exception when UnknownA is set
            var fakeErrorMessage = "Some Error";
            var mockLevel = new Mock<ILevel>();

            _ = mockLevel.SetupSet(m => m.UnknownA = It.IsAny<ushort>())
                    .Throws<ArgumentException>(() => new ArgumentException(fakeErrorMessage));

            var act = () => deserialize(data, mockLevel.Object);
            _ = act.Should().Throw<InvalidDataException>().WithMessage(fakeErrorMessage);
        }

        [TestMethod]
        public void Deserialize_ShouldThrowAnExceptionIfTheThemeIsNotValid()
        {
            var level = new Level();
            var data = getSerializedData(level);

            data[10] = 100;

            var act = () => deserialize(data, level);
            _ = act.Should().Throw<InvalidDataException>().WithMessage($"Theme value should be between 0-3, 100 given");
        }

        [TestMethod]
        public void ShouldGetAndSetUnknownA()
        {
            var level = new Level()
            {
                UnknownA = 10
            };

            var result = serializeAndDeserialize(level);
            _ = result.UnknownA.Should().Be(10);
        }

        [TestMethod]
        public void ShouldGetAndSetGrassTheme()
        {
            var level = new Level()
            {
                Theme = LevelTheme.Grass
            };

            var result = serializeAndDeserialize(level);
            _ = result.Theme.Should().Be(LevelTheme.Grass);
        }

        [TestMethod]
        public void ShouldGetAndSetLegoTheme()
        {
            var level = new Level()
            {
                Theme = LevelTheme.Lego
            };

            var result = serializeAndDeserialize(level);
            _ = result.Theme.Should().Be(LevelTheme.Lego);
        }

        [TestMethod]
        public void ShouldGetAndSetSnowTheme()
        {
            var level = new Level()
            {
                Theme = LevelTheme.Snow
            };

            var result = serializeAndDeserialize(level);
            _ = result.Theme.Should().Be(LevelTheme.Snow);
        }

        [TestMethod]
        public void ShouldGetAndSetSpaceTheme()
        {
            var level = new Level()
            {
                Theme = LevelTheme.Space
            };

            var result = serializeAndDeserialize(level);
            _ = result.Theme.Should().Be(LevelTheme.Space);
        }

        [TestMethod]
        public void Serialize_ShouldWriteTheCorrectHeaderBytes()
        {
            var level = new Level();
            var data = getSerializedData(level);

            using var stream = new MemoryStream(data);
            using var reader = new BinaryReader(stream);

            byte[] validHeader = { 0x20, 0x20, 0x49, 0x41, 0x12, 0x0, 0x0, 0x0 };

            _ = stream.Seek(0, SeekOrigin.Begin);
            var result = reader.ReadBytes(validHeader.Length);
            _ = result.Should().BeEquivalentTo(validHeader);
        }
    }
}