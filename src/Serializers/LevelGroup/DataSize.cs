using LemballEditor.Serializers.Level;
using System.IO;

namespace LemballEditor.Serializers.LevelGroup
{
    /// <summary>
    /// Serializes/Deserializes the directory size uint
    /// </summary>
    public class DataSize : ISerializer<Models.LevelGroup>
    {
        public Models.LevelGroup Deserialize(BinaryReader reader, Models.LevelGroup model)
        {
            _ = reader.ReadUInt32();

            return model;
        }

        public void Serialize(Models.LevelGroup model, BinaryWriter writer)
        {
            throw new System.NotImplementedException();
        }
    }
}
