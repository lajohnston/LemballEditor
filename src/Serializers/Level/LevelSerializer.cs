using LemballEditor.Models;
using System.Collections.Generic;
using System.IO;

namespace LemballEditor.Serializers.Level
{
    /// <summary>
    /// Converts to and from Lemmings Paintball level data and a Level instance
    /// </summary>
    public class LevelSerializer : ISerializer<ILevel>
    {
        // Header constant; '  IA' followed by 18, 0, 0, 0
        private static readonly byte[] LEVEL_HEADER = { 0x20, 0x20, 0x49, 0x41, 0x12, 0x0, 0x0, 0x0 };

        /// <summary>
        /// An ordered list of sub-serializes to call
        /// </summary>
        private readonly List<ISerializer<ILevel>> propertySerializers;

        /// <summary>
        /// Creates a level serializer
        /// </summary>
        public LevelSerializer()
        {
            propertySerializers = new List<ISerializer<ILevel>>()
            {
                new Constant<ILevel>(LEVEL_HEADER),
                new UnknownA(),
                new Theme(),
                new TimeLimit(),
                new UnusedNumberOfLemmings(),
                new FlagsRequiredIndicator(),
                new UnknownB(),
            };
        }

        /// <summary>
        /// Reads the given stream for Lemmings Paintball level data and sets the data to the given level
        /// </summary>
        /// <param name="reader">The reader to read the byte data</param>
        /// <param name="level">The level to load the data to</param>
        /// <exception cref="InvalidDataException">Thrown when the data is not valid</exception>
        public ILevel Deserialize(BinaryReader reader, ILevel level = null)
        {
            foreach (var serializer in propertySerializers)
            {
                _ = serializer.Deserialize(reader, level);
            }

            return level;
        }

        /// <summary>
        /// Converts the given level to a Lemmings Paintball compatible binary format
        /// </summary>
        /// <param name="level">The level to serialize</param>
        /// <param name="writer">The writer to write the bytes to</param>
        public void Serialize(ILevel level, BinaryWriter writer)
        {
            foreach (var serializer in propertySerializers)
            {
                serializer.Serialize(level, writer);
            }
        }
    }
}
