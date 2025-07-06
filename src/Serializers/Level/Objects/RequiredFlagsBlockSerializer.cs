using System.IO;

namespace LemballEditor.Serializers.Level.Objects
{
    public class RequiredFlagsBlockSerializer : ISerializer<PendingObjectList>
    {
        public PendingObjectList Deserialize(BinaryReader reader, PendingObjectList list)
        {
            var requiredFlags = reader.ReadInt16();

            if (requiredFlags != list.Level.FlagsRequired)
            {
                throw new InvalidDataException($"Required flags mismatch. Expected {list.Level.FlagsRequired}, but found {requiredFlags} at position {reader.BaseStream.Position - 2}.");
            }

            var unknownFlagIndicator = reader.ReadInt16();

            if (unknownFlagIndicator < 1 || unknownFlagIndicator > 4)
            {
                throw new InvalidDataException($"Unknown flag indicator {unknownFlagIndicator} at position {reader.BaseStream.Position - 2}. Expected a value between 1-4.");
            }

            list.Level.UnknownFlagIndicator = (byte)unknownFlagIndicator;

            return list;
        }

        public void Serialize(PendingObjectList model, BinaryWriter writer)
        {
            writer.Write((ushort)model.Level.FlagsRequired);
            writer.Write((ushort)model.Level.UnknownFlagIndicator);
        }
    }
}
