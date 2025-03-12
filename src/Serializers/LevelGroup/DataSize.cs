using LemballEditor.Serializers.Level;
using System.IO;

namespace LemballEditor.Serializers.LevelGroup
{
    /// <summary>
    /// Serializes/Deserializes the directory size uint
    /// </summary>
    public class DataSize : ISerializer<PendingLevelGroup>
    {
        public PendingLevelGroup Deserialize(BinaryReader reader, PendingLevelGroup model)
        {
            _ = reader.ReadUInt32();

            return model;
        }

        public void Serialize(PendingLevelGroup model, BinaryWriter writer)
        {
            throw new System.NotImplementedException();
        }
    }
}
