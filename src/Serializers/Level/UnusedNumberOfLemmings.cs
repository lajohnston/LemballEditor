using LemballEditor.Models;
using System;
using System.IO;

namespace LemballEditor.Serializers.Level
{
    /// <summary>
    /// This is a ushort storing the number of Lemmings in the level, but it appears it's unused and
    /// the value is instead computed from other data
    /// </summary>
    public class UnusedNumberOfLemmings : ISerializer<ILevel>
    {
        public ILevel Deserialize(BinaryReader reader, ILevel level)
        {
            var value = reader.ReadUInt16();

            if (value > byte.MaxValue)
            {
                throw new InvalidDataException($"NumberOfLemmings out of byte range. {value} given");
            }

            try
            {
                level.NumberOfLemmings = (byte)value;
            }
            catch (ArgumentException error)
            {
                throw new InvalidDataException(error.Message);
            }

            return level;
        }

        public void Serialize(ILevel level, BinaryWriter writer)
        {
            writer.Write((ushort)level.NumberOfLemmings);
        }
    }
}
