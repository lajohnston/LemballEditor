using LemballEditor.Models;
using LemballEditor.Serializers;
using LemballEditor.Serializers.Level;
using LemballEditor.Serializers.Level.Map;
using LemballEditor.Serializers.LevelDirectory;
using System;
using System.Data;
using System.Text;

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
        /// Creates a serializer to serialize and deserialize a level binary
        /// </summary>
        public static readonly Func<ISerializer<ILevel>> CreateLevelSerializer = () => new Sequence<ILevel>(
            new ISerializer<ILevel>[] {
                new Constant<ILevel>(new byte[] { 0x20, 0x20, 0x49, 0x41, 0x12, 0x0, 0x0, 0x0 }), // Header constant; '  IA' followed by 18, 0, 0, 0
                new UnknownA(),
                new Theme(),
                new TimeLimit(),
                new UnusedNumberOfLemmings(),
                new FlagsRequiredIndicator(),
                new UnknownB(),
                CreateMapSerializer()
            }
        );

        /// <summary>
        /// Creates a VSRSerializer
        /// </summary>
        public static readonly Func<VsrSerializerV1> CreateVsrSerializer = () => new VsrSerializerV1(CreateLevelDirectorySerializer(), CreateLevelDirectory);

        /// <summary>
        /// Creates a serializer to serialize and deserialize a level directory within a VSR file
        /// </summary>
        public static readonly Func<ISerializer<LevelDirectory>> CreateLevelDirectorySerializer = () => new Sequence<LevelDirectory>(
            new ISerializer<LevelDirectory>[] {
                new Constant<LevelDirectory>(Encoding.ASCII.GetBytes("CRID"), "Invalid directory header"),
                new DataSize(),
                new LevelCount(),
                new Constant<LevelDirectory>(BitConverter.GetBytes((uint) 3)),
                new AddressToFileInfoList(),
                new FileNameList(),
                new FileInfoList(),
                new LevelList(CreateLevelSerializer(), () => CreateLevel(1, 1)),
            }
        );

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
        public static readonly Func<LevelGroupName, LevelGroup> CreateLevelGroup = (LevelGroupName groupName) => new LevelGroup(groupName);

        /// <summary>
        /// Creates a LevelDirectory model
        /// </summary>
        public static readonly Func<LevelDirectory> CreateLevelDirectory = () => new LevelDirectory();
    }
}
