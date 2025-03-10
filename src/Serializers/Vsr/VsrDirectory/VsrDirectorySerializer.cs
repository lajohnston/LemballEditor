using LemballEditor.Serializers.Level;
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

        public VsrDirectorySerializer()
        {
            propertySerializers = new List<ISerializer<Models.VsrDirectory>>()
            {
                new Constant<Models.VsrDirectory>(Encoding.ASCII.GetBytes("CRID"), "Invalid directory header"),
                new DirectorySize(),
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="directory"></param>
        /// <returns></returns>
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
