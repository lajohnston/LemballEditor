using FluentAssertions;
using LemballEditor.Models;
using LemballEditor.Models.LevelObjects;
using LemballEditor.Serializers;
using LemballEditor.Serializers.Level;
using LemballEditor.Serializers.Level.Objects;
using Moq;

[TestClass]
public class ObjectListSerializerTests
{
    private Mock<ILevel>? mockLevel;
    private Mock<Func<PendingObjectList>>? mockCreatePendingObjectList;
    private PendingObjectList? pendingObjectList;
    private Mock<ISerializer<PendingObjectList>>? mockPendingObjectListSerializer;

    private ObjectListSerializer? serializer;

    [TestInitialize]
    public void TestInitialize()
    {
        this.mockLevel = new Mock<ILevel>();
        this.pendingObjectList = new PendingObjectList();
        this.mockCreatePendingObjectList = new Mock<Func<PendingObjectList>>();
        this.mockPendingObjectListSerializer = new Mock<ISerializer<PendingObjectList>>();

        _ = this.mockCreatePendingObjectList
            .Setup(x => x())
            .Returns(this.pendingObjectList);

        _ = this.mockPendingObjectListSerializer
            .Setup(x => x.Deserialize(It.IsAny<BinaryReader>(), It.IsAny<PendingObjectList>()))
            .Returns(this.pendingObjectList);

        _ = this.mockLevel
            .Setup(x => x.GetObjects())
            .Returns(Array.Empty<ILevelObject>());

        this.serializer = new ObjectListSerializer(
            this.mockCreatePendingObjectList.Object,
            this.mockPendingObjectListSerializer.Object
        );
    }

    [TestMethod]
    public void Deserialize_ShouldReturnTheGivenLevel()
    {
        using var reader = new BinaryReader(new MemoryStream());
        var result = this.serializer!.Deserialize(reader, this.mockLevel!.Object);

        _ = result.Should().Be(this.mockLevel.Object);
    }

    [TestMethod]
    public void Deserialize_ShouldPassReaderToPendingObjectListSerializer()
    {
        using var reader = new BinaryReader(new MemoryStream());

        _ = this.serializer!.Deserialize(reader, this.mockLevel!.Object);

        this.mockPendingObjectListSerializer
            !.Verify(x => x.Deserialize(It.Is<BinaryReader>(r => r == reader), It.IsAny<PendingObjectList>()));
    }

    [TestMethod]
    public void Deserialize_ShouldCreateAnObjectListAndPassToTheSerializer()
    {
        using var reader = new BinaryReader(new MemoryStream());

        _ = this.serializer!.Deserialize(reader, this.mockLevel!.Object);

        this.mockPendingObjectListSerializer
            !.Verify(x => x.Deserialize(
                It.IsAny<BinaryReader>(),
                It.Is<PendingObjectList>(given => given == this.pendingObjectList))
            );
    }

    [TestMethod]
    public void Deserialize_ShouldAddEachObjectToTheLevel()
    {
        var objectA = new Mock<ILevelObject>().Object;
        var objectB = new Mock<ILevelObject>().Object;

        this.pendingObjectList!.Add(objectA);
        this.pendingObjectList!.Add(objectB);

        using var reader = new BinaryReader(new MemoryStream());

        _ = this.serializer!.Deserialize(reader, this.mockLevel!.Object);

        this.mockLevel!.Verify(x => x.AddObject(objectA), Times.Once);
        this.mockLevel!.Verify(x => x.AddObject(objectB), Times.Once);
    }

    [TestMethod]
    public void Serialize_ShouldCreateAPendingObjectListAndAddEachLevelObjectToIt()
    {
        using var writer = BinaryWriter.Null;

        var objectA = new Mock<ILevelObject>().Object;
        var objectB = new Mock<ILevelObject>().Object;

        _ = this.mockLevel!.Setup(x => x.GetObjects())
            .Returns([objectA, objectB]);

        this.serializer!.Serialize(this.mockLevel!.Object, writer);

        this.mockCreatePendingObjectList!
            .Verify(x => x(), Times.Once);

        _ = this.pendingObjectList!.GetLevelObjects().Should().BeEquivalentTo([objectA, objectB]);
    }

    [TestMethod]
    public void Serialize_ShouldPassThePendingObjectListToThePendingObjectListSerializer()
    {
        using var writer = BinaryWriter.Null;

        this.serializer!.Serialize(this.mockLevel!.Object, writer);

        this.mockPendingObjectListSerializer!
            .Verify(x => x.Serialize(
                It.Is<PendingObjectList>(given => given == this.pendingObjectList),
                It.IsAny<BinaryWriter>()
            ), Times.Once);
    }

    [TestMethod]
    public void Serialize_ShouldPassTheWriterToThePendingObjectListSerializer()
    {
        using var writer = BinaryWriter.Null;

        this.serializer!.Serialize(this.mockLevel!.Object, writer);

        this.mockPendingObjectListSerializer!
            .Verify(x => x.Serialize(
                It.IsAny<PendingObjectList>(),
                It.Is<BinaryWriter>(w => w == writer)
            ), Times.Once);
    }

    [TestMethod]
    public void Serialize_ShouldEnsureIdsAreAssignedToTheObjectsBeforePassingToThePendingObjectListSerializer()
    {
        using var writer = BinaryWriter.Null;

        this.pendingObjectList!.Add(new Mock<ILevelObject>().Object);
        this.pendingObjectList!.Add(new Mock<ILevelObject>().Object);

        this.serializer!.Serialize(this.mockLevel!.Object, writer);

        this.mockPendingObjectListSerializer!
            .Verify(x => x.Serialize(
                It.Is<PendingObjectList>(givenList => givenList.GetPendingObjects().All(pendingObject => pendingObject.Id != null)),
                It.IsAny<BinaryWriter>()
            ), Times.Once);
    }
}
