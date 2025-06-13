using LemballEditor.Models;
using LemballEditor.Serializers;
using LemballEditor.Serializers.Level;
using LemballEditor.Serializers.Level.Map;
using LemballEditor.Serializers.Level.Objects;
using LemballEditor.Serializers.LevelDirectory;
using LemballEditor.Serializers.Vsr;
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
        public static readonly Func<ISerializer<IMap>> CreateMapSerializer = () =>
        {
            return new SequenceSerializer<IMap>(
                new ISerializer<IMap>[] {
                    new ConstantSerializer<IMap>(Encoding.ASCII.GetBytes("FSDG"), "map header"),
                    new SizeSerializer(CreateMap),
                    new TileMapSerializer(CreateTile)
                }
            );
        };

        /// <summary>
        /// Creates a serializer to serialize and deserialize a level binary
        /// </summary>
        public static readonly Func<ISerializer<ILevel>> CreateLevelSerializer = () => new SequenceSerializer<ILevel>(
            new ISerializer<ILevel>[] {
                new ConstantSerializer<ILevel>(new byte[] { 0x20, 0x20, 0x49, 0x41, 0x12, 0x0, 0x0, 0x0 }, "level header"), // Header constant; '  IA' followed by 18, 0, 0, 0
                new UnknownASerializer(),
                new ThemeSerializer(),
                new TimeLimitSerializer(),
                new UnusedNumberOfLemmingsSerializer(),
                new FlagsRequiredIndicatorSerializer(),
                new UnknownBSerializer(),
                new LevelMapSerializer(CreateMapSerializer())
            }
        );

        /// <summary>
        /// Creates a serializer to serialize and deserialize a VSR file and its levels
        /// </summary>
        public static readonly Func<ISerializer<(Models.Vsr, Models.LevelPack)>> CreateVsrLevelPackSerializer = () => new SequenceSerializer<(Models.Vsr, Models.LevelPack)>(
            new ISerializer<(Models.Vsr, Models.LevelPack)>[] {
                new DirectoryPointerListSerializer(),
                new AssetBinarySerializer(),
                new FirstLevelFileIdSerializer(),
                new FixedLevelCountSerializer(),
                new Serializers.Vsr.LevelPackSerializer(CreateLevelDirectorySerializer(), CreateLevelDirectory)
            }
        );

        /// <summary>
        /// Creates a serializer to serialize and deserialize a level directory within a VSR file
        /// </summary>
        public static readonly Func<ISerializer<LevelDirectorySerializer>> CreateLevelDirectorySerializer = () => new SequenceSerializer<LevelDirectorySerializer>(
            new ISerializer<LevelDirectorySerializer>[] {
                new ConstantSerializer<LevelDirectorySerializer>(Encoding.ASCII.GetBytes("CRID"), "Invalid directory header"),
                new DataSizeSerializer(),
                new LevelCountSerializer(),
                new ConstantSerializer<LevelDirectorySerializer>(BitConverter.GetBytes((uint) 3), "level directory '3' constant"),
                new AddressToFileInfoListSerializer(),
                new FileNameListSerializer(),
                new FileInfoListSerializer(),
                new LevelListSerializer(CreateLevelSerializer(), () => CreateLevel(1, 1)),
            }
        );

        /// <summary>
        /// Creates a VSR model
        /// </summary>
        public static readonly Func<Vsr> CreateVsr = () => new Vsr();

        /// <summary>
        /// Creates a LevelPack model
        /// </summary>
        public static readonly Func<Models.LevelPack> CreateLevelPack = () => new Models.LevelPack();

        /// <summary>
        /// Creates a LevelGroup model
        /// </summary>
        public static readonly Func<LevelGroupName, LevelGroup> CreateLevelGroup = (LevelGroupName groupName) => new LevelGroup(groupName);

        /// <summary>
        /// Creates a LevelDirectory model containing an optional level group for the given type
        /// </summary>
        public static readonly Func<LevelGroupName?, LevelDirectorySerializer> CreateLevelDirectory = (LevelGroupName? levelGroupName) =>
        {
            var directory = new LevelDirectorySerializer();

            if (levelGroupName.HasValue)
            {
                directory.LevelGroup = new LevelGroup(levelGroupName.Value);
            }

            return directory;
        };

        /// <summary>
        /// Creates a serializer for the BOMG data block
        /// </summary>
        public static readonly Func<ISerializer<ILevel>> CreateBomgBlockSerializer = () => new DataBlockSerializer<ILevel>("BOMG", new BomgBlockSerializer());
    }
}
