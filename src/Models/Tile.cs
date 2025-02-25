using System;

namespace LemballEditor.Models
{
    /// <summary>
    /// A tile in the level map
    /// </summary>
    public class Tile : ITile
    {
        /// <summary>
        /// The tile pattern reference
        /// </summary>
        public uint TileRef { get; set; }

        /// <summary>
        /// The elevation of the tile in pixels. Max value is 88 before the graphics get cropped
        /// </summary>
        public byte Elevation
        {
            get => _elevation;
            set
            {
                if (value > 88)
                {
                    throw new ArgumentException($"Max elevation is 88. {value} given");
                }

                _elevation = value;
            }
        }
        private byte _elevation;

        /// <summary>
        /// Creates a new tile with the given tileRef and elevation
        /// </summary>
        /// <param name="tileRef"></param>
        /// <param name="elevation"></param>
        public Tile(uint tileRef = 521, byte elevation = 0)
        {
            TileRef = tileRef;
            Elevation = elevation;
        }
    }
}
