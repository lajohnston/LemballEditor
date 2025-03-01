using System;

namespace LemballEditor.Models
{
    public static class ModelFactory
    {
        /// <summary>
        /// Creates a level containing a map of the given size
        /// </summary>
        /// <param name="mapSizeX">The map X size in tiles</param>
        /// <param name="mapSizeY">The map Y size in tiles</param>
        /// <returns>Level instance</returns>
        public static readonly Func<ushort, ushort, ILevel> LevelFactory = (xSize, ySize) => new Level(MapFactory(xSize, ySize));

        /// <summary>
        /// Creates a new map tile
        /// </summary>
        /// <param name="tileRef">The tile pattern reference</param>
        /// <param name="elevation">The tile column's elevation in pixels</param>
        /// <returns>Tile instance</returns>
        public static readonly Func<ushort, byte, ITile> TileFactory = (tileRef, elevation) => new Tile(tileRef, elevation);

        /// <summary>
        /// Creates a map of the given size
        /// </summary>
        /// <param name="xTiles">The X size of the map in tiles</param>
        /// <param name="yTiles">The Y size of the map in tiles</param>
        /// <returns>Map instance</returns>
        public static readonly Func<ushort, ushort, IMap> MapFactory = (xSize, ySize) => new Map(TileFactory, xSize, ySize);
    }
}
