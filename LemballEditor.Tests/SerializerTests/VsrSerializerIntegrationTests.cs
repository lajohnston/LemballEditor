using FluentAssertions;
using System.Runtime.CompilerServices;

namespace LemballEditor.Tests.SerializerTests
{
    [TestClass]
    public class VsrSerializerIntegrationTests
    {
        public static string GetVsrFixturePath([CallerFilePath] string? callerFilePath = null)
        {
            // Get the directory of the current source file
            var projectDir = Directory.GetParent(Path.GetDirectoryName(callerFilePath)).ToString();

            return Path.Combine(projectDir, "Fixtures", "PBAIMOG.VSR");
        }

        [TestMethod]
        public void ItShouldDeserializeAndSerializeTheStockVsr()
        {
            Assert.Inconclusive("This test is not yet implemented");

            var vsrPath = GetVsrFixturePath();

            if (!File.Exists(vsrPath))
            {
                Assert.Inconclusive($"VSR fixture not found in path {vsrPath}");
            }

            using var sourceVsrStream = File.OpenRead(vsrPath);
            using var sourceReader = new BinaryReader(sourceVsrStream);

            var serializer = ServiceFactory.CreateVsrSerializer();
            var deserializedVsr = serializer.Deserialize(sourceReader, new Models.Vsr());

            using var writeStream = new MemoryStream();
            using var writer = new BinaryWriter(writeStream);
            serializer.Serialize(deserializedVsr, writer);

            sourceVsrStream.Position = 0;
            writeStream.Position = 0;

            _ = writeStream.Length.Should().Be(sourceVsrStream.Length, "The serialized VSR should have the same length as the original VSR");

            for (var i = 0; i < sourceVsrStream.Length; i++)
            {
                var expectedValue = sourceVsrStream.ReadByte();
                var actualValue = writeStream.ReadByte();

                _ = actualValue.Should().Be(expectedValue, $"Expected byte address {i} to be {expectedValue}, not {actualValue}");
            }
        }
    }
}
