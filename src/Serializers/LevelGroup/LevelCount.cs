using LemballEditor.Serializers.Level;
using System;
using System.IO;

namespace LemballEditor.Serializers.LevelGroup
{
    public class LevelCount : ISerializer<Models.LevelGroup>
    {
        public uint DeserializedFileCount { get; private set; }

        public Models.LevelGroup Deserialize(BinaryReader reader, Models.LevelGroup directory)
        {
            DeserializedFileCount = reader.ReadUInt32();

            return directory;
        }

        public void Serialize(Models.LevelGroup directory, BinaryWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
