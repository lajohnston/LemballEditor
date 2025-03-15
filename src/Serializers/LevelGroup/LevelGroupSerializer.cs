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
    public class LevelGroupSerializer : ISerializer<ValueTuple<Models.LevelGroup, LevelGroupContext>>
    {
        /// <summary>
        /// An ordered list of sub-serializers to call
        /// </summary>
        private readonly List<ISerializer<LevelGroupContext>> propertySerializers;

        public LevelGroupSerializer()
        {
            propertySerializers = new List<ISerializer<LevelGroupContext>>()
            {
                new Constant<LevelGroupContext>(Encoding.ASCII.GetBytes("CRID"), "Invalid directory header"),
                new DataSize(),
                new LevelCount(),
                new Constant<LevelGroupContext>(BitConverter.GetBytes((uint) 3)),
            };
        }

        /// <summary>
        /// Deserialize a directory within a VSR file
        /// </summary>
        /// <param name="reader">Reader to read the input stream</param>
        /// <param name="levelGroup">The LevelGroup to set the files to</param>
        /// <returns>The given directory</returns>
        public (Models.LevelGroup, LevelGroupContext) Deserialize(BinaryReader reader, (Models.LevelGroup, LevelGroupContext) models)
        {
            var (levelGroup, context) = models;

            var pendingLevelGroup = new LevelGroupContext();

            foreach (var serializer in propertySerializers)
            {
                _ = serializer.Deserialize(reader, pendingLevelGroup);
            }

            return models;
        }

        public void Serialize((Models.LevelGroup, LevelGroupContext) model, BinaryWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
