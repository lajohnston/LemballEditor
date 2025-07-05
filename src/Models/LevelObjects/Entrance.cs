using System;

namespace LemballEditor.Models.LevelObjects
{
    public class Entrance : ILevelObject
    {
        public Position Position { get; }

        public byte NumberOfLemmings { get; private set; }

        public Entrance(Position position, byte numberOfLemmings = 1)
        {
            this.Position = position;

            if (numberOfLemmings < 1 || numberOfLemmings > 4)
            {
                throw new ArgumentOutOfRangeException(nameof(numberOfLemmings), "Number of lemmings must be between 1 and 4.");
            }

            this.NumberOfLemmings = numberOfLemmings;
        }

        public ILevelObject WithPosition(Position position)
        {
            return new Entrance(position);
        }

        public Entrance WithNumberOfLemmings(byte numberOfLemmings)
        {
            return new Entrance(this.Position, numberOfLemmings);
        }
    }
}
