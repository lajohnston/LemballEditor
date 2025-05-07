using LemballEditor.Serializers.LevelDirectory;
using FluentAssertions;
using LemballEditor.Models;

namespace LemballEditor.Tests.SerializerTests.LevelDirectoryTests
{
    [TestClass]
    public class LevelDirectoryTests
    {
        public void ItShouldStoreTheFixedLevelCount()
        {
            var levelDirectory = new LevelDirectory();
            levelDirectory.FixedLevelCount = 5;
            levelDirectory.FixedLevelCount.Should().Be(5);
        }

        public void ItShouldStoreTheFirstFileId()
        {
            var levelDirectory = new LevelDirectory();
            levelDirectory.FirstFileId = 100;
            levelDirectory.FixedLevelCount.Should().Be(100);
        }

        public void ItShouldStoreTheDirectoryAddress()
        {
            var levelDirectory = new LevelDirectory();
            levelDirectory.Address = 2000;
            levelDirectory.Address.Should().Be(2000);
        }

        public void ItShouldStoreTheLevelGroup()
        {
            var levelDirectory = new LevelDirectory();
            var levelGroup = new LevelGroup(LevelGroupName.Fun);
            levelDirectory.LevelGroup = levelGroup;
            levelDirectory.LevelGroup.Should().Be(levelGroup);
        }
    }
}
