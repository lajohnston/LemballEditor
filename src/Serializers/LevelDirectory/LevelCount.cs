using LemballEditor.Serializers.Level;
using System;
using System.IO;

namespace LemballEditor.Serializers.LevelDirectory
{
    public class LevelCount : ISerializer<LevelDirectoryContext>
    {
        public LevelDirectoryContext Deserialize(BinaryReader reader, LevelDirectoryContext model)
        {
            var levelCount = reader.ReadUInt32();

            if (levelCount > 29)
            {
                throw new InvalidDataException($"Number of levels in level group higher than 29 maximum: {levelCount}");
            }

            model.LevelCount = (byte)levelCount;

            return model;
        }

        public void Serialize(LevelDirectoryContext model, BinaryWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
