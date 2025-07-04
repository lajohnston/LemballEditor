using System;
using System.IO;

namespace LemballEditor.Serializers.Level.Objects
{
    public class BalloonBlockSerializer : ISerializer<PendingObjectList>
    {
        public PendingObjectList Deserialize(BinaryReader reader, PendingObjectList model)
        {
            var unknown = reader.ReadUInt16(); // Read unknown value (related to balloon post count)

            if (unknown != 0)
            {
                throw new NotSupportedException($"Unsupported balloon block value: {unknown} as position {reader.BaseStream.Position - 2}");
            }

            _ = reader.ReadBytes(24); // Skip 24 bytes of unknown data

            return model;
        }

        public void Serialize(PendingObjectList model, BinaryWriter writer)
        {
            writer.Write((ushort)0); // unknown value (related to balloon post count)
            writer.Write(new byte[24]);
        }
    }
}
