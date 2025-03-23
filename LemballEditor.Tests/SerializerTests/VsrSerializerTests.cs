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
            vsr.LevelPack = null;

            var (serializer, levelGroupSerializerMock) = CreateVsrSerializer();
            _ = serializer.Deserialize(reader, vsr);

            levelGroupSerializerMock.Verify(
                m => m.Deserialize(It.IsAny<BinaryReader>(), It.IsAny<LevelDirectory>()),
                Times.Never);
        }

        [TestMethod]
        public void Deserialize_ShouldPassEachLevelGroupInTheLevelPackToTheLevelGroupSerializer()
        {
            var data = GetValidData(744, 1000);

            using var stream = new MemoryStream(data);
            using var reader = new BinaryReader(stream);

            var vsr = ServiceFactory.CreateVsr();
            vsr.LevelPack = ServiceFactory.CreateLevelPack();

            var (serializer, levelGroupSerializerMock) = CreateVsrSerializer();

            _ = levelGroupSerializerMock.Setup(
                m => m.Deserialize(
                    reader,
                    It.IsAny<LevelDirectory>()
                )
            ).Returns<BinaryReader, LevelDirectory>((reader, model) => model);

            _ = serializer.Deserialize(reader, vsr);

            foreach (var groupName in Enum.GetValues(typeof(LevelGroupName)).Cast<LevelGroupName>())
            {
                levelGroupSerializerMock.Verify(
                    m => m.Deserialize(
                        It.Is<BinaryReader>(r => r == reader),
                        It.IsAny<LevelDirectory>()
                    )
                );
            }
        }

        [TestMethod]
        public void Deserialize_ShouldSetTheLevelGroupsReturnedFromTheLevelGroupSerializerToTheLevelPack()
        {
            var data = GetValidData(744, 1000);

            using var stream = new MemoryStream(data);
            using var reader = new BinaryReader(stream);

            var vsr = ServiceFactory.CreateVsr();
            var levelPack = ServiceFactory.CreateLevelPack();
            vsr.LevelPack = levelPack;

            var serializedModels = new List<LevelDirectory>();
            for (var i = 0; i < 5; i++)
            {
                serializedModels.Add(new LevelDirectory() { LevelGroup = new Models.LevelGroup() });
            }

            var (serializer, levelGroupSerializerMock) = CreateVsrSerializer();

            _ = levelGroupSerializerMock.Setup(
                m => m.Deserialize(
                    It.IsAny<BinaryReader>(),
                    It.IsAny<LevelDirectory>()
                )
            ).Returns(new Queue<LevelDirectory>(serializedModels).Dequeue);

            _ = serializer.Deserialize(reader, vsr);

            _ = levelPack.GetLevelGroup(LevelGroupName.Fun).Should().Be(serializedModels[0].LevelGroup);
            _ = levelPack.GetLevelGroup(LevelGroupName.Tricky).Should().Be(serializedModels[1].LevelGroup);
            _ = levelPack.GetLevelGroup(LevelGroupName.Taxing).Should().Be(serializedModels[2].LevelGroup);
            _ = levelPack.GetLevelGroup(LevelGroupName.Mayhem).Should().Be(serializedModels[3].LevelGroup);
            _ = levelPack.GetLevelGroup(LevelGroupName.Network).Should().Be(serializedModels[4].LevelGroup);
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
                LevelPack = new LevelPack(),
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

        [TestMethod]
        public void Serialize_ShouldSetTheAddressesOfEachLevelDirectory()
        {
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            var vsr = new Vsr
            {
                AssetData = new byte[100],
                LevelPack = new LevelPack(),
            };

            vsr.SetLevelDirectoryPointer(LevelGroupName.Fun, 8);
            vsr.SetLevelDirectoryPointer(LevelGroupName.Tricky, 12);
            vsr.SetLevelDirectoryPointer(LevelGroupName.Taxing, 16);
            vsr.SetLevelDirectoryPointer(LevelGroupName.Mayhem, 20);
            vsr.SetLevelDirectoryPointer(LevelGroupName.Network, 24);

            var (vsrSerializer, mockLevelGroupSerializer) = CreateVsrSerializer();

            _ = mockLevelGroupSerializer.Setup(
                m => m.Serialize(
                    It.IsAny<LevelDirectory>(),
                    It.IsAny<BinaryWriter>()
                )
            ).Callback(() =>
            {
                stream.Position += 100;
            });

            var serializer = new VsrSerializer(mockLevelGroupSerializer.Object);
            serializer.Serialize(vsr, writer);

            using var reader = new BinaryReader(stream);

            stream.Position = vsr.GetLevelDirectoryPointer(LevelGroupName.Fun);
            _ = reader.ReadUInt32().Should().Be(vsr.FunAddress);

            stream.Position = vsr.GetLevelDirectoryPointer(LevelGroupName.Tricky);
            _ = reader.ReadUInt32().Should().Be(vsr.FunAddress + 100);

            stream.Position = vsr.GetLevelDirectoryPointer(LevelGroupName.Taxing);
            _ = reader.ReadUInt32().Should().Be(vsr.FunAddress + 200);

            stream.Position = vsr.GetLevelDirectoryPointer(LevelGroupName.Mayhem);
            _ = reader.ReadUInt32().Should().Be(vsr.FunAddress + 300);

            stream.Position = vsr.GetLevelDirectoryPointer(LevelGroupName.Network);
            _ = reader.ReadUInt32().Should().Be(vsr.FunAddress + 400);
        }

        [TestMethod]
        public void Serialize_ShouldSetTheDirectoryAddressToEachLevelGroupContext()
        {
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            var vsr = new Vsr
            {
                AssetData = new byte[100],
                LevelPack = new LevelPack(),
            };

            var (vsrSerializer, mockLevelGroupSerializer) = CreateVsrSerializer();

            var givenBaseAddresses = new List<uint>();

            _ = mockLevelGroupSerializer.Setup(
                m => m.Serialize(
                    It.IsAny<LevelDirectory>(),
                    It.IsAny<BinaryWriter>()
                )
            ).Callback<LevelDirectory, BinaryWriter>((model, writer) =>
            {
                givenBaseAddresses.Add(model.BaseAddress);
                stream.Position += 100;
            });

            var serializer = new VsrSerializer(mockLevelGroupSerializer.Object);
            serializer.Serialize(vsr, writer);

            _ = givenBaseAddresses.Should().BeEquivalentTo([100, 200, 300, 400, 500]);
        }
    }
}
