using LemballEditor.Serializers.Level;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LemballEditor.Serializers.LevelGroup
{
    public class LevelGroupSerializer : ISerializer<Models.LevelGroup>
    {
        /// <summary>
        /// An ordered list of sub-serializers to call
        /// </summary>
        private readonly List<ISerializer<Models.LevelGroup>> propertySerializers;

        private readonly LevelCount fileCountSerializer;

        public LevelGroupSerializer()
        {
            fileCountSerializer = new LevelCount();

            propertySerializers = new List<ISerializer<Models.LevelGroup>>()
            {
                new Constant<Models.LevelGroup>(Encoding.ASCII.GetBytes("CRID"), "Invalid directory header"),
                new DataSize(),
                fileCountSerializer,
                new Constant<Models.LevelGroup>(BitConverter.GetBytes((uint) 3)),
            };
        }

        /// <summary>
        /// Deserialize a directory within a VSR file
        /// </summary>
        /// <param name="reader">Reader to read the input stream</param>
        /// <param name="directory">The directory model to set the files to</param>
        /// <returns>The given directory</returns>
        public Models.LevelGroup Deserialize(BinaryReader reader, Models.LevelGroup levelGroup)
        {
            foreach (var serializer in propertySerializers)
            {
                _ = serializer.Deserialize(reader, levelGroup);
            }

            return levelGroup;
        }

        public void Serialize(Models.LevelGroup model, BinaryWriter writer)
        {
            throw new System.NotImplementedException();
        }
    }
}
