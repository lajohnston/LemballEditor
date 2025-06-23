using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LemballEditor.Models.LevelObjects;

namespace LemballEditor.Serializers.Level.Objects
{
    public class CollectableBlockSerializer : ISerializer<PendingObjectList>
    {
        private readonly Func<Position, Flag> createFlag;

        private readonly ISerializer<Position> positionSerializer;

        private static readonly Dictionary<Type, ushort> TypeToIdMap = new Dictionary<Type, ushort>
        {
            { typeof(Flag), 12 }
        };

        private static readonly Dictionary<ushort, Type> IdToTypeMap = TypeToIdMap.ToDictionary(kvp => kvp.Value, kvp => kvp.Key);

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
                var id = reader.ReadUInt16();
                var typeId = reader.ReadUInt16();
                var position = this.positionSerializer.Deserialize(reader, null);
                _ = reader.ReadUInt16(); // skip padding

                var type = IdToTypeMap.TryGetValue(typeId, out var foundType)
                    ? foundType
                    : null;

                if (type == typeof(Flag))
                {
                    var flag = this.createFlag(position);
                    list.Add(flag, id);
                }
                else
                {
                    var address = reader.BaseStream.Position - 6;
                    throw new NotSupportedException($"Unsupported item type {type} at position {address}");
                }
            }

            return list;
        }

        public void Serialize(PendingObjectList list, BinaryWriter writer)
        {
            var collectables = list.GetObjectsOfTypes(typeof(Flag)).ToArray();

            writer.Write((ushort)collectables.Length);

            foreach (var collectable in collectables)
            {
                if (collectable.Id == null)
                {
                    throw new InvalidOperationException("Collectable must have an ID assigned before serialization.");
                }

                var typeId = this.GetTypeId(collectable.LevelObject);

                writer.Write((ushort)collectable.Id);
                writer.Write(typeId);
                this.positionSerializer.Serialize(collectable.LevelObject.GetPosition(), writer);
                writer.Write((ushort)0); // padding
            }
        }

        private ushort GetTypeId(ILevelObject levelObject)
        {
            return TypeToIdMap.TryGetValue(levelObject.GetType(), out var id)
                ? id
                : throw new NotSupportedException($"Unsupported collectable type: {levelObject.GetType().Name}");
        }
    }
}
