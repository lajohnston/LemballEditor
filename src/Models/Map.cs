namespace LemballEditor.Models
{
    public class Map : IMap
    {
        /// <summary>
        /// Number of X tiles
        /// </summary>
        public byte XTileSize => 64;

        /// <summary>
        /// Number of Y tiles
        /// </summary>
        public byte YTileSize => 64;

        /// <summary>
        /// Total number of tiles
        /// </summary>
        public ushort TileCount => (ushort)(XTileSize * YTileSize);
    }
}
