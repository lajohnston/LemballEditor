using LemballEditor.Models;
using System;
using System.IO;

namespace LemballEditor.Serializers.Level.Map
{
    /// <summary>
    /// Serializes/Deserializes the tile data
    /// </summary>
    public class TileMap : ISerializer<IMap>
    {
        private readonly Func<uint, byte, ITile> tileFactory;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tileFactory">Lambda to create tile instances</param>
        public TileMap(Func<uint, byte, ITile> tileFactory)
        {
            this.tileFactory = tileFactory;
        }

        /// <summary>
        /// Deserializes the next tile from the stream
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="xTile">The current xTile, used for error reporting</param>
        /// <param name="yTile">The current yTile, used for error reporting</param>
        /// <returns>ITile instance</returns>
        /// <exception cref="InvalidDataException">If any of the tile data is unexpected</exception>
        private ITile DeserializeNextTile(BinaryReader reader, ushort xTile, ushort yTile)
        {
            var tileRef = reader.ReadUInt32();
            var elevation = reader.ReadUInt16();

            if (elevation > 88)
            {
                throw new InvalidDataException($"Max elevation for tile is 88. Tile {xTile + 1}x{yTile + 1} has {elevation}");
            }

            var tile = tileFactory(tileRef, (byte)elevation);
            return tile;
        }

        /// <summary>
        /// Deserializes the tilemap data from the stream
        /// </summary>
        /// <param name="reader">Thr reader stream</param>
        /// <param name="map">The map to read data to</param>
        /// <returns>The given map, populated with the deserialized tiles</returns>
        public IMap Deserialize(BinaryReader reader, IMap map)
        {
            for (byte yTile = 0; yTile < map.YTiles; yTile++)
            {
                for (byte xTile = 0; xTile < map.XTiles; xTile++)
                {
                    var tile = DeserializeNextTile(reader, xTile, yTile);
                    map.SetTile(xTile, yTile, tile);
                }
            }

            return map;
        }

        /// <summary>
        /// Serializes the tilemap tile data to the stream
        /// </summary>
        /// <param name="map">The map to serialize</param>
        /// <param name="writer">The stream to write to</param>
        public void Serialize(IMap map, BinaryWriter writer)
        {
            foreach (var tile in map.GetTileIterator())
            {
                writer.Write(tile.TileRef);
                writer.Write((ushort)tile.Elevation);
            }
        }
    }
}
