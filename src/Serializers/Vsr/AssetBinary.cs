using System.IO;

namespace LemballEditor.Serializers.Vsr
{
    public class AssetBinary : ISerializer<(Models.Vsr, Models.LevelPack)>
    {
        /// <summary>
        /// Reads the asset data up to the FUN directory pointer and sets it to the model
        /// </summary>
        public (Models.Vsr, Models.LevelPack) Deserialize(BinaryReader reader, (Models.Vsr, Models.LevelPack) models)
        {
            var (vsr, _) = models;
            var startAddress = reader.BaseStream.Position;

            var funDirectoryPointer = vsr.GetLevelDirectoryPointer(Models.LevelGroupName.Fun);
            reader.BaseStream.Position = funDirectoryPointer;
            var funDirectoryAddress = reader.ReadInt32();

            reader.BaseStream.Position = startAddress;
            var assetData = reader.ReadBytes(funDirectoryAddress);

            vsr.AssetData = assetData;

            return models;
        }

        /// <summary>
        /// Writes the asset data to the stream
        /// </summary>
        public void Serialize((Models.Vsr, Models.LevelPack) models, BinaryWriter writer)
        {
            var (vsr, _) = models;
            writer.Write(vsr.AssetData);
        }
    }
}
