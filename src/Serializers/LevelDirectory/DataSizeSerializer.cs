using System.IO;

namespace LemballEditor.Serializers.LevelDirectory
{
    /// <summary>
    /// Serializes/Deserializes the directory size uint
    /// </summary>
    public class DataSizeSerializer : ISerializer<LevelDirectorySerializer>
    {
        public LevelDirectorySerializer Deserialize(BinaryReader reader, LevelDirectorySerializer model)
        {
            _ = reader.ReadUInt32();

            return model;
        }

        public void Serialize(LevelDirectorySerializer model, BinaryWriter writer)
        {
            writer.Write((uint)0);
        }
    }
}
