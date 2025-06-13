using System.IO;

namespace LemballEditor.Serializers.LevelDirectory
{
    public class LevelCountSerializer : ISerializer<LevelDirectorySerializer>
    {
        public LevelDirectorySerializer Deserialize(BinaryReader reader, LevelDirectorySerializer model)
        {
            var levelCount = reader.ReadUInt32();

            if (levelCount > 29)
            {
                throw new InvalidDataException($"Number of levels in level group higher than 29 maximum: {levelCount}");
            }

            model.FixedLevelCount = (byte)levelCount;

            return model;
        }

        public void Serialize(LevelDirectorySerializer model, BinaryWriter writer)
        {
            writer.Write((uint)model.FixedLevelCount);
        }
    }
}
