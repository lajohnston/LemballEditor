using LemballEditor.Models;
using LemballEditor.Serializers.Level;
using LemballEditor.Serializers.LevelDirectory;
using System;
using System.IO;
using System.Text;

namespace LemballEditor.Serializers
{
    /// <summary>
    /// Deserializes a full VSR file
    /// </summary>
    public class VsrSerializer : ISerializer<Models.Vsr>
    {
        /// <summary>
        /// Serializer that serialises and deserialises data to and from a level pack
        /// </summary>
        private readonly ISerializer<(Models.LevelGroup, LevelDirectoryContext)> levelGroupSerializer;

        public VsrSerializer(ISerializer<(Models.LevelGroup, LevelDirectoryContext)> levelGroupSerializer)
        {
            this.levelGroupSerializer = levelGroupSerializer;
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

            if (demoStringAddress <= 0)
            {
                throw new InvalidDataException("Unable to locate FUN directory pointer in VSR data");
            }

            return (uint)(startAddress + demoStringAddress - 188);
        }

        /// <summary>
        /// Deserialize a stream containing a full VSR file into a Vsr instance
        /// </summary>
        /// <param name="reader">BinaryReader reading the VSR stream</param>
        /// <returns></returns>
        /// <exception cref="InvalidDataException">If the data isn't valid VSR data</exception>
        public Models.Vsr Deserialize(BinaryReader reader, Models.Vsr vsr)
        {
            var header = Encoding.ASCII.GetString(reader.ReadBytes(4));

            if (header != "CRID")
            {
                throw new InvalidDataException("Invalid VSR");
            }

            var funDirectoryPointer = GetFunDirectoryPointer(reader);

            // Set pointers
            vsr.SetLevelDirectoryPointer(LevelGroupName.Fun, funDirectoryPointer);
            vsr.SetLevelDirectoryPointer(LevelGroupName.Tricky, funDirectoryPointer + 36);
            vsr.SetLevelDirectoryPointer(LevelGroupName.Taxing, funDirectoryPointer + (36 * 2));
            vsr.SetLevelDirectoryPointer(LevelGroupName.Mayhem, funDirectoryPointer + (36 * 3));
            vsr.SetLevelDirectoryPointer(LevelGroupName.Network, funDirectoryPointer + (36 * 4));

            // Get the FUN directory address
            _ = reader.BaseStream.Seek(funDirectoryPointer, SeekOrigin.Begin);
            var funDirectoryAddress = reader.ReadUInt32();

            // Set asset data, excluding the level files at the end (from FUN onwards)
            _ = reader.BaseStream.Seek(0, SeekOrigin.Begin);
            vsr.AssetData = reader.ReadBytes((int)funDirectoryAddress);

            // Level data
            if (vsr.LevelPack != null)
            {
                foreach (LevelGroupName levelGroupName in Enum.GetValues(typeof(LevelGroupName)))
                {
                    var existingGroup = vsr.LevelPack.GetLevelGroup(levelGroupName);
                    var levelGroupContext = new LevelDirectoryContext();
                    var (levelGroup, _) = levelGroupSerializer.Deserialize(
                        reader,
                        (existingGroup, levelGroupContext)
                    );

                    vsr.LevelPack.SetLevelGroup(levelGroupName, levelGroup);
                }
            }

            return vsr;
        }

        /// <summary>
        /// Serializes a Vsr instance into a stream to produce Lemmings Paintball compatible VSR data
        /// </summary>
        /// <param name="vsr">The VSR instance</param>
        /// <param name="writer">Writer for the stream</param>
        public void Serialize(Models.Vsr vsr, BinaryWriter writer)
        {
            var stream = writer.BaseStream;
            var basePosition = stream.Position;

            writer.Write(vsr.AssetData);

            foreach (LevelGroupName levelGroupName in Enum.GetValues(typeof(LevelGroupName)))
            {
                var directoryAddress = (uint)stream.Position;
                var context = new LevelDirectoryContext
                {
                    BaseAddress = directoryAddress
                };

                var levelGroup = vsr.LevelPack.GetLevelGroup(levelGroupName);
                levelGroupSerializer.Serialize((levelGroup, context), writer);

                var endPosition = stream.Position;

                stream.Position = basePosition + vsr.GetLevelDirectoryPointer(levelGroupName);
                writer.Write(directoryAddress);

                stream.Position = endPosition;
            }
        }
    }
}
