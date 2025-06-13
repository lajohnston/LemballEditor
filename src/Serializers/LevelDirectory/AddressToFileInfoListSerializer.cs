using System.IO;

namespace LemballEditor.Serializers.LevelDirectory
{
    /// <summary>
    /// The address of the file descriptors in the directory, bypassing the file names.
    /// </summary>
    public class AddressToFileInfoListSerializer : ISerializer<LevelDirectorySerializer>
    {
        private static readonly uint HEADER_SIZE = 20;
        private static readonly uint FILE_NAME_SIZE = 12;

        private uint CalculateValue(LevelDirectorySerializer directory)
        {
            return directory.Address + HEADER_SIZE + (directory.FixedLevelCount * FILE_NAME_SIZE);
        }

        public LevelDirectorySerializer Deserialize(BinaryReader reader, LevelDirectorySerializer levelDirectory)
        {
            var value = reader.ReadUInt32();
            var expected = CalculateValue(levelDirectory);

            if (value != expected)
            {
                throw new InvalidDataException($"File descriptor address invalid. Expected {expected} not {value}");
            }

            return levelDirectory;
        }

        public void Serialize(LevelDirectorySerializer levelDirectory, BinaryWriter writer)
        {
            writer.Write(CalculateValue(levelDirectory));
        }
    }
}
