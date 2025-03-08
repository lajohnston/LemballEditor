using LemballEditor.Models;
using LemballEditor.Serializers.Level;
using System.IO;

namespace LemballEditor.Serializers
{
    public class LevelGroupSerializer : ISerializer<LevelGroup>
    {
        public LevelGroup Deserialize(BinaryReader reader, LevelGroup result)
        {
            throw new System.NotImplementedException();
        }

        public void Serialize(LevelGroup result, BinaryWriter writer)
        {
            throw new System.NotImplementedException();
        }
    }
}
