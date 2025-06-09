using System.Runtime.CompilerServices;
using System.Text;
using FluentAssertions;

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

        public static Stream GetVsrStream()
        {
            var fixturePath = GetFixturePath();
            var vsrPath = Path.Combine(fixturePath, "PBAIMOG.VSR");

            if (!File.Exists(vsrPath))
            {
                Assert.Inconclusive($"VSR fixture not found in path {vsrPath}");
            }

            return File.OpenRead(vsrPath);
        }

        /// <summary>
        /// Iterates through the binary reader to find all occurrences of the given byte sequence. Search bytes must have a length of 4 bytes.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        private IEnumerable<long> FindBytes(BinaryReader reader, byte[] searchBytes)
        {
            var alignment = 4;

            if (searchBytes.Length != alignment)
            {
                throw new ArgumentException($"Search string must have a length of {alignment}");
            }

            var startPosition = reader.BaseStream.Position;

            while (true)
            {
                var chunk = reader.ReadBytes(alignment);

                if (chunk.Length < alignment)
                {
                    break; // End of stream
                }

                if (chunk.SequenceEqual(searchBytes))
                {
                    var position = reader.BaseStream.Position;
                    yield return position - alignment;
                    reader.BaseStream.Position = position;
                }
            }

            reader.BaseStream.Position = startPosition;
        }

        /// <summary>
        /// Iterates through each data block in the binary reader that starts with the given header. The header must be 4 bytes long.
        /// </summary>
        private IEnumerable<(byte[], long)> GetBlocks(BinaryReader reader, string header)
        {
            var searchBytes = Encoding.ASCII.GetBytes(header);
            var startPosition = reader.BaseStream.Position;

            foreach (var position in this.FindBytes(reader, searchBytes))
            {
                reader.BaseStream.Position = position + 4; // point to size
                var size = reader.ReadInt32();
                var paddedSize = (size + 3) / 4 * 4;

                reader.BaseStream.Position = position;      // point to start of block
                var data = reader.ReadBytes(paddedSize);    // read the block data
                reader.BaseStream.Position = position;      // restore previous position

                yield return (data, position);
            }

            reader.BaseStream.Position = startPosition;
        }

        [TestMethod]
        public void ItShouldSerializeAndDeserializeAllBomgBlocksInTheStockVsr()
        {
            Assert.Inconclusive("This behaviour is not yet implemented");

            using var sourceVsrStream = GetVsrStream();
            using var sourceReader = new BinaryReader(sourceVsrStream);

            var serializer = ServiceFactory.CreateBomgBlockSerializer();

            foreach (var (blockData, position) in this.GetBlocks(sourceReader, "BOMG"))
            {
                using var blockReader = new BinaryReader(new MemoryStream(blockData));

                var deserializeAct = () => serializer.Deserialize(blockReader, ServiceFactory.CreateLevel(1, 1));
                var deserializedLevel = deserializeAct.Should().NotThrow($"BOMG block at position {position} should be deserialized").Subject;

                using var blockWriter = new BinaryWriter(new MemoryStream());
                serializer.Serialize(deserializedLevel, blockWriter);

                blockWriter.BaseStream.Position = 0;
                var serializedBlock = ((MemoryStream)blockWriter.BaseStream).ToArray();

                _ = serializedBlock.Should().BeEquivalentTo(blockData, $"BOMG data at position {position} should match the original data");
            }
        }

        [TestMethod]
        public void ItShouldDeserializeAndSerializeTheStockVsr()
        {
            Assert.Inconclusive("This test is not yet implemented");

            using var sourceVsrStream = GetVsrStream();
            using var sourceReader = new BinaryReader(sourceVsrStream);

            var serializer = ServiceFactory.CreateVsrLevelPackSerializer();
            var (deserializedVsr, deserializedLevelPack) = serializer.Deserialize(sourceReader, (new Models.Vsr(), new Models.LevelPack()));

            using var writer = new BinaryWriter(new MemoryStream());

            serializer.Serialize((deserializedVsr, deserializedLevelPack), writer);

            writer.BaseStream.Position = 0;
            var debugOutput = File.OpenWrite(Path.Combine(GetFixturePath(), "debug.vsr"));
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
