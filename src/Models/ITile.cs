namespace LemballEditor.Models
{
    /// <summary>
    /// A tile in the level map
    /// </summary>
    public interface ITile
    {
        /// <summary>
        /// The tile pattern reference
        /// </summary>
        uint TileRef { get; set; }

        /// <summary>
        /// The elevation of the tile in pixels. Max value is 88 before the graphics get cropped
        /// </summary>
        byte Elevation { get; set; }
    }
}
