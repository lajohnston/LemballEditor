using System.Text;
using FluentAssertions;
using LemballEditor.Models;
using LemballEditor.Serializers.Level.Objects;
using Moq;

namespace LemballEditor.Tests.SerializerTests.LevelTests.ObjectTests
{
    [TestClass]
    public class NameBlockSerializerTests
    {
        private NameBlockSerializer? serializer;
        private Mock<ILevel>? mockLevel;
        private PendingObjectList? model;

        [TestInitialize]
        public void Setup()
        {
            this.serializer = new NameBlockSerializer();

            this.mockLevel = new Mock<ILevel>();

            _ = this.mockLevel.SetupProperty(l => l.Name);
            this.model = new PendingObjectList(this.mockLevel.Object);
        }

        [DataTestMethod]
        [DataRow("TestLevel")]
        [DataRow("")]
        [DataRow("A")]
        [DataRow("1234567890123456789012345678901")]
        public void Deserialize_ShouldReadToTheEndOfTheBlockAndSetTheLevelName(string levelName)
        {
            var nameBytes = Encoding.ASCII.GetBytes(levelName);
            var buffer = new byte[32];
            Array.Copy(nameBytes, buffer, nameBytes.Length);

            using var stream = new MemoryStream(buffer);
            using var reader = new BinaryReader(stream);

            var result = this.serializer!.Deserialize(reader, this.model);

            _ = this.mockLevel!.Object.Name.Should().Be(levelName);
            _ = result.Should().BeSameAs(this.model);

            _ = stream.Position.Should().Be(32);
        }

        [DataTestMethod]
        [DataRow("12345678901234567890123456789012")]
        [DataRow("123456789012345678901234567890123")]
        public void Deserialize_ShouldThrow_WhenNameExceeds31Characters(string levelName)
        {
            var nameBytes = Encoding.ASCII.GetBytes(levelName);
            var buffer = new byte[40];
            Array.Copy(nameBytes, buffer, nameBytes.Length);

            using var stream = new MemoryStream(buffer);
            using var reader = new BinaryReader(stream);

            var act = () => this.serializer!.Deserialize(reader, this.model);

            _ = act.Should().Throw<InvalidDataException>()
                .WithMessage("Level name at position 0 does not contain a null terminator");
        }

        [DataTestMethod]
        [DataRow("TestLevel")]
        [DataRow("")]
        [DataRow("A")]
        [DataRow("1234567890123456789012345678901")]
        public void Serialize_ShouldWriteNameAndPadWithZeros(string levelName)
        {
            this.mockLevel!.Object.Name = levelName;

            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            this.serializer!.Serialize(this.model, writer);

            var written = stream.ToArray();
            var expectedBytes = Encoding.ASCII.GetBytes(levelName);

            _ = written.Length.Should().Be(32);
            _ = written.AsSpan(0, expectedBytes.Length).ToArray().Should().BeEquivalentTo(expectedBytes);
            _ = written.AsSpan(expectedBytes.Length).ToArray().Should().OnlyContain(b => b == 0);
        }

        [TestMethod]
        public void Serialize_ShouldPadWithZeros_WhenNameIsNull()
        {
            this.mockLevel!.Object.Name = null;

            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            this.serializer!.Serialize(this.model, writer);

            var written = stream.ToArray();

            _ = written.Length.Should().Be(32);
            _ = written.AsSpan(32).ToArray().Should().OnlyContain(b => b == 0);
        }

        [TestMethod]
        public void Serialize_ShouldThrow_WhenNameExceeds31Characters()
        {
            this.mockLevel!.Object.Name = new string('A', 32);

            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            var act = () => this.serializer!.Serialize(this.model, writer);

            _ = act.Should().Throw<ArgumentException>()
                .WithMessage("Level name cannot exceed 31 characters.");
        }
    }
}
