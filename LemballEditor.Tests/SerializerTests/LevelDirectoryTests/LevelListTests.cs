using System.Text;
using FluentAssertions;
using LemballEditor.Models;
using LemballEditor.Serializers;
using LemballEditor.Serializers.LevelDirectory;
using Moq;

namespace LemballEditor.Tests.SerializerTests.LevelDirectoryTests
{
    [TestClass]
    public class LevelListTests
    {
        private (LevelList, Mock<ISerializer<ILevel>>, Mock<Func<ILevel>>, ILevel[]) CreateSerializer(LevelDirectory model)
        {
            var mockLevelSerializer = new Mock<ISerializer<ILevel>>();

            _ = mockLevelSerializer.Setup(m => m.Deserialize(It.IsAny<BinaryReader>(), It.IsAny<ILevel>()))
                .Returns((BinaryReader reader, ILevel level) => level);

            var levels = Enumerable.Range(0, model.FixedLevelCount)
                .Select(_ => new Mock<ILevel>().Object)
                .ToArray();

            var createdLevels = new Queue<ILevel>(levels);

            var mockLevelFactory = new Mock<Func<ILevel>>();
            _ = mockLevelFactory.Setup(m => m()).Returns(createdLevels.Dequeue);

            var serializer = new LevelList(mockLevelSerializer.Object, mockLevelFactory.Object);

            return (serializer, mockLevelSerializer, mockLevelFactory, levels);
        }

        private LevelDirectory CreateLevelDirectory(int numberOfLevels)
        {
            var levelDirectory = new LevelDirectory
            {
                FixedLevelCount = (byte)numberOfLevels,
                LevelGroup = new LevelGroup(LevelGroupName.Fun)
            };

            return levelDirectory;
        }

        [TestMethod]
        public void Deserialize_ShouldReturnTheGivenLevelDirectory()
        {
            var levelDirectory = new LevelDirectory();
            var (serializer, _, _, _) = this.CreateSerializer(levelDirectory);

            using var reader = new BinaryReader(new MemoryStream());

            var result = serializer.Deserialize(reader, levelDirectory);
            _ = result.Should().Be(levelDirectory);
        }

        [TestMethod]
        [DataRow(1)]
        [DataRow(2)]
        public void Deserialize_ShouldCreateLevelsModelsForEachLevelAndPassThemToTheLevelDeserializer(int numberOfLevels)
        {
            var levelDirectory = this.CreateLevelDirectory(numberOfLevels);
            var (serializer, mockLevelSerializer, mockLevelFactory, levels) = this.CreateSerializer(levelDirectory);

            var createdLevels = new Queue<ILevel>(levels);
            var expectedLevels = new Queue<ILevel>(levels);

            mockLevelFactory.Setup(m => m())
                .Returns(createdLevels.Dequeue())
                .Verifiable();

            mockLevelSerializer.Setup(m =>
                m.Deserialize(
                    It.IsAny<BinaryReader>(),
                    It.Is<ILevel>(level => level == expectedLevels.Dequeue())
                )
            ).Verifiable();

            using var reader = new BinaryReader(new MemoryStream());
            _ = serializer.Deserialize(reader, levelDirectory);

            mockLevelFactory.Verify();
            mockLevelSerializer.Verify();
        }

        [TestMethod]
        public void Deserialize_ShouldPassTheReaderPositionedAtTheStartOfEachLevelToTheLevelDeserializer()
        {
            var numberOfLevels = 2;
            var levelDirectory = this.CreateLevelDirectory(numberOfLevels);
            var (serializer, mockLevelSerializer, mockLevelFactory, _) = this.CreateSerializer(levelDirectory);

            using var reader = new BinaryReader(new MemoryStream());
            reader.BaseStream.Position = 1000;

            // First level
            mockLevelSerializer.Setup(m =>
                m.Deserialize(
                    It.Is<BinaryReader>(r => r == reader && reader.BaseStream.Position == 1008),
                    It.IsAny<ILevel>()
                )
            ).Callback((BinaryReader reader, ILevel level) => reader.BaseStream.Position += 100)
            .Returns((BinaryReader reader, ILevel level) => level)
            .Verifiable();

            // Second level
            mockLevelSerializer.Setup(m =>
                m.Deserialize(
                    It.Is<BinaryReader>(r => r == reader && reader.BaseStream.Position == 1116),
                    It.IsAny<ILevel>()
                )
            )
            .Returns((BinaryReader reader, ILevel level) => level)
            .Verifiable();

            _ = serializer.Deserialize(reader, levelDirectory);

            mockLevelSerializer.Verify();
        }

        [TestMethod]
        public void Deserialize_ShouldAddTheDeserializedLevelToTheLevelGroup()
        {
            var levelDirectory = this.CreateLevelDirectory(2);
            var (serializer, _, _, levels) = this.CreateSerializer(levelDirectory);

            using var reader = new BinaryReader(new MemoryStream());
            _ = serializer.Deserialize(reader, levelDirectory);

            _ = levelDirectory.LevelGroup.GetLevel(0).Should().Be(levels[0]);
            _ = levelDirectory.LevelGroup.GetLevel(1).Should().Be(levels[1]);
        }

        [TestMethod]
        public void Serialize_ShouldWriteTheBinStringForEachLevel()
        {
            var levelDirectory = this.CreateLevelDirectory(3);

            levelDirectory.AddSerializedLevel(new byte[50]);
            levelDirectory.AddSerializedLevel(new byte[50]);
            levelDirectory.AddSerializedLevel(new byte[50]);

            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            var (serializer, _, _, _) = this.CreateSerializer(levelDirectory);
            serializer.Serialize(levelDirectory, writer);

            using var reader = new BinaryReader(stream);

            stream.Position = 0;
            _ = Encoding.ASCII.GetString(reader.ReadBytes(4)).Should().Be(" NIB");

            stream.Position += 4 + 50; // skip size and level bytes
            _ = Encoding.ASCII.GetString(reader.ReadBytes(4)).Should().Be(" NIB");

            stream.Position += 4 + 50; // skip size and level bytes
            _ = Encoding.ASCII.GetString(reader.ReadBytes(4)).Should().Be(" NIB");
        }

        [TestMethod]
        public void Serialize_ShouldWriteTheBinarySizeOfEachLevel()
        {
            var levelDirectory = this.CreateLevelDirectory(3);

            levelDirectory.AddSerializedLevel(new byte[50]);
            levelDirectory.AddSerializedLevel(new byte[60]);
            levelDirectory.AddSerializedLevel(new byte[70]);

            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            var (serializer, _, _, _) = this.CreateSerializer(levelDirectory);
            serializer.Serialize(levelDirectory, writer);

            using var reader = new BinaryReader(stream);

            stream.Position = 4;
            _ = reader.ReadUInt32().Should().Be(50);

            stream.Position += 50 + 4; // skip level and header bytes
            _ = reader.ReadUInt32().Should().Be(60);

            stream.Position += 60 + 4; // skip level and header bytes
            _ = reader.ReadUInt32().Should().Be(70);
        }

        [TestMethod]
        public void Serialize_ShouldWriteTheLevelDataForEachLevel()
        {
            var levelDirectory = this.CreateLevelDirectory(3);

            var levels = new byte[][]
            {
                Enumerable.Repeat((byte)1, 50).ToArray(),
                Enumerable.Repeat((byte)2, 50).ToArray(),
                Enumerable.Repeat((byte)3, 50).ToArray(),
            };

            levelDirectory.AddSerializedLevel(levels[0]);
            levelDirectory.AddSerializedLevel(levels[1]);
            levelDirectory.AddSerializedLevel(levels[2]);

            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            var (serializer, _, _, _) = this.CreateSerializer(levelDirectory);
            serializer.Serialize(levelDirectory, writer);

            using var reader = new BinaryReader(stream);

            stream.Position = 8;    // skip header and size bytes
            _ = reader.ReadBytes(50).Should().BeEquivalentTo(levels[0]);

            stream.Position += 8;   // skip header and size bytes
            _ = reader.ReadBytes(50).Should().BeEquivalentTo(levels[1]);

            stream.Position += 8;   // skip header and size bytes
            _ = reader.ReadBytes(50).Should().BeEquivalentTo(levels[2]);
        }
    }
}
