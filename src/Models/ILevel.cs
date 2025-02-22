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
    }
}