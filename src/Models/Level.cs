using System;
using System.Collections.Generic;
using LemballEditor.Models.LevelObjects;

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
            get => this._flagsRequired;
            set
            {
                if (value > 4)
                {
                    throw new ArgumentException($"Max FlagsRequired is 4. {value} given");
                }

                this._flagsRequired = value;
            }
        }

        /// <summary>
        /// The level map/terrain
        /// </summary>
        public IMap Map { get; set; }

        /// <summary>
        /// The number of Lemmings in the level
        /// TODO - Compute this value from the entrances
        /// </summary>
        private byte _numberOfLemmings;
        public byte NumberOfLemmings
        {
            get => this._numberOfLemmings;
            set
            {
                if (value < 1 || value > 4)
                {
                    throw new ArgumentException($"NumberOfLemmings should be between 1-4. {value} given");
                }

                this._numberOfLemmings = value;
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
            get => this._timeLimitInSeconds;
            set
            {
                if (value > 599)
                {
                    throw new ArgumentException("TimeLimitInSeconds should be no larger than 599");
                }

                this._timeLimitInSeconds = value;
            }
        }

        /// <summary>
        /// An unknown value. It's always either 6, 7, 9 or 10 in the official levels
        /// </summary>
        private byte _unknownA;
        public byte UnknownA
        {
            get => this._unknownA;
            set
            {
                if (value != 6 && value != 7 && value != 9 && value != 10)
                {
                    throw new ArgumentException($"Expected UnknownA to be the value of 6, 7, 9 or 10. {value} given");
                }

                this._unknownA = value;
            }
        }

        /// <summary>
        /// An unknown value
        /// </summary>
        public ushort UnknownB { get; set; }

        /// <summary>
        /// List of all level objects
        /// </summary>
        private readonly List<ILevelObject> objects = new List<ILevelObject>();

        /// <summary>
        /// Creates a new level instance with sensible defaults
        /// </summary>
        public Level(IMap map)
        {
            this.FlagsRequired = 1;
            this.NumberOfLemmings = 1;
            this.Theme = LevelTheme.Grass;
            this.TimeLimitInSeconds = null;
            this.UnknownA = 10;
            this.Map = map;

            this.objects = new List<ILevelObject>();
        }

        public void AddObject(ILevelObject levelObject)
        {
            if (levelObject == null)
            {
                throw new ArgumentNullException(nameof(levelObject), "Cannot add a null object to the level");
            }

            this.objects.Add(levelObject);
        }

        public IReadOnlyList<ILevelObject> GetObjects()
        {
            return this.objects.AsReadOnly();
        }
    }
}
