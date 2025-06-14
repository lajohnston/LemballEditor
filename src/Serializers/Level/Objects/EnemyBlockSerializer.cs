using System;
using System.IO;
using LemballEditor.Models;

namespace LemballEditor.Serializers.Level.Objects
{
    public class EnemyBlockSerializer : ISerializer<ILevel>
    {
        public ILevel Deserialize(BinaryReader reader, ILevel model)
        {
            var enemyCount = reader.ReadInt16();

            return enemyCount > 0 ? throw new NotImplementedException("YMNE deserialization is not implemented yet.") : model;
        }

        public void Serialize(ILevel model, BinaryWriter writer)
        {
            writer.Write((ushort)0); // object count
            writer.Write((ushort)0); // unknown
        }
    }
}
