using LemballEditor.Serializers;
using Moq;

namespace LemballEditor.Tests.SerializerTests
{
    [TestClass]
    public class SequenceTests
    {
        [TestMethod]
        public void Deserialize_ShouldRunEachDeserializerInSequence()
        {
            var first = new Mock<ISerializer<int>>(MockBehavior.Strict);
            var second = new Mock<ISerializer<int>>(MockBehavior.Strict);
            var third = new Mock<ISerializer<int>>(MockBehavior.Strict);

            var mockSequence = new MockSequence();

            first.InSequence(mockSequence).Setup(m => m.Deserialize(It.IsAny<BinaryReader>(), It.IsAny<int>())).Returns(0);
            second.InSequence(mockSequence).Setup(m => m.Deserialize(It.IsAny<BinaryReader>(), It.IsAny<int>())).Returns(1);
            third.InSequence(mockSequence).Setup(m => m.Deserialize(It.IsAny<BinaryReader>(), It.IsAny<int>())).Returns(2);

            ISerializer<int>[] mockSerializers = [
                first.Object,
                second.Object,
                third.Object,
            ];

            var sequence = new Sequence<int>(mockSerializers);

            using var reader = new BinaryReader(new MemoryStream());
            sequence.Deserialize(reader, 0);

            first.Verify(m => m.Deserialize(It.IsAny<BinaryReader>(), It.IsAny<int>()), Times.Once);
            second.Verify(m => m.Deserialize(It.IsAny<BinaryReader>(), It.IsAny<int>()), Times.Once);
            third.Verify(m => m.Deserialize(It.IsAny<BinaryReader>(), It.IsAny<int>()), Times.Once);
        }

        [TestMethod]
        public void Deserialize_PassTheBinaryReaderToEachDeserializer()
        {
            var first = new Mock<ISerializer<int>>();
            var second = new Mock<ISerializer<int>>();

            using var reader = new BinaryReader(new MemoryStream());

            first.Setup(m => m.Deserialize(It.Is<BinaryReader>(r => r == reader), It.IsAny<int>())).Returns(0).Verifiable();
            second.Setup(m => m.Deserialize(It.Is<BinaryReader>(r => r == reader), It.IsAny<int>())).Returns(0).Verifiable();

            ISerializer<int>[] mockSerializers = [
                first.Object,
                second.Object,
            ];

            var sequence = new Sequence<int>(mockSerializers);
            sequence.Deserialize(reader, 0);

            first.Verify();
            second.Verify();
        }

        [TestMethod]
        public void Deserialize_PassTheModelToEachDeserializerInTurnAndReturnTheFinalResult()
        {
            var first = new Mock<ISerializer<int>>();
            var second = new Mock<ISerializer<int>>();
            var third = new Mock<ISerializer<int>>();

            using var reader = new BinaryReader(new MemoryStream());

            var initialModel = 0;

            first.Setup(m => m.Deserialize(It.IsAny<BinaryReader>(), It.Is<int>(m => m == initialModel))).Returns(1).Verifiable();
            second.Setup(m => m.Deserialize(It.IsAny<BinaryReader>(), It.Is<int>(m => m == 1))).Returns(2).Verifiable();
            third.Setup(m => m.Deserialize(It.IsAny<BinaryReader>(), It.Is<int>(m => m == 2))).Returns(3).Verifiable();

            ISerializer<int>[] mockSerializers = [
                first.Object,
                second.Object,
                third.Object
            ];

            var sequence = new Sequence<int>(mockSerializers);
            sequence.Deserialize(reader, initialModel);

            first.Verify();
            second.Verify();
            third.Verify();
        }

        [TestMethod]
        public void Serialize_ShouldPassTheModelAndBinaryWriterToEachSerializer()
        {
            var first = new Mock<ISerializer<int>>();
            var second = new Mock<ISerializer<int>>();
            var third = new Mock<ISerializer<int>>();

            using var writer = BinaryWriter.Null;

            var model = 100;

            first.Setup(m => m.Serialize(It.Is<int>(m => m == model), It.Is<BinaryWriter>(w => w == writer))).Verifiable();
            second.Setup(m => m.Serialize(It.Is<int>(m => m == model), It.Is<BinaryWriter>(w => w == writer))).Verifiable();
            third.Setup(m => m.Serialize(It.Is<int>(m => m == model), It.Is<BinaryWriter>(w => w == writer))).Verifiable();

            ISerializer<int>[] mockSerializers = [
                first.Object,
                second.Object,
                third.Object
            ];

            var sequence = new Sequence<int>(mockSerializers);
            sequence.Serialize(model, writer);

            first.Verify();
            second.Verify();
            third.Verify();
        }
    }
}
