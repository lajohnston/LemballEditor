using System;
using System.IO;
using System.Linq;
using System.Text;
using LemballEditor.Models;

namespace LemballEditor.Serializers.Vsr
{
    /// <summary>
    /// Deserialises the fixed level count for each level group in the VSR file.
    /// </summary>
    public class FixedLevelCountSerializer : ISerializer<(Models.Vsr, Models.LevelPack)>
    {
        public (Models.Vsr, Models.LevelPack) Deserialize(BinaryReader reader, (Models.Vsr, Models.LevelPack) models)
        {
            var (vsr, _) = models;
            var originalPosition = reader.BaseStream.Position;
            reader.BaseStream.Position = (int)vsr.FunAddress;

            foreach (var levelGroup in Enum.GetValues(typeof(LevelGroupName)).Cast<LevelGroupName>())
            {
                var header = Encoding.ASCII.GetString(reader.ReadBytes(4));

                if (header != "CRID")
                {
                    throw new InvalidDataException($"Invalid header '{header}' for level group '{levelGroup}'");
                }

                var dataSize = reader.ReadInt32();
                var levelCount = reader.ReadUInt32();

                if (levelCount > 29)
                {
                    throw new InvalidDataException($"Invalid level count '{levelCount}' for level group '{levelGroup}'. Maximum allowed is 29.");
                }

                vsr.SetFixedLevelCount(levelGroup, (byte)levelCount);

                reader.BaseStream.Position += dataSize - 4; // skip to the next directory
            }

            reader.BaseStream.Position = originalPosition; 

            return models;
        }

        public void Serialize((Models.Vsr, Models.LevelPack) models, BinaryWriter writer)
        {
            // Do nothing
        }
    }
}
