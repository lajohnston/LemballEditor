using LemballEditor.Serializers.Level;
using System.IO;

namespace LemballEditor.Serializers.LevelDirectory
{
    /// <summary>
    /// Serializes/Deserializes the directory size uint
    /// </summary>
    public class DataSize : ISerializer<LevelDirectoryContext>
    {
        public LevelDirectoryContext Deserialize(BinaryReader reader, LevelDirectoryContext model)
        {
            _ = reader.ReadUInt32();

            return model;
        }

        public void Serialize(LevelDirectoryContext model, BinaryWriter writer)
        {
            throw new System.NotImplementedException();
        }
    }
}
