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
        byte XTileSize { get; }

        /// <summary>
        /// Number of Y tiles
        /// </summary>
        byte YTileSize { get; }

        /// <summary>
        /// The number of tiles in the level
        /// </summary>
        ushort TileCount { get; }
    }
}
