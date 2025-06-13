using System.IO;
using LemballEditor.Models;

namespace LemballEditor.Serializers.Level
{
    public class LevelMapSerializer : ISerializer<ILevel>
    {
        private readonly ISerializer<IMap> mapSerializer;

        public LevelMapSerializer(ISerializer<IMap> mapSerializer)
        {
            this.mapSerializer = mapSerializer;
        }

        public ILevel Deserialize(BinaryReader reader, ILevel model)
        {
            var map = this.mapSerializer.Deserialize(reader, null);
            model.Map = map;
            return model;
        }

        public void Serialize(ILevel model, BinaryWriter writer)
        {
            this.mapSerializer.Serialize(model.Map, writer);
        }
    }
}
