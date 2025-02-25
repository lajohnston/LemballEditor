using LemballEditor.Models;
using System.IO;

namespace LemballEditor.Serializers.Level
{
    /// <summary>
    /// Serializes and deserializes the UnknownB ushort
    /// The ushort has lots of values but changing them has no apparent effect on the level.
    /// Common values are 0, 240, 260, but also high numbers such as 33425. Perhaps they are 2
    /// separate byte values, or flags
    /// </summary>
    public class UnknownB : ISerializer<ILevel>
    {
        public ILevel Deserialize(BinaryReader reader, ILevel level)
        {
            var value = reader.ReadUInt16();
            level.UnknownB = value;
            return level;
        }

        public void Serialize(ILevel level, BinaryWriter writer)
        {
            writer.Write(level.UnknownB);
        }
    }
}
