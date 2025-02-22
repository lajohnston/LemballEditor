using FluentAssertions;
using LemballEditor.Models;

namespace LemballEditor.Tests.ModelTests
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

        [TestMethod]
        public void ShouldDefaultTheLevelThemeToGrass()
        {
            var level = new Level();
            _ = level.Theme.Should().Be(LevelTheme.Grass);
        }

        [TestMethod]
        public void ShouldStoreTheGivenLevelTheme()
        {
            var level = new Level
            {
                Theme = LevelTheme.Snow
            };
            _ = level.Theme.Should().Be(LevelTheme.Snow);
        }

        [TestMethod]
        public void ShouldStoreTheTimeLimitInSeconds()
        {
            var level = new Level
            {
                TimeLimitInSeconds = 60
            };
            _ = level.TimeLimitInSeconds.Should().Be(60);
        }

        [TestMethod]
        public void ShouldSetTheTimeLimitToInfiniteByDefault()
        {
            var level = new Level();
            _ = level.TimeLimitInSeconds.Should().BeNull();
        }

        [TestMethod]
        public void ShouldNotAllowTimeLimitValuesAbove599()
        {
            var level = new Level();
            var act = () => level.TimeLimitInSeconds = 600;
            _ = act.Should().Throw<ArgumentException>().WithMessage("Value should be no larger than 599");
        }
    }
}
