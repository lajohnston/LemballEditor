using LemballEditor.Models;
using LemballEditor.Serializers.Level;
using System;
using System.IO;

namespace LemballEditor.Serializers
{
    public class LevelPackSerializer : ISerializer<LevelPack>
    {
        private readonly ISerializer<Models.LevelGroup> levelGroupSerializer;
        private readonly Func<Models.LevelGroup> levelGroupFactory;

        public LevelPackSerializer(ISerializer<Models.LevelGroup> levelGroupSerializer, Func<Models.LevelGroup> levelGroupFactory)
        {
            this.levelGroupSerializer = levelGroupSerializer;
            this.levelGroupFactory = levelGroupFactory;
        }

        /// <summary>
        /// Deserialize binary data containing level directories into a LevelPack instance
        /// </summary>
        /// <param name="reader">BinaryReader reading the stream</param>
        /// <param name="levelPack">The levelPack to apply the data to</param>
        /// <returns>The deserialized level pack data</returns>
        public LevelPack Deserialize(BinaryReader reader, LevelPack levelPack)
        {
            foreach (LevelGroupName levelGroupName in Enum.GetValues(typeof(LevelGroupName)))
            {
                var levelGroup = levelGroupSerializer.Deserialize(reader, levelGroupFactory());
                levelPack.SetLevelGroup(levelGroupName, levelGroup);
            }

            return levelPack;
        }

        public void Serialize(LevelPack result, BinaryWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
