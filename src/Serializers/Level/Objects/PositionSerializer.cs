using System;
using System.IO;
using LemballEditor.Models.LevelObjects;

namespace LemballEditor.Serializers.Level.Objects
{
    public class PositionSerializer : ISerializer<Position>
    {
        private readonly Func<ushort, ushort, Position> createPosition;

        public PositionSerializer(Func<ushort, ushort, Position> createPosition)
        {
            this.createPosition = createPosition;
        }

        public Position Deserialize(BinaryReader reader, Position model)
        {
            var positionX = reader.ReadUInt16();
            var positionY = reader.ReadUInt16();
            return this.createPosition(positionX, positionY);
        }

        public void Serialize(Position model, BinaryWriter writer)
        {
            writer.Write(model.X);
            writer.Write(model.Y);
        }
    }
}
