using System;
using System.Collections.Generic;

namespace LemballEditor.Models
{
    public class Map : IMap
    {
        /// <summary>
        /// Number of X tiles
        /// </summary>
        public byte XTiles { get; private set; }

        /// <summary>
        /// Number of Y tiles
        /// </summary>
        public byte YTiles { get; private set; }

        /// <summary>
        /// Total number of tiles
        /// </summary>
        public ushort TileCount => (ushort)(XTiles * YTiles);

        /// <summary>
        /// The map tiles
        /// </summary>
        private readonly Tile[] tiles;

        /// <summary>
        /// Creates a new map of the given size in tiles
        /// </summary>
        /// <param name="xTiles">The number of xTiles</param>
        /// <param name="yTiles">The number of yTiles</param>
        public Map(byte xTiles, byte yTiles)
        {
            XTiles = xTiles;
            YTiles = yTiles;

            tiles = new Tile[xTiles * yTiles];

            for (var index = 0; index < TileCount; index++)
            {
                tiles[index] = new Tile();
            }
        }

        /// <summary>
        /// Calculate the tile array index from the given coordinate
        /// </summary>
        /// <param name="xTile">0-based xTile coordinate</param>
        /// <param name="yTile">0-based yTile cordinate</param>
        /// <returns>The tile index in the tiles array</returns>
        private int GetIndex(byte xTile, byte yTile)
        {
            if (xTile >= XTiles)
            {
                throw new IndexOutOfRangeException($"xTile {xTile} is out of bounds");
            }

            if (yTile >= YTiles)
            {
                throw new IndexOutOfRangeException($"yTile {yTile} is out of bounds");
            }

            return (yTile * XTiles) + xTile;
        }

        /// <summary>
        /// Sets the tile at the given coordinate
        /// </summary>
        /// <param name="xTile">0-based xTile coordinate</param>
        /// <param name="yTile">0-based yTile cordinate</param>
        /// <param name="tile">The tile to place at the given position</param>
        public void SetTile(byte xTile, byte yTile, Tile tile)
        {
            var index = GetIndex(xTile, yTile);

            tiles[index] = tile ?? throw new ArgumentNullException();
        }

        /// <summary>
        /// Get the tile at the given coordinate, or null if none has been set there
        /// </summary>
        /// <param name="xTile">0-based xTile coordinate</param>
        /// <param name="yTile">0-based yTile cordinate</param>
        /// <returns>Tile, or null if none set</returns>
        public Tile GetTile(byte xTile, byte yTile)
        {
            var index = GetIndex(xTile, yTile);

            return tiles[index];
        }

        /// <summary>
        /// Returns an iterator that iterates through the tiles starting from 0, 0,
        /// then along each xTile in the row, and each row
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Tile> GetTileIterator()
        {
            foreach (var tile in tiles)
            {
                yield return tile;
            }
        }
    }
}
