using System.Text;
using FluentAssertions;
using LemballEditor.Models;
using LemballEditor.Serializers;
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

            levelDirectoryFactoryMock.SetReturnsDefault(new LevelDirectory());

            VsrSerializer serializer = new(levelDirectorySerializerMock.Object, levelDirectoryFactoryMock.Object);
            return (serializer, levelDirectorySerializerMock, levelDirectoryFactoryMock);
        }

        /// <summary>
        /// Helper function to create valid VSR binary data
        /// </summary>
        /// <param name="levelDirectoryData">An array of level directory data (can created each using CreateFakeLevelDirectoryData)</param>
        /// <param name="funPointerAddress">The position of the Fun directory pointer</param>
        /// <param name="assetDataSize">The size of the asset data (all data excluding the level directories</param>
        /// <returns></returns>
        private byte[] CreateFakeVsrData(byte[][]? levelDirectoryData = null, uint funPointerAddress = 744, uint assetDataSize = 1000)
        {
            levelDirectoryData ??=
                [
                    this.CreateFakeLevelDirectoryData(32, 25),
                    this.CreateFakeLevelDirectoryData(32, 25),
                    this.CreateFakeLevelDirectoryData(32, 25),
                    this.CreateFakeLevelDirectoryData(32, 25),
                    this.CreateFakeLevelDirectoryData(32, 25)
                ];

            List<byte> data = [];
            data.AddRange(Encoding.ASCII.GetBytes("CRID"));             // Header
            data.AddRange(new byte[funPointerAddress - data.Count]);    // Padding to pointers

            var levelDirectoryAddress = assetDataSize;

            // Pointers to level directories
            foreach (var directoryData in levelDirectoryData)
            {
                data.AddRange(BitConverter.GetBytes(levelDirectoryAddress));
                data.AddRange(new byte[32]); // Padding to next pointer
                levelDirectoryAddress += (uint)directoryData.Length;
            }

            // Other data, containing the 'Demo_00' string
            var demoFileAddress = funPointerAddress + 188;

            data.AddRange(new byte[demoFileAddress - data.Count]);  // Padding
            data.AddRange(Encoding.ASCII.GetBytes("Demo_00"));      // Demo_00 string
            data.AddRange(new byte[assetDataSize - data.Count]);    // Rest of asset data

            // Add level directory data
            foreach (var directoryData in levelDirectoryData)
            {
                data.AddRange(directoryData);
            }

            return [.. data];
        }

        private byte[] CreateFakeLevelDirectoryData(uint size = 100, uint levelCount = 5, uint firstFileId = 0)
        {
            var data = new List<byte>();
            data.AddRange(Encoding.ASCII.GetBytes("CRID"));
            data.AddRange(BitConverter.GetBytes(size - 4));
            data.AddRange(BitConverter.GetBytes(levelCount));
            data.AddRange(new byte[8]);

            // Fake file names
            data.AddRange(new byte[levelCount * 12]);

            // First file descriptor (containing id)
            data.AddRange(new byte[4]);
            data.AddRange(BitConverter.GetBytes(firstFileId));
            data.AddRange(new byte[28]);

            // Remaining file descriptors
            data.AddRange(new byte[(levelCount - 1) * 36]);

            // Fake empty level data
            var footerBytesOfLastLevel = Encoding.ASCII.GetBytes("?DNE");
            data.AddRange(Enumerable.Repeat((byte)5, (int)size - 16));  // pad to min size
            data.AddRange(footerBytesOfLastLevel);

            return [.. data];
        }

        [TestMethod]
        public void Deserialize_ShouldThrowAnException_WhenTheStreamDoesNotBeginWithCridHeader()
        {
            byte[] data = [1, 2, 3];

            using var reader = new BinaryReader(new MemoryStream(data));
            var serializer = ServiceFactory.CreateVsrSerializer();

            Func<(Vsr, LevelPack)> act = () => serializer.Deserialize(reader, (ServiceFactory.CreateVsr(), null));
            _ = act.Should().Throw<InvalidDataException>().WithMessage("Invalid VSR");
        }

        [TestMethod]
        public void Deserialize_ShouldThrowAnException_WhenTheFunDirectoryCannotBeLocated()
        {
            List<byte> data = [.. Encoding.ASCII.GetBytes("CRID"), .. new byte[200]];

            using var reader = new BinaryReader(new MemoryStream([.. data]));
            var serializer = ServiceFactory.CreateVsrSerializer();

            Func<(Vsr, LevelPack)> act = () => serializer.Deserialize(reader, (ServiceFactory.CreateVsr(), null));
            _ = act.Should().Throw<InvalidDataException>().WithMessage("Unable to locate FUN directory pointer in VSR data");
        }

        [TestMethod]
        public void Deserialize_ShouldSetTheAssetDataUpToTheFunDirectoryPointerToTheModel()
        {
            uint funDirectoryAddress = 1500;

            var vsrData = this.CreateFakeVsrData(null, 744, funDirectoryAddress);

            using var reader = new BinaryReader(new MemoryStream(vsrData));

            var vsr = ServiceFactory.CreateVsr();
            var serializer = ServiceFactory.CreateVsrSerializer();
            _ = serializer.Deserialize(reader, (vsr, null));

            _ = vsr.AssetData.Should().BeEquivalentTo(vsrData.Take((int)funDirectoryAddress));
        }

        [TestMethod]
        public void Deserialize_ShouldNotCallTheLevelDirectorySerializer_WhenLevelPackIsNull()
        {
            var vsrData = this.CreateFakeVsrData();

            using var reader = new BinaryReader(new MemoryStream(vsrData));

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

            var vsrData = this.CreateFakeVsrData([
                this.CreateFakeLevelDirectoryData(directorySize, 1),
                this.CreateFakeLevelDirectoryData(directorySize, 2),
                this.CreateFakeLevelDirectoryData(directorySize, 3),
                this.CreateFakeLevelDirectoryData(directorySize, 4),
                this.CreateFakeLevelDirectoryData(directorySize, 5)
            ]);

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

            using var reader = new BinaryReader(new MemoryStream(vsrData));

            var (vsrSerialize, mockLevelDirectorySerializer, mockLevelDirectoryFactory) = this.CreateVsrSerializer();

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
            var vsrData = this.CreateFakeVsrData();

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

            using var reader = new BinaryReader(new MemoryStream(vsrData));

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

            var vsrData = this.CreateFakeVsrData([
                this.CreateFakeLevelDirectoryData(32, 1),
                this.CreateFakeLevelDirectoryData(32, 2),
                this.CreateFakeLevelDirectoryData(32, 3),
                this.CreateFakeLevelDirectoryData(32, 4),
                this.CreateFakeLevelDirectoryData(32, 5)
            ], funPointerAddress);

            using BinaryReader reader = new(new MemoryStream(vsrData));

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
        public void Deserialize_ShouldSetTheFixedLevelCountsForEachLevelGroup()
        {
            var vsrData = this.CreateFakeVsrData([
                this.CreateFakeLevelDirectoryData(32, 1),
                this.CreateFakeLevelDirectoryData(32, 2),
                this.CreateFakeLevelDirectoryData(32, 3),
                this.CreateFakeLevelDirectoryData(32, 4),
                this.CreateFakeLevelDirectoryData(32, 5)
            ]);

            using BinaryReader reader = new(new MemoryStream(vsrData));

            var serializer = ServiceFactory.CreateVsrSerializer();
            var (resultVsr, _) = serializer.Deserialize(reader, (ServiceFactory.CreateVsr(), null));

            _ = resultVsr.GetFixedLevelCount(LevelGroupName.Fun).Should().Be(1);
            _ = resultVsr.GetFixedLevelCount(LevelGroupName.Tricky).Should().Be(2);
            _ = resultVsr.GetFixedLevelCount(LevelGroupName.Taxing).Should().Be(3);
            _ = resultVsr.GetFixedLevelCount(LevelGroupName.Mayhem).Should().Be(4);
            _ = resultVsr.GetFixedLevelCount(LevelGroupName.Network).Should().Be(5);
        }

        [TestMethod]
        [DataRow(0)]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        [DataRow(4)]
        public void Deserialize_ShouldThrowAnExceptionIfAnyLevelDirectoryContainsMoreThan29Levels(int invalidGroupIndex)
        {
            var levelDirectoryData = new byte[5][];

            for (var i = 0; i < 5; i++)
            {
                levelDirectoryData[i] = this.CreateFakeLevelDirectoryData(100, i == invalidGroupIndex ? (uint)30 : 29);
            }

            var vsrData = this.CreateFakeVsrData(levelDirectoryData);
            using BinaryReader reader = new(new MemoryStream(vsrData));

            var serializer = ServiceFactory.CreateVsrSerializer();
            var act = () => serializer.Deserialize(reader, (ServiceFactory.CreateVsr(), null));

            _ = act.Should().Throw<InvalidDataException>()
                .WithMessage("Level directory contains more than 29 levels");
        }

        [TestMethod]
        public void Deserialize_ShouldStoreTheFileIdOfTheFirstLevelInTheFunDirectory()
        {
            uint fileId = 581;

            var vsrData = this.CreateFakeVsrData([
                this.CreateFakeLevelDirectoryData(32, 1, fileId),
                this.CreateFakeLevelDirectoryData(32, 2),
                this.CreateFakeLevelDirectoryData(32, 3),
                this.CreateFakeLevelDirectoryData(32, 4),
                this.CreateFakeLevelDirectoryData(32, 5)
            ]);

            using var reader = new BinaryReader(new MemoryStream(vsrData));
            var vsr = ServiceFactory.CreateVsr();

            (var vsrSerializer, _, _) = this.CreateVsrSerializer();
            _ = vsrSerializer.Deserialize(reader, (vsr, null));

            _ = vsr.FirstLevelFileId.Should().Be(fileId);
        }

        [TestMethod]
        public void Serialize_ShouldWriteTheAssetDataToTheStream()
        {
            var vsr = new Vsr
            {
                AssetData = Enumerable.Repeat((byte)1, 1000).ToArray()
            };

            var pointerStart = 8;

            vsr.SetLevelDirectoryPointer(LevelGroupName.Fun, (uint)pointerStart);
            vsr.SetLevelDirectoryPointer(LevelGroupName.Tricky, 12);
            vsr.SetLevelDirectoryPointer(LevelGroupName.Taxing, 16);
            vsr.SetLevelDirectoryPointer(LevelGroupName.Mayhem, 20);
            vsr.SetLevelDirectoryPointer(LevelGroupName.Network, 24);

            (var vsrSerializer, var _, _) = this.CreateVsrSerializer();

            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);
            vsrSerializer.Serialize((vsr, new LevelPack()), writer);

            var resultData = stream.ToArray();

            var dataBeforeFunPointer = resultData.Take(pointerStart);
            _ = dataBeforeFunPointer.Should().BeEquivalentTo(vsr.AssetData.Take(pointerStart));

            var dataAfterPointers = resultData.Skip(pointerStart + 20);
            _ = dataAfterPointers.Should().BeEquivalentTo(vsr.AssetData.Skip(pointerStart + 20));
        }

        [TestMethod]
        [DataRow(LevelGroupName.Fun)]
        [DataRow(LevelGroupName.Tricky)]
        [DataRow(LevelGroupName.Taxing)]
        [DataRow(LevelGroupName.Mayhem)]
        [DataRow(LevelGroupName.Network)]
        public void Serialize_ShouldSerializeTheLevelGroupsUsingTheLevelDirectorySerializer(LevelGroupName levelGroup)
        {
            (var vsrSerializer, var mockLevelDirectorySerializer, _) = this.CreateVsrSerializer();

            Vsr vsr = new()
            {
                AssetData = Enumerable.Repeat((byte)1, 1000).ToArray()
            };

            var levelPack = new LevelPack();
            using var writer = BinaryWriter.Null;

            mockLevelDirectorySerializer
                .Setup(m => m.Serialize(
                    It.Is<LevelDirectory>(ld => ld.LevelGroup == levelPack.GetLevelGroup(levelGroup)),
                    It.Is<BinaryWriter>(w => w == writer)
                ))
                .Verifiable();

            vsrSerializer.Serialize((vsr, levelPack), writer);

            mockLevelDirectorySerializer.Verify();
        }

        [TestMethod]
        public void Serialize_ShouldSetTheFixedLevelSizeForEachLevelDirectory()
        {
            (var vsrSerializer, var mockLevelDirectorySerializer, _) = this.CreateVsrSerializer();

            Vsr vsr = new()
            {
                AssetData = Enumerable.Repeat((byte)1, 1000).ToArray()
            };

            vsr.SetFixedLevelCount(LevelGroupName.Fun, 1);
            vsr.SetFixedLevelCount(LevelGroupName.Tricky, 2);
            vsr.SetFixedLevelCount(LevelGroupName.Taxing, 3);
            vsr.SetFixedLevelCount(LevelGroupName.Mayhem, 4);
            vsr.SetFixedLevelCount(LevelGroupName.Network, 5);

            using var writer = BinaryWriter.Null;

            mockLevelDirectorySerializer
                .Setup(m => m.Serialize(
                    It.Is<LevelDirectory>(ld => ld.FixedLevelCount == vsr.GetFixedLevelCount(ld.LevelGroup.LevelGroupName)),
                    It.Is<BinaryWriter>(w => w == writer)
                ))
                .Verifiable();

            vsrSerializer.Serialize((vsr, new LevelPack()), writer);

            mockLevelDirectorySerializer.Verify();
        }

        [TestMethod]
        [DataRow(LevelGroupName.Fun, 100, 100)]
        [DataRow(LevelGroupName.Tricky, 100, 200)]
        [DataRow(LevelGroupName.Taxing, 100, 300)]
        [DataRow(LevelGroupName.Mayhem, 100, 400)]
        [DataRow(LevelGroupName.Network, 100, 500)]
        public void Serialize_ShouldSetTheFirstFileIdForEachLevelDirectory(
            LevelGroupName levelGroupName,
            int firstFunFileId,
            int expectedFirstFileId
        )
        {
            (var vsrSerializer, var mockLevelDirectorySerializer, _) = this.CreateVsrSerializer();

            Vsr vsr = new()
            {
                AssetData = Enumerable.Repeat((byte)1, 1000).ToArray(),
                FirstLevelFileId = (uint)firstFunFileId
            };

            mockLevelDirectorySerializer
                .Setup(m => m.Serialize(
                    It.Is<LevelDirectory>(ld => ld.LevelGroup.LevelGroupName == levelGroupName && ld.FirstFileId == expectedFirstFileId), 
                    It.IsAny<BinaryWriter>()
                ))
                .Verifiable();

            vsr.SetFixedLevelCount(LevelGroupName.Fun, 100);
            vsr.SetFixedLevelCount(LevelGroupName.Tricky, 100);
            vsr.SetFixedLevelCount(LevelGroupName.Taxing, 100);
            vsr.SetFixedLevelCount(LevelGroupName.Mayhem, 100);
            vsr.SetFixedLevelCount(LevelGroupName.Network, 100);

            using var writer = BinaryWriter.Null;
            vsrSerializer.Serialize((vsr, new LevelPack()), writer);

            mockLevelDirectorySerializer.Verify();
        }

        [TestMethod]
        [DataRow(LevelGroupName.Fun, 1000, 100, 1000)]
        [DataRow(LevelGroupName.Tricky, 1000, 100, 1100)]
        [DataRow(LevelGroupName.Taxing, 1000, 100, 1200)]
        [DataRow(LevelGroupName.Mayhem, 1000, 100, 1300)]
        [DataRow(LevelGroupName.Network, 1000, 100, 1400)]
        public void Serialize_ShouldSetTheAddressForEachLevelDirectory(LevelGroupName levelGroupName, int assetsSize, int eachDirectorySize, int expectedAddress)
        {
            (var vsrSerializer, var mockLevelDirectorySerializer, _) = this.CreateVsrSerializer();

            Vsr vsr = new()
            {
                AssetData = Enumerable.Repeat((byte)1, assetsSize).ToArray()
            };

            using var writer = new BinaryWriter(new MemoryStream());

            mockLevelDirectorySerializer
                .Setup(m => m.Serialize(
                    It.IsAny<LevelDirectory>(),
                    It.IsAny<BinaryWriter>()
                ))
                .Callback((LevelDirectory givenLevelDirectory, BinaryWriter writer) =>
                {
                    writer.Write(new byte[eachDirectorySize]);
                });

            mockLevelDirectorySerializer
                .Setup(m => m.Serialize(
                    It.Is<LevelDirectory>(ld => ld.LevelGroup.LevelGroupName == levelGroupName && ld.Address == expectedAddress),
                    It.IsAny<BinaryWriter>()
                ))
                .Verifiable();

            vsrSerializer.Serialize((vsr, new LevelPack()), writer);

            mockLevelDirectorySerializer.Verify();
        }

        [TestMethod]
        [DataRow(LevelGroupName.Fun, 1000)]
        [DataRow(LevelGroupName.Tricky, 1100)]
        [DataRow(LevelGroupName.Taxing, 1200)]
        [DataRow(LevelGroupName.Mayhem, 1300)]
        [DataRow(LevelGroupName.Network, 1400)]
        public void Serialize_ShouldSetTheDirectoryAddressesForEachLevelGroup(LevelGroupName levelGroupName, int expectedAddress)
        {
            var (vsrSerializer, mockLevelDirectorySerializer, _) = this.CreateVsrSerializer();

            Vsr vsr = new()
            {
                AssetData = Enumerable.Repeat((byte)1, 1000).ToArray(),
            };

            vsr.SetLevelDirectoryPointer(LevelGroupName.Fun, 700);
            vsr.SetLevelDirectoryPointer(LevelGroupName.Tricky, 710);
            vsr.SetLevelDirectoryPointer(LevelGroupName.Taxing, 720);
            vsr.SetLevelDirectoryPointer(LevelGroupName.Mayhem, 730);
            vsr.SetLevelDirectoryPointer(LevelGroupName.Network, 740);

            var levelPack = new LevelPack();
            using var writer = new BinaryWriter(new MemoryStream());

            _ = mockLevelDirectorySerializer
                .Setup(m => m.Serialize(
                    It.IsAny<LevelDirectory>(),
                    It.IsAny<BinaryWriter>()
                ))
                .Callback((LevelDirectory givenLevelDirectory, BinaryWriter writer) =>
                {
                    writer.Write(new byte[100]);
                });

            vsrSerializer.Serialize((vsr, levelPack), writer);

            using var reader = new BinaryReader(writer.BaseStream);

            reader.BaseStream.Position = vsr.GetLevelDirectoryPointer(levelGroupName);
            _ = reader.ReadUInt32().Should().Be((uint)expectedAddress);
        }
    }
}
