using FluentAssertions;
using LemballEditor.Models;
using LemballEditor.Serializers;
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

            _ = act.Should().Throw<InvalidDataException>("Invalid binary format: Invalid header.");
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