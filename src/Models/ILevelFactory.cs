namespace LemballEditor.Models
{
    internal interface ILevelFactory
    {
        /// <summary>
        /// Creates a level containing a map of the given size
        /// </summary>
        /// <param name="mapSizeX">The map X size in tiles</param>
        /// <param name="mapSizeY">The map Y size in tiles</param>
        /// <returns>Level instance</returns>
        ILevel CreateLevel(ushort mapSizeX, ushort mapSizeY);

        /// <summary>
        /// Creates a map of the given size
        /// </summary>
        /// <param name="xTiles">The X size of the map in tiles</param>
        /// <param name="yTiles">The Y size of the map in tiles</param>
        /// <returns>Map instance</returns>
        IMap CreateMap(ushort xTiles, ushort yTiles);
    }
}
