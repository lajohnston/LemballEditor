using System.Collections.Generic;

namespace LemballEditor.Models
{
    public enum LevelGroupName
    {
        Fun,
        Tricky,
        Taxing,
        Mayhem,
        Network
    }

    /// <summary>
    /// Maintains a sequence of levels (such as Fun, Tricky, Taxing, Mayhem or Network)
    /// </summary>
    public class LevelGroup
    {
        /// <summary>
        /// The level sequence
        /// </summary>
        private readonly List<ILevel> levels;

        /// <summary>
        /// Creates an empty level group
        /// </summary>
        public LevelGroup()
        {
            levels = new List<ILevel>();
        }

        /// <summary>
        /// Returns the level at the given index
        /// </summary>
        /// <param name="index">The 0-based index of the level</param>
        /// <returns>The level at that index, or null if there is none</returns>
        public ILevel GetLevel(int index)
        {
            return index < 0 || index >= levels.Count ? null : levels[index];
        }

        /// <summary>
        /// Adds the given level at the end of the level group
        /// </summary>
        /// <param name="level">The level to add</param>
        public void AddLevel(ILevel level)
        {
            levels.Add(level);
        }
    }
}
