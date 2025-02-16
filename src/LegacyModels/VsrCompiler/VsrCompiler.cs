using System.IO;

namespace VsrCompiler
{
    /// <summary>
    /// Compiles a level pack (which contains compiled user levels) into a Lemball
    /// compatible VSR resource file.
    /// </summary>
    public class VsrCompiler
    {
        /// <summary>
        /// The address that points to the address of the Fun directory
        /// </summary>
        private readonly int funPointer;

        /// <summary>
        /// The address of the Fun directory (start of the level portion)
        /// </summary>
        private readonly uint funAddress;

        /// <summary>
        /// The Vsr file id of the first level
        /// </summary>
        private readonly int firstLevelFileId;

        /// <summary>
        /// Whether the Lemball version is the full version (true) or the demo (false)
        /// </summary>
        public bool FullVersion { get; private set; }

        /// <summary>
        /// The path to the original Lemball Vsr file
        /// </summary>
        private readonly string sourceVsrPath;

        /// <summary>
        /// Creates a new VSRCompiler
        /// </summary>
        /// <exception cref="UnsupportedVSRException">If the VSR version is unsupported</exception>
        /// <param name="sourceVsrPath">The full path to the original VSR file</param>
        public VsrCompiler(string sourceVsrPath, string backupPath)
        {
            // Throw exception if Vsr path is invalid
            if (!File.Exists(sourceVsrPath))
            {
                throw new FileNotFoundException();
            }

            // Set sourceVSRPath
            this.sourceVsrPath = sourceVsrPath;

            // Read original VSR file
            using (FileStream originalVSR = new FileStream(sourceVsrPath, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader reader = new BinaryReader(originalVSR))
                {
                    // Read first four bytes, which should be 'CRID'
                    if (new string(reader.ReadChars(4)) != "CRID")
                    {
                        throw new InvalidDataException("This is not a valid VSR file");
                    }

                    // Obtain the Fun directory pointer
                    funPointer = FindFunDirPointer(reader);

                    // If the fun pointer wasn't found, the VSR file is not supported
                    if (funPointer == -1)
                    {
                        throw new UnsupportedVSRException();
                    }
                    else
                    {
                        // Obtain the Fun directory address
                        _ = reader.BaseStream.Seek(funPointer, SeekOrigin.Begin);
                        funAddress = reader.ReadUInt32();

                        // Obtain number of levels in the Fun directory
                        _ = reader.BaseStream.Seek(funAddress + 8, SeekOrigin.Begin);
                        int numberOfFunLevels = reader.ReadInt32();

                        // Determine whether the VSR is for the full version of Lemball
                        FullVersion = numberOfFunLevels == 25;

                        // Obtain the file id of the first level
                        _ = reader.BaseStream.Seek(
                            funAddress + 20             // Go to address of first file name in Fun directory
                            + (numberOfFunLevels * 12)  // Skip file names
                            + 4,                        // Go to first file id
                            SeekOrigin.Begin);
                        firstLevelFileId = reader.ReadInt32();
                    }
                }
            }

            // Backup VSR file if it doesn't already exist
            try
            {
                File.Copy(sourceVsrPath, backupPath, false);
            }
            catch (IOException)
            {
                // File already exists
            }
        }

        /// <summary>
        /// Searches for the Fun directory pointer in a Vsr file. This pointer is 188 bytes before the
        /// 'Demo_00' filename string. In all known Lemball Vsr versions, Demo_00 is the first file
        /// in the archive. 188 bytes before it references the address of the fifth from last directory,
        /// which stores the levels for the Fun difficulty.
        /// </summary>
        /// <param name="vsrReader">The reader that is reading the vsr file</param>
        /// <returns>The pointer to the Fun directory, or -1 if it was not found</returns>
        private int FindFunDirPointer(BinaryReader vsrReader)
        {
            // Search from byte
            int startAddress = 908;
            _ = vsrReader.BaseStream.Seek(startAddress, SeekOrigin.Begin);

            // Load 150 bytes as a string
            string vsrString = new string(vsrReader.ReadChars(150));

            // Search for Demo_00 filename
            int index = vsrString.IndexOf("Demo_00");

            // If Demo_00 file name was not found, return -1
            if (index == -1)
            {
                return -1;
            }
            else
            {
                // The Fun directory pointer is 188 bytes before the Demo_00 file name string
                return startAddress + index - 188;
            }
        }

        /// <summary>
        /// Reads a compiled level pack and converts it to a VSR file
        /// </summary>
        /// <exception cref="UnsupportedLevelPackFormatException">The level pack format is not supported
        /// by this version</exception>
        /// <param name="reader">A BinaryReader that is reading the level pack</param>
        /// <param name="outputPath">The path to write the new VSR file to</param>
        public void CompileVSR(BinaryReader levelPackReader, string outputPath)
        {
            // Throw exception if original Vsr file cannot be found
            if (!File.Exists(sourceVsrPath))
            {
                throw new FileNotFoundException();
            }

            // Go to beginning of stream
            _ = levelPackReader.BaseStream.Seek(0, SeekOrigin.Begin);

            // Check that first four bytes of level pack equal 'LPLP'
            if (new string(levelPackReader.ReadChars(4)) != "LPLP")
            {
                throw new InvalidDataException();
            }

            // Check level pack version
            ushort formatVersion = levelPackReader.ReadUInt16();

            // Load level pack reader based on version
            LevelPack levelPack;
            switch (formatVersion)
            {
                case 0:
                    levelPack = new LevelPack(levelPackReader, FullVersion);
                    break;
                default:
                    throw new UnsupportedLevelPackFormatException();
            }

            // Create binary editor to construct the new VSR file
            using (MemoryStream memStream = new MemoryStream(10485760))
            {
                BinaryEditor newVSR = new BinaryEditor(memStream);

                // Append resource portion of original VSR file (up to but not including the Fun directory)
                newVSR.AppendFile(sourceVsrPath, 0, (int)funAddress);

                // Compile levels portion of VSR
                levelPack.Compile(newVSR, firstLevelFileId, (uint)funPointer);

                // Save new VSR
                newVSR.SaveToFile(outputPath);
            }
        }
    }
}