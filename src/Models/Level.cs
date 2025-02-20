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
        /// Creates a new level instance with sensible defaults
        /// </summary>
        public Level()
        {
            UnknownA = 9;
            Theme = LevelTheme.Grass;
        }
    }
}
