using System.IO;
using LemballEditor.Models;

namespace LemballEditor.Serializers.Level.Objects
{
    public class GphsBlockSerializer : ISerializer<ILevel>
    {
        public ILevel Deserialize(BinaryReader reader, ILevel model)
        {
            return model;
        }

        public void Serialize(ILevel model, BinaryWriter writer)
        {

        }
    }
}
