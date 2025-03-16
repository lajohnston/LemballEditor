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
    public class LevelDirectorySerializer : ISerializer<ValueTuple<Models.LevelGroup, LevelDirectoryContext>>
    {
        /// <summary>
        /// An ordered list of sub-serializers to call
        /// </summary>
        private readonly List<ISerializer<LevelDirectoryContext>> propertySerializers;

        public LevelDirectorySerializer()
        {
            propertySerializers = new List<ISerializer<LevelDirectoryContext>>()
            {
                new Constant<LevelDirectoryContext>(Encoding.ASCII.GetBytes("CRID"), "Invalid directory header"),
                new DataSize(),
                new LevelCount(),
                new Constant<LevelDirectoryContext>(BitConverter.GetBytes((uint) 3)),
            };
        }

        /// <summary>
        /// Deserialize a directory within a VSR file
        /// </summary>
        /// <param name="reader">Reader to read the input stream</param>
        /// <param name="levelGroup">The LevelGroup to set the files to</param>
        /// <returns>The given directory</returns>
        public (Models.LevelGroup, LevelDirectoryContext) Deserialize(BinaryReader reader, (Models.LevelGroup, LevelDirectoryContext) models)
        {
            var (levelGroup, context) = models;

            foreach (var serializer in propertySerializers)
            {
                _ = serializer.Deserialize(reader, context);
            }

            return models;
        }

        public void Serialize((Models.LevelGroup, LevelDirectoryContext) model, BinaryWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
