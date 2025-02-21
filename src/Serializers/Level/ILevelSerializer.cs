using LemballEditor.Models;
using System.IO;

namespace LemballEditor.Serializers.Level
{
    public interface ILevelSerializer
    {
        void Deserialize(ILevel level, BinaryReader reader);

        void Serialize(ILevel level, BinaryWriter writer);
    }
}
