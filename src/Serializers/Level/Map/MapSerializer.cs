using LemballEditor.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LemballEditor.Serializers.Level.Map
{
    public class MapSerializer : ISerializer<ILevel>
    {
        internal static readonly int HEADER_SIZE = 12;

        /// <summary>
        /// An ordered list of sub-serializers to call
        /// </summary>
        private readonly List<ISerializer<IMap>> propertySerializers;

        public MapSerializer(Func<ushort, ushort, IMap> mapFactory, Func<uint, byte, ITile> tileFactory)
        {
            propertySerializers = new List<ISerializer<IMap>>()
            {
                new Constant<IMap>(Encoding.ASCII.GetBytes("FSDG"), "map header"),
                new Size(mapFactory),
                new TileMap(tileFactory)
            };
        }

        /// <summary>
        /// Reads the map data from the given stream and applies to the given level if it's valid
        /// </summary>
        /// <param name="reader">The reader to read the byte data</param>
        /// <param name="level">The level to load the data to</param>
        /// <exception cref="InvalidDataException">Thrown when the data is not valid</exception>
        public ILevel Deserialize(BinaryReader reader, ILevel level)
        {
            IMap map = null;

            foreach (var serializer in propertySerializers)
            {
                map = serializer.Deserialize(reader, map);
            }

            level.Map = map;

            return level;
        }

        /// <summary>
        /// Converts the given level to a Lemmings Paintball compatible binary format
        /// </summary>
        /// <param name="level">The level to serialize</param>
        /// <param name="writer">The writer to write the bytes to</param>
        public void Serialize(ILevel level, BinaryWriter writer)
        {
            var map = level.Map;

            foreach (var serializer in propertySerializers)
            {
                serializer.Serialize(map, writer);
            }
        }
    }
}
