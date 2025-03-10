using FluentAssertions;
using LemballEditor.Models;
using LemballEditor.Serializers.Vsr.VsrDirectory;

namespace LemballEditor.Tests.SerializerTests.VsrTests
{
    [TestClass]
    public class DirectorySizeTests
    {
        [TestMethod]
        public void Deserialize_ShouldSkipTheDirectorySize()
        {
            using var stream = new MemoryStream(new byte[10]);
            using var reader = new BinaryReader(stream);

            var directory = new VsrDirectory();

            var serializer = new DirectorySize();
            _ = serializer.Deserialize(reader, directory);

            _ = stream.Position.Should().Be(4);
        }

        [TestMethod]
        public void Deserialize_ShouldReturnTheGivenModel()
        {
            using var stream = new MemoryStream(new byte[10]);
            using var reader = new BinaryReader(stream);

            var directory = new VsrDirectory();

            var serializer = new DirectorySize();
            _ = serializer.Deserialize(reader, directory).Should().Be(directory);
        }
    }
}
