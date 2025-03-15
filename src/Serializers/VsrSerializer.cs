using LemballEditor.Models;
using LemballEditor.Serializers.Level;
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
        private readonly ISerializer<Models.LevelGroup> levelGroupSerializer;

        public VsrSerializer(ISerializer<Models.LevelGroup> levelGroupSerializer)
        {
            this.levelGroupSerializer = levelGroupSerializer;
        }

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
            vsr.FunDirectoryPointer = funDirectoryPointer;
            vsr.TrickyDirectoryPointer = funDirectoryPointer + 36;
            vsr.TaxingDirectoryPointer = funDirectoryPointer + (36 * 2);
            vsr.MayhemDirectoryPointer = funDirectoryPointer + (36 * 3);
            vsr.NetworkDirectoryPointer = funDirectoryPointer + (36 * 4);

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
                    var levelGroup = levelGroupSerializer.Deserialize(reader, vsr.LevelPack.GetLevelGroup(levelGroupName));
                    vsr.LevelPack.SetLevelGroup(levelGroupName, levelGroup);
                }
            }

            return vsr;
        }

        public void Serialize(Models.Vsr result, BinaryWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
