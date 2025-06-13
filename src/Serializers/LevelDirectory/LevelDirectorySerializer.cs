using System;
using System.Collections.Generic;
using LemballEditor.Models;

namespace LemballEditor.Serializers.LevelDirectory
{
    /// <summary>
    /// Gathers temporary serialized data for deserializing a LevelGroup
    /// </summary>
    public class LevelDirectorySerializer
    {
        /// <summary>
        /// Blank level data used to fill up the directory to the hardcoded number of levels
        /// </summary>
        private readonly static byte[] BLANK_LEVEL = Convert.FromBase64String("ICBJQRIAAAAJAAAAWAIBAAEAAABGU0RHEgAAAAEAAQAJAgAAAAAAAEJPTUcMAAAAAAAAAFlNTkUKAAAAAAAAAEdQSFMKAAAAAAAAAEVET04KAAAAAAAAAExMQUIKAAAAAAAAAEVOSU0KAAAAAAAAAExMT0MKAAAAAAAAAE1JTkEKAAAAAAAAAFRGSUwKAAAAAAAAAFJPT0QKAAAAAAAAAEtDT1IKAAAAAAAAAEROQUgKAAAAAAAAAFJTQUwKAAAAAAAAAE5PT0IiAAAADwBapVqlWqVapVqlWqVapVqlWqVapVqlWqUAAEVNQU4oAAAAKEJsYW5rKQC7u7u7u7u7u7u7u7u7u7u7u7u7u7u7u7tNQVJUCgAAAAAAAAAgRUNJCgAAAAAAAABFVk9NCgAAAAAAAABOVUdQCgAAAAAAAAAxU0xQDAAAAAAACQJHQUxGDAAAAAEAAgBURkVEDAAAAAYCAABLTkxTCgAAAAAAAABTVk5JCgAAAAAAAABXVEVOCgAAAAAAAAA/RE5F");

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

        public LevelDirectorySerializer()
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
            foreach (var level in serializedLevels)
            {
                yield return level;
            }

            if (serializedLevels.Count < this.FixedLevelCount)
            {
                for (int i = serializedLevels.Count; i < this.FixedLevelCount; i++)
                {
                    yield return BLANK_LEVEL;
                }
            }
        }

        /// <summary>
        /// Returns the size of the serialized level data in bytes
        /// </summary>
        public uint GetDataSizeInBytes()
        {
            uint size = 0;

            foreach (var level in this.GetSerializedLevels())
            {
                size += (uint)level.Length;
            }

            return size;
        }
    }
}
