using System.IO;

namespace LemballEditor.Serializers.Level.Objects
{
    public class GphsBlockSerializer : ISerializer<PendingObjectList>
    {
        public PendingObjectList Deserialize(BinaryReader reader, PendingObjectList list)
        {
            return list;
        }

        public void Serialize(PendingObjectList model, BinaryWriter writer)
        {

        }
    }
}
