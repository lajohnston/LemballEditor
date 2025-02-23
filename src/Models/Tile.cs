using System;

namespace LemballEditor.Models
{
    /// <summary>
    /// A tile in the level map
    /// </summary>
    public class Tile
    {
        /// <summary>
        /// The default tile ref if none is given. This is a standard grass/ground style
        /// </summary>
        private static readonly uint DEFAULT_TILE_REF = 521;

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

        public Tile() : this(DEFAULT_TILE_REF)
        {
        }

        /// <summary>
        /// Creates a new tile with the given tileRef
        /// </summary>
        /// <param name="tileRef">The tile pattern reference</param>
        public Tile(uint tileRef) : this(tileRef, 0)
        {
        }

        /// <summary>
        /// Creates a new tile with the given tileRef and elevation
        /// </summary>
        /// <param name="tileRef"></param>
        /// <param name="elevation"></param>
        public Tile(uint tileRef, byte elevation)
        {
            TileRef = tileRef;
            Elevation = elevation;
        }
    }
}
