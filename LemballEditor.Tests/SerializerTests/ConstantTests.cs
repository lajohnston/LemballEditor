using System.Text;
using FluentAssertions;
using LemballEditor.Serializers;

namespace LemballEditor.Tests.SerializerTests
{
    [TestClass]
    public sealed class ConstantTests
    {
        [TestMethod]
        [DataRow("Foo", 0, "Unexpected Foo at position 0")]
        [DataRow("Foo", 10, "Unexpected Foo at position 10")]
        [DataRow("Bar", 200, "Unexpected Bar at position 200")]
        public void Deserialize_ShouldThrowAnInvalidDataException_WhenTheReadDataDoesNotMatchTheConstantData(
            string dataDescripion,
            int invalidAddress,
            string expectedMessage
        )
        {
            var validData = Encoding.UTF8.GetBytes("VALID DATA");

            var invalidData = new List<byte>();

            // Padding before invalid data
            if (invalidAddress > 0)
            {
                invalidData.AddRange(new byte[invalidAddress]);
            }

            invalidData.AddRange(Encoding.UTF8.GetBytes("INVALID DATA"));

            using var stream = new MemoryStream(invalidData.ToArray());
            using var reader = new BinaryReader(stream);

            reader.BaseStream.Position = invalidAddress;

            var fakeModel = "fakeModel";
            var act = () => new Constant<string>(validData, dataDescripion).Deserialize(reader, fakeModel);
            _ = act.Should().Throw<InvalidDataException>().WithMessage(expectedMessage);
        }

        [TestMethod]
        public void Serialize_ShouldWriteTheConstantDataBytesToTheStream()
        {
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            var validHeader = Encoding.UTF8.GetBytes("VALID HEADER");

            new Constant<bool>(validHeader, "data").Serialize(true, writer);

            _ = stream.ToArray().Should().BeEquivalentTo(validHeader);
        }
    }
}
