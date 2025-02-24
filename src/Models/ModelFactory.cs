namespace LemballEditor.Models
{
    public class ModelFactory : IModelFactory
    {
        /// <summary>
        /// Creates a level containing a map of the given size
        /// </summary>
        /// <param name="mapSizeX">The map X size in tiles</param>
        /// <param name="mapSizeY">The map Y size in tiles</param>
        /// <returns>Level instance</returns>
        public ILevel CreateLevel(ushort mapSizeX, ushort mapSizeY)
        {
            var map = CreateMap(mapSizeX, mapSizeY);
            return new Level(map);
        }

        /// <summary>
        /// Creates a map of the given size
        /// </summary>
        /// <param name="xTiles">The X size of the map in tiles</param>
        /// <param name="yTiles">The Y size of the map in tiles</param>
        /// <returns>Map instance</returns>
        public IMap CreateMap(ushort xTiles, ushort yTiles)
        {
            return new Map(xTiles, yTiles);
        }
    }
}
