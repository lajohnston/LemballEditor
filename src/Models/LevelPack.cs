using System.Collections.Generic;

namespace LemballEditor.Models
{
    /// <summary>
    /// A LevelPack consisting of level groups (Fun, Tricky, Taxing, Mayhem, Network)
    /// </summary>
    public class LevelPack
    {
        /// <summary>
        /// The level groups, indexed by level group name
        /// </summary>
        private readonly Dictionary<LevelGroupName, LevelGroup> levelGroups;

        /// <summary>
        /// Initialises a LevelPack instance
        /// </summary>
        public LevelPack()
        {
            levelGroups = new Dictionary<LevelGroupName, LevelGroup>
            {
                { LevelGroupName.Fun, new LevelGroup() },
                { LevelGroupName.Tricky, new LevelGroup() },
                { LevelGroupName.Taxing, new LevelGroup() },
                { LevelGroupName.Mayhem, new LevelGroup() },
                { LevelGroupName.Network, new LevelGroup() }
            };
        }

        /// <summary>
        /// Retrieves the level group for the given group name
        /// </summary>
        /// <param name="group">The name of the level group</param>
        /// <returns>The level group</returns>
        public LevelGroup GetLevelGroup(LevelGroupName group)
        {
            return levelGroups[group];
        }
    }
}
