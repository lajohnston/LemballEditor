using System.IO;
using System.Text;

namespace LemballEditor.Serializers.LevelDirectory
{
    public class FileInfoListSerializer : ISerializer<PendingLevelGroup>
    {
        public PendingLevelGroup Deserialize(BinaryReader reader, PendingLevelGroup model)
        {
            _ = reader.BaseStream.Seek(model.FixedLevelCount * 36, SeekOrigin.Current);

            return model;
        }

        /// <summary>
        /// Writes the file header information to the stream
        /// </summary>
        public void Serialize(PendingLevelGroup model, BinaryWriter writer)
        {
            var levels = model.GetSerializedLevels();

            var fileNameAddress = model.Address + 20;
            var fileId = model.FirstFileId;

            var directoryHeaderSize = 20 + (12 * model.FixedLevelCount) + (36 * model.FixedLevelCount);
            var levelAddress = model.Address + directoryHeaderSize;

            foreach (var level in levels)
            {
                writer.Write(fileNameAddress);
                writer.Write(fileId++);
                writer.Write(Encoding.ASCII.GetBytes(" NIB"));
                writer.Write((uint)levelAddress);
                writer.Write((uint)level.Length);
                writer.Write(new byte[16]);

                fileNameAddress += 12;
                levelAddress += (uint)level.Length;
            }
        }
    }
}
