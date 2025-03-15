using LemballEditor.Models;
using LemballEditor.Serializers;
using LemballEditor.Serializers.Level;
using LemballEditor.Serializers.Level.Map;
using LemballEditor.Serializers.LevelGroup;
using System;

namespace LemballEditor
{
    /// <summary>
    /// Contains factory methods to create the various models and serializers
    /// </summary>
    public static class ServiceFactory
    {
        /// <summary>
        /// Creates a level containing a map of the given size
        /// </summary>
        /// <param name="mapSizeX">The map X size in tiles</param>
        /// <param name="mapSizeY">The map Y size in tiles</param>
        /// <returns>Level instance</returns>
        public static readonly Func<ushort, ushort, ILevel> CreateLevel = (xSize, ySize) => new Level(CreateMap(xSize, ySize));

        /// <summary>
        /// Creates a new map tile
        /// </summary>
        /// <param name="tileRef">The tile pattern reference</param>
        /// <param name="elevation">The tile column's elevation in pixels</param>
        /// <returns>Tile instance</returns>
        public static readonly Func<uint, byte, ITile> CreateTile = (tileRef, elevation) => new Tile(tileRef, elevation);

        /// <summary>
        /// Creates a map of the given size
        /// </summary>
        /// <param name="xTiles">The X size of the map in tiles</param>
        /// <param name="yTiles">The Y size of the map in tiles</param>
        /// <returns>Map instance</returns>
        public static readonly Func<ushort, ushort, IMap> CreateMap = (xSize, ySize) => new Map(CreateTile, xSize, ySize);

        /// <summary>
        /// Creates a MapSerializer
        /// </summary>
        public static readonly Func<MapSerializer> CreateMapSerializer = () => new MapSerializer(CreateMap, CreateTile);

        /// <summary>
        /// Creates a LevelSerializer
        /// </summary>
        public static readonly Func<LevelSerializer> CreateLevelSerializer = () => new LevelSerializer(CreateMapSerializer());

        /// <summary>
        /// Creates a VSRSerializer
        /// </summary>
        public static readonly Func<VsrSerializer> CreateVsrSerializer = () => new VsrSerializer(CreateLevelGroupSerializer());

        /// <summary>
        /// Creates a LevelGroupSerializer
        /// </summary>
        public static readonly Func<LevelGroupSerializer> CreateLevelGroupSerializer = () => new LevelGroupSerializer();

        /// <summary>
        /// Creates a VSR model
        /// </summary>
        public static readonly Func<Vsr> CreateVsr = () => new Vsr();

        /// <summary>
        /// Creates a LevelPack model
        /// </summary>
        public static readonly Func<LevelPack> CreateLevelPack = () => new LevelPack();

        /// <summary>
        /// Creates a LevelGroup model
        /// </summary>
        public static readonly Func<LevelGroup> CreateLevelGroup = () => new LevelGroup();
    }
}
