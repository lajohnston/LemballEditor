using FluentAssertions;
using LemballEditor.Models;
using LemballEditor.Serializers;
using LemballEditor.Serializers.Level;
using Moq;

namespace LemballEditor.Tests.SerializerTests
{
    [TestClass]
    public class LevelPackSerializerTests
    {
        [TestMethod]
        public void Deserialize_ShouldCreateTheLevelGroupsAndPassEachToTheLevelGroupDeserializer()
        {
            using var stream = new MemoryStream();
            using var reader = new BinaryReader(stream);

            var levelGroups = new List<LevelGroup>() { new(), new(), new(), new(), new(), };
            var levelGroupSerializer = new Mock<ISerializer<LevelGroup>>();

            var timesCalled = 0;
            LevelGroup LevelGroupFactory()
            {
                var levelGroup = levelGroups[timesCalled];
                timesCalled++;
                return levelGroup;
            }

            foreach (var levelGroup in levelGroups)
            {
                _ = levelGroupSerializer.Setup(m => m.Deserialize(
                    It.Is<BinaryReader>(r => r == reader),
                    It.Is<LevelGroup>(lg => lg == levelGroup)
                )).Returns(levelGroup);
            }

            var levelPack = new LevelPack();
            var serializer = new LevelPackSerializer(levelGroupSerializer.Object, LevelGroupFactory);
            _ = serializer.Deserialize(reader, levelPack);

            levelGroupSerializer.VerifyAll();

            _ = levelPack.GetLevelGroup(LevelGroupName.Fun).Should().Be(levelGroups[0]);
            _ = levelPack.GetLevelGroup(LevelGroupName.Tricky).Should().Be(levelGroups[1]);
            _ = levelPack.GetLevelGroup(LevelGroupName.Taxing).Should().Be(levelGroups[2]);
            _ = levelPack.GetLevelGroup(LevelGroupName.Mayhem).Should().Be(levelGroups[3]);
            _ = levelPack.GetLevelGroup(LevelGroupName.Network).Should().Be(levelGroups[4]);
        }

        [TestMethod]
        public void Deserialize_ShouldReturnTheGivenLevelPack()
        {
            using var stream = new MemoryStream();
            using var reader = new BinaryReader(stream);

            var levelGroupSerializer = new Mock<ISerializer<LevelGroup>>();

            static LevelGroup LevelGroupFactory()
            {
                return new LevelGroup();
            }

            _ = levelGroupSerializer.Setup(m => m.Deserialize(It.IsAny<BinaryReader>(), It.IsAny<LevelGroup>())).Returns(new LevelGroup());

            var levelPack = new LevelPack();
            var serializer = new LevelPackSerializer(levelGroupSerializer.Object, LevelGroupFactory);
            _ = serializer.Deserialize(reader, levelPack).Should().Be(levelPack);
        }
    }
}
