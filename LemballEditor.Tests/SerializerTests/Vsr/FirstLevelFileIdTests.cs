using FluentAssertions;
using LemballEditor.Serializers.Vsr;

namespace LemballEditor.Tests.SerializerTests.Vsr
{
    [TestClass]
    public class FirstLevelFileIdTests
    {
        [TestMethod]
        public void Deserialize_ShouldReturnTheModels()
        {
            var vsr = ServiceFactory.CreateVsr();
            vsr.AssetData = new byte[100];

            using var reader = new BinaryReader(new MemoryStream(new byte[200]));

            var serializer = new FirstLevelFileId();

            var result = serializer.Deserialize(reader, (vsr, null));
            _ = result.Item1.Should().Be(vsr);
            _ = result.Item2.Should().BeNull();
        }

        [TestMethod]
        [DataRow(25, 580)]
        [DataRow(2, 576)]
        public void Deserialize_ShouldSetTheFirstLevelFileIdToTheModelAndRestoreTheReaderPosition(int numberOfLevels, int firstFileId)
        {
            var assetData = new byte[100];
            var vsr = ServiceFactory.CreateVsr();
            vsr.AssetData = assetData;

            var directoryData = new List<byte>();
            directoryData.AddRange(assetData);
            directoryData.AddRange(new byte[8]); // header
            directoryData.AddRange(BitConverter.GetBytes(numberOfLevels));
            directoryData.AddRange(new byte[8 + (numberOfLevels * 12) + 4]); // other data
            directoryData.AddRange(BitConverter.GetBytes(firstFileId));

            using var reader = new BinaryReader(new MemoryStream(directoryData.ToArray()));

            var serializer = new FirstLevelFileId();
            _ = serializer.Deserialize(reader, (vsr, null));

            _ = vsr.FirstLevelFileId.Should().Be((uint)firstFileId);
        }

        [TestMethod]
        public void Deserialize_ShouldSetTheReaderPositionBackToZero()
        {
            var vsr = ServiceFactory.CreateVsr();
            vsr.AssetData = new byte[100];

            using var reader = new BinaryReader(new MemoryStream(new byte[200]));

            var serializer = new FirstLevelFileId();

            _ = serializer.Deserialize(reader, (vsr, null));
            _ = reader.BaseStream.Position.Should().Be(0);
        }

        [TestMethod]
        public void Serialize_ShouldNotActuallyWriteAnything()
        {
            var vsr = ServiceFactory.CreateVsr();
            var serializer = new FirstLevelFileId();

            var originalData = Enumerable.Repeat((byte)1, 100).ToArray();

            using var stream = new MemoryStream(originalData);
            using var writer = new BinaryWriter(stream);

            serializer.Serialize((vsr, null), writer);

            _ = stream.ToArray().Should().BeEquivalentTo(originalData);
        }
    }
}
