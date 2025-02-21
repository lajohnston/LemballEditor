using LemballEditor.Models;
using System.IO;
using System.Linq;

namespace LemballEditor.Serializers.Level
{
    /// <summary>
    /// Serializes/Deserializes the level format header constant
    /// </summary>
    public class Header : ILevelSerializer
    {
        // '  IA' followed by 18, 0, 0, 0
        private static readonly byte[] header = { 0x20, 0x20, 0x49, 0x41, 0x12, 0x0, 0x0, 0x0 };

        public void Deserialize(ILevel level, BinaryReader reader)
        {
            var headerBytes = reader.ReadBytes(header.Length);

            if (!headerBytes.SequenceEqual(header))
            {
                throw new InvalidDataException("Invalid header value");
            }
        }

        public void Serialize(ILevel level, BinaryWriter writer)
        {
            writer?.Write(header);
        }
    }
}
