using System.Collections.Generic;
using LemballEditor.Models.LevelObjects;

namespace LemballEditor.Models
{
    /// <summary>
    /// The level's theme/graphical style
    /// </summary>
    public enum LevelTheme
    {
        Grass,
        Lego,
        Snow,
        Space
    };

    /// <summary>
    /// A Lemmings Paintball level
    /// </summary>
    public interface ILevel
    {
        /// <summary>
        /// The number of flags required to win
        /// </summary>
        byte FlagsRequired { get; set; }

        /// <summary>
        /// An unknown flag indicator. This is always 1 or 2 in the official levels.
        /// </summary>
        byte UnknownFlagIndicator { get; set; }

        /// <summary>
        /// The level map/terrain
        /// </summary>
        IMap Map { get; set; }

        /// <summary>
        /// The number of starting Lemmings (1-4)
        /// </summary>
        byte NumberOfLemmings { get; set; }

        /// <summary>
        /// The level's theme
        /// </summary>
        LevelTheme Theme { get; set; }

        /// <summary>
        /// The level time limit in seconds, or null if infinite. The max value is 599
        /// </summary>
        ushort? TimeLimitInSeconds { get; set; }

        /// <summary>
        /// An unknown value. It's always either 6, 7, 9 or 10 in the official levels
        /// </summary>
        byte UnknownA { get; set; }

        /// <summary>
        /// An unknown value
        /// </summary>
        ushort UnknownB { get; set; }

        /// <summary>
        /// Add a level object to the level
        /// </summary>
        void AddObject(ILevelObject levelObject);

        /// <summary>
        /// Return a list of all objects in the level
        /// </summary>
        /// <returns></returns>
        IReadOnlyList<ILevelObject> GetObjects();

        /// <summary>
        /// The level name
        /// </summary>
        string Name { get; set; }
    }
}