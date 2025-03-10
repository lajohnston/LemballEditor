using FluentAssertions;
using LemballEditor.Models;
using LemballEditor.Serializers.Vsr.VsrDirectory;

namespace LemballEditor.Tests.SerializerTests.VsrTests
{
    [TestClass]
    public class VsrDirectorySerializerTests
    {
        [TestMethod]
        public void Deserialize_ShouldThrowAnInvalidDataException_WhenTheDataDoesNotBeginWithValidHeader()
        {
            byte[] data = [1, 2, 3];

            using var stream = new MemoryStream(data.ToArray());
            using var reader = new BinaryReader(stream);

            var directory = new VsrDirectory();
            var serializer = new VsrDirectorySerializer();

            var act = () => serializer.Deserialize(reader, directory);
            _ = act.Should().Throw<InvalidDataException>().WithMessage("Invalid directory header");
        }
    }
}
