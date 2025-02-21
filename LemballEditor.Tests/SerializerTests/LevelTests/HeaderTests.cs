using FluentAssertions;
using LemballEditor.Models;
using LemballEditor.Serializers.Level;
using System.Text;

namespace LemballEditor.Tests.SerializerTests.LevelTests
{
    [TestClass]
    public sealed class HeaderTests
    {
        [TestMethod]
        public void Deserialize_ShouldThrowAnInvalidDataException_WhenHeaderIsNotValid()
        {
            using var stream = new MemoryStream();
            using var reader = new BinaryReader(stream);

            var invalidHeader = Encoding.UTF8.GetBytes("UNEXPECTED HEADER BYTES");

            var act = () => new Header().Deserialize(new Level(), reader);
            _ = act.Should().Throw<InvalidDataException>().WithMessage("Invalid header value");
        }

        [TestMethod]
        public void Serialize_ShouldWriteTheHeaderBytesToTheStream()
        {
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            byte[] expectedHeader = { 0x20, 0x20, 0x49, 0x41, 0x12, 0x0, 0x0, 0x0 };

            new Header().Serialize(new Level(), writer);

            _ = stream.ToArray().Should().BeEquivalentTo(expectedHeader);
        }
    }
}
