using System;
using System.IO;
using LemballEditor.Models;
using LemballEditor.Models.LevelObjects;

namespace LemballEditor.Serializers.Level.Objects
{
    public class ItemBlockSerializer : ISerializer<ILevel>
    {
        private readonly Func<Position, Flag> createFlag;

        private readonly ISerializer<Position> positionSerializer;

        public ItemBlockSerializer(ISerializer<Position> positionSerializer, Func<Position, Flag> createFlag)
        {
            this.positionSerializer = positionSerializer;
            this.createFlag = createFlag;
        }

        public ILevel Deserialize(BinaryReader reader, ILevel level)
        {
            var itemCount = reader.ReadInt16();

            for (var i = 0; i < itemCount; i++)
            {
                _ = reader.ReadInt16(); // item ID
                var type = reader.ReadUInt16();
                var position = this.positionSerializer.Deserialize(reader, null);
                _ = reader.ReadUInt16(); // padding

                switch (type)
                {
                    case 12:
                        var flag = this.createFlag(position);
                        level.AddObject(flag);
                        break;
                    default:
                        var address = reader.BaseStream.Position - 6;
                        throw new NotSupportedException($"Unspported item type {type} at position {address}");
                }
            }

            return level;
        }

        public void Serialize(ILevel model, BinaryWriter writer)
        {
            throw new NotImplementedException("Item block serialization is not implemented yet.");
        }
    }
}
