using FluentAssertions;
using LemballEditor.Serializers.LevelGroup;
using System.Text;

namespace LemballEditor.Tests.SerializerTests.LevelGroupTests
{
    [TestClass]
    public class FileNameListTests
    {
        [TestMethod]
        public void Deserialize_ShouldThrowAnException_WhenTheLevelsNamesAreNotValid()
        {
            byte numberOfLevels = 2;
            var invalidName = "Lev_01";

            var data = new List<byte>();
            data.AddRange(Encoding.ASCII.GetBytes("Level_00"));
            data.AddRange(new byte[4]); // padding

            data.AddRange(Encoding.ASCII.GetBytes(invalidName));
            data.AddRange(new byte[4]); // padding

            using var stream = new MemoryStream(data.ToArray());
            using var reader = new BinaryReader(stream);

            var group = new LevelGroupContext
            {
                LevelCount = numberOfLevels
            };

            var serializer = new FileNameList();
            var act = () => serializer.Deserialize(reader, group);

            _ = act.Should().Throw<InvalidDataException>().WithMessage("Invalid file name");
        }

        [TestMethod]
        public void Deserialize_ShouldThrowAnException_WhenTheLevelsNamesAreNotFollowedByEightZeros()
        {
            byte numberOfLevels = 2;

            var data = new List<byte>();
            data.AddRange(Encoding.ASCII.GetBytes("Level_00"));
            data.AddRange(new byte[4]); // padding

            data.AddRange(Encoding.ASCII.GetBytes("Level_01"));
            data.AddRange(new byte[] { 0, 0, 1, 0 }); // invalid padding

            using var stream = new MemoryStream(data.ToArray());
            using var reader = new BinaryReader(stream);

            var group = new LevelGroupContext
            {
                LevelCount = numberOfLevels
            };

            var serializer = new FileNameList();
            var act = () => serializer.Deserialize(reader, group);

            _ = act.Should().Throw<InvalidDataException>().WithMessage("Invalid data found after file name");
        }

        [TestMethod]
        public void Deserialize_ShouldThrowAnException_WhenTheFirstLevelIsNotLevel00()
        {
            byte numberOfLevels = 1;

            var data = new List<byte>();
            data.AddRange(Encoding.ASCII.GetBytes("Level_01"));
            data.AddRange(new byte[4]); // padding

            using var stream = new MemoryStream(data.ToArray());
            using var reader = new BinaryReader(stream);

            var group = new LevelGroupContext
            {
                LevelCount = numberOfLevels
            };

            var serializer = new FileNameList();
            var act = () => serializer.Deserialize(reader, group);

            _ = act.Should().Throw<InvalidDataException>().WithMessage("Invalid file name");
        }

        [TestMethod]
        public void Deserialize_ShouldThrowAnException_WhenTheLevelNamesArNotSequential()
        {
            byte numberOfLevels = 2;

            var data = new List<byte>();
            data.AddRange(Encoding.ASCII.GetBytes("Level_00"));
            data.AddRange(new byte[4]); // padding

            data.AddRange(Encoding.ASCII.GetBytes("Level_02"));
            data.AddRange(new byte[4]); // padding

            using var stream = new MemoryStream(data.ToArray());
            using var reader = new BinaryReader(stream);

            var group = new LevelGroupContext
            {
                LevelCount = numberOfLevels
            };

            var serializer = new FileNameList();
            var act = () => serializer.Deserialize(reader, group);

            _ = act.Should().Throw<InvalidDataException>().WithMessage("Invalid file name");
        }

        [TestMethod]
        public void Deserialize_ShouldSkipTheFileNameBytesAndReturnTheGivenModel_WhenTheDataIsValid()
        {
            byte numberOfLevels = 2;

            var data = new List<byte>();
            data.AddRange(Encoding.ASCII.GetBytes("Level_00"));
            data.AddRange(new byte[4]); // padding

            data.AddRange(Encoding.ASCII.GetBytes("Level_01"));
            data.AddRange(new byte[4]); // padding

            using var stream = new MemoryStream(data.ToArray());
            using var reader = new BinaryReader(stream);

            var group = new LevelGroupContext
            {
                LevelCount = numberOfLevels
            };

            var serializer = new FileNameList();
            _ = serializer.Deserialize(reader, group).Should().Be(group);

            _ = stream.Position.Should().Be(numberOfLevels * 12);
        }

        [TestMethod]
        public void Serialize_WriteTheSequentialFilesNameWithFourBytesOfPadding()
        {
            byte numberOfLevels = 11;

            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);
            using var reader = new BinaryReader(stream);

            var group = new LevelGroupContext
            {
                LevelCount = numberOfLevels
            };

            var expected = new List<byte>();
            expected.AddRange(Encoding.ASCII.GetBytes("Level_00"));
            expected.AddRange(new byte[4]);
            expected.AddRange(Encoding.ASCII.GetBytes("Level_01"));
            expected.AddRange(new byte[4]);
            expected.AddRange(Encoding.ASCII.GetBytes("Level_02"));
            expected.AddRange(new byte[4]);
            expected.AddRange(Encoding.ASCII.GetBytes("Level_03"));
            expected.AddRange(new byte[4]);
            expected.AddRange(Encoding.ASCII.GetBytes("Level_04"));
            expected.AddRange(new byte[4]);
            expected.AddRange(Encoding.ASCII.GetBytes("Level_05"));
            expected.AddRange(new byte[4]);
            expected.AddRange(Encoding.ASCII.GetBytes("Level_06"));
            expected.AddRange(new byte[4]);
            expected.AddRange(Encoding.ASCII.GetBytes("Level_07"));
            expected.AddRange(new byte[4]);
            expected.AddRange(Encoding.ASCII.GetBytes("Level_08"));
            expected.AddRange(new byte[4]);
            expected.AddRange(Encoding.ASCII.GetBytes("Level_09"));
            expected.AddRange(new byte[4]);
            expected.AddRange(Encoding.ASCII.GetBytes("Level_10"));
            expected.AddRange(new byte[4]);

            var serializer = new FileNameList();
            serializer.Serialize(group, writer);

            _ = stream.ToArray().Should().BeEquivalentTo(expected.ToArray());
        }
    }
}
