using System;

namespace LemballEditor.Models
{
    public class Level : ILevel
    {
        /// <summary>
        /// An unknown value. It's always either 6, 7, 9 or 10 in the official levels
        /// </summary>
        private ushort _unknownA;
        public ushort UnknownA
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
                    throw new ArgumentException("Value should be no larger than 599");
                }

                _timeLimitInSeconds = value;
            }
        }

        /// <summary>
        /// Creates a new level instance with sensible defaults
        /// </summary>
        public Level()
        {
            UnknownA = 9;
            Theme = LevelTheme.Grass;
            TimeLimitInSeconds = null;
        }
    }
}
