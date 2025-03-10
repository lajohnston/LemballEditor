using System.IO;

namespace LemballEditor.Serializers.Level
{
    public interface ISerializer<TResult>
    {
        TResult Deserialize(BinaryReader reader, TResult model);

        void Serialize(TResult model, BinaryWriter writer);
    }
}
