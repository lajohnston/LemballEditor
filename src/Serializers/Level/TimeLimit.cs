using LemballEditor.Models;
using System.IO;

namespace LemballEditor.Serializers.Level
{
    /// <summary>
    /// Serializes/deserializes the level's time limit in seconds, of '600' for an unlimited time limit
    /// </summary>
    public class TimeLimit : ILevelSerializer
    {
        public void Deserialize(ILevel level, BinaryReader reader)
        {
            var value = reader.ReadUInt16();
            level.TimeLimitInSeconds = value > 599 ? null : (ushort?)value;
        }

        public void Serialize(ILevel level, BinaryWriter writer)
        {
            var value = (ushort)(level.TimeLimitInSeconds == null ? 600 : level.TimeLimitInSeconds);
            writer.Write(value);
        }
    }
}
