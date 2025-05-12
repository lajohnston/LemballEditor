using System;
using System.Collections.Generic;
using LemballEditor.Models;

namespace LemballEditor.Serializers.LevelDirectory
{
    /// <summary>
    /// Gathers temporary serialized data for deserializing a LevelGroup
    /// </summary>
    public class LevelDirectory
    {
        /// <summary>
        /// The absolute address of the LevelDirectory within the VSR file
        /// </summary>
        public uint Address { get; set; }

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
        /// The LevelGroup which will hold the levels
        /// </summary>
        public LevelGroup LevelGroup { get; set; }

        /// <summary>
        /// A collection of serialized levels
        /// </summary>
        private List<byte[]> serializedLevels;

        public LevelDirectory()
        {
            serializedLevels = new List<byte[]>();
        }

        /// <summary>
        /// Adds a serialized level at the end of the directory
        /// </summary>
        /// <param name="level">Serialized level data</param>
        /// <exception cref="InvalidOperationException">If the level directory fixed capacity is exceeded</exception>
        public void AddSerializedLevel(byte[] level)
        {
            if (serializedLevels.Count == this.FixedLevelCount)
            {
                throw new InvalidOperationException("Level directory fixed size exceeded");
            }

            serializedLevels.Add(level);
        }

        /// <summary>
        /// Returns an iterator for the serialized levels in the directory
        /// </summary>
        public IEnumerable<byte[]> GetSerializedLevels()
        {
            return serializedLevels.AsReadOnly();
        }
    }
}
