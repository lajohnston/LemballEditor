using FluentAssertions;
using System.Text;

namespace LemballEditor.Tests.SerializerTests
{
    [TestClass]
    public class VsrSerializerTests
    {
        private byte[] GetValidData(uint funPointerAddress, uint funDirectoryAddress)
        {
            var demoFileAddress = funPointerAddress + 188;

            List<byte> data = [];
            data.AddRange(Encoding.ASCII.GetBytes("CRID"));             // header
            data.AddRange(new byte[funPointerAddress - data.Count]);    // padding
            data.AddRange(BitConverter.GetBytes(funDirectoryAddress));  // Fun directory pointer
            data.AddRange(new byte[demoFileAddress - data.Count]);      // padding
            data.AddRange(Encoding.ASCII.GetBytes("Demo_00"));          // Demo_00 string
            data.AddRange(new byte[funDirectoryAddress]);               // Padding

            return data.ToArray();
        }

        [TestMethod]
        public void Deserialize_ShouldThrowAnException_WhenTheStreamDoesNotBeginWithCridHeader()
        {
            byte[] data = [1, 2, 3];
            using var stream = new MemoryStream(data);
            using var reader = new BinaryReader(stream);

            var serializer = ServiceFactory.CreateVsrSerializer();
            var act = () => serializer.Deserialize(reader, ServiceFactory.CreateVsr());

            _ = act.Should().Throw<InvalidDataException>().WithMessage("Invalid VSR");
        }

        [TestMethod]
        public void Deserialize_ShouldThrowAnException_WhenTheFunDirectoryCannotBeLocated()
        {
            List<byte> data = [.. Encoding.ASCII.GetBytes("CRID"), .. new byte[200]];

            using var stream = new MemoryStream(data.ToArray());
            using var reader = new BinaryReader(stream);

            var serializer = ServiceFactory.CreateVsrSerializer();
            var act = () => serializer.Deserialize(reader, ServiceFactory.CreateVsr());

            _ = act.Should().Throw<InvalidDataException>().WithMessage("Unable to locate FUN directory pointer in VSR data");
        }

        [TestMethod]
        public void Deserialize_ShouldSetTheAssetDataUpToTheFunDirectoryPointerToTheModel()
        {
            uint funDirectoryAddress = 1500;
            var data = GetValidData(744, funDirectoryAddress);

            using var stream = new MemoryStream(data);
            using var reader = new BinaryReader(stream);

            var vsr = ServiceFactory.CreateVsr();
            var serializer = ServiceFactory.CreateVsrSerializer();
            _ = serializer.Deserialize(reader, vsr);

            _ = vsr.AssetData.Should().NotBeNull();
            _ = vsr.AssetData.Length.Should().Be((int)funDirectoryAddress);
        }

        [TestMethod]
        public void Deserialize_ShouldSetTheDirectoryPointersToTheModel()
        {
            uint funPointerAddress = 800;
            var data = GetValidData(funPointerAddress, 1000);

            using var stream = new MemoryStream(data);
            using var reader = new BinaryReader(stream);

            var vsr = ServiceFactory.CreateVsr();
            var serializer = ServiceFactory.CreateVsrSerializer();
            _ = serializer.Deserialize(reader, vsr);

            _ = vsr.FunDirectoryPointer.Should().Be(funPointerAddress);
            _ = vsr.TrickyDirectoryPointer.Should().Be(funPointerAddress + 36);
            _ = vsr.TaxingDirectoryPointer.Should().Be(funPointerAddress + (36 * 2));
            _ = vsr.MayhemDirectoryPointer.Should().Be(funPointerAddress + (36 * 3));
            _ = vsr.NetworkDirectoryPointer.Should().Be(funPointerAddress + (36 * 4));
        }
    }
}
