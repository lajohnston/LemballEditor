using System;
using System.IO;
using LemballEditor.Models;

namespace LemballEditor.Serializers.Level.Objects
{
    public class EnemyPathNodesBlockSerializer : ISerializer<ILevel>
    {
        public ILevel Deserialize(BinaryReader reader, ILevel model)
        {
            var nodeCount = reader.ReadInt16();

            return nodeCount > 0 ? throw new NotImplementedException("EDON deserialization is not implemented yet.") : model;
        }

        public void Serialize(ILevel model, BinaryWriter writer)
        {
            writer.Write((ushort)0); // object count
        }
    }
}
