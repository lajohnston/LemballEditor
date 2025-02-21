using LemballEditor.Models;
using System;
using System.IO;

namespace LemballEditor.Serializers.Level
{
    public class UnknownA : ILevelSerializer
    {
        public void Deserialize(ILevel level, BinaryReader reader)
        {
            var value = reader.ReadUInt16();
            try
            {
                level.UnknownA = value;
            }
            catch (ArgumentException error)
            {
                throw new InvalidDataException(error.Message);
            }
        }

        public void Serialize(ILevel level, BinaryWriter writer)
        {
            writer?.Write(level.UnknownA);
        }
    }
}
