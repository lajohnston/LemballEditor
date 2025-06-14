using System;
using System.IO;
using LemballEditor.Models;

namespace LemballEditor.Serializers.Level.Objects
{
    public class PaintGlobeBlockSerializer : ISerializer<ILevel>
    {
        public ILevel Deserialize(BinaryReader reader, ILevel model)
        {
            var objectCount = reader.ReadInt16();

            return objectCount > 0 ? throw new NotImplementedException("LLAB deserialization for objects is not implemented yet.") : model;
        }

        public void Serialize(ILevel model, BinaryWriter writer)
        {
            writer.Write((ushort)0); // object count
        }
    }
}
