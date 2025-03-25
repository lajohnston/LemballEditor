using LemballEditor.Serializers.Level;
using System.IO;

namespace LemballEditor.Serializers.LevelDirectory
{
    /// <summary>
    /// The address of the file descriptors in the directory, bypassing the file names.
    /// </summary>
    public class AddressToFileDescriptors : ISerializer<LevelDirectory>
    {
        private static readonly uint HEADER_SIZE = 16;
        private static readonly uint FILE_NAME_SIZE = 12;

        private uint CalculateValue(LevelDirectory directory)
        {
            return directory.DirectoryAddress + HEADER_SIZE + (directory.FixedLevelCount * FILE_NAME_SIZE);
        }

        public LevelDirectory Deserialize(BinaryReader reader, LevelDirectory levelDirectory)
        {
            var value = reader.ReadUInt32();
            var expected = CalculateValue(levelDirectory);

            if (value != expected)
            {
                throw new InvalidDataException($"File descriptor address invalid. Expected {expected} not {value}");
            }

            return levelDirectory;
        }

        public void Serialize(LevelDirectory levelDirectory, BinaryWriter writer)
        {
            writer.Write(CalculateValue(levelDirectory));
        }
    }
}
