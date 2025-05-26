using System;
using System.IO;
using System.Text;
using LemballEditor.Models;

namespace LemballEditor.Serializers
{
    /// <summary>
    /// Deserializes a full VSR file
    /// </summary>
    public class VsrSerializerV1 : ISerializer<(Models.Vsr, Models.LevelPack)>
    {
        /// <summary>
        /// Serializer that serialises and deserialises data to and from a level pack
        /// </summary>
        private readonly ISerializer<LevelDirectory.LevelDirectory> levelDirectorySerializer;

        private readonly Func<LevelDirectory.LevelDirectory> levelDirectoryFactory;

        public VsrSerializerV1(
            ISerializer<LevelDirectory.LevelDirectory> levelDirectorySerializer,
            Func<LevelDirectory.LevelDirectory> levelDirectoryFactory)
        {
            this.levelDirectorySerializer = levelDirectorySerializer;
            this.levelDirectoryFactory = levelDirectoryFactory;
        }

        /// <summary>
        /// Locates the address that points to the FUN directory in the VSR data
        /// </summary>
        /// <param name="reader">Reader reading the source VSR</param>
        /// <returns>The pointer address</returns>
        /// <exception cref="InvalidDataException">If the data does not appear valid</exception>
        private uint GetFunDirectoryPointer(BinaryReader reader)
        {
            // Read 150 bytes from address 908
            var startAddress = 908;
            _ = reader.BaseStream.Seek(startAddress, SeekOrigin.Begin);
            var vsrString = new string(reader.ReadChars(150));

            // Search for 'Demo_00', which is the first file in all known VSR versions
            var demoStringAddress = vsrString.IndexOf("Demo_00");

            return demoStringAddress <= 0
                ? throw new InvalidDataException("Unable to locate FUN directory pointer in VSR data")
                : (uint)(startAddress + demoStringAddress - 188);
        }

        /// <summary>
        /// Points the reader to the given level directory. The level pointers should have already been set
        /// to the VSR model
        /// </summary>
        private void SeekLevelDirectory(BinaryReader reader, Models.Vsr vsr, LevelGroupName levelGroupName)
        {
            var directoryPointer = vsr.GetLevelDirectoryPointer(levelGroupName);
            _ = reader.BaseStream.Seek(directoryPointer, SeekOrigin.Begin);
            var directoryAddress = reader.ReadUInt32();
            _ = reader.BaseStream.Seek(directoryAddress, SeekOrigin.Begin);
        }

        /// <summary>
        /// Stores the current (fixed) level sizes for each level group in the VSR instance
        /// </summary>
        /// <exception cref="InvalidDataException">If valid data could not be parsed</exception>
        private void DeserializeFixedLevelSizes(BinaryReader reader, Models.Vsr vsr)
        {
            foreach (LevelGroupName levelGroupName in Enum.GetValues(typeof(LevelGroupName)))
            {
                this.SeekLevelDirectory(reader, vsr, levelGroupName);
                _ = reader.BaseStream.Seek(8, SeekOrigin.Current);

                var levelCount = reader.ReadUInt32();

                if (levelCount > 29)
                {
                    throw new InvalidDataException("Level directory contains more than 29 levels");
                }

                vsr.SetFixedLevelCount(levelGroupName, (byte)levelCount);
            }
        }

        /// <summary>
        /// Returns the file id of the first level of the Fun directory
        /// </summary>
        private uint GetFirstLevelFileId(BinaryReader reader, Models.Vsr vsr)
        {
            var funLevelCount = vsr.GetFixedLevelCount(LevelGroupName.Fun);
            this.SeekLevelDirectory(reader, vsr, LevelGroupName.Fun);

            _ = reader.BaseStream.Seek(20 + (funLevelCount * 12) + 4, SeekOrigin.Current);

            return reader.ReadUInt32();
        }

        /// <summary>
        /// Deserialize a stream containing a full VSR file into a Vsr instance
        /// </summary>
        /// <param name="reader">BinaryReader reading the VSR stream</param>
        /// <exception cref="InvalidDataException">If the data isn't valid VSR data</exception>
        public (Models.Vsr, Models.LevelPack) Deserialize(BinaryReader reader, (Models.Vsr, Models.LevelPack) models)
        {
            var header = Encoding.ASCII.GetString(reader.ReadBytes(4));

            if (header != "CRID")
            {
                throw new InvalidDataException("Invalid VSR");
            }

            var funDirectoryPointer = this.GetFunDirectoryPointer(reader);

            // Set pointers
            (var vsr, var levelPack) = models;
            vsr.SetLevelDirectoryPointer(LevelGroupName.Fun, funDirectoryPointer);
            vsr.SetLevelDirectoryPointer(LevelGroupName.Tricky, funDirectoryPointer + 36);
            vsr.SetLevelDirectoryPointer(LevelGroupName.Taxing, funDirectoryPointer + (36 * 2));
            vsr.SetLevelDirectoryPointer(LevelGroupName.Mayhem, funDirectoryPointer + (36 * 3));
            vsr.SetLevelDirectoryPointer(LevelGroupName.Network, funDirectoryPointer + (36 * 4));

            // Get the FUN directory address
            _ = reader.BaseStream.Seek(funDirectoryPointer, SeekOrigin.Begin);
            var funDirectoryAddress = reader.ReadUInt32();

            // Set asset data (everything up to but excluding the level directories)
            _ = reader.BaseStream.Seek(0, SeekOrigin.Begin);
            vsr.AssetData = reader.ReadBytes((int)funDirectoryAddress);

            if (levelPack != null)
            {
                foreach (LevelGroupName levelGroup in Enum.GetValues(typeof(LevelGroupName)))
                {
                    var levelDirectory = this.levelDirectorySerializer.Deserialize(reader, this.levelDirectoryFactory());
                    levelPack.SetLevelGroup(levelDirectory.LevelGroup);
                }
            }

            this.DeserializeFixedLevelSizes(reader, vsr);
            vsr.FirstLevelFileId = this.GetFirstLevelFileId(reader, vsr);

            return models;
        }

        /// <summary>
        /// Serializes a Vsr instance into a stream to produce Lemmings Paintball compatible VSR data
        /// </summary>
        /// <param name="vsr">The VSR model and LevelPack</param>
        /// <param name="writer">Writer for the stream</param>
        public void Serialize((Models.Vsr, Models.LevelPack) models, BinaryWriter writer)
        {
            var (vsr, levelPack) = models;

            if (levelPack == null)
            {
                throw new ArgumentNullException("LevelPack cannot be null");
            }

            writer.Write(vsr.AssetData);

            uint nextFileId = vsr.FirstLevelFileId;

            foreach (var levelGroup in levelPack.GetLevelGroups())
            {
                var levelDirectory = this.levelDirectoryFactory();
                levelDirectory.Address = (uint)writer.BaseStream.Position;
                levelDirectory.FirstFileId = nextFileId;
                levelDirectory.FixedLevelCount = vsr.GetFixedLevelCount(levelGroup.LevelGroupName);
                levelDirectory.LevelGroup = levelGroup;

                _ = writer.Seek((int)vsr.GetLevelDirectoryPointer(levelGroup.LevelGroupName), SeekOrigin.Begin);
                writer.Write(levelDirectory.Address);

                _ = writer.Seek((int)levelDirectory.Address, SeekOrigin.Begin);
                this.levelDirectorySerializer.Serialize(levelDirectory, writer);

                nextFileId += levelDirectory.FixedLevelCount;
            }
        }
    }
}
