using FluentAssertions;
using LemballEditor.Models;

namespace LemballEditor.Tests.ModelTests
{
    [TestClass]
    public class LevelTests
    {
        [TestMethod]
        public void Constructor_ShouldInitialiseSensibleDefaults()
        {
            var level = new Level();
            _ = level.FlagsRequired.Should().Be(1);
            _ = level.NumberOfLemmings.Should().Be(1);
            _ = level.Theme.Should().Be(LevelTheme.Grass);
            _ = level.TimeLimitInSeconds.Should().BeNull();
            _ = level.UnknownA.Should().Be(10);
            _ = level.UnknownB.Should().Be(0);
        }

        [TestMethod]
        public void ShouldStoreValidUnknownAValues()
        {
            var level = new Level
            {
                UnknownA = 6
            };
            _ = level.UnknownA.Should().Be(6);

            level.UnknownA = 7;
            _ = level.UnknownA.Should().Be(7);

            level.UnknownA = 9;
            _ = level.UnknownA.Should().Be(9);

            level.UnknownA = 10;
            _ = level.UnknownA.Should().Be(10);
        }

        [TestMethod]
        public void ShouldThrowAnArgumentException_IfTheUnknownAValueIsNotValid()
        {
            var level = new Level();
            var act = () => level.UnknownA = 100;

            _ = act.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void ShouldStoreValidUnknownBValues()
        {
            var level = new Level
            {
                UnknownB = 33425
            };

            _ = level.UnknownB.Should().Be(33425);
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
            _ = act.Should().Throw<ArgumentException>().WithMessage("TimeLimitInSeconds should be no larger than 599");
        }

        [TestMethod]
        public void ShouldNotAllowNumberOfLemmingsToBeZero()
        {
            var level = new Level();
            var act = () => level.NumberOfLemmings = 0;
            _ = act.Should().Throw<ArgumentException>().WithMessage("NumberOfLemmings should be between 1-4. 0 given");
        }

        [TestMethod]
        public void ShouldNotAllowNumberOfLemmingsToBeAboveFour()
        {
            var level = new Level();
            var act = () => level.NumberOfLemmings = 5;
            _ = act.Should().Throw<ArgumentException>().WithMessage("NumberOfLemmings should be between 1-4. 5 given");
        }

        [TestMethod]
        public void ShouldStoreTheNumberOfLemmings()
        {
            var level = new Level()
            {
                NumberOfLemmings = 1
            };

            _ = level.NumberOfLemmings.Should().Be(1);

            level.NumberOfLemmings = 2;
            _ = level.NumberOfLemmings.Should().Be(2);

            level.NumberOfLemmings = 3;
            _ = level.NumberOfLemmings.Should().Be(3);

            level.NumberOfLemmings = 4;
            _ = level.NumberOfLemmings.Should().Be(4);
        }

        [TestMethod]
        public void ShouldStoreTheNumberOfFlagsRequiredToWinTheLevel()
        {
            var level = new Level()
            {
                FlagsRequired = 0
            };

            _ = level.FlagsRequired.Should().Be(0);

            level.FlagsRequired = 1;
            _ = level.FlagsRequired.Should().Be(1);

            level.FlagsRequired = 2;
            _ = level.FlagsRequired.Should().Be(2);

            level.FlagsRequired = 3;
            _ = level.FlagsRequired.Should().Be(3);

            level.FlagsRequired = 4;
            _ = level.FlagsRequired.Should().Be(4);
        }

        [TestMethod]
        public void ShouldNotAllowFlagsRequiredToBeAboveFour()
        {
            var level = new Level();
            var act = () => level.FlagsRequired = 5;
            _ = act.Should().Throw<ArgumentException>().WithMessage("Max FlagsRequired is 4. 5 given");
        }

        [TestMethod]
        public void ShouldStoreAMapInstance()
        {
            var level = new Level();
            var map = new Map(64, 64);

            level.Map = map;
            _ = level.Map.Should().Be(map);
        }
    }
}
