using FluentAssertions;
using LemballEditor.Models;

namespace LemballEditor.Tests.ModelTests
{
    [TestClass]
    public sealed class LevelPackTests
    {
        [TestMethod]
        public void ShouldReturnTheFunLevelGroup()
        {
            var levelPack = new LevelPack();
            var result = levelPack.GetLevelGroup(LevelGroupName.Fun);
            _ = result.Should().NotBeNull("because the Fun level group should have been returned");
        }

        [TestMethod]
        public void ShouldReturnTheTrickyLevelGroup()
        {
            var levelPack = new LevelPack();
            var result = levelPack.GetLevelGroup(LevelGroupName.Tricky);
            _ = result.Should().NotBeNull("because the Tricky level group should have been returned");
        }

        [TestMethod]
        public void ShouldReturnTheTaxingLevelGroup()
        {
            var levelPack = new LevelPack();
            var result = levelPack.GetLevelGroup(LevelGroupName.Taxing);
            _ = result.Should().NotBeNull("because the Taxing level group should have been returned");
        }

        [TestMethod]
        public void ShouldReturnTheMayhemLevelGroup()
        {
            var levelPack = new LevelPack();
            var result = levelPack.GetLevelGroup(LevelGroupName.Mayhem);
            _ = result.Should().NotBeNull("because the Mayhem level group should have been returned");
        }

        [TestMethod]
        public void ShouldReturnTheNetworkLevelGroup()
        {
            var levelPack = new LevelPack();
            var result = levelPack.GetLevelGroup(LevelGroupName.Network);
            _ = result.Should().NotBeNull("because the Network level group should have been returned");
        }
    }
}
