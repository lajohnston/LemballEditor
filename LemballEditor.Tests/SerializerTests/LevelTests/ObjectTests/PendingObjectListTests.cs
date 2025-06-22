using FluentAssertions;
using LemballEditor.Models.LevelObjects;
using LemballEditor.Serializers.Level.Objects;
using Moq;

namespace LemballEditor.Tests.SerializerTests.LevelTests.ObjectTests
{
    [TestClass]
    public class PendingObjectListTests
    {
        [TestMethod]
        public void Add_ShouldThrowArgumentNullException_WhenLevelObjectIsNull()
        {
            var list = new PendingObjectList();
            var act = () => list.Add(null);

            _ = act.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Subscribe_ShouldNotifySubscribers_WhenObjectIsAdded()
        {
            var mockLevelObject = new Mock<ILevelObject>().Object;
            var list = new PendingObjectList();

            PendingObject? received = null;
            list.Subscribe(po => received = po);

            list.Add(mockLevelObject, 123);

            _ = received.Should().NotBeNull();
            _ = received.LevelObject.Should().Be(mockLevelObject);
            _ = received.Id.Should().Be(123);
        }

        [TestMethod]
        public void GetLevelObjects_ShouldReturnAllLevelObjects()
        {
            var mockLevelObject1 = new Mock<ILevelObject>();
            var mockLevelObject2 = new Mock<ILevelObject>();

            var list = new PendingObjectList();
            list.Add(mockLevelObject1.Object);
            list.Add(mockLevelObject2.Object);

            var levelObjects = list.GetLevelObjects();

            _ = levelObjects.Should().HaveCount(2);
            _ = levelObjects.Should().Contain(mockLevelObject1.Object);
            _ = levelObjects.Should().Contain(mockLevelObject2.Object);
        }

        [TestMethod]
        public void GetPendingObjects_ShouldReturnAllPendingObjects()
        {
            var mockLevelObject1 = new Mock<ILevelObject>().Object;
            var mockLevelObject2 = new Mock<ILevelObject>().Object;

            var list = new PendingObjectList();
            list.Add(mockLevelObject1, 123);
            list.Add(mockLevelObject2, 456);

            var pendingObjects = list.GetPendingObjects();
            _ = pendingObjects.Should().HaveCount(2);

            _ = pendingObjects[0].LevelObject.Should().Be(mockLevelObject1);
            _ = pendingObjects[0].Id.Should().Be(123);

            _ = pendingObjects[1].LevelObject.Should().Be(mockLevelObject2);
            _ = pendingObjects[1].Id.Should().Be(456);
        }

        [TestMethod]
        public void AssignIds_ShouldAssignAnIncrementingIdToEachPendingObject()
        {
            var list = new PendingObjectList();
            var mockLevelObject1 = new Mock<ILevelObject>();
            var mockLevelObject2 = new Mock<ILevelObject>();

            list.Add(mockLevelObject1.Object);
            list.Add(mockLevelObject2.Object);

            list.AssignIds();

            var pendingObjects = list.GetPendingObjects();
            _ = pendingObjects.Should().HaveCount(2);
            _ = pendingObjects[0].Id.Should().Be(0);
            _ = pendingObjects[1].Id.Should().Be(1);
        }
    }
}
