using System.IO;

namespace LemballEditor.Serializers.Vsr
{
    public class AssetBinary : ISerializer<Models.Vsr>
    {
        /// <summary>
        /// Reads the asset data up to the FUN directory pointer and sets it to the model
        /// </summary>
        public Models.Vsr Deserialize(BinaryReader reader, Models.Vsr vsr)
        {
            var startAddress = reader.BaseStream.Position;

            var funDirectoryPointer = vsr.GetLevelDirectoryPointer(Models.LevelGroupName.Fun);
            reader.BaseStream.Position = funDirectoryPointer;
            var funDirectoryAddress = reader.ReadInt32();

            reader.BaseStream.Position = startAddress;
            var assetData = reader.ReadBytes(funDirectoryAddress);

            vsr.AssetData = assetData;

            return vsr;
        }

        /// <summary>
        /// Writes the asset data to the stream
        /// </summary>
        public void Serialize(Models.Vsr vsr, BinaryWriter writer)
        {
            writer.Write(vsr.AssetData);
        }
    }
}
