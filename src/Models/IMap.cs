using System.Collections.Generic;

namespace LemballEditor.Models
{
    /// <summary>
    /// A level map consisting of tiles
    /// </summary>
    public interface IMap
    {
        /// <summary>
        /// Number of X tiles
        /// </summary>
        byte XTiles { get; }

        /// <summary>
        /// Number of Y tiles
        /// </summary>
        byte YTiles { get; }

        /// <summary>
        /// The number of tiles in the level
        /// </summary>
        ushort TileCount { get; }

        /// <summary>
        /// Returns an iterator that iterates through the tiles sequentially
        /// </summary>
        /// <returns></returns>
        IEnumerable<Tile> GetTileIterator();

        /// <summary>
        /// Sets the tile at the given coordinate
        /// </summary>
        /// <param name="xTile">0-based xTile coordinate</param>
        /// <param name="yTile">0-based yTile cordinate</param>
        /// <param name="tile">The tile to place at the given position</param>
        void SetTile(byte xTile, byte yTile, Tile tile);

        /// <summary>
        /// Get the tile at the given coordinate, or null if none has been set there
        /// </summary>
        /// <param name="xTile">0-based xTile coordinate</param>
        /// <param name="yTile">0-based yTile cordinate</param>
        /// <returns>Tile, or null if none set</returns>
        Tile GetTile(byte xTile, byte yTile);
    }
}
