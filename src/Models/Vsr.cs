using System.Collections.Generic;

namespace LemballEditor.Models
{
    /// <summary>
    /// The VSR resource file containing the game assets and levels
    /// </summary>
    public class Vsr
    {
        /// <summary>
        /// The static asset data, excluding the level directories
        /// </summary>
        public byte[] AssetData { get; set; }

        /// <summary>
        /// The pointers to the level directories within the VSR
        /// </summary>
        private readonly Dictionary<LevelGroupName, uint> levelDirectoryPointers;

        /// <summary>
        /// The fixed number of levels each level group should contain
        /// </summary>
        private readonly Dictionary<LevelGroupName, byte> fixedLevelCounts;

        /// <summary>
        /// The file ID of the first level in the FUN level group
        /// </summary>
        public uint FirstLevelFileId { get; set; } = 0;

        public Vsr()
        {
            levelDirectoryPointers = new Dictionary<LevelGroupName, uint>
            {
                { LevelGroupName.Fun, 0 },
                { LevelGroupName.Tricky, 0 },
                { LevelGroupName.Taxing, 0 },
                { LevelGroupName.Mayhem, 0 },
                { LevelGroupName.Network, 0 }
            };

            fixedLevelCounts = new Dictionary<LevelGroupName, byte>
            {
                { LevelGroupName.Fun, 0 },
                { LevelGroupName.Tricky, 0 },
                { LevelGroupName.Taxing, 0 },
                { LevelGroupName.Mayhem, 0 },
                { LevelGroupName.Network, 0 }
            };
        }

        /// <summary>
        /// The address of the Fun level directory, or null if unknown
        /// </summary>
        public uint? FunAddress
        {
            get
            {
                if (AssetData == null)
                {
                    return null;
                }

                return (uint)AssetData.Length;
            }
        }

        /// <summary>
        /// Sets the directory pointer for the given level group
        /// </summary>
        /// <param name="levelGroupName">The level group name</param>
        /// <param name="pointerAddress">The pointer address</param>
        public void SetLevelDirectoryPointer(LevelGroupName levelGroupName, uint pointerAddress)
        {
            levelDirectoryPointers[levelGroupName] = pointerAddress;
        }

        /// <summary>
        /// Retrieves the directory pointer for the given level group
        /// </summary>
        /// <param name="levelGroupName">The level group name</param>
        /// <returns>The pointer address</returns>
        public uint GetLevelDirectoryPointer(LevelGroupName levelGroupName)
        {
            return levelDirectoryPointers[levelGroupName];
        }

        /// <summary>
        /// Retrieve the fixed number of levels for the given level group
        /// </summary>
        public byte GetFixedLevelCount(LevelGroupName levelGroupName)
        {
            return fixedLevelCounts[levelGroupName];
        }

        /// <summary>
        /// Sets the fixed number of levels for the given level group
        /// </summary>
        public void SetFixedLevelCount(LevelGroupName levelGroupName, byte levelCount)
        {
            fixedLevelCounts[levelGroupName] = levelCount;
        }
    }
}
