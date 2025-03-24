using LemballEditor.Serializers.Level;
using System.IO;

namespace LemballEditor.Serializers.LevelDirectory
{
    public class LevelCount : ISerializer<LevelDirectory>
    {
        public LevelDirectory Deserialize(BinaryReader reader, LevelDirectory model)
        {
            var levelCount = reader.ReadUInt32();

            if (levelCount > 29)
            {
                throw new InvalidDataException($"Number of levels in level group higher than 29 maximum: {levelCount}");
            }

            model.FixedLevelCount = (byte)levelCount;

            return model;
        }

        public void Serialize(LevelDirectory model, BinaryWriter writer)
        {
            writer.Write((uint)model.FixedLevelCount);
        }
    }
}
