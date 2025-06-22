using FluentAssertions;

namespace LemballEditor.Tests.SerializerTests.IntegrationTests
{
    [TestClass]
    public class VsrSerializerIntegrationTests
    {
        [TestMethod]
        public void ItShouldDeserializeAndSerializeTheStockVsrLevels()
        {
            Assert.Inconclusive("This behaviour is not yet implemented");

            using var sourceVsrStream = TestHelper.GetVsrStream();
            using var sourceReader = new BinaryReader(sourceVsrStream);

            var serializer = ServiceFactory.CreateLevelSerializer();

            foreach (var (levelBlockData, position, index) in TestHelper.GetLevelBlocks(sourceReader))
            {
                using var blockReader = new BinaryReader(new MemoryStream(levelBlockData));

                var deserializeAct = () => serializer.Deserialize(blockReader, ServiceFactory.CreateLevel(1, 1));
                var deserializedLevel = deserializeAct.Should().NotThrow($"Level block {index} at position {position} should be deserialized").Subject;

                using var blockWriter = new BinaryWriter(new MemoryStream());
                serializer.Serialize(deserializedLevel, blockWriter);

                blockWriter.BaseStream.Position = 0;
                var serializedBlock = ((MemoryStream)blockWriter.BaseStream).ToArray();

                _ = serializedBlock.Length.Should().Be(
                    levelBlockData.Length,
                    $"Level block {index} at position {position} should match the original size"
                );

                _ = serializedBlock.Should().BeEquivalentTo(
                    levelBlockData,
                    $"Level block {index} at position {position} should match the original data"
                );
            }
        }

        [TestMethod]
        public void ItShouldDeserializeAndSerializeTheStockVsr()
        {
            Assert.Inconclusive("This test is not yet implemented");

            using var sourceVsrStream = TestHelper.GetVsrStream();
            using var sourceReader = new BinaryReader(sourceVsrStream);

            var serializer = ServiceFactory.CreateVsrLevelPackSerializer();
            var (deserializedVsr, deserializedLevelPack) = serializer.Deserialize(sourceReader, (new Models.Vsr(), new Models.LevelPack()));

            using var writer = new BinaryWriter(new MemoryStream());

            serializer.Serialize((deserializedVsr, deserializedLevelPack), writer);

            writer.BaseStream.Position = 0;
            var debugOutput = File.OpenWrite(Path.Combine(TestHelper.GetFixturePath(), "debug.vsr"));
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
