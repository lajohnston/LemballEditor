using System;
using System.Collections.Generic;

namespace LemballEditor.Models
{
    public class Map : IMap
    {
        /// <summary>
        /// Number of X tiles
        /// </summary>
        public ushort XTiles { get; private set; }

        /// <summary>
        /// Number of Y tiles
        /// </summary>
        public ushort YTiles { get; private set; }

        /// <summary>
        /// Total number of tiles
        /// </summary>
        public int TileCount => this.XTiles * this.YTiles;

        /// <summary>
        /// The tile ref used for the out-of-bounds border tiles.
        /// </summary>
        public ushort OutOfBoundsTileRef { get; set; } = 518;

        /// <summary>
        /// The map tiles
        /// </summary>
        private readonly ITile[] tiles;

        /// <summary>
        /// Creates a new map of the given size in tiles
        /// </summary>
        /// <param name="tileFactory">Function that returns a new tile</param>
        /// <param name="xTiles">The number of xTiles</param>
        /// <param name="yTiles">The number of yTiles</param>
        public Map(Func<uint, byte, ITile> tileFactory, ushort xTiles, ushort yTiles)
        {
            this.XTiles = xTiles;
            this.YTiles = yTiles;

            this.tiles = new ITile[xTiles * yTiles];

            for (var index = 0; index < this.TileCount; index++)
            {
                this.tiles[index] = tileFactory(521, 0);
            }
        }

        /// <summary>
        /// Calculate the tile array index from the given coordinate
        /// </summary>
        /// <param name="xTile">0-based xTile coordinate</param>
        /// <param name="yTile">0-based yTile cordinate</param>
        /// <returns>The tile index in the tiles array</returns>
        private int GetIndex(ushort xTile, ushort yTile)
        {
            return xTile >= this.XTiles
                ? throw new IndexOutOfRangeException($"xTile {xTile} is out of bounds")
                : yTile >= this.YTiles ? throw new IndexOutOfRangeException($"yTile {yTile} is out of bounds") : (yTile * this.XTiles) + xTile;
        }

        /// <summary>
        /// Sets the tile at the given coordinate
        /// </summary>
        /// <param name="xTile">0-based xTile coordinate</param>
        /// <param name="yTile">0-based yTile cordinate</param>
        /// <param name="tile">The tile to place at the given position</param>
        public void SetTile(ushort xTile, ushort yTile, ITile tile)
        {
            var index = this.GetIndex(xTile, yTile);

            this.tiles[index] = tile ?? throw new ArgumentNullException();
        }

        /// <summary>
        /// Get the tile at the given coordinate, or null if none has been set there
        /// </summary>
        /// <param name="xTile">0-based xTile coordinate</param>
        /// <param name="yTile">0-based yTile cordinate</param>
        /// <returns>Tile</returns>
        public ITile GetTile(ushort xTile, ushort yTile)
        {
            var index = this.GetIndex(xTile, yTile);

            return this.tiles[index];
        }

        /// <summary>
        /// Returns an iterator that iterates through the tiles starting from 0, 0,
        /// then along each xTile in the row, and each row
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ITile> GetTileIterator()
        {
            foreach (var tile in this.tiles)
            {
                yield return tile;
            }
        }
    }
}
