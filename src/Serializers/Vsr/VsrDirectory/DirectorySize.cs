using LemballEditor.Serializers.Level;
using System.IO;

namespace LemballEditor.Serializers.Vsr.VsrDirectory
{
    /// <summary>
    /// Serializes/Deserializes the directory size uint
    /// </summary>
    public class DirectorySize : ISerializer<Models.VsrDirectory>
    {
        public Models.VsrDirectory Deserialize(BinaryReader reader, Models.VsrDirectory model)
        {
            _ = reader.ReadUInt32();

            return model;
        }

        public void Serialize(Models.VsrDirectory model, BinaryWriter writer)
        {
            throw new System.NotImplementedException();
        }
    }
}
