using LemballEditor.Models;
using System.Collections.Generic;
using System.IO;

namespace LemballEditor.Serializers.Level
{
    /// <summary>
    /// Converts to and from Lemmings Paintball level data and a Level instance
    /// </summary>
    public class LevelSerializer : ILevelSerializer
    {
        private readonly List<ILevelSerializer> propertySerializers;

        public LevelSerializer()
        {
            propertySerializers = new List<ILevelSerializer>()
            {
                new Header(),
                new UnknownA(),
                new Theme(),
                new TimeLimit(),
            };
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

        /// <summary>
        /// Reads the given stream for Lemmings Paintball level data and sets the data to the given level
        /// </summary>
        /// <param name="level">The level to load the data to</param>
        /// <param name="reader">The reader to read the byte data</param>
        /// <exception cref="InvalidDataException">Thrown when the data is not valid</exception>
        public void Deserialize(ILevel level, BinaryReader reader)
        {
            foreach (var serializer in propertySerializers)
            {
                serializer.Deserialize(level, reader);
            }
        }
    }
}
