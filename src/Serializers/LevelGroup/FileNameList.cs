using LemballEditor.Serializers.Level;
using System.IO;
using System.Text;

namespace LemballEditor.Serializers.LevelGroup
{
    /// <summary>
    /// Serializes/Deserializes the level file names (not the in-game level titles)
    /// </summary>
    public class FileNameList : ISerializer<LevelGroupContext>
    {
        /// <summary>
        /// Validates the file name list and returns the given mode if it's valid
        /// </summary>
        /// <exception cref="InvalidDataException">If the file name list contains unexpected data</exception>
        public LevelGroupContext Deserialize(BinaryReader reader, LevelGroupContext model)
        {
            for (var levelNumber = 0; levelNumber < model.LevelCount; levelNumber++)
            {
                var name = Encoding.ASCII.GetString(reader.ReadBytes(8));

                if (name != "Level_" + levelNumber.ToString("00"))
                {
                    throw new InvalidDataException("Invalid file name");
                }

                var padding = reader.ReadUInt32();

                if (padding != 0)
                {
                    throw new InvalidDataException("Invalid data found after file name");
                }
            }

            return model;
        }

        /// <summary>
        /// Writes the sequential file name list to the stream
        /// </summary>
        public void Serialize(LevelGroupContext model, BinaryWriter writer)
        {
            for (var levelNumber = 0; levelNumber < model.LevelCount; levelNumber++)
            {
                writer.Write(Encoding.ASCII.GetBytes("Level_" + levelNumber.ToString("00")));
                writer.Write(new byte[4]);
            }
        }
    }
}
