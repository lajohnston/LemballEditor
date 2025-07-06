using System;
using System.IO;

namespace LemballEditor.Serializers.Level.Objects
{
    /// <summary>
    /// Serializer for the INVS block in the level file. This seems to contain bullet activated lifts
    /// </summary>
    public class InvsBlockSerializer : ISerializer<PendingObjectList>
    {
        public PendingObjectList Deserialize(BinaryReader reader, PendingObjectList list)
        {
            var objectCount = reader.ReadInt16();

            return objectCount > 0 ? throw new NotImplementedException("Invs deserialization is not implemented yet.") : list;
        }

        public void Serialize(PendingObjectList model, BinaryWriter writer)
        {
            writer.Write((ushort)0); // object count
        }
    }
}
