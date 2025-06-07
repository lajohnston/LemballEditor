using LemballEditor.Models;
using System;
using System.IO;

namespace LemballEditor.Serializers.Level.Map
{
    public class Size : ISerializer<IMap>
    {
        internal static readonly int HEADER_SIZE = 12;
        private static readonly int TILE_SIZE_BYTES = 6;

        private readonly Func<ushort, ushort, IMap> mapFactory;

        public Size(Func<ushort, ushort, IMap> mapFactory)
        {
            this.mapFactory = mapFactory;
        }

        public IMap Deserialize(BinaryReader reader, IMap map = null)
        {
            if (map != null)
            {
                throw new NotImplementedException("Map Size deserializer cannot be passed an existing Map instance as it creates its own");
            }

            // Map size
            var tileDataSize = reader.ReadUInt32();
            var xSize = reader.ReadUInt16();
            var ySize = reader.ReadUInt16();

            var expectedSize = (uint)((xSize * ySize * TILE_SIZE_BYTES) + HEADER_SIZE);
            if (tileDataSize != expectedSize)
            {
                throw new InvalidDataException($"Invalid map data size: {tileDataSize} given for a {xSize}x{ySize} map");
            }

            var newMap = mapFactory(xSize, ySize);

            return newMap;
        }

        public void Serialize(IMap map, BinaryWriter writer)
        {
            writer.Write((uint)((map.TileCount * TILE_SIZE_BYTES) + HEADER_SIZE));
            writer.Write(map.XTiles);
            writer.Write(map.YTiles);
        }
    }
}
