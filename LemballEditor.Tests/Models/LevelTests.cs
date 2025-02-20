using FluentAssertions;
using LemballEditor.Models;

namespace LemballEditor.Tests.Models
{
    [TestClass]
    public class LevelTests
    {
        [TestMethod]
        public void UnknownA_ShouldStoreValidValues()
        {
            var level = new Level();
            _ = level.UnknownA.Should().Be(9);

            level.UnknownA = 6;
            _ = level.UnknownA.Should().Be(6);

            level.UnknownA = 7;
            _ = level.UnknownA.Should().Be(7);

            level.UnknownA = 9;
            _ = level.UnknownA.Should().Be(9);

            level.UnknownA = 10;
            _ = level.UnknownA.Should().Be(10);
        }

        [TestMethod]
        public void UnknownA_ShouldThrowAnArgumentException_IfTheValueIsNotValid()
        {
            var level = new Level();
            var act = () => level.UnknownA = 100;

            _ = act.Should().Throw<ArgumentException>();
        }
    }
}
