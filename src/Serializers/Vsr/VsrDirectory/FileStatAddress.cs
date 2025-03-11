using LemballEditor.Serializers.Level;
using System;
using System.IO;

namespace LemballEditor.Serializers.Vsr.VsrDirectory
{
    /// <summary>
    /// Serializes/Deserializes the address that points to the first FileStat in the directory
    /// </summary>
    public class FileStatAddress : ISerializer<Models.VsrDirectory>
    {
        public uint DeserializedFileStatAddress { get; private set; }

        public Models.VsrDirectory Deserialize(BinaryReader reader, Models.VsrDirectory directory)
        {
            DeserializedFileStatAddress = reader.ReadUInt32();

            return directory;
        }

        public void Serialize(Models.VsrDirectory directory, BinaryWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
