using FluentAssertions;
using LemballEditor.Models.LevelObjects;
using LemballEditor.Serializers.Level.Objects;
using Moq;

namespace LemballEditor.Tests.SerializerTests.LevelTests.ObjectTests
{
    [TestClass]
    public class PendingObjectTests
    {
        [TestMethod]
        public void Constructor_ShouldSetTheLevelObject()
        {
            var levelObject = new Mock<ILevelObject>().Object;
            var pending = new PendingObject(levelObject);

            _ = pending.LevelObject.Should().BeSameAs(levelObject);
        }

        [TestMethod]
        public void Id_Property_ShouldGetAndSet()
        {
            var pending = new PendingObject(new Mock<ILevelObject>().Object)
            {
                Id = 42
            };

            _ = pending.Id.Should().Be(42);
        }

        [TestMethod]
        public void Constructor_ShouldThrow_WhenLevelObjectIsNull()
        {
            var act = () => new PendingObject(null);

            _ = act.Should().Throw<ArgumentNullException>();
        }
    }
}
