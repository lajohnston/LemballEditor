using System.IO;

namespace VsrCompiler
{
    /// <summary>
    /// A level pack that has been loaded from a level pack file
    /// </summary>
    internal class LevelPack
    {
        /// <summary>
        /// An array that holds each level group (Fun, Tricky, Taxing, Mayhem, Network)
        /// </summary>
        private readonly LevelGroup[] levelGroups;

        /// <summary>
        /// Reads a level pack into memory
        /// </summary>
        /// <param name="levelPackData">The binary reader that is reading the level pack</param>
        /// <param name="fullLemballVersion">Whether the version of Lemball is the full version (true) or the demo (false)</param>
        public LevelPack(BinaryReader levelPackData, bool fullLemballVersion)
        {
            // Get the max (and min) levels per level group (Fun, Tricky, Taxing, Mayhem, Network)
            byte[] maxLevels = fullLemballVersion ? (new byte[] { 25, 26, 29, 22, 12 }) : (new byte[] { 2, 1, 1, 1, 1 });

            // Initialise levelGroup list
            levelGroups = new LevelGroup[5];

            // Skip 2-byte null
            _ = levelPackData.ReadInt16();

            // Compiler version
            _ = levelPackData.ReadUInt32();

            // Get compressed and uncompressed sizes
            int uncompressedSize = levelPackData.ReadInt32();
            int dataSize = levelPackData.ReadInt32();

            bool isCompressed = uncompressedSize != 0;

            // Read level data
            byte[] data = levelPackData.ReadBytes(dataSize);

            // Decompress data if it begins with the GZip magic number
            if (isCompressed && data[0] == 31 && data[1] == 139)
            {
                BinaryEditor.Decompress(ref data, uncompressedSize);
            }
            // If data is not compressed, throw error if it doesn't start with 'DIR'
            else if (data[0] != 68 || data[1] != 73 || data[2] != 82)
            {
                throw new InvalidDataException();
            }

            // Save decompressed level data (debug)
            /*
            MemoryStream mem = new MemoryStream(data);
            FileStream file = new FileStream(@".\decompressedLevelData.dat", FileMode.Create);
            mem.WriteTo(file);
            mem.Close();
            file.Close();
            */

            // Create a memory stream that holds the level pack data
            using (MemoryStream levelData = new MemoryStream(data))
            {
                // Each level group directory
                for (int dir = 0; dir < 5; dir++)
                {
                    levelGroups[dir] = new LevelGroup(levelData, maxLevels[dir]);
                }
            }
        }

        /// <summary>
        /// Compiles the level pack into a VSR file
        /// </summary>
        /// <param name="binary">The BinaryEditor being used to create the VSR file</param>
        /// <param name="currentFileId">The file ID for the first level</param>
        /// <param name="funDirPointer">The address that stores the address of the Fun directory</param>
        public void Compile(BinaryEditor binary, int firstFileId, uint funDirPointer)
        {
            // Stores the next file Id to use
            int nextFileId = firstFileId;

            // Each level directory
            uint dirAddressPointer = funDirPointer;
            for (int dir = 0; dir < 5; dir++)
            {
                // Remember size before directory was added
                uint directoryAddress = binary.getSize();

                // Set address to directory
                binary.Set(dirAddressPointer, directoryAddress);

                // Compile directory
                levelGroups[dir].compileVSRDirectory(binary, ref nextFileId);

                // Set directory size value (near beginning of VSR)
                uint dirSize = binary.getSize() - directoryAddress;
                binary.Set(dirAddressPointer + 4, dirSize);

                // Go to next directory address pointer
                dirAddressPointer += 36;
            }
        }
    }
}