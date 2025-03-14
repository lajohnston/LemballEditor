using LemballEditor.Serializers.Level;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LemballEditor.Serializers.LevelGroup
{
    /// <summary>
    /// Serialises and deserialises LevelGroup data from VSR binary data
    /// </summary>
    public class LevelGroupSerializer : ISerializer<Models.LevelGroup>
    {
        /// <summary>
        /// An ordered list of sub-serializers to call
        /// </summary>
        private readonly List<ISerializer<PendingLevelGroup>> propertySerializers;

        public LevelGroupSerializer()
        {
            propertySerializers = new List<ISerializer<PendingLevelGroup>>()
            {
                new Constant<PendingLevelGroup>(Encoding.ASCII.GetBytes("CRID"), "Invalid directory header"),
                new DataSize(),
                new LevelCount(),
                new Constant<PendingLevelGroup>(BitConverter.GetBytes((uint) 3)),
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
            var pendingLevelGroup = new PendingLevelGroup();

            foreach (var serializer in propertySerializers)
            {
                _ = serializer.Deserialize(reader, pendingLevelGroup);
            }

            return levelGroup;
        }

        public void Serialize(Models.LevelGroup model, BinaryWriter writer)
        {
            throw new System.NotImplementedException();
        }
    }
}
