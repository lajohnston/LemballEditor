using FluentAssertions;
using LemballEditor.Models;
using LemballEditor.Serializers;
using LemballEditor.Serializers.Level;
using LemballEditor.Serializers.LevelDirectory;
using Moq;
using System.Text;


namespace LemballEditor.Tests.SerializerTests
{
    [TestClass]
    public class VsrSerializerTests
    {
        private (VsrSerializer, Mock<ISerializer<LevelDirectory>>) CreateVsrSerializer()
        {
            var levelGroupSerializerMock = new Mock<ISerializer<LevelDirectory>>();
            var serializer = new VsrSerializer(levelGroupSerializerMock.Object);
            return (serializer, levelGroupSerializerMock);
        }

        private byte[] Serialize(Vsr vsr, VsrSerializer vsrSerializer)
        {
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            vsrSerializer.Serialize(vsr, writer);
            return stream.ToArray();
        }
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

            return [.. data];
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

            using var stream = new MemoryStream([.. data]);
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

            _ = vsr.GetLevelDirectoryPointer(LevelGroupName.Fun).Should().Be(funPointerAddress);
            _ = vsr.GetLevelDirectoryPointer(LevelGroupName.Tricky).Should().Be(funPointerAddress + 36);
            _ = vsr.GetLevelDirectoryPointer(LevelGroupName.Taxing).Should().Be(funPointerAddress + (36 * 2));
            _ = vsr.GetLevelDirectoryPointer(LevelGroupName.Mayhem).Should().Be(funPointerAddress + (36 * 3));
            _ = vsr.GetLevelDirectoryPointer(LevelGroupName.Network).Should().Be(funPointerAddress + (36 * 4));
        }

        [TestMethod]
        public void Deserialize_ShouldNotCallTheLevelGroupDeserializer_WhenLevelPackIsNotSet()
        {
            var data = GetValidData(744, 1000);

            using var stream = new MemoryStream(data);
            using var reader = new BinaryReader(stream);

            var vsr = ServiceFactory.CreateVsr();

            var (serializer, levelGroupSerializerMock) = CreateVsrSerializer();
            _ = serializer.Deserialize(reader, vsr);

            levelGroupSerializerMock.Verify(
                m => m.Deserialize(It.IsAny<BinaryReader>(), It.IsAny<LevelDirectory>()),
                Times.Never);
        }

        [TestMethod]
        public void Serialize_ShouldWriteTheAssetDataToTheStream()
        {
            var assetData = new byte[100];

            for (var i = 0; i < assetData.Length; i++)
            {
                assetData[i] = (byte)(i % 256);
            }

            var vsr = new Vsr
            {
                AssetData = [.. assetData],
            };

            var pointerStart = 8;

            vsr.SetLevelDirectoryPointer(LevelGroupName.Fun, (uint)pointerStart);
            vsr.SetLevelDirectoryPointer(LevelGroupName.Tricky, 12);
            vsr.SetLevelDirectoryPointer(LevelGroupName.Taxing, 16);
            vsr.SetLevelDirectoryPointer(LevelGroupName.Mayhem, 20);
            vsr.SetLevelDirectoryPointer(LevelGroupName.Network, 24);

            var (vsrSerializer, _) = CreateVsrSerializer();
            var data = Serialize(vsr, vsrSerializer);

            var dataBeforeFunPointer = data.Take(pointerStart);
            _ = dataBeforeFunPointer.Should().BeEquivalentTo(assetData.Take(pointerStart));

            var dataAfterPointers = data.Skip(pointerStart + 20);
            _ = dataAfterPointers.Should().BeEquivalentTo(assetData.Skip(pointerStart + 20));
        }
    }
}
