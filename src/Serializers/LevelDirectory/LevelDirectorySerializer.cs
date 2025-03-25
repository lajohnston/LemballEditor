using LemballEditor.Serializers.Level;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LemballEditor.Serializers.LevelDirectory
{
    /// <summary>
    /// Serialises and deserialises LevelGroup data from VSR binary data
    /// </summary>
    public class LevelDirectorySerializer : ISerializer<LevelDirectory>
    {
        /// <summary>
        /// An ordered list of sub-serializers to call
        /// </summary>
        private readonly List<ISerializer<LevelDirectory>> propertySerializers;

        public LevelDirectorySerializer()
        {
            propertySerializers = new List<ISerializer<LevelDirectory>>()
            {
                new Constant<LevelDirectory>(Encoding.ASCII.GetBytes("CRID"), "Invalid directory header"),
                new DataSize(),
                new LevelCount(),
                new Constant<LevelDirectory>(BitConverter.GetBytes((uint) 3)),
                new AddressToFileDescriptors(),
                new FileNameList(),
            };
        }

        /// <summary>
        /// Deserialize a directory within a VSR file
        /// </summary>
        /// <param name="reader">Reader to read the input stream</param>
        /// <param name="levelGroup">The LevelGroup to set the files to</param>
        /// <returns>The given directory</returns>
        public LevelDirectory Deserialize(BinaryReader reader, LevelDirectory model)
        {
            foreach (var serializer in propertySerializers)
            {
                _ = serializer.Deserialize(reader, model);
            }

            return model;
        }

        public void Serialize(LevelDirectory model, BinaryWriter writer)
        {
            foreach (var serializer in propertySerializers)
            {
                serializer.Serialize(model, writer);
            }
        }
    }
}
