using LemballEditor.Serializers.LevelDirectory;
using FluentAssertions;
using LemballEditor.Models;

namespace LemballEditor.Tests.SerializerTests.LevelDirectoryTests
{
    [TestClass]
    public class LevelDirectoryTests
    {
        [TestMethod]
        public void ItShouldStoreTheFixedLevelCount()
        {
            var levelDirectory = new LevelDirectorySerializer();
            levelDirectory.FixedLevelCount = 5;
            levelDirectory.FixedLevelCount.Should().Be(5);
        }

        [TestMethod]
        public void ItShouldStoreTheFirstFileId()
        {
            var levelDirectory = new LevelDirectorySerializer();
            levelDirectory.FirstFileId = 100;
            levelDirectory.FirstFileId.Should().Be(100);
        }

        [TestMethod]
        public void ItShouldStoreTheDirectoryAddress()
        {
            var levelDirectory = new LevelDirectorySerializer();
            levelDirectory.Address = 2000;
            levelDirectory.Address.Should().Be(2000);
        }

        [TestMethod]
        public void ItShouldStoreTheLevelGroup()
        {
            var levelDirectory = new LevelDirectorySerializer();
            var levelGroup = new LevelGroup(LevelGroupName.Fun);
            levelDirectory.LevelGroup = levelGroup;
            levelDirectory.LevelGroup.Should().Be(levelGroup);
        }

        [TestMethod]
        public void AddSerializedLevel_ShouldThrowAnExceptionIfTheLevelsExceedTheFixedAmount()
        {
            var levelDirectory = new LevelDirectorySerializer();
            levelDirectory.FixedLevelCount = 1;

            levelDirectory.AddSerializedLevel(new byte[10]);

            var act = () => levelDirectory.AddSerializedLevel(new byte[10]);
            act.Should().Throw<InvalidOperationException>()
                .WithMessage("Level directory fixed size exceeded");
        }

        [TestMethod]
        public void GetSerializedLevels_ShouldReturnTheAddedLevelsInOrder()
        {
            var levelDirectory = new LevelDirectorySerializer();
            levelDirectory.FixedLevelCount = 3;

            var levels = new byte[][] {
                new byte[10],
                new byte[20],
                new byte[30],
            };

            foreach (var level in levels)
            {
                levelDirectory.AddSerializedLevel(level);
            }

            levelDirectory.GetSerializedLevels().ToArray().Should().BeEquivalentTo(levels);
        }

        [TestMethod]
        public void GetSerializedLevels_ShouldReturnBlankLevelsToFillTheFixedSize()
        {
            var levelDirectory = new LevelDirectorySerializer();
            levelDirectory.FixedLevelCount = 3;

            var level = new byte[10];
            levelDirectory.AddSerializedLevel(level);

            var resultLevels = levelDirectory.GetSerializedLevels().ToArray();
            resultLevels.Length.Should().Be(3);

            resultLevels[0].Should().BeEquivalentTo(level);
            resultLevels[1].Should().BeEquivalentTo(resultLevels[2]);
        }

        [TestMethod]
        [DataRow(new int[] { }, 0, 0)]
        [DataRow(new int[] { 100 }, 1, 100)]
        [DataRow(new int[] { }, 5, 396 * 5)]
        [DataRow(new int[] { 100, 200 }, 5, 300 + (396 * 3))]
        public void GetDataSizeInBytes_ShouldReturnTheSizeOfAllTheLevelsIncludingBlankLevelsInBytes(int[] levelSizes, int fixedNumberOfLevels, int expectedSize)
        {
            var levelDirectory = new LevelDirectorySerializer();
            levelDirectory.FixedLevelCount = (byte)fixedNumberOfLevels;

            foreach (uint levelSize in levelSizes)
            {
                levelDirectory.AddSerializedLevel(new byte[levelSize]);
            }

            levelDirectory.GetDataSizeInBytes().Should().Be((uint)expectedSize);
        }
    }
}
