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
        /// An unknown value. It's always either 6, 7, 9 or 10 in the official levels
        /// </summary>
        ushort UnknownA { get; set; }

        /// <summary>
        /// The level's theme
        /// </summary>
        LevelTheme Theme { get; set; }
    }
}