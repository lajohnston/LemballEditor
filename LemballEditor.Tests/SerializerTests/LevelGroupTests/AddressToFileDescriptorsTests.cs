using FluentAssertions;
using LemballEditor.Serializers.LevelDirectory;

namespace LemballEditor.Tests.SerializerTests.LevelGroupTests
{
    [TestClass]
    public class AddressToFileDescriptorsTests
    {
        [TestMethod]
        public void Deserialize_ShouldThrowAnInvalidDataExceptionIfTheValueIsNotValid()
        {
            var serializer = new AddressToFileDescriptors();
            var directory = new LevelDirectory()
            {
                DirectoryAddress = 20000,
                FixedLevelCount = 5
            };

            var expected = directory.DirectoryAddress + 16 + (uint)(directory.FixedLevelCount * 12);
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
            var serializer = new AddressToFileDescriptors();
            var directory = new LevelDirectory()
            {
                DirectoryAddress = 20000,
                FixedLevelCount = 5
            };

            var address = directory.DirectoryAddress + 16 + (uint)(directory.FixedLevelCount * 12);
            var data = BitConverter.GetBytes(address);

            using var stream = new MemoryStream(data);
            using var reader = new BinaryReader(stream);

            _ = serializer.Deserialize(reader, directory);
            _ = stream.Position.Should().Be(4);
        }

        [TestMethod]
        public void Deserialize_ShouldReturnTheDirectory_WhenTheValueIsValid()
        {
            var serializer = new AddressToFileDescriptors();
            var directory = new LevelDirectory()
            {
                DirectoryAddress = 50000,
                FixedLevelCount = 4
            };

            var address = directory.DirectoryAddress + 16 + (uint)(directory.FixedLevelCount * 12);
            var data = BitConverter.GetBytes(address);

            using var stream = new MemoryStream(data);
            using var reader = new BinaryReader(stream);

            _ = serializer.Deserialize(reader, directory).Should().Be(directory);
        }

        [TestMethod]
        public void Serialize_ShouldWriteTheAddressBasedOnTheLevelCountAndBaseAddress()
        {
            var serializer = new AddressToFileDescriptors();
            var directory = new LevelDirectory
            {
                DirectoryAddress = 1000,
                FixedLevelCount = 10
            };

            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            serializer.Serialize(directory, writer);
            _ = BitConverter.ToUInt32(stream.ToArray(), 0).Should().Be(directory.DirectoryAddress + 16 + (uint)(directory.FixedLevelCount * 12));
        }
    }
}
