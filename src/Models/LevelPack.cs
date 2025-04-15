using System;
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
        /// <param name="groupName">The name of the level group</param>
        /// <returns>The level group</returns>
        public LevelGroup GetLevelGroup(LevelGroupName groupName)
        {
            return levelGroups[groupName];
        }

        /// <summary>
        /// Sets the level group for the given group name
        /// </summary>
        /// <param name="groupName">The name of the level group</param>
        /// <param name="levelGroup">The level group</returns>
        public void SetLevelGroup(LevelGroupName groupName, LevelGroup levelGroup)
        {
            levelGroups[groupName] = levelGroup ?? throw new ArgumentNullException();
        }

        /// <summary>
        /// Returns an enumerable to iterate through the level groups in order of difficulty
        /// (Fun, Tricky, Taxing, Mayhem, Network)
        /// </summary>
        /// <returns></returns>
        public IEnumerable<LevelGroup> GetLevelGroups()
        {
            yield return this.GetLevelGroup(LevelGroupName.Fun);
            yield return this.GetLevelGroup(LevelGroupName.Tricky);
            yield return this.GetLevelGroup(LevelGroupName.Taxing);
            yield return this.GetLevelGroup(LevelGroupName.Mayhem);
            yield return this.GetLevelGroup(LevelGroupName.Network);
        }
    }
}
