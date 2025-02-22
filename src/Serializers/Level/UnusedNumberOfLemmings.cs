using LemballEditor.Models;
using System;
using System.IO;

namespace LemballEditor.Serializers.Level
{
    /// <summary>
    /// This is a ushort storing the number of Lemmings in the level, but it appears it's unused and
    /// the value is instead computed from other data
    /// </summary>
    public class UnusedNumberOfLemmings : ILevelSerializer
    {
        public void Deserialize(ILevel level, BinaryReader reader)
        {
            var value = reader.ReadUInt16();
            try
            {
                level.NumberOfLemmings = value;
            }
            catch (ArgumentException error)
            {
                throw new InvalidDataException(error.Message);
            }
        }

        public void Serialize(ILevel level, BinaryWriter writer)
        {
            writer?.Write(level.NumberOfLemmings);
        }
    }
}
