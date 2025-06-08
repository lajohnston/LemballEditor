using System.Runtime.Remoting.Messaging;
using System.Text;
using FluentAssertions;
using LemballEditor.Serializers;
using Moq;

namespace LemballEditor.Tests.SerializerTests
{
    [TestClass]
    public class DataBlockTests
    {
        public (DataBlock<int>, Mock<ISerializer<int>>) CreateSerializer(string header = "TEST")
        {
            var bodySerializer = new Mock<ISerializer<int>>();
            var serializer = new DataBlock<int>(header, bodySerializer.Object);
            return (serializer, bodySerializer);
        }

        [TestMethod]
        [DataRow(0)]
        [DataRow(10)]
        [DataRow(20)]
        public void Deserialize_ShouldThrowIfTheExpectedHeaderDoesNotMatch(int address)
        {
            var data = new List<byte>();

            if (address > 0)
            {
                data.AddRange(Enumerable.Repeat((byte)0, address));
            }

            data.AddRange(Encoding.ASCII.GetBytes("XXXX"));

            using var stream = new MemoryStream(data.ToArray());
            using var reader = new BinaryReader(stream);

            stream.Position = address;

            var (serializer, _) = this.CreateSerializer("TEST");
            var act = () => serializer.Deserialize(reader, 0);
            _ = act.Should().Throw<InvalidDataException>()
                .WithMessage($"Expected TEST header at address {address}");
        }

        [TestMethod]
        [DataRow("TEST", 8)]
        [DataRow("TESTTEST", 12)]
        public void Deserializer_ShouldPassTheReaderToTheBodyDeserializerAfterTheHeaderAndSizeBytes(string header, int expectedPosition)
        {
            var data = new List<byte>();
            data.AddRange(Encoding.ASCII.GetBytes(header));
            data.AddRange(new byte[4]); // size

            using var stream = new MemoryStream(data.ToArray());
            using var reader = new BinaryReader(stream);

            var (serializer, mockBodySerializer) = this.CreateSerializer(header);
            _ = serializer.Deserialize(reader, 0);

            mockBodySerializer.Verify(m => m.Deserialize(
                It.Is<BinaryReader>(r => r == reader),
                It.IsAny<int>()
            ), Times.Once);

            mockBodySerializer.Verify(m => m.Deserialize(
                It.Is<BinaryReader>(r => reader.BaseStream.Position == expectedPosition),
                It.IsAny<int>()
            ), Times.Once);
        }

        [TestMethod]
        public void Deserializer_ShouldPassTheModelToTheBodyDeserializer()
        {
            var data = new List<byte>();
            data.AddRange(Encoding.ASCII.GetBytes("TEST"));
            data.AddRange(new byte[4]); // size

            using var stream = new MemoryStream(data.ToArray());
            using var reader = new BinaryReader(stream);

            var (serializer, mockBodySerializer) = this.CreateSerializer("TEST");
            _ = serializer.Deserialize(reader, 42);

            mockBodySerializer.Verify(m => m.Deserialize(
                It.IsAny<BinaryReader>(),
                It.Is<int>(model => model == 42)
            ), Times.Once);
        }

        [TestMethod]
        public void Deserializer_ShouldReturnTheModelReturnedFromTheBodyDeserializer()
        {
            var data = new List<byte>();
            data.AddRange(Encoding.ASCII.GetBytes("TEST"));
            data.AddRange(new byte[4]); // size

            using var stream = new MemoryStream(data.ToArray());
            using var reader = new BinaryReader(stream);

            var (serializer, mockBodySerializer) = this.CreateSerializer("TEST");

            _ = mockBodySerializer.Setup(m => m.Deserialize(
                It.IsAny<BinaryReader>(),
                It.IsAny<int>()
            )).Returns((BinaryReader reader, int model) => 100);

            var result = serializer.Deserialize(reader, 0);
            _ = result.Should().Be(100);
        }

        [TestMethod]
        [DataRow(0, "1234", 2, 12)]
        [DataRow(0, "12345", 2, 12)]
        [DataRow(0, "123456", 2, 12)]
        [DataRow(0, "1234567", 2, 16)]

        [DataRow(0, "1234", 2, 12)]
        [DataRow(0, "1234", 3, 12)]
        [DataRow(0, "1234", 4, 12)]
        [DataRow(0, "1234", 5, 16)]

        [DataRow(1, "1234", 2, 12)]
        [DataRow(2, "1234", 2, 12)]
        [DataRow(3, "1234", 2, 16)]
        public void Deserialize_ShouldSkipThePaddingBytesAtTheEnd(int startPosition, string header, int bodySize, int expectedEndPosition)
        {
            var data = new List<byte>();
            data.AddRange(new byte[startPosition]); // padding to start position
            data.AddRange(Encoding.ASCII.GetBytes(header));
            data.AddRange(BitConverter.GetBytes(4)); // size
            data.AddRange(new byte[50]); // dummy data for body

            using var stream = new MemoryStream(data.ToArray());
            using var reader = new BinaryReader(stream);

            reader.BaseStream.Position = startPosition;

            var (serializer, mockBodySerializer) = this.CreateSerializer(header);

            _ = mockBodySerializer.Setup(m => m.Deserialize(
                It.IsAny<BinaryReader>(),
                It.IsAny<int>()
            )).Callback((BinaryReader r, int model) =>
            {
                _ = r.ReadBytes(bodySize);
            });

            var result = serializer.Deserialize(reader, 0);
            _ = reader.BaseStream.Position.Should().Be(expectedEndPosition);
        }

        [TestMethod]
        [DataRow("AAAA")]
        [DataRow("AAAAB")]
        public void Serialize_ShouldWriteTheHeader(string header)
        {
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            var (serializer, _) = this.CreateSerializer(header);

            serializer.Serialize(0, writer);

            using var reader = new BinaryReader(stream);
            stream.Position = 0;

            var result = Encoding.ASCII.GetString(reader.ReadBytes(header.Length));
            result.Should().Be(header);
        }

        [TestMethod]
        public void Serialize_ShouldPassTheWriterToTheBodySerializer()
        {
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);
         
            var (serializer, mockBodySerializer) = this.CreateSerializer("TEST");
            
            serializer.Serialize(0, writer);
            
            mockBodySerializer.Verify(m => m.Serialize(
                It.IsAny<int>(),
                It.Is<BinaryWriter>(w => w == writer)
            ), Times.Once);

            mockBodySerializer.Verify(m => m.Serialize(
                It.IsAny<int>(),
                It.Is<BinaryWriter>(w => w == writer && writer.BaseStream.Position == 8)
            ), Times.Once);
        }

        [TestMethod]
        public void Serialize_ShouldPassTheModelToTheBodySerializer()
        {
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            var (serializer, mockBodySerializer) = this.CreateSerializer();

            var model = 100;
            serializer.Serialize(model, writer);

            mockBodySerializer.Verify(m => m.Serialize(
                It.Is<int>(m => m == model),
                It.IsAny<BinaryWriter>()
            ), Times.Once);
        }

        [TestMethod]
        [DataRow("1234", 4, 12)]
        [DataRow("12345", 4, 13)]

        [DataRow("1234", 5, 13)]
        [DataRow("1234", 6, 14)]
        public void Serialize_ShouldWriteTheDataSize(string header, int bodySize, int expectedTotalSize)
        {
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            var (serializer, mockBodySerializer) = this.CreateSerializer(header);

            mockBodySerializer.Setup(m => m.Serialize(
                It.IsAny<int>(),
                It.IsAny<BinaryWriter>()
            )).Callback((int model, BinaryWriter w) =>
            {
                w.Write(new byte[bodySize]); // simulate body data
            });

            serializer.Serialize(0, writer);

            using var reader = new BinaryReader(stream);
            stream.Position = header.Length;

            var result = reader.ReadInt32();
            result.Should().Be(expectedTotalSize);
        }

        [TestMethod]
        [DataRow(0, "1234", 2, 12)]
        [DataRow(0, "12345", 2, 12)]
        [DataRow(0, "123456", 2, 12)]
        [DataRow(0, "1234567", 2, 16)]

        [DataRow(0, "1234", 2, 12)]
        [DataRow(0, "1234", 3, 12)]
        [DataRow(0, "1234", 4, 12)]
        [DataRow(0, "1234", 5, 16)]

        [DataRow(1, "1234", 2, 12)]
        [DataRow(2, "1234", 2, 12)]
        [DataRow(3, "1234", 2, 16)]
        public void Serialize_ShouldWriteThePaddingByteToBringTheAddressToTheNextMultipleOfFour(int startPosition, string header, int bodySize, int expectedEndPosition)
        {
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            stream.Position = startPosition;

            var (serializer, mockBodySerializer) = this.CreateSerializer(header);

            mockBodySerializer.Setup(m => m.Serialize(
                It.IsAny<int>(),
                It.IsAny<BinaryWriter>()
            )).Callback((int model, BinaryWriter w) =>
            {
                w.Write(new byte[bodySize]); // simulate body data
            });

            serializer.Serialize(0, writer);

            stream.Position.Should().Be(expectedEndPosition);
        }
    }
}
