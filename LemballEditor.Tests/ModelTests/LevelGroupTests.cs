using FluentAssertions;
using LemballEditor.Models;

namespace LemballEditor.Tests.ModelTests
{
    [TestClass]
    public sealed class LevelGroupTests
    {
        private static readonly IModelFactory modelFactory = new ModelFactory();

        [TestMethod]
        public void ShouldReturnNullIfTheLevelAtTheGivenIndexDoesNotExist()
        {
            var group = new LevelGroup();
            _ = group.GetLevel(0).Should().BeNull();
        }

        [TestMethod]
        public void ShouldStoreLevelsInTheOrderGiven()
        {
            var group = new LevelGroup();

            var level1 = modelFactory.CreateLevel(1, 1);
            var level2 = modelFactory.CreateLevel(1, 1);
            var level3 = modelFactory.CreateLevel(1, 1);

            group.AddLevel(level1);
            group.AddLevel(level2);
            group.AddLevel(level3);

            _ = group.GetLevel(0).Should().Be(level1);
            _ = group.GetLevel(1).Should().Be(level2);
            _ = group.GetLevel(2).Should().Be(level3);
        }
    }
}
