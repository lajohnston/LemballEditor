using FluentAssertions;
using LemballEditor.Models;
using LemballEditor.Serializers;
using LemballEditor.Serializers.LevelDirectory;
using Moq;

namespace LemballEditor.Tests.SerializerTests.Vsr
{
    [TestClass]
    public class LevelPackSerializerTests
    {
        private (Serializers.Vsr.LevelPackSerializer, Mock<ISerializer<PendingLevelGroup>>, Mock<Func<LevelGroupName?, PendingLevelGroup>>) CreateSerializer()
        {
            Mock<ISerializer<PendingLevelGroup>> mockLevelDirectorySerializer = new();
            Mock<Func<LevelGroupName?, PendingLevelGroup>> mockLevelDirectoryFactory = new();

            _ = mockLevelDirectorySerializer
                .Setup(m => m.Deserialize(
                    It.IsAny<BinaryReader>(),
                    It.IsAny<PendingLevelGroup>()
                ))
                .Returns((BinaryReader reader, PendingLevelGroup givenLevelDirectory) => givenLevelDirectory);

            _ = mockLevelDirectoryFactory.Setup(m => m(It.IsAny<LevelGroupName>()))
                .Returns((LevelGroupName levelGroupName) => ServiceFactory.CreateLevelDirectory(levelGroupName));

            _ = mockLevelDirectoryFactory.Setup(m => m(null)).Returns(() => ServiceFactory.CreateLevelDirectory(null));

            Serializers.Vsr.LevelPackSerializer serializer = new(mockLevelDirectorySerializer.Object, mockLevelDirectoryFactory.Object);

            return (serializer, mockLevelDirectorySerializer, mockLevelDirectoryFactory);
        }

        private (Models.Vsr, Models.LevelPack) CreateModels(int funAddress = 100)
        {
            var (vsr, levelPack) = (new Models.Vsr(), new Models.LevelPack());

            vsr.AssetData = new byte[funAddress];

            foreach (LevelGroupName levelGroupName in Enum.GetValues(typeof(LevelGroupName)))
            {
                levelPack.SetLevelGroup(ServiceFactory.CreateLevelGroup(levelGroupName));
            }

            return (vsr, levelPack);
        }

        [TestMethod]
        public void Deserialize_ShouldReturnTheVsr_WhenNoLevelPackIsGiven()
        {
            var (serializer, _, _) = this.CreateSerializer();
            var models = (new Models.Vsr(), (Models.LevelPack)null);

            var reader = new BinaryReader(new MemoryStream());

            var (resultVsr, resultLevelPack) = serializer.Deserialize(reader, models);

            _ = resultVsr.Should().Be(models.Item1);
            _ = resultLevelPack.Should().BeNull();
        }

        [TestMethod]
        public void Deserialize_ShouldReturnTheGivenModels_WhenALevelPackIsGiven()
        {
            var (serializer, _, _) = this.CreateSerializer();
            var models = CreateModels();

            var reader = new BinaryReader(new MemoryStream());

            var (resultVsr, resultLevelPack) = serializer.Deserialize(reader, models);

            _ = resultVsr.Should().Be(models.Item1);
            _ = resultLevelPack.Should().Be(models.Item2);
        }

        [TestMethod]
        public void Deserialize_ShouldPassTheReaderAndEachLevelDirectoryToTheLevelDirectoryDeserializer_WhenALevelPackIsGiven()
        {
            var (serializer, mockLevelDirectorySerializer, mockLevelDirectoryFactory) = this.CreateSerializer();
            var models = CreateModels();

            var reader = new BinaryReader(new MemoryStream());

            _ = serializer.Deserialize(reader, models);

            foreach (LevelGroupName levelGroupName in Enum.GetValues(typeof(LevelGroupName)))
            {
                mockLevelDirectoryFactory.Verify(m => m(levelGroupName), Times.Once);
                mockLevelDirectorySerializer.Verify(m => m.Deserialize(
                    It.Is<BinaryReader>(r => r == reader),
                    It.Is<PendingLevelGroup>(ld => ld.LevelGroup.LevelGroupName == levelGroupName)
                ), Times.Once);
            }
        }

        [TestMethod]
        public void Deserialize_ShouldSetTheCurrentAddressToEachLevelDirectory_WhenALevelPackIsGiven()
        {
            var (serializer, mockLevelDirectorySerializer, mockLevelDirectoryFactory) = this.CreateSerializer();

            var funAddress = (uint)100;
            var vsr = new Models.Vsr
            {
                AssetData = new byte[funAddress]
            };

            var reader = new BinaryReader(new MemoryStream(new byte[1000]));
            reader.BaseStream.Position = funAddress;

            var resultAddresses = new Dictionary<LevelGroupName, uint>();

            _ = mockLevelDirectorySerializer.Setup(m => m.Deserialize(
                It.IsAny<BinaryReader>(),
                It.IsAny<PendingLevelGroup>())
            ).Callback<BinaryReader, PendingLevelGroup>((r, ld) =>
            {
                resultAddresses[ld.LevelGroup.LevelGroupName] = ld.Address;
                reader.BaseStream.Position += 100;
            }).Returns((BinaryReader reader, PendingLevelGroup ld) => ld);

            _ = serializer.Deserialize(reader, (vsr, new Models.LevelPack()));

            _ = resultAddresses[LevelGroupName.Fun].Should().Be(100);
            _ = resultAddresses[LevelGroupName.Tricky].Should().Be(200);
            _ = resultAddresses[LevelGroupName.Taxing].Should().Be(300);
            _ = resultAddresses[LevelGroupName.Mayhem].Should().Be(400);
            _ = resultAddresses[LevelGroupName.Network].Should().Be(500);
        }

        [TestMethod]
        public void Deserialize_ShouldSetTheFixedLevelCountOfEachLevelDirectory_WhenALevelPackIsGiven()
        {
            var (serializer, mockLevelDirectorySerializer, mockLevelDirectoryFactory) = this.CreateSerializer();

            var models = CreateModels();
            var (vsr, _) = models;

            vsr.SetFixedLevelCount(LevelGroupName.Fun, 25);
            vsr.SetFixedLevelCount(LevelGroupName.Tricky, 26);
            vsr.SetFixedLevelCount(LevelGroupName.Taxing, 27);
            vsr.SetFixedLevelCount(LevelGroupName.Mayhem, 28);
            vsr.SetFixedLevelCount(LevelGroupName.Network, 29);

            var reader = new BinaryReader(new MemoryStream());

            var resultFixedLevelCounts = new Dictionary<LevelGroupName, byte>();

            _ = mockLevelDirectorySerializer.Setup(m => m.Deserialize(
                It.IsAny<BinaryReader>(),
                It.IsAny<PendingLevelGroup>())
            ).Callback<BinaryReader, PendingLevelGroup>((r, ld) =>
            {
                resultFixedLevelCounts[ld.LevelGroup.LevelGroupName] = ld.FixedLevelCount;
            }).Returns((BinaryReader reader, PendingLevelGroup ld) => ld);

            _ = serializer.Deserialize(reader, models);

            _ = resultFixedLevelCounts[LevelGroupName.Fun].Should().Be(25);
            _ = resultFixedLevelCounts[LevelGroupName.Tricky].Should().Be(26);
            _ = resultFixedLevelCounts[LevelGroupName.Taxing].Should().Be(27);
            _ = resultFixedLevelCounts[LevelGroupName.Mayhem].Should().Be(28);
            _ = resultFixedLevelCounts[LevelGroupName.Network].Should().Be(29);
        }

        [TestMethod]
        public void Deserialize_ShouldSetEachDeserializedLevelGroupToTheLevelPack_WhenALevelPackIsGiven()
        {
            var levelGroups = Enum.GetValues(typeof(LevelGroupName))
                .Cast<LevelGroupName>()
                .Select(ServiceFactory.CreateLevelGroup)
                .ToList();

            var levelGroupOrder = new Queue<LevelGroup>(levelGroups);

            var (serializer, mockLevelDirectorySerializer, _) = this.CreateSerializer();

            _ = mockLevelDirectorySerializer.Setup(m => m.Deserialize(
                It.IsAny<BinaryReader>(),
                It.IsAny<PendingLevelGroup>())
            ).Returns((BinaryReader reader, PendingLevelGroup givenLevelDirectory) =>
            {
                var levelDirectory = new PendingLevelGroup
                {
                    LevelGroup = levelGroupOrder.Dequeue()
                };
                return levelDirectory;
            });

            var reader = new BinaryReader(new MemoryStream());
            var models = CreateModels();
            var (vsr, levelPack) = serializer.Deserialize(reader, models);

            _ = levelPack.GetLevelGroup(LevelGroupName.Fun).Should().Be(levelGroups[0]);
            _ = levelPack.GetLevelGroup(LevelGroupName.Tricky).Should().Be(levelGroups[1]);
            _ = levelPack.GetLevelGroup(LevelGroupName.Taxing).Should().Be(levelGroups[2]);
            _ = levelPack.GetLevelGroup(LevelGroupName.Mayhem).Should().Be(levelGroups[3]);
            _ = levelPack.GetLevelGroup(LevelGroupName.Network).Should().Be(levelGroups[4]);
        }

        [TestMethod]
        public void Serialize_ShouldPassEachLevelDirectoryToTheLevelDirectorySerializer()
        {
            var (serializer, mockLevelDirectorySerializer, mockLevelDirectoryFactory) = this.CreateSerializer();

            var levelDirectories = Enumerable.Range(0, 5).Select(_ => new PendingLevelGroup()).ToArray();
            var levelDirectoryCreationOrder = new Queue<PendingLevelGroup>(levelDirectories);

            _ = mockLevelDirectoryFactory.Setup(m => m(null)).Returns(levelDirectoryCreationOrder.Dequeue);

            var (vsr, levelPack) = this.CreateModels();

            var writer = BinaryWriter.Null;
            serializer.Serialize((vsr, levelPack), writer);

            foreach (var levelDirectory in levelDirectories)
            {
                mockLevelDirectorySerializer.Verify(m => m.Serialize(
                    It.Is<PendingLevelGroup>(ld => ld == levelDirectory),
                    It.IsAny<BinaryWriter>()
                ), Times.Once);
            }
        }

        [TestMethod]
        public void Serialize_ShouldSetTheCurrentWritePositionAddressToEachDirectory()
        {
            var (serializer, mockLevelDirectorySerializer, _) = this.CreateSerializer();
            var (vsr, levelPack) = this.CreateModels();

            var givenLevelDirectoryAddresses = new List<uint>();

            _ = mockLevelDirectorySerializer.Setup(m => m.Serialize(
                It.IsAny<PendingLevelGroup>(),
                It.IsAny<BinaryWriter>()))
                .Callback<PendingLevelGroup, BinaryWriter>((levelDirectory, writer) =>
                {
                    givenLevelDirectoryAddresses.Add(levelDirectory.Address);
                    writer.BaseStream.Position += 100;
                });

            using var writer = new BinaryWriter(new MemoryStream());
            serializer.Serialize((vsr, levelPack), writer);

            _ = givenLevelDirectoryAddresses.ToArray().Should().BeEquivalentTo(new uint[] { 0, 100, 200, 300, 400 });
        }

        [TestMethod]
        public void Serialize_ShouldWriteTheCurrentAddressAsThePointerForEachDirectory()
        {
            var (serializer, mockLevelDirectorySerializer, _) = this.CreateSerializer();
            var (vsr, levelPack) = this.CreateModels();

            uint funAddress = 100;

            vsr.SetLevelDirectoryPointer(LevelGroupName.Fun, 10);
            vsr.SetLevelDirectoryPointer(LevelGroupName.Tricky, 20);
            vsr.SetLevelDirectoryPointer(LevelGroupName.Taxing, 30);
            vsr.SetLevelDirectoryPointer(LevelGroupName.Mayhem, 40);
            vsr.SetLevelDirectoryPointer(LevelGroupName.Network, 50);

            var expectedAddresses = new Dictionary<LevelGroupName, uint>
            {
                [LevelGroupName.Fun] = funAddress,
                [LevelGroupName.Tricky] = funAddress + 100,
                [LevelGroupName.Taxing] = funAddress + 200,
                [LevelGroupName.Mayhem] = funAddress + 300,
                [LevelGroupName.Network] = funAddress + 400
            };

            _ = mockLevelDirectorySerializer.Setup(m => m.Serialize(
                It.IsAny<PendingLevelGroup>(),
                It.IsAny<BinaryWriter>()))
                .Callback<PendingLevelGroup, BinaryWriter>((levelDirectory, writer) =>
                {
                    writer.BaseStream.Position += 100;
                });

            var stream = new MemoryStream(new byte[1000]);
            using var writer = new BinaryWriter(stream);

            writer.BaseStream.Position = funAddress;
            serializer.Serialize((vsr, levelPack), writer);

            using var reader = new BinaryReader(stream);

            foreach (LevelGroupName levelGroupName in Enum.GetValues(typeof(LevelGroupName)))
            {
                reader.BaseStream.Position = vsr.GetLevelDirectoryPointer(levelGroupName);
                var writtenAddress = reader.ReadUInt32();

                _ = writtenAddress.Should().Be(expectedAddresses[levelGroupName], $"for {levelGroupName} level group");
            }
        }

        [TestMethod]
        public void Serialize_ShouldSetTheLevelGroupForEachDirectory()
        {
            var (serializer, mockLevelDirectorySerializer, _) = this.CreateSerializer();
            var (vsr, levelPack) = this.CreateModels();

            var givenLevelGroups = new List<LevelGroup>();

            _ = mockLevelDirectorySerializer.Setup(m => m.Serialize(
                It.IsAny<PendingLevelGroup>(),
                It.IsAny<BinaryWriter>()))
                .Callback<PendingLevelGroup, BinaryWriter>((levelDirectory, writer) =>
                {
                    givenLevelGroups.Add(levelDirectory.LevelGroup);
                });

            using var writer = new BinaryWriter(new MemoryStream());
            serializer.Serialize((vsr, levelPack), writer);

            _ = givenLevelGroups[0].LevelGroupName.Should().Be(LevelGroupName.Fun);
            _ = givenLevelGroups[1].LevelGroupName.Should().Be(LevelGroupName.Tricky);
            _ = givenLevelGroups[2].LevelGroupName.Should().Be(LevelGroupName.Taxing);
            _ = givenLevelGroups[3].LevelGroupName.Should().Be(LevelGroupName.Mayhem);
            _ = givenLevelGroups[4].LevelGroupName.Should().Be(LevelGroupName.Network);
        }

        [TestMethod]
        public void Serialize_ShouldSetTheFixedLevelCountOfEachDirectory()
        {
            var (serializer, mockLevelDirectorySerializer, _) = this.CreateSerializer();
            var (vsr, levelPack) = this.CreateModels();

            vsr.FirstLevelFileId = 500;
            vsr.SetFixedLevelCount(LevelGroupName.Fun, 10);
            vsr.SetFixedLevelCount(LevelGroupName.Tricky, 20);
            vsr.SetFixedLevelCount(LevelGroupName.Taxing, 30);
            vsr.SetFixedLevelCount(LevelGroupName.Mayhem, 40);
            vsr.SetFixedLevelCount(LevelGroupName.Network, 50);

            var givenFirstFileIds = new Dictionary<LevelGroupName, byte>();

            _ = mockLevelDirectorySerializer.Setup(m => m.Serialize(
                It.IsAny<PendingLevelGroup>(),
                It.IsAny<BinaryWriter>()))
                .Callback<PendingLevelGroup, BinaryWriter>((levelDirectory, writer) =>
                {
                    givenFirstFileIds[levelDirectory.LevelGroup.LevelGroupName] = levelDirectory.FixedLevelCount;
                });

            using var writer = new BinaryWriter(new MemoryStream());
            serializer.Serialize((vsr, levelPack), writer);

            _ = givenFirstFileIds[LevelGroupName.Fun].Should().Be(10);
            _ = givenFirstFileIds[LevelGroupName.Tricky].Should().Be(20);
            _ = givenFirstFileIds[LevelGroupName.Taxing].Should().Be(30);
            _ = givenFirstFileIds[LevelGroupName.Mayhem].Should().Be(40);
            _ = givenFirstFileIds[LevelGroupName.Network].Should().Be(50);
        }

        [TestMethod]
        public void Serialize_ShouldSetTheFirstFileIdOfEachDirectory()
        {
            var (serializer, mockLevelDirectorySerializer, _) = this.CreateSerializer();
            var (vsr, levelPack) = this.CreateModels();

            vsr.FirstLevelFileId = 500;
            vsr.SetFixedLevelCount(LevelGroupName.Fun, 10);
            vsr.SetFixedLevelCount(LevelGroupName.Tricky, 20);
            vsr.SetFixedLevelCount(LevelGroupName.Taxing, 30);
            vsr.SetFixedLevelCount(LevelGroupName.Mayhem, 40);
            vsr.SetFixedLevelCount(LevelGroupName.Network, 50);

            var givenFirstFileIds = new Dictionary<LevelGroupName, uint>();

            _ = mockLevelDirectorySerializer.Setup(m => m.Serialize(
                It.IsAny<PendingLevelGroup>(),
                It.IsAny<BinaryWriter>()))
                .Callback<PendingLevelGroup, BinaryWriter>((levelDirectory, writer) =>
                {
                    givenFirstFileIds[levelDirectory.LevelGroup.LevelGroupName] = levelDirectory.FirstFileId;
                });

            using var writer = new BinaryWriter(new MemoryStream());
            serializer.Serialize((vsr, levelPack), writer);

            _ = givenFirstFileIds[LevelGroupName.Fun].Should().Be(500);
            _ = givenFirstFileIds[LevelGroupName.Tricky].Should().Be(510);
            _ = givenFirstFileIds[LevelGroupName.Taxing].Should().Be(530);
            _ = givenFirstFileIds[LevelGroupName.Mayhem].Should().Be(560);
            _ = givenFirstFileIds[LevelGroupName.Network].Should().Be(600);
        }
    }
}
