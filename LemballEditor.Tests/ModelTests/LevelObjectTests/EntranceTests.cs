using FluentAssertions;
using LemballEditor.Models.LevelObjects;

namespace LemballEditor.Tests.ModelTests.LevelObjectTests
{
    [TestClass]
    public class EntranceTests
    {
        [TestMethod]
        public void Constructor_SetsPositionAndNumberOfLemmings()
        {
            var position = new Position(10, 20);
            var entrance = new Entrance(position, 3);

            _ = entrance.Position.Should().Be(position);
            _ = entrance.NumberOfLemmings.Should().Be(3);
        }

        [TestMethod]
        public void Constructor_ShouldThrow_WhenNumberOfLemmingsIsZero()
        {
            var position = new Position(1, 1);
            var act = () => new Entrance(position, 0);
            _ = act.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        [DataRow(5)]
        [DataRow(6)]
        public void Constructor_ShouldThrow_WhenNumberOfLemmingsIsOverFour(int numberOfLemmings)
        {
            var position = new Position(1, 1);
            var act = () => new Entrance(position, (byte)numberOfLemmings);
            _ = act.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void WithPosition_ReturnsNewEntranceWithNewPosition()
        {
            var position1 = new Position(1, 2);
            var entrance = new Entrance(position1, 2);

            var position2 = new Position(3, 4);
            var newEntrance = (Entrance)entrance.WithPosition(position2);

            _ = newEntrance.Position.Should().Be(position2);
            _ = newEntrance.NumberOfLemmings.Should().Be(1);
        }

        [TestMethod]
        public void WithNumberOfLemmings_ReturnsNewEntranceWithNewLemmingCount()
        {
            var position = new Position(5, 6);
            var entrance = new Entrance(position, 2);

            var newEntrance = entrance.WithNumberOfLemmings(4);

            _ = newEntrance.Should().NotBe(entrance);
            _ = newEntrance.Position.Should().Be(position);
            _ = newEntrance.NumberOfLemmings.Should().Be(4);
        }
    }
}
