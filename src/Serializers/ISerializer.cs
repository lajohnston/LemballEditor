using System.IO;

namespace LemballEditor.Serializers.Level
{
    public interface ISerializer<TResult>
    {
        TResult Deserialize(BinaryReader reader, TResult result);

        void Serialize(TResult result, BinaryWriter writer);
    }
}
