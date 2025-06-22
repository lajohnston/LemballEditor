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
        public void Add_ShouldAddPendingObjectAndReturnIt()
        {
            var mockLevelObject = new Mock<ILevelObject>();
            var list = new PendingObjectList();

            var pendingObject = list.Add(mockLevelObject.Object);

            _ = pendingObject.Should().NotBeNull();
            _ = pendingObject.LevelObject.Should().Be(mockLevelObject.Object);
            _ = list.GetLevelObjects().Should().ContainSingle().Which.Should().Be(mockLevelObject.Object);
        }

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
            var mockLevelObject = new Mock<ILevelObject>();
            var list = new PendingObjectList();

            PendingObject? received = null;
            list.Subscribe(po => received = po);

            var pendingObject = list.Add(mockLevelObject.Object);

            _ = received.Should().Be(pendingObject);
        }

        [TestMethod]
        public void GetLevelObjects_ShouldReturnAllLevelObjects()
        {
            var mockLevelObject1 = new Mock<ILevelObject>();
            var mockLevelObject2 = new Mock<ILevelObject>();

            var list = new PendingObjectList();
            _ = list.Add(mockLevelObject1.Object);
            _ = list.Add(mockLevelObject2.Object);

            var levelObjects = list.GetLevelObjects();

            _ = levelObjects.Should().HaveCount(2);
            _ = levelObjects.Should().Contain(mockLevelObject1.Object);
            _ = levelObjects.Should().Contain(mockLevelObject2.Object);
        }

        [TestMethod]
        public void GetPendingObjects_ShouldReturnAllPendingObjects()
        {
            var mockLevelObject1 = new Mock<ILevelObject>();
            var mockLevelObject2 = new Mock<ILevelObject>();

            var list = new PendingObjectList();
            var pendingObject1 = list.Add(mockLevelObject1.Object);
            var pendingObject2 = list.Add(mockLevelObject2.Object);

            var pendingObjects = list.GetPendingObjects();
            _ = pendingObjects.Should().HaveCount(2);

            _ = pendingObjects[0].Should().Be(pendingObject1);
            _ = pendingObjects[1].Should().Be(pendingObject2);
        }

        [TestMethod]
        public void AssignIds_ShouldAssignAnIncrementingIdToEachPendingObject()
        {
            var list = new PendingObjectList();
            var mockLevelObject1 = new Mock<ILevelObject>();
            var mockLevelObject2 = new Mock<ILevelObject>();

            _ = list.Add(mockLevelObject1.Object);
            _ = list.Add(mockLevelObject2.Object);

            list.AssignIds();

            var pendingObjects = list.GetPendingObjects();
            _ = pendingObjects.Should().HaveCount(2);
            _ = pendingObjects[0].Id.Should().Be(0);
            _ = pendingObjects[1].Id.Should().Be(1);
        }
    }
}
