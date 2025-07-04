using System;
using System.IO;
using System.Text;

namespace LemballEditor.Serializers.Level.Objects
{
    public class NameBlockSerializer : ISerializer<PendingObjectList>
    {
        public PendingObjectList Deserialize(BinaryReader reader, PendingObjectList model)
        {
            var startPosition = reader.BaseStream.Position;
            var data = reader.ReadBytes(32);
            var nullIndex = Array.IndexOf(data, (byte)0);

            if (nullIndex == -1)
            {
                throw new InvalidDataException($"Level name at position {startPosition} does not contain a null terminator");
            }

            var name = Encoding.ASCII.GetString(data, 0, nullIndex);
            model.Level.Name = name;

            return model;
        }

        public void Serialize(PendingObjectList model, BinaryWriter writer)
        {
            var levelName = model.Level.Name ?? string.Empty;

            if (levelName.Length > 31)
            {
                throw new ArgumentException("Level name cannot exceed 31 characters.");
            }

            var nameBytes = Encoding.ASCII.GetBytes(levelName);
            writer.Write(nameBytes);
            writer.Write(new byte[32 - nameBytes.Length]);
        }
    }
}
