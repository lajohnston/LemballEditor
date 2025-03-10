using LemballEditor.Serializers.Level;
using System;
using System.IO;

namespace LemballEditor.Serializers.Vsr.VsrDirectory
{
    public class FileCount : ISerializer<Models.VsrDirectory>
    {
        public uint DeserializedFileCount { get; private set; }

        public Models.VsrDirectory Deserialize(BinaryReader reader, Models.VsrDirectory directory)
        {
            DeserializedFileCount = reader.ReadUInt32();

            return directory;
        }

        public void Serialize(Models.VsrDirectory directory, BinaryWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
