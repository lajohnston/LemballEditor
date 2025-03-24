using LemballEditor.Serializers.Level;
using System.IO;

namespace LemballEditor.Serializers.LevelDirectory
{
    /// <summary>
    /// Serializes/Deserializes the directory size uint
    /// </summary>
    public class DataSize : ISerializer<LevelDirectory>
    {
        public LevelDirectory Deserialize(BinaryReader reader, LevelDirectory model)
        {
            _ = reader.ReadUInt32();

            return model;
        }

        public void Serialize(LevelDirectory model, BinaryWriter writer)
        {
            writer.Write((uint)0);
        }
    }
}
