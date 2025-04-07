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
        /// The raw level directory data
        /// </summary>
        //public byte[] LevelData { get; set; }

        /// <summary>
        /// The level directory data, indexed by level group name
        /// </summary>
        private readonly Dictionary<LevelGroupName, byte[]> levelDirectoryData;

        /// <summary>
        /// The pointers to the level directories within the VSR
        /// </summary>
        private readonly Dictionary<LevelGroupName, uint> levelDirectoryPointers;

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

            levelDirectoryData = new Dictionary<LevelGroupName, byte[]>();
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
        /// Sets the raw level directory data for the given level group
        /// </summary>
        /// <param name="levelGroupName">The level group</param>
        /// <param name="data">The raw VSR binary data</param>
        public void SetLevelDirectoryData(LevelGroupName levelGroupName, byte[] data)
        {
            this.levelDirectoryData[levelGroupName] = data;
        }

        /// <summary>
        /// Retrieves the raw level directory data for the given level group
        /// </summary>
        /// <param name="levelGroupName">The level group</param>
        /// <returns>The raw VSR binary data</returns>
        public byte[] GetLevelDirectoryData(LevelGroupName levelGroupName)
        {
            return this.levelDirectoryData[levelGroupName];
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
    }
}
