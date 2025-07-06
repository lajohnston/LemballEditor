using System;
using System.IO;

namespace LemballEditor.Serializers.Level.Objects
{
    /// <summary>
    /// Serializer for the unknown SLNK block in the level file.
    /// </summary>
    public class SlnkBlockSerializer : ISerializer<PendingObjectList>
    {
        public PendingObjectList Deserialize(BinaryReader reader, PendingObjectList list)
        {
            var objectCount = reader.ReadInt16();

            return objectCount > 0 ? throw new NotImplementedException("SLNK deserialization is not implemented yet.") : list;
        }

        public void Serialize(PendingObjectList model, BinaryWriter writer)
        {
            writer.Write((ushort)0); // object count
        }
    }
}
