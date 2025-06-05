using FluentAssertions;
using LemballEditor.Serializers.Vsr;

namespace LemballEditor.Tests.SerializerTests.Vsr
{
    [TestClass]
    public class AssetBinaryTests
    {
        [TestMethod]
        [DataRow(744, 1000)]
        [DataRow(740, 1200)]
        public void Deserialize_ShouldReadTheAssetDataUpToTheFunDirectoryToTheModel(int funPointer, int funDirectoryAddress)
        {
            var vsr = ServiceFactory.CreateVsr();
            vsr.SetLevelDirectoryPointer(Models.LevelGroupName.Fun, (uint)funPointer);

            var vsrData = new List<byte>();
            vsrData.AddRange(new byte[funPointer]);                             // Padding to the FUN directory pointer
            vsrData.AddRange(BitConverter.GetBytes((uint)funDirectoryAddress)); // FUN directory address
            vsrData.AddRange(new byte[funDirectoryAddress - vsrData.Count]);    // Rest of asset data up to the FUN directory

            using var reader = new BinaryReader(new MemoryStream(vsrData.ToArray()));

            var serializer = new AssetBinary();
            _ = serializer.Deserialize(reader, (vsr, null));

            _ = vsr.AssetData.Should().BeEquivalentTo(vsrData.Take(funDirectoryAddress));
            _ = reader.BaseStream.Position.Should().Be(funDirectoryAddress);
        }

        [TestMethod]
        public void Serialize_ShouldWriteTheAssetDataToTheStream()
        {
            var serializer = new AssetBinary();

            var vsr = ServiceFactory.CreateVsr();
            vsr.AssetData = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            serializer.Serialize((vsr, null), writer);

            _ = stream.ToArray().Should().BeEquivalentTo(vsr.AssetData);
        }
    }
}
