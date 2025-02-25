using FluentAssertions;
using LemballEditor.Models;
using LemballEditor.Serializers.Level;
using System.Text;

namespace LemballEditor.Tests.SerializerTests.LevelTests
{
    [TestClass]
    public sealed class ConstantTests
    {
        private static readonly IModelFactory modelFactory = new ModelFactory();

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
        public void Serialize_ShouldWriteTheHeaderBytesToTheStream()
        {
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            var validHeader = Encoding.UTF8.GetBytes("VALID HEADER");

            new Constant<bool>(validHeader).Serialize(true, writer);

            _ = stream.ToArray().Should().BeEquivalentTo(validHeader);
        }
    }
}
