using System.IO;

namespace VsrCompiler
{
    /// <summary>
    /// Contains compiled Lemball-compatible level data
    /// </summary>
    internal class CompiledLevel
    {
        /// <summary>
        /// The compiled level data
        /// </summary>
        private readonly byte[] levelData;

        /// <summary>
        /// Loads a level from a level pack
        /// </summary>
        /// <param name="levelPackData">The reader that is reading the level pack. Must be at the beginning
        /// of the level file.</param>
        public CompiledLevel(BinaryReader levelPackData)
        {
            // Verify that position is at the beginning of a level
            if (new string(levelPackData.ReadChars(4)) != "LEV ")
            {
                throw new InvalidDataException();
            }

            // Get data size
            int size = levelPackData.ReadInt32();

            // Load compiled level into the byte array
            levelData = levelPackData.ReadBytes(size);

            // Verify that all the file has been loaded
            if (!DataIsValid())
            {
                throw new InvalidDataException();
            }
        }

        /// <summary>
        /// Loads a level from a separate file
        /// </summary>
        public CompiledLevel(string levelFilePath)
        {
            using (FileStream file = new FileStream(levelFilePath, FileMode.Open, FileAccess.Read))
            {
                // Create byte array
                levelData = new byte[file.Length];

                // Read data into levelData array
                _ = file.Read(levelData, 0, levelData.Length);

                // Verify that all the file has been loaded
                if (!DataIsValid())
                {
                    throw new InvalidDataException();
                }
            }
        }

        public CompiledLevel(byte[] data)
        {
            levelData = data;
        }

        /// <summary>
        /// Performs simple verification to ensure that the level data is valid
        /// </summary>
        /// <returns>true if the data is valid, otherwise false</returns>
        private bool DataIsValid()
        {
            // First four bytes should be '  IA'
            if (!readAndCompareString(0, "  IA"))
            {
                return false;
            }

            // The four bytes from addr.20 should be 'FSDG'
            if (!readAndCompareString(20, "FSDG"))
            {
                return false;
            }

            // Last four bytes should be '?DNE'
            if (!readAndCompareString(levelData.Length - 4, "?DNE"))
            {
                return false;
            }

            // All tests have been passed
            return true;
        }

        /// <summary>
        /// Reads a string stored within the level data and compare it to another string
        /// </summary>
        /// <param name="startAddr">The address to read from</param>
        /// <param name="testValue">The value to test for</param>
        /// <returns>true if the string matches, otherwise false</returns>
        private bool readAndCompareString(int startAddr, string testValue)
        {
            int strLength = testValue.Length;
            int dataLength = levelData.Length;
            char[] readString = new char[strLength];

            // Read each byte into a char array
            for (int i = 0; i < strLength && startAddr + i < dataLength; i++)
            {
                readString[i] = (char)levelData[startAddr + i];
            }

            // Compare the char array to the test value
            return new string(readString) == testValue;
        }

        /// <summary>
        /// Gets the compiled level data
        /// </summary>
        /// <returns>The compiled level data, stored in a byte array</returns>
        public byte[] getData()
        {
            return levelData;
        }
    }
}
