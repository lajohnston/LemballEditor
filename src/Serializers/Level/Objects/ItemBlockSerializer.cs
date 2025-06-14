using System;
using System.IO;
using LemballEditor.Models;

namespace LemballEditor.Serializers.Level.Objects
{
    public class ItemBlockSerializer : ISerializer<ILevel>
    {
        public ILevel Deserialize(BinaryReader reader, ILevel model)
        {
            var itemCount = reader.ReadInt16();

            for (var i = 0; i < itemCount; i++)
            {
                _ = reader.ReadInt16(); // item ID
                var type = reader.ReadUInt16();
                var positionX = reader.ReadUInt16();
                var positionY = reader.ReadUInt16();
                _ = reader.ReadUInt16(); // padding
            }

            return itemCount > 0 ? throw new NotImplementedException("Item block deserialization for objects is not implemented yet.") : model;
        }

        public void Serialize(ILevel model, BinaryWriter writer)
        {
            writer.Write((ushort)0); // object count
        }
    }
}
