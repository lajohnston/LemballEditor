using LemballEditor.Serializers.Level;
using System.IO;

namespace LemballEditor.Serializers.LevelGroup
{
    /// <summary>
    /// Serializes/Deserializes the directory size uint
    /// </summary>
    public class DataSize : ISerializer<LevelGroupContext>
    {
        public LevelGroupContext Deserialize(BinaryReader reader, LevelGroupContext model)
        {
            _ = reader.ReadUInt32();

            return model;
        }

        public void Serialize(LevelGroupContext model, BinaryWriter writer)
        {
            throw new System.NotImplementedException();
        }
    }
}
