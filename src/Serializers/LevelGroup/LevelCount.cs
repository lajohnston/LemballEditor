using LemballEditor.Serializers.Level;
using System;
using System.IO;

namespace LemballEditor.Serializers.LevelGroup
{
    public class LevelCount : ISerializer<PendingLevelGroup>
    {
        public PendingLevelGroup Deserialize(BinaryReader reader, PendingLevelGroup model)
        {
            model.LevelCount = reader.ReadUInt32();

            return model;
        }

        public void Serialize(PendingLevelGroup model, BinaryWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
