using LemballEditor.Serializers.Level;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LemballEditor.Serializers.Vsr.VsrDirectory
{
    public class VsrDirectorySerializer : ISerializer<Models.VsrDirectory>
    {
        /// <summary>
        /// An ordered list of sub-serializers to call
        /// </summary>
        private readonly List<ISerializer<Models.VsrDirectory>> propertySerializers;

        private readonly FileCount fileCountSerializer;

        public VsrDirectorySerializer()
        {
            fileCountSerializer = new FileCount();

            propertySerializers = new List<ISerializer<Models.VsrDirectory>>()
            {
                new Constant<Models.VsrDirectory>(Encoding.ASCII.GetBytes("CRID"), "Invalid directory header"),
                new DirectorySize(),
                fileCountSerializer,
                new Constant<Models.VsrDirectory>(BitConverter.GetBytes((uint) 3)),
                new FileStatAddress(),
            };
        }

        /// <summary>
        /// Deserialize a directory within a VSR file
        /// </summary>
        /// <param name="reader">Reader to read the input stream</param>
        /// <param name="directory">The directory model to set the files to</param>
        /// <returns>The given directory</returns>
        public Models.VsrDirectory Deserialize(BinaryReader reader, Models.VsrDirectory directory)
        {
            foreach (var serializer in propertySerializers)
            {
                _ = serializer.Deserialize(reader, directory);
            }

            return directory;
        }

        public void Serialize(Models.VsrDirectory model, BinaryWriter writer)
        {
            throw new System.NotImplementedException();
        }
    }
}
