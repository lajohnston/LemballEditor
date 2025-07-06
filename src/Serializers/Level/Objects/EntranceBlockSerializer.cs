using System;
using System.IO;
using System.Linq;
using LemballEditor.Models.LevelObjects;

namespace LemballEditor.Serializers.Level.Objects
{
    public class EntranceBlockSerializer : ISerializer<PendingObjectList>
    {
        private readonly ISerializer<Position> positionSerializer;
        private readonly Func<Position, byte, Entrance> entranceFactory;

        public EntranceBlockSerializer(ISerializer<Position> positionSerializer, Func<Position, byte, Entrance> entranceFactory)
        {
            this.positionSerializer = positionSerializer;
            this.entranceFactory = entranceFactory;
        }

        public PendingObjectList Deserialize(BinaryReader reader, PendingObjectList list)
        {
            var entranceCount = reader.ReadInt16();

            for (var i = 0; i < entranceCount; i++)
            {
                var position = this.positionSerializer.Deserialize(reader, null);

                var unknown = reader.ReadUInt16();

                if (unknown != 0)
                {
                    throw new NotSupportedException($"Unexpected value {unknown} at position {reader.BaseStream.Position - 2}");
                }

                var numberOfLemmings = reader.ReadUInt16();

                if (numberOfLemmings < 1 || numberOfLemmings > 4)
                {
                    throw new InvalidDataException($"Unexpected number of lemmings value at position {reader.BaseStream.Position - 2}. Value must be between 1 and 4, but found {numberOfLemmings}");
                }

                var entrance = this.entranceFactory(position, (byte)numberOfLemmings);
                list.Add(entrance);
            }

            return list;
        }

        public void Serialize(PendingObjectList model, BinaryWriter writer)
        {
            var entrances = model.GetObjectsOfTypes(typeof(Entrance))
                .Select(po => po.LevelObject as Entrance);

            writer.Write((ushort)entrances.Count());

            foreach (var entrance in entrances)
            {
                this.positionSerializer.Serialize(entrance.Position, writer);
                writer.Write((ushort)0);
                writer.Write((ushort)entrance.NumberOfLemmings);
            }
        }
    }
}
