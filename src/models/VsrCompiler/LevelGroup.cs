using System;
using System.IO;

namespace VsrCompiler
{
    internal class LevelGroup
    {
        /// <summary>
        /// An array containing the compiled levels that are in this level group
        /// </summary>
        private readonly CompiledLevel[] compiledUserLevels;

        /// <summary>
        /// A blank level that is used to fill up the directory the the hardcoded number of levels
        /// </summary>
        private static CompiledLevel blankLevel;

        /// <summary>
        /// The total number of levels that this LevelGroup is hardcoded by Lemball to contain
        /// </summary>
        private readonly int fixedNumberOfLevels;

        /// <summary>
        /// Loads a LevelGroup from a directory within a level pack
        /// </summary>
        /// <param name="levelPackData">The reader that is reading the level pack</param>
        /// <param name="maxLevels">The set number of levels that this group is hardcoded by Lemball to contain</param>
        public LevelGroup(MemoryStream dataStream, int numberOfLevels)
        {
            // Create reader to read level pack data
            BinaryReader levelPackData = new BinaryReader(dataStream);

            // Set the hardcoded number of levels
            fixedNumberOfLevels = numberOfLevels;

            // The blank level is created when needed
            blankLevel = null;

            // Ensure level pack reader position is at the beginning of a directory
            if (new string(levelPackData.ReadChars(3)) != "DIR")
            {
                throw new InvalidDataException();
            }

            // Read the number of user levels within the current level pack directory
            int userLevels = levelPackData.ReadByte();

            // Create an array to hold the levels
            compiledUserLevels = new CompiledLevel[userLevels];

            // Read each user level
            for (int level = 0; level < userLevels; level++)
            {
                // Read level
                compiledUserLevels[level] = new CompiledLevel(levelPackData);
            }
        }

        /// <summary>
        /// Compiles a VSR directory that contains the user's levels
        /// </summary>
        /// <param name="binary">The binary editor being used to create the VSR file</param>
        /// <param name="currentFileId">The current file id</param>
        public void compileVSRDirectory(BinaryEditor binary, ref int currentFileId)
        {
            // Directory header
            binary.Append("CRID");

            // Size (set later)
            uint directorySizePointer = binary.getSize();
            binary.Append("NULL");

            // Number of files in directory (hardcoded number of levels)
            binary.Append(fixedNumberOfLevels);

            // Unknown, always 3
            binary.Append(3);

            // Address of the first file info block
            binary.Append((int)binary.getSize() + 4 + (fixedNumberOfLevels * 12));

            // Write file names
            uint fileNamesAddress = binary.getSize();
            writeFileNames(binary);

            // Write file info block
            uint fileInfoBlockAddress = binary.getSize();
            writeFileInfoBlocks(binary, fileNamesAddress, ref currentFileId);

            // Write each level
            uint fileAddrPointer = fileInfoBlockAddress + 12;
            for (int levelNumber = 0; levelNumber < fixedNumberOfLevels; levelNumber++)
            {
                // Set file address in file info block
                binary.Set(fileAddrPointer, binary.getSize());

                // Header
                binary.Append(" NIB");

                // File size (set later)
                uint fileSizePointer = binary.getSize();
                binary.Append("SIZE");

                // Append compiled level file
                uint sizeBeforeFile = binary.getSize();
                binary.Append(getLevel(levelNumber).getData());

                // Calculate file size
                uint fileSize = binary.getSize() - sizeBeforeFile;

                // Set size in file info block
                binary.Set(fileAddrPointer + 4, fileSize);

                // Set size at beginning of level file
                binary.Set(fileSizePointer, fileSize);

                // Next file address pointer
                fileAddrPointer += 36;
            }

            // Set directory size
            /* 
             * Relative to the size pointer, should point to the beginning of the '?DNE' tag of
             * the last level in the directory (the '?')
             */
            uint size = binary.getSize() - directorySizePointer - 4;
            binary.Set(directorySizePointer, size);
        }

        /// <summary>
        /// Writes the file names stored within the directory
        /// </summary>
        /// <param name="binary">The binary editor to use to write the names</param>
        private void writeFileNames(BinaryEditor binary)
        {
            // First part of each file name
            string fileName = "Level_";

            // Each file name
            for (int fileNumber = 0; fileNumber < fixedNumberOfLevels; fileNumber++)
            {
                // Append 'Level_' part of file name
                binary.Append(fileName);

                // Append level number
                // Append 0 if necessary, to make number two digits long (i.e. '05')
                if (fileNumber < 10)
                {
                    binary.Append("0");
                }

                // Level number
                binary.Append(fileNumber.ToString());

                // Null padding (4-bytes)
                binary.Append(0);
            }
        }

        /// <summary>
        /// Writes the file info block, which resides beneath the file names and stores
        /// the file id, address, type, and size
        /// </summary>
        /// <param name="binary">The BinaryEditor to write the info blocks to</param> 
        /// <param name="firstFileNameAddress">The address of the first file name in the directory</param>
        /// <param name="fileId">The next file id to use</param>
        private void writeFileInfoBlocks(BinaryEditor binary, long firstFileNameAddress, ref int nextFileId)
        {
            // The file type for each file
            string dataType = " NIB";

            // Initialise the fileNameAddress to the first file name
            int fileNameAddress = (int)firstFileNameAddress;

            // Each file block
            for (int fileNumber = 0; fileNumber < fixedNumberOfLevels; fileNumber++)
            {
                // Address of the file name
                binary.Append(fileNameAddress);

                // Append file Id, and increment for the next file
                binary.Append(nextFileId);
                nextFileId++;

                // Data type (BIN)
                binary.Append(dataType);

                // Address (value set later)
                binary.Append("ADDR");

                // File size (set later)
                binary.Append("SIZE");

                // Null 16 bytes
                binary.Append((long)0);
                binary.Append((long)0);

                // Add 12 to file name address to get next file name address;
                fileNameAddress += 12;
            }
        }

        /// <summary>
        /// Get the level with the specified (0-based) number. Returns a user level
        /// if there is one, otherwise returns the blank level which is used to pad the
        /// directory so that it contains the fixed number of levels required by Lemball
        /// </summary>
        /// <param name="levelNumber">The level number (0-based)</param>
        /// <returns>A compiled user level, or a blank level</returns>
        public CompiledLevel getLevel(int levelNumber)
        {
            return levelNumber > compiledUserLevels.Length - 1 ? getNullLevel() : compiledUserLevels[levelNumber];
        }

        /// <summary>
        /// Gets a blank level, which is used to pad the directory to contain the hardcoded
        /// number of levels that Lemball requires it to have
        /// </summary>
        /// <returns>A blank level</returns>
        private static CompiledLevel getNullLevel()
        {
            // Create the blank level if it doesn't already exist
            if (blankLevel == null)
            {
                byte[] data = Convert.FromBase64String("ICBJQRIAAAAJAAAAWAIBAAEAAABGU0RHEgAAAAEAAQAJAgAAAAAAAEJPTUcMAAAAAAAAAFlNTkUKAAAAAAAAAEdQSFMKAAAAAAAAAEVET04KAAAAAAAAAExMQUIKAAAAAAAAAEVOSU0KAAAAAAAAAExMT0MKAAAAAAAAAE1JTkEKAAAAAAAAAFRGSUwKAAAAAAAAAFJPT0QKAAAAAAAAAEtDT1IKAAAAAAAAAEROQUgKAAAAAAAAAFJTQUwKAAAAAAAAAE5PT0IiAAAADwBapVqlWqVapVqlWqVapVqlWqVapVqlWqUAAEVNQU4oAAAAKEJsYW5rKQC7u7u7u7u7u7u7u7u7u7u7u7u7u7u7u7tNQVJUCgAAAAAAAAAgRUNJCgAAAAAAAABFVk9NCgAAAAAAAABOVUdQCgAAAAAAAAAxU0xQDAAAAAAACQJHQUxGDAAAAAEAAgBURkVEDAAAAAYCAABLTkxTCgAAAAAAAABTVk5JCgAAAAAAAABXVEVOCgAAAAAAAAA/RE5F");
                blankLevel = new CompiledLevel(data);
            }

            return blankLevel;
        }
    }
}