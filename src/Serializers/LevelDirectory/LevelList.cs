using System;
using System.IO;
using System.Text;
using LemballEditor.Models;

namespace LemballEditor.Serializers.LevelDirectory
{
    /// <summary>
    /// Serializes/Deserializes the levels in a level directory
    /// </summary>
    public class LevelList : ISerializer<LevelDirectory>
    {
        private readonly ISerializer<ILevel> levelSerializer;
        private readonly Func<ILevel> levelFactory;

        public LevelList(ISerializer<ILevel> levelSerializer, Func<ILevel> levelFactory)
        {
            this.levelSerializer = levelSerializer;
            this.levelFactory = levelFactory;
        }

        /// <summary>
        /// Deserializes each level in the directory and adds them to the level group
        /// </summary>
        public LevelDirectory Deserialize(BinaryReader reader, LevelDirectory model)
        {
            for (var i = 0; i < model.FixedLevelCount; i++)
            {
                reader.BaseStream.Position += 8;

                var levelModel = this.levelSerializer.Deserialize(reader, this.levelFactory());
                model.LevelGroup.AddLevel(levelModel);
            }

            return model;
        }

        /// <summary>
        /// Writes the serialized levels to the stream, including header information for each file
        /// </summary>
        public void Serialize(LevelDirectory model, BinaryWriter writer)
        {
            foreach (var level in model.GetSerializedLevels())
            {
                writer.Write(Encoding.ASCII.GetBytes(" NIB"));
                writer.Write((uint)level.Length);
                writer.Write(level);
            }
        }
    }
}
