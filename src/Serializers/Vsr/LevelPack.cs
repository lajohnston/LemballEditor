using System;
using System.IO;
using LemballEditor.Models;

namespace LemballEditor.Serializers.Vsr
{
    /// <summary>
    /// Serializes/Deserializes levels to and from a VSR binary
    /// </summary>
    public class LevelPack : ISerializer<(Models.Vsr, Models.LevelPack)>
    {
        /// <summary>
        /// Serializer that serialises and deserialises data to and from a level pack
        /// </summary>
        private readonly ISerializer<LevelDirectory.LevelDirectory> levelDirectorySerializer;

        /// <summary>
        /// Level directory factory that creates a new LevelDirectory containing a LevelGroup of the given type
        /// </summary>
        private readonly Func<LevelGroupName?, LevelDirectory.LevelDirectory> levelDirectoryFactory;

        public LevelPack(
            ISerializer<LevelDirectory.LevelDirectory> levelDirectorySerializer,
            Func<LevelGroupName?, LevelDirectory.LevelDirectory> levelDirectoryFactory)
        {
            this.levelDirectorySerializer = levelDirectorySerializer;
            this.levelDirectoryFactory = levelDirectoryFactory;
        }

        /// <summary>
        /// Deserializes each level group from the VSR file and adds it to the LevelPack.
        /// </summary>
        public (Models.Vsr, Models.LevelPack) Deserialize(BinaryReader reader, (Models.Vsr, Models.LevelPack) models)
        {
            var (vsr, levelPack) = models;

            if (levelPack != null)
            {
                reader.BaseStream.Position = (int)vsr.FunAddress;

                foreach (LevelGroupName levelGroup in Enum.GetValues(typeof(LevelGroupName)))
                {
                    var levelDirectory = this.levelDirectoryFactory(levelGroup);
                    levelDirectory.Address = (uint)reader.BaseStream.Position;

                    var resultLevelDirectory = this.levelDirectorySerializer.Deserialize(reader, levelDirectory);
                    levelPack.SetLevelGroup(resultLevelDirectory.LevelGroup);
                }
            }

            return models;
        }

        public void Serialize((Models.Vsr, Models.LevelPack) models, BinaryWriter writer)
        {
            var (vsr, levelPack) = models;
            var nextFileId = vsr.FirstLevelFileId;

            foreach (var levelGroup in levelPack.GetLevelGroups())
            {
                var levelDirectory = this.levelDirectoryFactory(null);
                levelDirectory.Address = (uint)writer.BaseStream.Position;
                levelDirectory.FirstFileId = nextFileId;
                levelDirectory.FixedLevelCount = vsr.GetFixedLevelCount(levelGroup.LevelGroupName);
                levelDirectory.LevelGroup = levelGroup;

                nextFileId += levelDirectory.FixedLevelCount;

                // Address of level directory
                writer.BaseStream.Position = vsr.GetLevelDirectoryPointer(levelGroup.LevelGroupName);
                writer.Write(levelDirectory.Address);

                // Level directory data
                writer.BaseStream.Position = levelDirectory.Address;
                this.levelDirectorySerializer.Serialize(levelDirectory, writer);
            }
        }
    }
}
