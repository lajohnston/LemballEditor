using LemballEditor.Models;
using System;
using System.IO;
using System.Linq;

namespace LemballEditor.Serializers
{
    /// <summary>
    /// Converts to and from Lemmings Paintball level data and a Level instance
    /// </summary>
    public static class LevelSerializer
    {
        // Define a static array of Func<int, string>
        private static readonly Action<Level, BinaryReader, BinaryWriter>[] DATA_FORMAT = new Action<Level, BinaryReader, BinaryWriter>[]
        {
            EnsureHeader
        };

        /// <summary>
        /// Ensures the given header is writen to, or can be read from, the given reader/writer
        /// </summary>
        /// <param name="level">The level to serialize or deserialize</param>
        /// <param name="reader">If given, the header will be read from the stream and asserted</param>
        /// <param name="writer">If given, the header will be written to the stream</param>
        /// <exception cref="InvalidDataException">If a reader is given and the data stream doesn't contain a valid header</exception>
        private static void EnsureHeader(Level level, BinaryReader reader = null, BinaryWriter writer = null)
        {
            // '  IA' followed by 18, 0, 0, 0
            byte[] header = { 0x20, 0x20, 0x49, 0x41, 0x12, 0x0, 0x0, 0x0 };

            if (reader != null)
            {
                var headerBytes = reader.ReadBytes(header.Length);

                if (!headerBytes.SequenceEqual(header))
                {
                    throw new InvalidDataException("Invalid binary format: Invalid header value");
                }
            }
            else
            {
                writer?.Write(header);
            }
        }

        /// <summary>
        /// Converts the given level to a Lemmings Paintball compatible binary format
        /// </summary>
        /// <param name="level">The level to serialize</param>
        /// <param name="writer">The writer to write the bytes to</param>
        public static void Serialize(Level level, BinaryWriter writer)
        {
            foreach (var func in DATA_FORMAT)
            {
                func(level, null, writer);
            }
        }

        /// <summary>
        /// Reads the given stream for Lemmings Paintball level data and sets the data to the given level
        /// </summary>
        /// <param name="level">The level to load the data to</param>
        /// <param name="reader">The reader to read the byte data</param>
        /// <exception cref="InvalidDataException">Thrown when the data is not valid</exception>
        public static void Deserialize(Level level, BinaryReader reader)
        {
            foreach (var func in DATA_FORMAT)
            {
                func(level, reader, null);
            }
        }
    }
}
