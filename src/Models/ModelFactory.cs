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
        /// <param name="modelFactory">Instance of a model factory</param>
        /// <param name="xTiles">The X size of the map in tiles</param>
        /// <param name="yTiles">The Y size of the map in tiles</param>
        /// <returns>Map instance</returns>
        public IMap CreateMap(ushort xTiles, ushort yTiles)
        {
            return new Map(this, xTiles, yTiles);
        }

        /// <summary>
        /// Creates a new map tile
        /// </summary>
        /// <param name="tileRef">The tile pattern reference</param>
        /// <param name="elevation">The tile column's elevation in pixels</param>
        /// <returns>Tile instance</returns>
        public ITile CreateTile(ushort tileRef = 521, byte elevation = 0)
        {
            return new Tile(tileRef, elevation);
        }
    }
}
