using FluentAssertions;
using LemballEditor.Serializers.LevelDirectory;

namespace LemballEditor.Tests.SerializerTests.LevelDirectoryTests
{
    [TestClass]
    public class AddressToFileInfoListTests
    {
        [TestMethod]
        public void Deserialize_ShouldThrowAnInvalidDataExceptionIfTheValueIsNotValid()
        {
            var serializer = new AddressToFileInfoList();
            var directory = new LevelDirectory()
            {
                Address = 20000,
                FixedLevelCount = 5
            };

            var expected = directory.Address + 20 + (uint)(directory.FixedLevelCount * 12);
            var invalidValue = expected + 1;

            var data = BitConverter.GetBytes(invalidValue);

            using var stream = new MemoryStream(data);
            using var reader = new BinaryReader(stream);

            var act = () => serializer.Deserialize(reader, directory);

            _ = act.Should().Throw<InvalidDataException>().WithMessage($"File descriptor address invalid. Expected {expected} not {invalidValue}");
        }

        [TestMethod]
        public void Deserialize_ShouldReadPastThePointer_WhenTheValidIsValid()
        {
            var serializer = new AddressToFileInfoList();
            var directory = new LevelDirectory()
            {
                Address = 20000,
                FixedLevelCount = 5
            };

            var address = directory.Address + 20 + (uint)(directory.FixedLevelCount * 12);
            var data = BitConverter.GetBytes(address);

            using var stream = new MemoryStream(data);
            using var reader = new BinaryReader(stream);

            _ = serializer.Deserialize(reader, directory);
            _ = stream.Position.Should().Be(4);
        }

        [TestMethod]
        public void Deserialize_ShouldReturnTheDirectory_WhenTheValueIsValid()
        {
            var serializer = new AddressToFileInfoList();
            var directory = new LevelDirectory()
            {
                Address = 50000,
                FixedLevelCount = 4
            };

            var address = directory.Address + 20 + (uint)(directory.FixedLevelCount * 12);
            var data = BitConverter.GetBytes(address);

            using var stream = new MemoryStream(data);
            using var reader = new BinaryReader(stream);

            _ = serializer.Deserialize(reader, directory).Should().Be(directory);
        }

        [TestMethod]
        public void Serialize_ShouldWriteTheAddressBasedOnTheLevelCountAndBaseAddress()
        {
            var serializer = new AddressToFileInfoList();
            var directory = new LevelDirectory
            {
                Address = 1000,
                FixedLevelCount = 10
            };

            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            serializer.Serialize(directory, writer);
            _ = BitConverter.ToUInt32(stream.ToArray(), 0).Should().Be(directory.Address + 20 + (uint)(directory.FixedLevelCount * 12));
        }
    }
}
