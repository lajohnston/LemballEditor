using System;
using System.IO;
using LemballEditor.Models.LevelObjects;

namespace LemballEditor.Serializers.Level.Objects
{
    public class CollectableBlockSerializer : ISerializer<PendingObjectList>
    {
        private readonly Func<Position, Flag> createFlag;

        private readonly ISerializer<Position> positionSerializer;

        public CollectableBlockSerializer(ISerializer<Position> positionSerializer, Func<Position, Flag> createFlag)
        {
            this.positionSerializer = positionSerializer;
            this.createFlag = createFlag;
        }

        public PendingObjectList Deserialize(BinaryReader reader, PendingObjectList list)
        {
            var itemCount = reader.ReadInt16();

            for (var i = 0; i < itemCount; i++)
            {
                var id = reader.ReadUInt16(); // item ID
                var type = reader.ReadUInt16();
                var position = this.positionSerializer.Deserialize(reader, null);
                _ = reader.ReadUInt16(); // padding

                switch (type)
                {
                    case 12:
                        var flag = this.createFlag(position);
                        list.Add(flag, id);
                        break;
                    default:
                        var address = reader.BaseStream.Position - 6;
                        throw new NotSupportedException($"Unspported item type {type} at position {address}");
                }
            }

            return list;
        }

        public void Serialize(PendingObjectList model, BinaryWriter writer)
        {
            throw new NotImplementedException("Item block serialization is not implemented yet.");
        }
    }
}
