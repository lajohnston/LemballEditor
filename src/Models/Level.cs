using System;

namespace LemballEditor.Models
{
    public class Level : ILevel
    {
        /// <summary>
        /// The number of flags required to win the level
        /// </summary>
        private byte _flagsRequired;
        public byte FlagsRequired
        {
            get => _flagsRequired;
            set
            {
                if (value < 1 || value > 4)
                {
                    throw new ArgumentException($"FlagsRequired should be between 1-4. {value} given");
                }

                _flagsRequired = value;
            }
        }

        /// <summary>
        /// The number of Lemmings in the level
        /// TODO - Compute this value from the entrances
        /// </summary>
        private byte _numberOfLemmings;
        public byte NumberOfLemmings
        {
            get => _numberOfLemmings;
            set
            {
                if (value < 1 || value > 4)
                {
                    throw new ArgumentException($"NumberOfLemmings should be between 1-4. {value} given");
                }

                _numberOfLemmings = value;
            }
        }

        /// <summary>
        /// The level's theme/graphical style
        /// </summary>
        public LevelTheme Theme { get; set; }

        /// <summary>
        /// The level time limit in seconds, or null if infinite. The max value is 599
        /// </summary>
        private ushort? _timeLimitInSeconds;
        public ushort? TimeLimitInSeconds
        {
            get => _timeLimitInSeconds;
            set
            {
                if (value > 599)
                {
                    throw new ArgumentException("TimeLimitInSeconds should be no larger than 599");
                }

                _timeLimitInSeconds = value;
            }
        }

        /// <summary>
        /// An unknown value. It's always either 6, 7, 9 or 10 in the official levels
        /// </summary>
        private byte _unknownA;
        public byte UnknownA
        {
            get => _unknownA;
            set
            {
                if (value != 6 && value != 7 && value != 9 && value != 10)
                {
                    throw new ArgumentException($"Expected UnknownA to be the value of 6, 7, 9 or 10. {value} given");
                }

                _unknownA = value;
            }
        }

        /// <summary>
        /// Creates a new level instance with sensible defaults
        /// </summary>
        public Level()
        {
            NumberOfLemmings = 1;
            Theme = LevelTheme.Grass;
            TimeLimitInSeconds = null;
            UnknownA = 10;
        }
    }
}
