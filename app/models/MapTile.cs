using System;
namespace LemballEditor.Model
{
    /// <summary>
    /// Represents a single tile type in a map that stores a reference to the image that the tile uses
    /// and its current elevation.
    /// </summary>
    public class MapTile
    {
        /// <summary>
        /// The tile image number of this map tile
        /// </summary>
        public uint TileImageRef { get; set; }

        /// <summary>
        /// Returns the maximum elevation that this tile can be set to, dependent on its tile image ref
        /// </summary>
        /// <returns></returns>
        private byte MaxElevation => View.Level.ImageCache.GetMaxElevation(TileImageRef);

        /// <summary>
        /// The tile's elevation
        /// </summary>
        private byte elevation;

        /// <summary>
        /// Public accessor to the elevation property. Ensures value is within correct range
        /// </summary>
        public byte Elevation
        {
            get => elevation;
            set
            {
                // Get the maximum elevation possible for the tile image ref
                byte maxElevation = MaxElevation;

                // If value exceeds maximum, set elevation to maximum, otherwise set to value
                elevation = value > maxElevation ? maxElevation : value;
            }
        }

        /// <summary>
        /// Creates a map tile with the specified time image ref and elevation
        /// </summary>
        /// <param name="tileRef"></param>
        /// <param name="elevation"></param>
        public MapTile(uint tileRef, byte elevation)
        {
            TileImageRef = tileRef;
            Elevation = elevation;
        }

        /// <summary>
        /// Creates a map tile with the default elevation and tile image ref
        /// </summary>
        public MapTile()
        {
            TileImageRef = 521;
            elevation = 0;
        }

        /// <summary>
        /// Specifies whether the tile's elevation can be altered by the specified amount
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public bool CanAlterElevation(int amount)
        {
            int newVal = elevation + amount;
            return newVal >= 0 && newVal <= MaxElevation;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="amount"></param>
        public void AlterElevation(int amount)
        {
            int newValue = elevation + amount;
            newValue = Math.Min(newValue, MaxElevation);
            newValue = Math.Max(newValue, 0);
            elevation = (byte)newValue;
        }

    }
}