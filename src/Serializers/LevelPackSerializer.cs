using LemballEditor.Models;
using LemballEditor.Serializers.Level;
using System;
using System.IO;

namespace LemballEditor.Serializers
{
    public class LevelPackSerializer : ISerializer<LevelPack>
    {
        public LevelPack Deserialize(BinaryReader reader, LevelPack levelPack = null)
        {
            throw new NotImplementedException();
        }

        public void Serialize(LevelPack result, BinaryWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
