using System;
using System.IO;

namespace LemballEditor.Serializers.Level.Objects
{
    public class AnimatedTileBlockSerializer : ISerializer<PendingObjectList>
    {
        public PendingObjectList Deserialize(BinaryReader reader, PendingObjectList list)
        {
            var objectCount = reader.ReadInt16();

            return objectCount > 0 ? throw new NotImplementedException("MINA deserialization for objects is not implemented yet.") : list;
        }

        public void Serialize(PendingObjectList list, BinaryWriter writer)
        {
            writer.Write((ushort)0); // object count
        }
    }
}
