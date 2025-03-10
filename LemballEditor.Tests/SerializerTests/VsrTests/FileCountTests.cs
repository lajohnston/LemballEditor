using FluentAssertions;
using LemballEditor.Models;
using LemballEditor.Serializers.Vsr.VsrDirectory;

namespace LemballEditor.Tests.SerializerTests.VsrTests
{
    [TestClass]
    public class FileCountTests
    {
        [TestMethod]
        public void Deserialize_ShouldStoreTheGivenNumberOfFilesForLater()
        {
            uint numberOfFiles = 28;
            var data = BitConverter.GetBytes(numberOfFiles);

            using var stream = new MemoryStream(data);
            using var reader = new BinaryReader(stream);

            var directory = new VsrDirectory();

            var serializer = new FileCount();
            _ = serializer.Deserialize(reader, directory);

            _ = serializer.DeserializedFileCount.Should().Be(numberOfFiles);
            _ = stream.Position.Should().Be(4);
        }

        [TestMethod]
        public void Deserialize_ShouldReturnTheGivenModel()
        {
            using var stream = new MemoryStream(new byte[10]);
            using var reader = new BinaryReader(stream);

            var directory = new VsrDirectory();

            var serializer = new FileCount();
            _ = serializer.Deserialize(reader, directory).Should().Be(directory);
        }
    }
}
