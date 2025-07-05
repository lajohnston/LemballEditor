using FluentAssertions;
using LemballEditor.Models;
using LemballEditor.Models.LevelObjects;
using LemballEditor.Serializers.Level.Objects;
using Moq;

namespace LemballEditor.Tests.SerializerTests.LevelTests.ObjectTests
{
    public class FakeObjectClassA : ILevelObject
    {
        public Position Position => new(0, 0);

        public ILevelObject WithPosition(Position position)
        {
            throw new NotImplementedException();
        }
    }

    public class FakeObjectClassB : ILevelObject
    {
        public Position Position => new(0, 0);

        public ILevelObject WithPosition(Position position)
        {
            throw new NotImplementedException();
        }
    }

    [TestClass]
    public class PendingObjectListTests
    {
        private Mock<ILevel>? mockLevel;
        private PendingObjectList? list;

        [TestInitialize]
        public void TestInitialize()
        {
            this.mockLevel = new Mock<ILevel>();
            this.list = new PendingObjectList(this.mockLevel.Object);
        }

        [TestMethod]
        public void Add_ShouldThrowArgumentNullException_WhenLevelObjectIsNull()
        {
            var act = () => this.list?.Add(null);

            _ = act.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Subscribe_ShouldNotifySubscribers_WhenObjectIsAdded()
        {
            var mockLevelObject = new Mock<ILevelObject>().Object;

            PendingObject? received = null;
            this.list!.Subscribe(po => received = po);

            this.list.Add(mockLevelObject, 123);

            _ = received.Should().NotBeNull();
            _ = received.LevelObject.Should().Be(mockLevelObject);
            _ = received.Id.Should().Be(123);
        }

        [TestMethod]
        public void GetLevelObjects_ShouldReturnAllLevelObjects()
        {
            var mockLevelObject1 = new Mock<ILevelObject>();
            var mockLevelObject2 = new Mock<ILevelObject>();

            this.list!.Add(mockLevelObject1.Object);
            this.list.Add(mockLevelObject2.Object);

            var levelObjects = this.list.GetLevelObjects();

            _ = levelObjects.Should().HaveCount(2);
            _ = levelObjects.Should().Contain(mockLevelObject1.Object);
            _ = levelObjects.Should().Contain(mockLevelObject2.Object);
        }

        [TestMethod]
        public void GetPendingObjects_ShouldReturnAllPendingObjects()
        {
            var mockLevelObject1 = new Mock<ILevelObject>().Object;
            var mockLevelObject2 = new Mock<ILevelObject>().Object;

            this.list!.Add(mockLevelObject1, 123);
            this.list.Add(mockLevelObject2, 456);

            var pendingObjects = this.list.GetPendingObjects();
            _ = pendingObjects.Should().HaveCount(2);

            _ = pendingObjects[0].LevelObject.Should().Be(mockLevelObject1);
            _ = pendingObjects[0].Id.Should().Be(123);

            _ = pendingObjects[1].LevelObject.Should().Be(mockLevelObject2);
            _ = pendingObjects[1].Id.Should().Be(456);
        }

        [TestMethod]
        public void AssignIds_ShouldAssignAnIncrementingIdToEachPendingObject()
        {
            var mockLevelObject1 = new Mock<ILevelObject>();
            var mockLevelObject2 = new Mock<ILevelObject>();

            this.list!.Add(mockLevelObject1.Object);
            this.list.Add(mockLevelObject2.Object);

            this.list.AssignIds();

            var pendingObjects = this.list.GetPendingObjects();
            _ = pendingObjects.Should().HaveCount(2);
            _ = pendingObjects[0].Id.Should().Be(0);
            _ = pendingObjects[1].Id.Should().Be(1);
        }

        [TestMethod]
        public void GetObjectsOfTypes_ShouldReturnOnlyObjectsOfTheSpecifiedType()
        {
            var matchingObjectA = new Mock<FakeObjectClassA>().Object;
            var matchingObjectB = new Mock<FakeObjectClassA>().Object;

            var otherObject = new Mock<ILevelObject>().Object;

            this.list!.Add(matchingObjectA, 1);
            this.list.Add(matchingObjectB, 2);
            this.list.Add(otherObject, 3);

            var result = this.list.GetObjectsOfTypes(typeof(FakeObjectClassA)).ToArray();

            _ = result.Length.Should().Be(2);

            _ = result[0].LevelObject.Should().Be(matchingObjectA);
            _ = result[0].Id.Should().Be(1);

            _ = result[1].LevelObject.Should().Be(matchingObjectB);
            _ = result[1].Id.Should().Be(2);
        }

        [TestMethod]
        public void GetObjectsOfTypes_ShouldReturnOnlyObjectsThatIsOneOfTheSpecifiedTypes()
        {
            var matchingObjectA = new Mock<FakeObjectClassA>().Object;
            var matchingObjectB = new Mock<FakeObjectClassB>().Object;

            var otherObject = new Mock<ILevelObject>().Object;

            this.list!.Add(matchingObjectA, 1);
            this.list.Add(matchingObjectB, 2);
            this.list.Add(otherObject, 3);

            var result = this.list.GetObjectsOfTypes(typeof(FakeObjectClassA), typeof(FakeObjectClassB)).ToArray();

            _ = result.Length.Should().Be(2);

            _ = result[0].LevelObject.Should().Be(matchingObjectA);
            _ = result[0].Id.Should().Be(1);

            _ = result[1].LevelObject.Should().Be(matchingObjectB);
            _ = result[1].Id.Should().Be(2);
        }
    }
}
