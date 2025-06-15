using System.IO;

namespace LemballEditor.Serializers.LevelDirectory
{
    /// <summary>
    /// The address of the file descriptors in the directory, bypassing the file names.
    /// </summary>
    public class AddressToFileInfoListSerializer : ISerializer<PendingLevelGroup>
    {
        private static readonly uint HEADER_SIZE = 20;
        private static readonly uint FILE_NAME_SIZE = 12;

        private uint CalculateValue(PendingLevelGroup directory)
        {
            return directory.Address + HEADER_SIZE + (directory.FixedLevelCount * FILE_NAME_SIZE);
        }

        public PendingLevelGroup Deserialize(BinaryReader reader, PendingLevelGroup levelDirectory)
        {
            var value = reader.ReadUInt32();
            var expected = this.CalculateValue(levelDirectory);

            return value != expected
                ? throw new InvalidDataException($"File descriptor address invalid. Expected {expected} not {value}")
                : levelDirectory;
        }

        public void Serialize(PendingLevelGroup levelDirectory, BinaryWriter writer)
        {
            writer.Write(this.CalculateValue(levelDirectory));
        }
    }
}
