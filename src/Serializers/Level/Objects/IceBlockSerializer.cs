using System;
using System.IO;

namespace LemballEditor.Serializers.Level.Objects
{
    /// <summary>
    /// Serialize for the ICE block (slippery slide tiles)
    /// </summary>
    public class IceBlockSerializer : ISerializer<PendingObjectList>
    {
        public PendingObjectList Deserialize(BinaryReader reader, PendingObjectList list)
        {
            var objectCount = reader.ReadInt16();

            return objectCount > 0 ? throw new NotImplementedException("ICE deserialization is not implemented yet.") : list;
        }

        public void Serialize(PendingObjectList model, BinaryWriter writer)
        {
            writer.Write((ushort)0); // object count
        }
    }
}
