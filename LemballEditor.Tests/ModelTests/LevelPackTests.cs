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
        public void ShouldGetAndSetTheFunLevelGroup()
        {
            var levelGroup = new LevelGroup();
            var levelPack = new LevelPack();

            levelPack.SetLevelGroup(LevelGroupName.Fun, levelGroup);
            _ = levelPack.GetLevelGroup(LevelGroupName.Fun).Should().Be(levelGroup);
        }

        [TestMethod]
        public void ShouldGetAndSetTheTrickyLevelGroup()
        {
            var levelGroup = new LevelGroup();
            var levelPack = new LevelPack();

            levelPack.SetLevelGroup(LevelGroupName.Tricky, levelGroup);
            _ = levelPack.GetLevelGroup(LevelGroupName.Tricky).Should().Be(levelGroup);
        }

        [TestMethod]
        public void ShouldGetAndSetTheTaxingLevelGroup()
        {
            var levelGroup = new LevelGroup();
            var levelPack = new LevelPack();

            levelPack.SetLevelGroup(LevelGroupName.Taxing, levelGroup);
            _ = levelPack.GetLevelGroup(LevelGroupName.Taxing).Should().Be(levelGroup);
        }

        [TestMethod]
        public void ShouldGetAndSetTheMayhemLevelGroup()
        {
            var levelGroup = new LevelGroup();
            var levelPack = new LevelPack();

            levelPack.SetLevelGroup(LevelGroupName.Mayhem, levelGroup);
            _ = levelPack.GetLevelGroup(LevelGroupName.Mayhem).Should().Be(levelGroup);
        }

        [TestMethod]
        public void ShouldGetAndSetTheNetworkLevelGroup()
        {
            var levelGroup = new LevelGroup();
            var levelPack = new LevelPack();

            levelPack.SetLevelGroup(LevelGroupName.Network, levelGroup);
            _ = levelPack.GetLevelGroup(LevelGroupName.Network).Should().Be(levelGroup);
        }

        [TestMethod]
        public void ShouldThrowAnArgumentException_IfTheGivenLevelGroupIsNull()
        {
            var levelPack = new LevelPack();

            var act = () => levelPack.SetLevelGroup(LevelGroupName.Fun, null);
            _ = act.Should().Throw<ArgumentNullException>();
        }
    }
}
