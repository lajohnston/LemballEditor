using System;
using System.IO;
using System.Linq;

namespace LemballEditor.Serializers.Level.Objects
{
    /// <summary>
    /// Serializer for the NETW block in the level file (presumably Network level related)
    /// </summary>
    public class NetworkBlockSerializer : ISerializer<PendingObjectList>
    {
        private static readonly byte[] DATA = new byte[] { 1, 0, 0, 0, 0, 0, 0, 0, 4, 0 };

        public PendingObjectList Deserialize(BinaryReader reader, PendingObjectList list)
        {
            var unknown = reader.ReadBytes(10);

            return !unknown.SequenceEqual(DATA)
                ? throw new NotImplementedException($"Unsupported data at position {reader.BaseStream.Position - 10}.")
                : list;
        }

        public void Serialize(PendingObjectList model, BinaryWriter writer)
        {
            writer.Write(DATA);
        }
    }
}
