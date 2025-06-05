using FluentAssertions;
using System.Runtime.CompilerServices;

namespace LemballEditor.Tests.SerializerTests
{
    [TestClass]
    public class VsrSerializerIntegrationTests
    {
        public static string GetFixturePath([CallerFilePath] string? callerFilePath = null)
        {
            // Get the directory of the current source file
            var projectDir = Directory.GetParent(Path.GetDirectoryName(callerFilePath)).ToString();

            return Path.Combine(projectDir, "Fixtures");
        }

        [TestMethod]
        public void ItShouldDeserializeAndSerializeTheStockVsr()
        {
            Assert.Inconclusive("This test is not yet implemented");

            var fixturePath = GetFixturePath();
            var vsrPath = Path.Combine(fixturePath, "PBAIMOG.VSR");

            if (!File.Exists(vsrPath))
            {
                Assert.Inconclusive($"VSR fixture not found in path {vsrPath}");
            }

            using var sourceVsrStream = File.OpenRead(vsrPath);
            using var sourceReader = new BinaryReader(sourceVsrStream);

            var serializer = ServiceFactory.CreateVsrLevelPackSerializer();
            var (deserializedVsr, _) = serializer.Deserialize(sourceReader, (new Models.Vsr(), null));

            using var writer = new BinaryWriter(new MemoryStream());

            serializer.Serialize((deserializedVsr, new Models.LevelPack()), writer);

            writer.BaseStream.Position = 0;
            var debugOutput = File.OpenWrite(Path.Combine(fixturePath, "debug.vsr"));
            writer.BaseStream.CopyTo(debugOutput);

            _ = writer.BaseStream.Length.Should().Be(sourceVsrStream.Length, "The serialized VSR should have the same length as the original VSR");

            sourceVsrStream.Position = 0;
            writer.BaseStream.Position = 0;

            for (var i = 0; i < sourceVsrStream.Length; i++)
            {
                var expectedValue = sourceVsrStream.ReadByte();
                var actualValue = writer.BaseStream.ReadByte();

                _ = actualValue.Should().Be(expectedValue, $"Expected byte address {i} to be {expectedValue}, not {actualValue}");
            }
        }
    }
}
