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
    public class FixedLevelCount : ISerializer<Models.Vsr>
    {
        public Models.Vsr Deserialize(BinaryReader reader, Models.Vsr model)
        {
            var originalPosition = reader.BaseStream.Position;
            reader.BaseStream.Position = (int)model.FunAddress;

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

                model.SetFixedLevelCount(levelGroup, (byte)levelCount);

                reader.BaseStream.Position += dataSize - 4; // skip to the next directory
            }

            reader.BaseStream.Position = originalPosition; 

            return model;
        }

        public void Serialize(Models.Vsr model, BinaryWriter writer)
        {
            // Do nothing
        }
    }
}
