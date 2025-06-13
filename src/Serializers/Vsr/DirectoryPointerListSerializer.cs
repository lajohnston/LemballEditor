using System.IO;
using System.Text;
using LemballEditor.Models;

namespace LemballEditor.Serializers.Vsr
{
    public class DirectoryPointerListSerializer : ISerializer<(Models.Vsr, Models.LevelPack)>
    {
        /// <summary>
        /// Extracts the level directory pointers from the VSR data and sets them to the model
        /// </summary>
        /// <exception cref="InvalidDataException">If the directory pointers could not be extracted</exception>
        public (Models.Vsr, Models.LevelPack) Deserialize(BinaryReader reader, (Models.Vsr, Models.LevelPack) models)
        {
            var (vsr, _) = models;
            var originalPosition = reader.BaseStream.Position;
            var header = Encoding.ASCII.GetString(reader.ReadBytes(4));

            if (header != "CRID")
            {
                throw new InvalidDataException("Invalid VSR asset data");
            }

            var funDirectoryPointer = this.GetFunDirectoryPointer(reader);

            vsr.SetLevelDirectoryPointer(LevelGroupName.Fun, funDirectoryPointer);
            vsr.SetLevelDirectoryPointer(LevelGroupName.Tricky, funDirectoryPointer + 36);
            vsr.SetLevelDirectoryPointer(LevelGroupName.Taxing, funDirectoryPointer + (36 * 2));
            vsr.SetLevelDirectoryPointer(LevelGroupName.Mayhem, funDirectoryPointer + (36 * 3));
            vsr.SetLevelDirectoryPointer(LevelGroupName.Network, funDirectoryPointer + (36 * 4));

            _ = reader.BaseStream.Position = originalPosition;

            return models;
        }

        public void Serialize((Models.Vsr, Models.LevelPack) models, BinaryWriter writer)
        {
            // do nothing
        }

        /// <summary>
        /// Locates the address that points to the FUN directory in the VSR data
        /// </summary>
        /// <param name="reader">Reader reading the source VSR</param>
        /// <returns>The pointer address</returns>
        /// <exception cref="InvalidDataException">If the data does not appear valid</exception>
        private uint GetFunDirectoryPointer(BinaryReader reader)
        {
            // Read 64 bytes from address 900 (known addresses are 928 or 932);
            var startSearchAddress = 900;
            _ = reader.BaseStream.Position = startSearchAddress;
            var vsrString = Encoding.ASCII.GetString(reader.ReadBytes(64));

            // Search for 'Demo_00', which is the first file in all known VSR versions
            var demoStringOffset = vsrString.IndexOf("Demo_00");

            return demoStringOffset == -1
                ? throw new InvalidDataException("Unable to locate FUN directory pointer in VSR data")
                : (uint)(startSearchAddress + demoStringOffset - 188);
        }
    }
}
