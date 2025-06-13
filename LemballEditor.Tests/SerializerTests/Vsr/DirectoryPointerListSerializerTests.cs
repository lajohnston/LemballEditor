using System.Text;
using FluentAssertions;
using LemballEditor.Models;
using LemballEditor.Serializers.Vsr;

namespace LemballEditor.Tests.SerializerTests.Vsr
{
    [TestClass]
    public class DirectoryPointerListSerializerTests
    {
        private byte[] CreateValidData(int funPointerAddress = 744)
        {
            List<byte> vsrData = [];
            vsrData.AddRange(Encoding.ASCII.GetBytes("CRID"));          // Header
            vsrData.AddRange(new byte[funPointerAddress - 4 + 188]);    // Padding to Demo_00 string
            vsrData.AddRange(Encoding.ASCII.GetBytes("Demo_00"));       // Demo_00 string

            return vsrData.ToArray();
        }

        [TestMethod]
        public void Deserialize_ShouldThrowAnException_WhenTheStreamDoesNotBeginWithCridHeader()
        {
            var vsr = ServiceFactory.CreateVsr();
            byte[] data = [1, 2, 3];

            using var reader = new BinaryReader(new MemoryStream(data));
            var serializer = new DirectoryPointerListSerializer();

           var act = () => serializer.Deserialize(reader, (vsr, null));
            _ = act.Should().Throw<InvalidDataException>().WithMessage("Invalid VSR asset data");
        }

        [TestMethod]
        public void Deserialize_ShouldThrowAnException_WhenTheFunDirectoryCannotBeLocated()
        {
            var vsr = ServiceFactory.CreateVsr();
            List<byte> data = [.. Encoding.ASCII.GetBytes("CRID"), .. new byte[200]];

            using var reader = new BinaryReader(new MemoryStream([.. data]));
            var serializer = new DirectoryPointerListSerializer();

            var act = () => serializer.Deserialize(reader, (vsr, null));
            _ = act.Should().Throw<InvalidDataException>().WithMessage("Unable to locate FUN directory pointer in VSR data");
        }

        [TestMethod]
        [DataRow(740)]
        [DataRow(744)]
        public void Deserialize_ShouldExtractTheDirectoryPointerAddressesAndSetToTheModel(int funPointerAddress)
        {
            var vsrData = CreateValidData(funPointerAddress);

            using var reader = new BinaryReader(new MemoryStream(vsrData));

            var vsr = ServiceFactory.CreateVsr();
            var serializer = new DirectoryPointerListSerializer();
            _ = serializer.Deserialize(reader, (vsr, null));

            _ = vsr.GetLevelDirectoryPointer(LevelGroupName.Fun).Should().Be((uint)funPointerAddress);
            _ = vsr.GetLevelDirectoryPointer(LevelGroupName.Tricky).Should().Be((uint)funPointerAddress + 36);
            _ = vsr.GetLevelDirectoryPointer(LevelGroupName.Taxing).Should().Be((uint)funPointerAddress + (36 * 2));
            _ = vsr.GetLevelDirectoryPointer(LevelGroupName.Mayhem).Should().Be((uint)funPointerAddress + (36 * 3));
            _ = vsr.GetLevelDirectoryPointer(LevelGroupName.Network).Should().Be((uint)funPointerAddress + (36 * 4));
        }

        [TestMethod]
        public void Deserialize_ShouldSetTheReadAddressBackToZero()
        {
            var vsrData = CreateValidData();

            using var stream = new MemoryStream(vsrData.ToArray());
            using var reader = new BinaryReader(stream);

            var vsr = ServiceFactory.CreateVsr();
            var serializer = new DirectoryPointerListSerializer();

            _ = serializer.Deserialize(reader, (vsr, null));
            stream.Position.Should().Be(0);
        }

        [TestMethod]
        public void Serialize_ShouldNotActuallyWriteAnything()
        {
            var serializer = new DirectoryPointerListSerializer();

            var originalData = CreateValidData();

            using var stream = new MemoryStream(originalData);
            using var writer = new BinaryWriter(stream);

            var vsr = ServiceFactory.CreateVsr();
            serializer.Serialize((vsr, null), writer);

            stream.ToArray().Should().BeEquivalentTo(originalData);
        }
    }
}
