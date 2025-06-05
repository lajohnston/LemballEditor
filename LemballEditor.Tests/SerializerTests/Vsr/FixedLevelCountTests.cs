using System.Text;
using FluentAssertions;
using LemballEditor.Models;
using LemballEditor.Serializers.Vsr;

namespace LemballEditor.Tests.SerializerTests.Vsr
{
    [TestClass]
    public class FixedLevelCountTests
    {
        private byte[] GetValidData(int funAddress, int[] levelCounts, int directoryDataSizes = 100)
        {
            var data = new List<byte>();
            var assetData = new byte[funAddress];
            data.AddRange(assetData);

            foreach (var levelCount in levelCounts)
            {
                data.AddRange(Encoding.ASCII.GetBytes("CRID")); // header
                data.AddRange(BitConverter.GetBytes(directoryDataSizes));
                data.AddRange(BitConverter.GetBytes(levelCount));
                data.AddRange(new byte[directoryDataSizes - 4]); // blank data
            }

            return data.ToArray();
        }

        [TestMethod]
        public void Deserialize_ShouldReturnTheModels()
        {
            var vsr = ServiceFactory.CreateVsr();
            vsr.AssetData = new byte[100];

            var data = this.GetValidData(vsr.AssetData.Length, new int[5] { 1, 2, 3, 4, 5 });

            using var reader = new BinaryReader(new MemoryStream(data));

            var serializer = new FixedLevelCount();
            var result = serializer.Deserialize(reader, (vsr, null));
            _ = result.Item1.Should().Be(vsr);
            _ = result.Item2.Should().BeNull();
        }

        [TestMethod]
        [DataRow(new int[5] { 1, 2, 3, 4, 5 })]
        [DataRow(new int[5] { 25, 26, 29, 22, 12 })]
        public void Deserialize_ShouldSetTheFixedLevelCountsForEachLevelGroup(int[] levelCounts)
        {
            var vsr = ServiceFactory.CreateVsr();
            vsr.AssetData = new byte[100];

            var data = this.GetValidData(vsr.AssetData.Length, levelCounts);

            using var reader = new BinaryReader(new MemoryStream(data.ToArray()));
            var serializer = new FixedLevelCount();

            var (resultVsr, _) = serializer.Deserialize(reader, (vsr, null));
            _ = resultVsr.GetFixedLevelCount(Models.LevelGroupName.Fun).Should().Be((byte)levelCounts[0]);
            _ = resultVsr.GetFixedLevelCount(Models.LevelGroupName.Tricky).Should().Be((byte)levelCounts[1]);
            _ = resultVsr.GetFixedLevelCount(Models.LevelGroupName.Taxing).Should().Be((byte)levelCounts[2]);
            _ = resultVsr.GetFixedLevelCount(Models.LevelGroupName.Mayhem).Should().Be((byte)levelCounts[3]);
            _ = resultVsr.GetFixedLevelCount(Models.LevelGroupName.Network).Should().Be((byte)levelCounts[4]);
        }

        [TestMethod]
        public void Deserialize_ShouldRestoreTheReaderPositionToTheOriginalPosition()
        {
            var vsr = ServiceFactory.CreateVsr();
            vsr.AssetData = new byte[100];

            int[] levelCounts = new int[5] { 1, 2, 3, 4, 5 };
            var data = this.GetValidData(vsr.AssetData.Length, levelCounts);

            using var reader = new BinaryReader(new MemoryStream(data.ToArray()));
            var serializer = new FixedLevelCount();

            var originalPosition = reader.BaseStream.Position;
            _ = serializer.Deserialize(reader, (vsr, null));

            reader.BaseStream.Position.Should().Be(originalPosition);
        }

        [TestMethod]
        [DataRow(LevelGroupName.Fun)]
        [DataRow(LevelGroupName.Tricky)]
        [DataRow(LevelGroupName.Taxing)]
        [DataRow(LevelGroupName.Mayhem)]
        [DataRow(LevelGroupName.Network)]
        public void Deserialize_ShouldThrowAnException_WhenADirectoryDoesNotContainAValidHeader(LevelGroupName invalidLevelGroup)
        {
            var vsr = ServiceFactory.CreateVsr();
            vsr.AssetData = new byte[100];

            var data = new List<byte>();
            data.AddRange(vsr.AssetData);

            foreach (var levelGroup in Enum.GetValues(typeof(LevelGroupName)).Cast<LevelGroupName>())
            {
                var header = levelGroup == invalidLevelGroup ? "XXXX" : "CRID"; // invalid header for the specified level group
                data.AddRange(Encoding.ASCII.GetBytes(header));
                data.AddRange(BitConverter.GetBytes(100));
                data.AddRange(BitConverter.GetBytes(1));
                data.AddRange(new byte[100 - 4]); // blank data
            }

            using var reader = new BinaryReader(new MemoryStream(data.ToArray()));

            var serializer = new FixedLevelCount();

            Action act = () => serializer.Deserialize(reader, (vsr, null));
            _ = act.Should().Throw<InvalidDataException>().WithMessage($"Invalid header 'XXXX' for level group '{invalidLevelGroup}'");
        }

        [TestMethod]
        [DataRow(LevelGroupName.Fun)]
        [DataRow(LevelGroupName.Tricky)]
        [DataRow(LevelGroupName.Taxing)]
        [DataRow(LevelGroupName.Mayhem)]
        [DataRow(LevelGroupName.Network)]
        public void Deserialize_ShouldThrowAnException_WhenADirectoryContainsMoreThan29Levels(LevelGroupName invalidLevelGroup)
        {
            var vsr = ServiceFactory.CreateVsr();
            vsr.AssetData = new byte[100];

            var data = new List<byte>();
            data.AddRange(vsr.AssetData);

            foreach (var levelGroup in Enum.GetValues(typeof(LevelGroupName)).Cast<LevelGroupName>())
            {
                var levelCount = levelGroup == invalidLevelGroup ? 30 : 1; // invalid level count for the specified level group

                data.AddRange(Encoding.ASCII.GetBytes("CRID"));
                data.AddRange(BitConverter.GetBytes(100));
                data.AddRange(BitConverter.GetBytes(levelCount));
                data.AddRange(new byte[100 - 4]); // blank data
            }

            using var reader = new BinaryReader(new MemoryStream(data.ToArray()));

            var serializer = new FixedLevelCount();

            Action act = () => serializer.Deserialize(reader, (vsr, null));
            _ = act.Should().Throw<InvalidDataException>().WithMessage($"Invalid level count '30' for level group '{invalidLevelGroup}'. Maximum allowed is 29.");
        }

        [TestMethod]
        public void Serialize_ShouldNotActuallyWriteAnything()
        {
            var vsr = ServiceFactory.CreateVsr();
            var serializer = new FixedLevelCount();

            var originalData = Enumerable.Repeat((byte)1, 100).ToArray();

            using var stream = new MemoryStream(originalData);
            using var writer = new BinaryWriter(stream);

            serializer.Serialize((vsr, null), writer);

            _ = stream.ToArray().Should().BeEquivalentTo(originalData);
        }
    }
}
