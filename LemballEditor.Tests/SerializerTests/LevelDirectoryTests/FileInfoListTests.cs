using System.Text;
using FluentAssertions;
using LemballEditor.Serializers.LevelDirectory;

namespace LemballEditor.Tests.SerializerTests.LevelDirectoryTests
{
    [TestClass]
    public class FileInfoListTests
    {
        [TestMethod]
        [DataRow(5, 5 * 36)]
        [DataRow(10, 10 * 36)]
        [DataRow(12, 12 * 36)]
        public void Deserialize_ShouldSkipOverTheFileInfoDataAndReturnTheModel(int numberOfFiles, int expectedResultPosition)
        {
            var levelDirectory = new LevelDirectorySerializer
            {
                FixedLevelCount = (byte)numberOfFiles
            };

            using var reader = new BinaryReader(new MemoryStream(new byte[100]));

            var serializer = new FileInfoListSerializer();
            var result = serializer.Deserialize(reader, levelDirectory);

            _ = reader.BaseStream.Position.Should().Be(expectedResultPosition);
            _ = result.Should().Be(levelDirectory);
        }

        [TestMethod]
        [DataRow(1000)]
        [DataRow(1200)]
        public void Serialize_ShouldWriteTheFileNameAddressForEachLevel(int directoryAddress)
        {
            var levelDirectory = new LevelDirectorySerializer
            {
                Address = (uint)directoryAddress,
                FixedLevelCount = 3
            };

            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            var serializer = new FileInfoListSerializer();
            serializer.Serialize(levelDirectory, writer);

            using var reader = new BinaryReader(stream);

            stream.Position = 0;
            _ = reader.ReadUInt32().Should().Be(levelDirectory.Address + 20);

            stream.Position += 32;
            _ = reader.ReadUInt32().Should().Be(levelDirectory.Address + 20 + 12);

            stream.Position += 32;
            _ = reader.ReadUInt32().Should().Be(levelDirectory.Address + 20 + 12 + 12);
        }

        [TestMethod]
        [DataRow((uint)500)]
        [DataRow((uint)600)]
        public void Serialize_ShouldWriteTheIncrementingFileIdForEachLevel(uint firstFileId)
        {
            var levelDirectory = new LevelDirectorySerializer
            {
                FixedLevelCount = 3,
                FirstFileId = firstFileId
            };

            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            var serializer = new FileInfoListSerializer();
            serializer.Serialize(levelDirectory, writer);

            using var reader = new BinaryReader(stream);

            stream.Position = 4;
            _ = reader.ReadUInt32().Should().Be(firstFileId);

            stream.Position += 32;
            _ = reader.ReadUInt32().Should().Be(firstFileId + 1);

            stream.Position += 32;
            _ = reader.ReadUInt32().Should().Be(firstFileId + 2);
        }

        [TestMethod]
        public void Serialize_ShouldWriteTheBinStringForEachLevel()
        {
            var levelDirectory = new LevelDirectorySerializer
            {
                FixedLevelCount = 3
            };

            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            var serializer = new FileInfoListSerializer();
            serializer.Serialize(levelDirectory, writer);

            using var reader = new BinaryReader(stream);

            stream.Position = 8;
            _ = Encoding.ASCII.GetString(reader.ReadBytes(4)).Should().Be(" NIB");

            stream.Position += 32;
            _ = Encoding.ASCII.GetString(reader.ReadBytes(4)).Should().Be(" NIB");

            stream.Position += 32;
            _ = Encoding.ASCII.GetString(reader.ReadBytes(4)).Should().Be(" NIB");
        }

        [TestMethod]
        [DataRow(1000, new uint[] { 100, 50, 100 }, new uint[] { 1000, 1100, 1150 })]
        [DataRow(1000, new uint[] { 20, 20, 100 }, new uint[] { 1000, 1020, 1040 })]
        public void Serialize_ShouldWriteTheAddressForEachLevel(int firstLevelAddress, uint[] levelSizes, uint[] expectedLevelAddresses)
        {
            var directoryHeaderSize = 20 + (12 * levelSizes.Length) + (36 * levelSizes.Length);

            var levelDirectory = new LevelDirectorySerializer
            {
                Address = (uint)(firstLevelAddress - directoryHeaderSize),
                FixedLevelCount = (byte)levelSizes.Length
            };

            foreach (var levelSize in levelSizes)
            {
                var level = new byte[levelSize];
                levelDirectory.AddSerializedLevel(level);
            }

            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            var serializer = new FileInfoListSerializer();
            serializer.Serialize(levelDirectory, writer);

            using var reader = new BinaryReader(stream);

            stream.Position = 12;
            foreach (var expectedLevelSize in expectedLevelAddresses)
            {
                _ = reader.ReadUInt32().Should().Be(expectedLevelSize);
                stream.Position += 32;
            }
        }

        [TestMethod]
        [DataRow(new uint[] { 1000, 1100, 1150 })]
        [DataRow(new uint[] { 1000, 1020 })]
        public void Serialize_ShouldWriteTheFileSizesForEachLevel(uint[] levelSizes)
        {
            var levelDirectory = new LevelDirectorySerializer
            {
                FixedLevelCount = (byte)levelSizes.Length
            };

            foreach (var levelSize in levelSizes)
            {
                var level = new byte[levelSize];
                levelDirectory.AddSerializedLevel(level);
            }

            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            var serializer = new FileInfoListSerializer();
            serializer.Serialize(levelDirectory, writer);

            using var reader = new BinaryReader(stream);

            stream.Position = 16;
            foreach (var expectedLevelSize in levelSizes)
            {
                _ = reader.ReadUInt32().Should().Be(expectedLevelSize);
                stream.Position += 32;
            }
        }

        [TestMethod]
        public void Serialize_ShouldPadEachFileDataTo36Bytes()
        {
            var levelDirectory = new LevelDirectorySerializer
            {
                FixedLevelCount = 3
            };

            levelDirectory.AddSerializedLevel(new byte[100]);
            levelDirectory.AddSerializedLevel(new byte[100]);
            levelDirectory.AddSerializedLevel(new byte[100]);

            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            var serializer = new FileInfoListSerializer();
            serializer.Serialize(levelDirectory, writer);

            using var reader = new BinaryReader(stream);

            stream.Position = 20;
            _ = reader.ReadBytes(16).Should().BeEquivalentTo(new byte[16]);

            stream.Position += 20;
            _ = reader.ReadBytes(16).Should().BeEquivalentTo(new byte[16]);

            stream.Position += 20;
            _ = reader.ReadBytes(16).Should().BeEquivalentTo(new byte[16]);
        }
    }
}
