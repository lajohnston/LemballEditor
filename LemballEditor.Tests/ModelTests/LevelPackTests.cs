using FluentAssertions;
using LemballEditor.Models;

namespace LemballEditor.Tests.ModelTests
{
    [TestClass]
    public sealed class LevelPackTests
    {
        [TestMethod]
        public void ShouldCreateDefaultLevelGroups()
        {
            var levelPack = new LevelPack();
            _ = levelPack.GetLevelGroup(LevelGroupName.Fun).Should().NotBeNull("because the Fun level group should have been returned");
            _ = levelPack.GetLevelGroup(LevelGroupName.Tricky).Should().NotBeNull("because the Tricky level group should have been returned");
            _ = levelPack.GetLevelGroup(LevelGroupName.Taxing).Should().NotBeNull("because the Taxing level group should have been returned");
            _ = levelPack.GetLevelGroup(LevelGroupName.Mayhem).Should().NotBeNull("because the Mayhem level group should have been returned");
            _ = levelPack.GetLevelGroup(LevelGroupName.Network).Should().NotBeNull("because the Network level group should have been returned");
        }

        [TestMethod]
        [DataRow(LevelGroupName.Fun)]
        [DataRow(LevelGroupName.Tricky)]
        [DataRow(LevelGroupName.Taxing)]
        [DataRow(LevelGroupName.Mayhem)]
        [DataRow(LevelGroupName.Network)]
        public void ShouldGetAndSetEachLevelGroup(LevelGroupName levelGroupName)
        {
            var levelGroup = new LevelGroup(levelGroupName);
            var levelPack = new LevelPack();

            levelPack.SetLevelGroup(levelGroup);
            _ = levelPack.GetLevelGroup(levelGroupName).Should().Be(levelGroup);
        }

        [TestMethod]
        public void ShouldThrowAnArgumentException_IfTheGivenLevelGroupIsNull()
        {
            var levelPack = new LevelPack();

            var act = () => levelPack.SetLevelGroup(null);
            _ = act.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void GetLevelGroups_ShouldReturnAnIteratorForEachLevelGroupInOrder()
        {
            var levelPack = new LevelPack();

            var expectedGroupOrder = new Queue<LevelGroup>([
                levelPack.GetLevelGroup(LevelGroupName.Fun),
                levelPack.GetLevelGroup(LevelGroupName.Tricky),
                levelPack.GetLevelGroup(LevelGroupName.Taxing),
                levelPack.GetLevelGroup(LevelGroupName.Mayhem),
                levelPack.GetLevelGroup(LevelGroupName.Network),
            ]);

            var iterator = levelPack.GetLevelGroups();
            iterator.ToArray().Length.Should().Be(5);

            foreach (var levelGroup in iterator)
            {
                levelGroup.Should().Be(expectedGroupOrder.Dequeue());
            }
        }
    }
}
