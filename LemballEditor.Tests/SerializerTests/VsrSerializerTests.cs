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

        private byte[] Serialize((Models.Vsr, Models.LevelPack?) models, VsrSerializer vsrSerializer)
        {
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            vsrSerializer.Serialize(models, writer);
            return stream.ToArray();
        }

        private byte[] CreateFakeAssetData(uint funPointerAddress = 744, uint funDirectoryAddress = 1000)
        {
            var demoFileAddress = funPointerAddress + 188;

            List<byte> data = [];
            data.AddRange(Encoding.ASCII.GetBytes("CRID"));             // Header
            data.AddRange(new byte[funPointerAddress - data.Count]);    // Padding
            data.AddRange(BitConverter.GetBytes(funDirectoryAddress));  // Fun directory pointer
            data.AddRange(new byte[demoFileAddress - data.Count]);      // Padding
            data.AddRange(Encoding.ASCII.GetBytes("Demo_00"));          // Demo_00 string
            data.AddRange(new byte[funDirectoryAddress - data.Count]);  // Rest of asset data

            return [.. data];
        }

        private byte[] CreateFakeLevelDirectoryData(byte fakeByteValue, uint size = 100)
        {
            return [
                ..Encoding.ASCII.GetBytes("CRID"),
                ..BitConverter.GetBytes(size + 4),
                ..Enumerable.Repeat(fakeByteValue, (int)size),
                ..Encoding.ASCII.GetBytes("?DNE")
            ];
        }

        private byte[] CreateFakeVsrData(uint funPointerAddress = 744, uint assetDataSize = 1000)
        {
            return [
                ..CreateFakeAssetData(funPointerAddress, assetDataSize),
                ..CreateFakeLevelDirectoryData(1),
                ..CreateFakeLevelDirectoryData(2),
                ..CreateFakeLevelDirectoryData(3),
                ..CreateFakeLevelDirectoryData(4),
                ..CreateFakeLevelDirectoryData(5)
            ];
        }

        [TestMethod]
        public void Deserialize_ShouldThrowAnException_WhenTheStreamDoesNotBeginWithCridHeader()
        {
            byte[] data = [1, 2, 3];
            using var stream = new MemoryStream(data);
            using var reader = new BinaryReader(stream);

            var serializer = ServiceFactory.CreateVsrSerializer();
            var act = () => serializer.Deserialize(reader, (ServiceFactory.CreateVsr(), null));

            _ = act.Should().Throw<InvalidDataException>().WithMessage("Invalid VSR");
        }

        [TestMethod]
        public void Deserialize_ShouldThrowAnException_WhenTheFunDirectoryCannotBeLocated()
        {
            List<byte> data = [.. Encoding.ASCII.GetBytes("CRID"), .. new byte[200]];

            using var stream = new MemoryStream([.. data]);
            using var reader = new BinaryReader(stream);

            var serializer = ServiceFactory.CreateVsrSerializer();
            var act = () => serializer.Deserialize(reader, (ServiceFactory.CreateVsr(), null));

            _ = act.Should().Throw<InvalidDataException>().WithMessage("Unable to locate FUN directory pointer in VSR data");
        }

        [TestMethod]
        public void Deserialize_ShouldSetTheAssetDataUpToTheFunDirectoryPointerToTheModel()
        {
            uint funDirectoryAddress = 1500;
            var data = CreateFakeVsrData(744, funDirectoryAddress);

            using var stream = new MemoryStream(data);
            using var reader = new BinaryReader(stream);

            var vsr = ServiceFactory.CreateVsr();
            var serializer = ServiceFactory.CreateVsrSerializer();
            _ = serializer.Deserialize(reader, (vsr, null));

            _ = vsr.AssetData.Should().BeEquivalentTo(data.Take((int)funDirectoryAddress));
        }

        [TestMethod]
        public void Deserialize_ShouldSetTheDirectoryDataForEachLevelGroup()
        {
            byte[] funDirectoryData = CreateFakeLevelDirectoryData(1, 100);
            byte[] trickyDirectoryData = CreateFakeLevelDirectoryData(2, 100);
            byte[] taxingDirectoryData = CreateFakeLevelDirectoryData(3, 100);
            byte[] mayhemDirectoryData = CreateFakeLevelDirectoryData(4, 100);
            byte[] networkDirectoryData = CreateFakeLevelDirectoryData(5, 100);

            byte[] assetData = [
                ..CreateFakeAssetData(),
                ..funDirectoryData,
                ..trickyDirectoryData,
                ..taxingDirectoryData,
                ..mayhemDirectoryData,
                ..networkDirectoryData
            ];

            using var stream = new MemoryStream(assetData);
            using var reader = new BinaryReader(stream);

            var vsr = ServiceFactory.CreateVsr();
            var serializer = ServiceFactory.CreateVsrSerializer();
            _ = serializer.Deserialize(reader, (vsr, null));

            vsr.GetLevelDirectoryData(LevelGroupName.Fun).Should().BeEquivalentTo(funDirectoryData);
            vsr.GetLevelDirectoryData(LevelGroupName.Tricky).Should().BeEquivalentTo(trickyDirectoryData);
            vsr.GetLevelDirectoryData(LevelGroupName.Taxing).Should().BeEquivalentTo(taxingDirectoryData);
            vsr.GetLevelDirectoryData(LevelGroupName.Mayhem).Should().BeEquivalentTo(mayhemDirectoryData);
            vsr.GetLevelDirectoryData(LevelGroupName.Network).Should().BeEquivalentTo(networkDirectoryData);
        }

        [TestMethod]
        [DataRow(LevelGroupName.Fun)]
        [DataRow(LevelGroupName.Tricky)]
        [DataRow(LevelGroupName.Taxing)]
        [DataRow(LevelGroupName.Mayhem)]
        [DataRow(LevelGroupName.Network)]
        public void Deserialize_ShouldThrowAnInvalidDataException_WhenADirectoryDoesNotBeginWithCRIDHeader(LevelGroupName invalidLevelGroup)
        {
            byte[] validData = CreateFakeLevelDirectoryData(1, 100);
            byte[] invalidData = Encoding.ASCII.GetBytes("Some invalid header");

            byte[] assetData = [
                ..CreateFakeAssetData(),
                ..invalidLevelGroup == LevelGroupName.Fun ? invalidData : validData,
                ..invalidLevelGroup == LevelGroupName.Tricky ? invalidData : validData,
                ..invalidLevelGroup == LevelGroupName.Taxing ? invalidData : validData,
                ..invalidLevelGroup == LevelGroupName.Mayhem ? invalidData : validData,
                ..invalidLevelGroup == LevelGroupName.Network ? invalidData : validData,
            ];

            using var stream = new MemoryStream(assetData);
            using var reader = new BinaryReader(stream);

            var vsr = ServiceFactory.CreateVsr();
            var serializer = ServiceFactory.CreateVsrSerializer();
            var act = () => serializer.Deserialize(reader, (vsr, null));

            var expectedMessage = "Invalid " + Enum.GetName(typeof(LevelGroupName), invalidLevelGroup) + " directory header";

            act.Should().Throw<InvalidDataException>()
                .WithMessage(expectedMessage);
        }

        [TestMethod]
        [DataRow(LevelGroupName.Fun)]
        [DataRow(LevelGroupName.Tricky)]
        [DataRow(LevelGroupName.Taxing)]
        [DataRow(LevelGroupName.Mayhem)]
        [DataRow(LevelGroupName.Network)]
        public void Deserialize_ShouldThrowAnInvalidDataException_WhenADirectoryDoesNotEndWithAValidFooter(LevelGroupName invalidLevelGroup)
        {
            byte[] validData = CreateFakeLevelDirectoryData(1, 100);

            byte[] invalidData = [
                ..Encoding.ASCII.GetBytes("CRID"),
                ..BitConverter.GetBytes(100),
                ..Enumerable.Repeat((byte)1, 100),
                ..Encoding.ASCII.GetBytes("????")
            ];

            byte[] assetData = [
                ..CreateFakeAssetData(),
                ..invalidLevelGroup == LevelGroupName.Fun ? invalidData : validData,
                ..invalidLevelGroup == LevelGroupName.Tricky ? invalidData : validData,
                ..invalidLevelGroup == LevelGroupName.Taxing ? invalidData : validData,
                ..invalidLevelGroup == LevelGroupName.Mayhem ? invalidData : validData,
                ..invalidLevelGroup == LevelGroupName.Network ? invalidData : validData,
            ];

            using var stream = new MemoryStream(assetData);
            using var reader = new BinaryReader(stream);

            var vsr = ServiceFactory.CreateVsr();
            var serializer = ServiceFactory.CreateVsrSerializer();
            var act = () => serializer.Deserialize(reader, (vsr, null));

            var expectedMessage = "Invalid " + Enum.GetName(typeof(LevelGroupName), invalidLevelGroup) + " directory footer";

            act.Should().Throw<InvalidDataException>()
                .WithMessage(expectedMessage);
        }

        [TestMethod]
        public void Deserialize_ShouldSetTheDirectoryPointersToTheModel()
        {
            uint funPointerAddress = 800;
            var data = CreateFakeVsrData(funPointerAddress, 1000);

            using var stream = new MemoryStream(data);
            using var reader = new BinaryReader(stream);

            var vsr = ServiceFactory.CreateVsr();
            var serializer = ServiceFactory.CreateVsrSerializer();
            _ = serializer.Deserialize(reader, (vsr, null));

            _ = vsr.GetLevelDirectoryPointer(LevelGroupName.Fun).Should().Be(funPointerAddress);
            _ = vsr.GetLevelDirectoryPointer(LevelGroupName.Tricky).Should().Be(funPointerAddress + 36);
            _ = vsr.GetLevelDirectoryPointer(LevelGroupName.Taxing).Should().Be(funPointerAddress + (36 * 2));
            _ = vsr.GetLevelDirectoryPointer(LevelGroupName.Mayhem).Should().Be(funPointerAddress + (36 * 3));
            _ = vsr.GetLevelDirectoryPointer(LevelGroupName.Network).Should().Be(funPointerAddress + (36 * 4));
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
            var data = Serialize((vsr, null), vsrSerializer);

            var dataBeforeFunPointer = data.Take(pointerStart);
            _ = dataBeforeFunPointer.Should().BeEquivalentTo(assetData.Take(pointerStart));

            var dataAfterPointers = data.Skip(pointerStart + 20);
            _ = dataAfterPointers.Should().BeEquivalentTo(assetData.Skip(pointerStart + 20));
        }
    }
}
