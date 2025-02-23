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
    }
}
