using System.Text;
using FluentAssertions;
using LemballEditor.Models;
using LemballEditor.Serializers;
using LemballEditor.Serializers.Level;
using LemballEditor.Serializers.LevelDirectory;
using Moq;

namespace LemballEditor.Tests.SerializerTests
{
    [TestClass]
    public class VsrSerializerTests
    {
        private (VsrSerializer, Mock<ISerializer<LevelDirectory>>, Mock<Func<LevelDirectory>>) CreateVsrSerializer()
        {
            Mock<ISerializer<LevelDirectory>> levelDirectorySerializerMock = new();
            Mock<Func<LevelDirectory>> levelDirectoryFactoryMock = new();

            VsrSerializer serializer = new(levelDirectorySerializerMock.Object, levelDirectoryFactoryMock.Object);
            return (serializer, levelDirectorySerializerMock, levelDirectoryFactoryMock);
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
                ..this.CreateFakeAssetData(funPointerAddress, assetDataSize),
                ..this.CreateFakeLevelDirectoryData(1),
                ..this.CreateFakeLevelDirectoryData(2),
                ..this.CreateFakeLevelDirectoryData(3),
                ..this.CreateFakeLevelDirectoryData(4),
                ..this.CreateFakeLevelDirectoryData(5)
            ];
        }

        [TestMethod]
        public void Deserialize_ShouldThrowAnException_WhenTheStreamDoesNotBeginWithCridHeader()
        {
            byte[] data = [1, 2, 3];
            using var stream = new MemoryStream(data);
            using var reader = new BinaryReader(stream);

            var serializer = ServiceFactory.CreateVsrSerializer();
            Func<(Vsr, LevelPack)> act = () => serializer.Deserialize(reader, (ServiceFactory.CreateVsr(), null));

            _ = act.Should().Throw<InvalidDataException>().WithMessage("Invalid VSR");
        }

        [TestMethod]
        public void Deserialize_ShouldThrowAnException_WhenTheFunDirectoryCannotBeLocated()
        {
            List<byte> data = [.. Encoding.ASCII.GetBytes("CRID"), .. new byte[200]];

            using var stream = new MemoryStream([.. data]);
            using var reader = new BinaryReader(stream);

            var serializer = ServiceFactory.CreateVsrSerializer();
            Func<(Vsr, LevelPack)> act = () => serializer.Deserialize(reader, (ServiceFactory.CreateVsr(), null));

            _ = act.Should().Throw<InvalidDataException>().WithMessage("Unable to locate FUN directory pointer in VSR data");
        }

        [TestMethod]
        public void Deserialize_ShouldSetTheAssetDataUpToTheFunDirectoryPointerToTheModel()
        {
            uint funDirectoryAddress = 1500;
            var data = this.CreateFakeVsrData(744, funDirectoryAddress);

            using MemoryStream stream = new(data);
            using BinaryReader reader = new(stream);

            var vsr = ServiceFactory.CreateVsr();
            var serializer = ServiceFactory.CreateVsrSerializer();
            _ = serializer.Deserialize(reader, (vsr, null));

            _ = vsr.AssetData.Should().BeEquivalentTo(data.Take((int)funDirectoryAddress));
        }

        [TestMethod]
        public void Deserialize_ShouldNotCallTheLevelDirectorySerializer_WhenLevelPackIsNull()
        {
            uint funDirectoryAddress = 1000;
            var assetData = this.CreateFakeAssetData(744, funDirectoryAddress);
            byte[] vsrData = [
                ..assetData,
                ..this.CreateFakeLevelDirectoryData(1),
                ..this.CreateFakeLevelDirectoryData(2),
                ..this.CreateFakeLevelDirectoryData(3),
                ..this.CreateFakeLevelDirectoryData(4),
                ..this.CreateFakeLevelDirectoryData(5)
            ];

            using MemoryStream stream = new(vsrData);
            using BinaryReader reader = new(stream);

            var (vsrSerialize, mockLevelDirectorySerializer, mockLevelDirectoryFactory) = this.CreateVsrSerializer();

            _ = vsrSerialize.Deserialize(reader, (ServiceFactory.CreateVsr(), null));
            mockLevelDirectorySerializer.Verify(m => m.Deserialize(It.IsAny<BinaryReader>(), It.IsAny<LevelDirectory>()), Times.Never);
            mockLevelDirectoryFactory.Verify(m => m(), Times.Never);
        }

        [TestMethod]
        public void Deserialize_ShouldPassEachDirectoryToTheLevelDirectorySerializer_WhenALevelPackModelIsGiven()
        {
            uint funDirectoryAddress = 1000;
            uint directorySize = 100;
            var assetData = this.CreateFakeAssetData(744, funDirectoryAddress);

            byte[] vsrData = [
                ..assetData,
                ..this.CreateFakeLevelDirectoryData(1, directorySize),
                ..this.CreateFakeLevelDirectoryData(2, directorySize),
                ..this.CreateFakeLevelDirectoryData(3, directorySize),
                ..this.CreateFakeLevelDirectoryData(4, directorySize),
                ..this.CreateFakeLevelDirectoryData(5, directorySize),
            ];

            Queue<uint> expectedReaderAddresses = new(new uint[] { funDirectoryAddress, 1100, 1200, 1300, 1400 });
            var directoryModels = Enumerable.Repeat(new LevelDirectory(), 5);

            Queue<LevelDirectory> createdDirectoryModels = new(directoryModels);
            Queue<LevelDirectory> expectedDirectoryModels = new(directoryModels);
            Queue<LevelGroupName> levelGroupOrder = new(new[] {
                LevelGroupName.Fun,
                LevelGroupName.Tricky,
                LevelGroupName.Taxing,
                LevelGroupName.Mayhem,
                LevelGroupName.Network
            });

            using MemoryStream stream = new(vsrData);
            using BinaryReader reader = new(stream);

            (
                var vsrSerialize,
                var mockLevelDirectorySerializer,
                var mockLevelDirectoryFactory
            ) = this.CreateVsrSerializer();

            mockLevelDirectoryFactory.Setup(m => m()).Returns(createdDirectoryModels.Dequeue()).Verifiable();

            mockLevelDirectorySerializer
                .Setup(m => m.Deserialize(
                    It.Is<BinaryReader>(r => r == reader && reader.BaseStream.Position == expectedReaderAddresses.Dequeue()),
                    It.Is<LevelDirectory>(d => d == expectedDirectoryModels.Dequeue())
                ))
                .Callback((BinaryReader reader, LevelDirectory givenLevelDirectory) =>
                {
                    _ = reader.BaseStream.Seek(directorySize, SeekOrigin.Current);
                    givenLevelDirectory.LevelGroup = new LevelGroup(levelGroupOrder.Dequeue());
                })
                .Returns((BinaryReader reader, LevelDirectory givenLevelDirectory) => givenLevelDirectory)
                .Verifiable();

            LevelPack givenLevelPack = new();
            (var resultVsr, var resultLevelPack) = vsrSerialize.Deserialize(reader, (ServiceFactory.CreateVsr(), givenLevelPack));

            mockLevelDirectorySerializer.Verify();
        }

        [TestMethod]
        public void Deserialize_ShouldAddTheLevelGroupsToTheLevelPack_WhenALevelPackModelIsGiven()
        {
            uint funDirectoryAddress = 1000;
            uint directorySize = 100;
            var assetData = this.CreateFakeAssetData(744, funDirectoryAddress);

            byte[] vsrData = [
                ..assetData,
                ..this.CreateFakeLevelDirectoryData(1, directorySize),
                ..this.CreateFakeLevelDirectoryData(2, directorySize),
                ..this.CreateFakeLevelDirectoryData(3, directorySize),
                ..this.CreateFakeLevelDirectoryData(4, directorySize),
                ..this.CreateFakeLevelDirectoryData(5, directorySize),
            ];

            var directoryModels = Enumerable.Repeat(new LevelDirectory(), 5);
            LevelGroup[] levelGroups = {
                new(LevelGroupName.Fun),
                new(LevelGroupName.Tricky),
                new(LevelGroupName.Taxing),
                new(LevelGroupName.Mayhem),
                new(LevelGroupName.Network)
            };

            Queue<LevelDirectory> createdDirectoryModels = new(directoryModels);
            Queue<LevelGroup> expectedLevelGroups = new(levelGroups);

            using MemoryStream stream = new(vsrData);
            using BinaryReader reader = new(stream);

            (
                var vsrSerialize,
                var mockLevelDirectorySerializer,
                var mockLevelDirectoryFactory
            ) = this.CreateVsrSerializer();

            mockLevelDirectoryFactory.Setup(m => m()).Returns(createdDirectoryModels.Dequeue()).Verifiable();

            _ = mockLevelDirectorySerializer
                .Setup(m => m.Deserialize(It.IsAny<BinaryReader>(), It.IsAny<LevelDirectory>()))
                .Callback((BinaryReader reader, LevelDirectory givenLevelDirectory) =>
                {
                    givenLevelDirectory.LevelGroup = expectedLevelGroups.Dequeue();
                })
                .Returns((BinaryReader reader, LevelDirectory givenLevelDirectory) => givenLevelDirectory);

            LevelPack givenLevelPack = new();
            (var resultVsr, var resultLevelPack) = vsrSerialize.Deserialize(reader, (ServiceFactory.CreateVsr(), givenLevelPack));

            _ = givenLevelPack.GetLevelGroup(LevelGroupName.Fun).Should().Be(levelGroups.ElementAt(0));
            _ = givenLevelPack.GetLevelGroup(LevelGroupName.Tricky).Should().Be(levelGroups.ElementAt(1));
            _ = givenLevelPack.GetLevelGroup(LevelGroupName.Taxing).Should().Be(levelGroups.ElementAt(2));
            _ = givenLevelPack.GetLevelGroup(LevelGroupName.Mayhem).Should().Be(levelGroups.ElementAt(3));
            _ = givenLevelPack.GetLevelGroup(LevelGroupName.Network).Should().Be(levelGroups.ElementAt(4));
        }

        [TestMethod]
        public void Deserialize_ShouldSetTheDirectoryPointersToTheModel()
        {
            uint funPointerAddress = 800;
            var data = this.CreateFakeVsrData(funPointerAddress, 1000);

            using MemoryStream stream = new(data);
            using BinaryReader reader = new(stream);

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

            Vsr vsr = new()
            {
                AssetData = [.. assetData],
            };

            var pointerStart = 8;

            vsr.SetLevelDirectoryPointer(LevelGroupName.Fun, (uint)pointerStart);
            vsr.SetLevelDirectoryPointer(LevelGroupName.Tricky, 12);
            vsr.SetLevelDirectoryPointer(LevelGroupName.Taxing, 16);
            vsr.SetLevelDirectoryPointer(LevelGroupName.Mayhem, 20);
            vsr.SetLevelDirectoryPointer(LevelGroupName.Network, 24);

            (var vsrSerializer, var _, _) = this.CreateVsrSerializer();
            var data = this.Serialize((vsr, new LevelPack()), vsrSerializer);

            var dataBeforeFunPointer = data.Take(pointerStart);
            _ = dataBeforeFunPointer.Should().BeEquivalentTo(assetData.Take(pointerStart));

            var dataAfterPointers = data.Skip(pointerStart + 20);
            _ = dataAfterPointers.Should().BeEquivalentTo(assetData.Skip(pointerStart + 20));
        }
    }
}
