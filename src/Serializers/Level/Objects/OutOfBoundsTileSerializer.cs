using System.IO;

namespace LemballEditor.Serializers.Level.Objects
{
    public class OutOfBoundsTileBlockSerializer : ISerializer<PendingObjectList>
    {
        public PendingObjectList Deserialize(BinaryReader reader, PendingObjectList model)
        {
            var tileRef = reader.ReadUInt16();
            model.Level.Map.OutOfBoundsTileRef = tileRef;
            return model;
        }

        public void Serialize(PendingObjectList model, BinaryWriter writer)
        {
            var tileRef = model.Level.Map.OutOfBoundsTileRef;
            writer.Write(tileRef);
        }
    }
}