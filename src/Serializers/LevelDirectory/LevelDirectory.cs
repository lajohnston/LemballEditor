using LemballEditor.Models;

namespace LemballEditor.Serializers.LevelDirectory
{
    /// <summary>
    /// Gathers temporary serialized data for deserializing a LevelGroup
    /// </summary>
    public class LevelDirectory
    {
        /// <summary>
        /// The hard-coded number of levels the level group supports in the VSR file.
        /// If the number of levels falls below this the remainder should be filled with
        /// blank levels.
        /// </summary>
        public byte FixedLevelCount { get; set; }

        /// <summary>
        /// The ID of the first level in the directory
        /// </summary>
        public uint FirstFileId {  get; set; }

        /// <summary>
        /// The absolute address of the LevelDirectory within the VSR file
        /// </summary>
        public uint DirectoryAddress { get; set; }

        /// <summary>
        /// The LevelGroup which will hold the levels
        /// </summary>
        public LevelGroup LevelGroup { get; set; }
    }
}
