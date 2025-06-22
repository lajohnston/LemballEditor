using System;
using System.IO;

namespace LemballEditor.Serializers.Level.Objects
{
    public class EnemyPathNodesBlockSerializer : ISerializer<PendingObjectList>
    {
        public PendingObjectList Deserialize(BinaryReader reader, PendingObjectList list)
        {
            var nodeCount = reader.ReadInt16();

            return nodeCount > 0 ? throw new NotImplementedException("EDON deserialization is not implemented yet.") : list;
        }

        public void Serialize(PendingObjectList list, BinaryWriter writer)
        {
            writer.Write((ushort)0); // object count
        }
    }
}
