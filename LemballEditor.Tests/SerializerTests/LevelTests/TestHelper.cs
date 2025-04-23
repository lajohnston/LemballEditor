using FluentAssertions;
using LemballEditor.Models;
using LemballEditor.Serializers;

namespace LemballEditor.Tests.SerializerTests.LevelTests
{
    internal static class TestHelper
    {
        public static void AssertWrite(ILevel level, ISerializer<ILevel> serializer, ushort expectedValue)
        {
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            serializer.Serialize(level, writer);

            _ = stream.ToArray().Should().BeEquivalentTo(BitConverter.GetBytes(expectedValue));
        }
    }
}
