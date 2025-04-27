using FluentAssertions;
using LemballEditor.Serializers;
using System.Text;

namespace LemballEditor.Tests.SerializerTests
{
    [TestClass]
    public sealed class ConstantTests
    {
        [TestMethod]
        public void Deserialize_ShouldThrowAnInvalidDataException_WhenTheReadDataDoesNotMatchTheConstantData()
        {
            var validData = Encoding.UTF8.GetBytes("VALID HEADER");
            var invalidData = Encoding.UTF8.GetBytes("INVALID HEADER");

            using var stream = new MemoryStream(invalidData);
            using var reader = new BinaryReader(stream);

            var act = () => new Constant<bool>(validData).Deserialize(reader, true);
            _ = act.Should().Throw<InvalidDataException>().WithMessage("Invalid value");
        }

        [TestMethod]
        public void Serialize_ShouldWriteTheConstantDataBytesToTheStream()
        {
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            var validHeader = Encoding.UTF8.GetBytes("VALID HEADER");

            new Constant<bool>(validHeader).Serialize(true, writer);

            _ = stream.ToArray().Should().BeEquivalentTo(validHeader);
        }
    }
}
