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
    }
}
