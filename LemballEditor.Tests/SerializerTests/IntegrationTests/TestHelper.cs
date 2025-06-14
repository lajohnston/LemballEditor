using System.Runtime.CompilerServices;
using System.Text;

namespace LemballEditor.Tests.SerializerTests.IntegrationTests
{
    public class TestHelper
    {
        public static string GetFixturePath([CallerFilePath] string? callerFilePath = null)
        {
            // Get the directory of the current source file
            var projectDir = Directory.GetParent(Directory.GetParent(Path.GetDirectoryName(callerFilePath)).ToString());

            return Path.Combine(projectDir.ToString(), "Fixtures");
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
        private static IEnumerable<long> FindBytes(BinaryReader reader, byte[] searchBytes)
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
        public static IEnumerable<(byte[], long, int)> GetBlocks(BinaryReader reader, string header)
        {
            var searchBytes = Encoding.ASCII.GetBytes(header);
            var startPosition = reader.BaseStream.Position;

            var index = 0;
            foreach (var position in FindBytes(reader, searchBytes))
            {
                reader.BaseStream.Position = position + 4; // point to size
                var size = reader.ReadInt32();
                var paddedSize = (size + 3) / 4 * 4;

                reader.BaseStream.Position = position;      // point to start of block
                var data = reader.ReadBytes(paddedSize);    // read the block data
                reader.BaseStream.Position = position;      // restore previous position

                yield return (data, position, index);
                index++;
            }

            reader.BaseStream.Position = startPosition;
        }

        public static IEnumerable<(byte[], long, int)> GetLevelBlocks(BinaryReader reader)
        {
            var searchBytes = Encoding.ASCII.GetBytes("  IA");
            var startPosition = reader.BaseStream.Position;

            var index = 0;
            foreach (var position in FindBytes(reader, searchBytes))
            {
                reader.BaseStream.Position = position - 4;  // point to size
                var size = reader.ReadInt32();
                var data = reader.ReadBytes(size);          // read the block data
                reader.BaseStream.Position = position;      // restore previous position

                yield return (data, position, index);
                index++;
            }

            reader.BaseStream.Position = startPosition;
        }
    }
}
