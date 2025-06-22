using System;
using System.IO;

namespace LemballEditor.Serializers.Level.Objects
{
    public class EnemyBlockSerializer : ISerializer<PendingObjectList>
    {
        public PendingObjectList Deserialize(BinaryReader reader, PendingObjectList list)
        {
            var enemyCount = reader.ReadInt16();

            return enemyCount > 0 ? throw new NotImplementedException("YMNE deserialization is not implemented yet.") : list;
        }

        public void Serialize(PendingObjectList list, BinaryWriter writer)
        {
            writer.Write((ushort)0); // object count
            writer.Write((ushort)0); // unknown
        }
    }
}
