using System.IO;

namespace LemballEditor.Serializers.LevelDirectory
{
    /// <summary>
    /// Serializes/Deserializes the directory size uint
    /// </summary>
    public class DataSizeSerializer : ISerializer<PendingLevelGroup>
    {
        public PendingLevelGroup Deserialize(BinaryReader reader, PendingLevelGroup model)
        {
            _ = reader.ReadUInt32();

            return model;
        }

        public void Serialize(PendingLevelGroup model, BinaryWriter writer)
        {
            writer.Write((uint)0);
        }
    }
}
