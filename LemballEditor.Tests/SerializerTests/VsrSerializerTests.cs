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
        private Mock<ISerializer<(Models.LevelGroup, LevelDirectoryContext)>> CreateMockLevelGroupSerializer()
        {
            return new Mock<ISerializer<(Models.LevelGroup, LevelDirectoryContext)>>();
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

            var levelGroupSerializerMock = CreateMockLevelGroupSerializer();

            var serializer = new VsrSerializer(levelGroupSerializerMock.Object);
            _ = serializer.Deserialize(reader, vsr);

            levelGroupSerializerMock.Verify(
                m => m.Deserialize(It.IsAny<BinaryReader>(), It.IsAny<(Models.LevelGroup, LevelDirectoryContext)>()),
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

            var levelGroupSerializerMock = CreateMockLevelGroupSerializer();
            _ = levelGroupSerializerMock.Setup(
                m => m.Deserialize(
                    reader,
                    It.IsAny<(Models.LevelGroup, LevelDirectoryContext)>()
                )
            ).Returns<BinaryReader, (Models.LevelGroup, LevelDirectoryContext)>((reader, models) => models);

            var serializer = new VsrSerializer(levelGroupSerializerMock.Object);
            _ = serializer.Deserialize(reader, vsr);

            foreach (var groupName in Enum.GetValues(typeof(LevelGroupName)).Cast<LevelGroupName>())
            {
                levelGroupSerializerMock.Verify(
                    m => m.Deserialize(
                        It.Is<BinaryReader>(r => r == reader),
                        It.IsAny<(Models.LevelGroup, LevelDirectoryContext)>()
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

            var serializedGroups = new List<Models.LevelGroup>();
            var serializedModels = new List<(Models.LevelGroup, LevelDirectoryContext)>();
            for (var i = 0; i < 5; i++)
            {
                var group = new Models.LevelGroup();
                serializedGroups.Add(group);
                serializedModels.Add((group, new LevelDirectoryContext()));
            }
            ;

            var levelGroupSerializerMock = CreateMockLevelGroupSerializer();
            _ = levelGroupSerializerMock.Setup(
                m => m.Deserialize(
                    It.IsAny<BinaryReader>(),
                    It.IsAny<(Models.LevelGroup, LevelDirectoryContext)>()
                )
            ).Returns(new Queue<(Models.LevelGroup, LevelDirectoryContext)>(serializedModels).Dequeue);

            var serializer = new VsrSerializer(levelGroupSerializerMock.Object);
            _ = serializer.Deserialize(reader, vsr);

            _ = levelPack.GetLevelGroup(LevelGroupName.Fun).Should().Be(serializedGroups[0]);
            _ = levelPack.GetLevelGroup(LevelGroupName.Tricky).Should().Be(serializedGroups[1]);
            _ = levelPack.GetLevelGroup(LevelGroupName.Taxing).Should().Be(serializedGroups[2]);
            _ = levelPack.GetLevelGroup(LevelGroupName.Mayhem).Should().Be(serializedGroups[3]);
            _ = levelPack.GetLevelGroup(LevelGroupName.Network).Should().Be(serializedGroups[4]);
        }
    }
}
