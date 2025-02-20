namespace LemballEditor.Models
{
    /// <summary>
    /// A Lemmings Paintball level
    /// </summary>
    public interface ILevel
    {
        /// <summary>
        /// An unknown value. It's always either 6, 7, 9 or 10 in the official levels
        /// </summary>
        ushort UnknownA { get; set; }
    }
}