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
        private static readonly Action<ILevel, BinaryReader, BinaryWriter>[] DATA_FORMAT = new Action<ILevel, BinaryReader, BinaryWriter>[]
        {
            ProcessHeader,
            ProcessUnknownA,
            ProcessTheme
        };

        /// <summary>
        /// Ensures the given header is writen to, or can be read from, the given reader/writer
        /// </summary>
        /// <param name="level">The level to serialize or deserialize</param>
        /// <param name="reader">If given, the header will be read from the stream and asserted</param>
        /// <param name="writer">If given, the header will be written to the stream</param>
        /// <exception cref="InvalidDataException">If a reader is given and the data stream doesn't contain a valid header</exception>
        private static void ProcessHeader(ILevel level, BinaryReader reader = null, BinaryWriter writer = null)
        {
            // '  IA' followed by 18, 0, 0, 0
            byte[] header = { 0x20, 0x20, 0x49, 0x41, 0x12, 0x0, 0x0, 0x0 };

            if (reader != null)
            {
                var headerBytes = reader.ReadBytes(header.Length);

                if (!headerBytes.SequenceEqual(header))
                {
                    throw new InvalidDataException("Invalid header value");
                }
            }
            else
            {
                writer?.Write(header);
            }
        }

        /// <summary>
        /// Reads/Writes the UnknownA value
        /// </summary>
        /// <param name="level">The level to serialize/deserialize</param>
        /// <param name="reader">If given, will read the value and set it to the level</param>
        /// <param name="writer">If given, will write the value to the stream</param>
        private static void ProcessUnknownA(ILevel level, BinaryReader reader = null, BinaryWriter writer = null)
        {
            if (reader != null)
            {
                var value = reader.ReadUInt16();
                try
                {
                    level.UnknownA = value;
                }
                catch (ArgumentException error)
                {
                    throw new InvalidDataException(error.Message);
                }
            }
            else
            {
                writer?.Write(level.UnknownA);
            }
        }

        /// <summary>
        /// Reads/Writes the Theme value
        /// </summary>
        /// <param name="level">The level to serialize/deserialize</param>
        /// <param name="reader">If given, will read the value and set it to the level</param>
        /// <param name="writer">If given, will write the value to the stream</param>
        private static void ProcessTheme(ILevel level, BinaryReader reader = null, BinaryWriter writer = null)
        {
            if (reader != null)
            {
                var value = reader.ReadUInt16();

                switch (value)
                {
                    case 0:
                        level.Theme = LevelTheme.Grass;
                        break;
                    case 1:
                        level.Theme = LevelTheme.Lego;
                        break;
                    case 2:
                        level.Theme = LevelTheme.Snow;
                        break;
                    case 3:
                        level.Theme = LevelTheme.Space;
                        break;
                    default:
                        throw new InvalidDataException($"Theme value should be between 0-3, {value} given");
                }
            }
            else
            {
                switch (level.Theme)
                {
                    case LevelTheme.Grass:
                        writer?.Write(0);
                        break;
                    case LevelTheme.Lego:
                        writer?.Write(1);
                        break;
                    case LevelTheme.Snow:
                        writer?.Write(2);
                        break;
                    case LevelTheme.Space:
                        writer?.Write(3);
                        break;
                }
            }
        }

        /// <summary>
        /// Converts the given level to a Lemmings Paintball compatible binary format
        /// </summary>
        /// <param name="level">The level to serialize</param>
        /// <param name="writer">The writer to write the bytes to</param>
        public static void Serialize(ILevel level, BinaryWriter writer)
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
        public static void Deserialize(ILevel level, BinaryReader reader)
        {
            foreach (var func in DATA_FORMAT)
            {
                func(level, reader, null);
            }
        }
    }
}
