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

            mockLevelSerializer.Setup(m => m.Deserialize(It.IsAny<BinaryReader>(), It.IsAny<ILevel>()))
                .Returns((BinaryReader reader, ILevel level) => level);

            var levels = Enumerable.Range(0, model.FixedLevelCount)
                .Select(_ => new Mock<ILevel>().Object)
                .ToArray();

            var createdLevels = new Queue<ILevel>(levels);

            var mockLevelFactory = new Mock<Func<ILevel>>();
            mockLevelFactory.Setup(m => m()).Returns(() => createdLevels.Dequeue());

            var serializer = new LevelList(mockLevelSerializer.Object, mockLevelFactory.Object);

            return (serializer, mockLevelSerializer, mockLevelFactory, levels);
        }

        private LevelDirectory CreateLevelDirectory(int numberOfLevels)
        {
            var levelDirectory = new LevelDirectory();
            levelDirectory.FixedLevelCount = (byte)numberOfLevels;
            levelDirectory.LevelGroup = new LevelGroup(LevelGroupName.Fun);

            return levelDirectory;

        }

        [TestMethod]
        public void Deserialize_ShouldReturnTheGivenLevelDirectory()
        {
            var levelDirectory = new LevelDirectory();
            var (serializer, _, _, _) = CreateSerializer(levelDirectory);

            using var reader = new BinaryReader(new MemoryStream());

            var result = serializer.Deserialize(reader, levelDirectory);
            result.Should().Be(levelDirectory);
        }

        [TestMethod]
        [DataRow(1)]
        [DataRow(2)]
        public void Deserialize_ShouldCreateLevelsModelsForEachLevelAndPassThemToTheLevelDeserializer(int numberOfLevels)
        {
            var levelDirectory = CreateLevelDirectory(numberOfLevels);
            var (serializer, mockLevelSerializer, mockLevelFactory, levels) = CreateSerializer(levelDirectory);

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
            serializer.Deserialize(reader, levelDirectory);

            mockLevelFactory.Verify();
            mockLevelSerializer.Verify();
        }

        [TestMethod]
        public void Deserialize_ShouldPassTheReaderPositionedAtTheStartOfEachLevelToTheLevelDeserializer()
        {
            var numberOfLevels = 2;
            var levelDirectory = CreateLevelDirectory(numberOfLevels);
            var (serializer, mockLevelSerializer, mockLevelFactory, _) = CreateSerializer(levelDirectory);


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

            serializer.Deserialize(reader, levelDirectory);

            mockLevelSerializer.Verify();
        }

        [TestMethod]
        public void Deserialize_ShouldAddTheDeserializedLevelToTheLevelGroup()
        {
            var levelDirectory = CreateLevelDirectory(2);
            var (serializer, mockLevelSerializer, mockLevelFactory, levels) = CreateSerializer(levelDirectory);

            using var reader = new BinaryReader(new MemoryStream());
            serializer.Deserialize(reader, levelDirectory);

            levelDirectory.LevelGroup.GetLevel(0).Should().Be(levels[0]);
            levelDirectory.LevelGroup.GetLevel(1).Should().Be(levels[1]);
        }
    }
}
