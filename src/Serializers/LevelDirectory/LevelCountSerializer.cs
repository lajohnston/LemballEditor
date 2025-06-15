using System.IO;

namespace LemballEditor.Serializers.LevelDirectory
{
    public class LevelCountSerializer : ISerializer<PendingLevelGroup>
    {
        public PendingLevelGroup Deserialize(BinaryReader reader, PendingLevelGroup model)
        {
            var levelCount = reader.ReadUInt32();

            if (levelCount > 29)
            {
                throw new InvalidDataException($"Number of levels in level group higher than 29 maximum: {levelCount}");
            }

            model.FixedLevelCount = (byte)levelCount;

            return model;
        }

        public void Serialize(PendingLevelGroup model, BinaryWriter writer)
        {
            writer.Write((uint)model.FixedLevelCount);
        }
    }
}
