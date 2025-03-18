namespace LemballEditor.Models
{
    /// <summary>
    /// The VSR resource file containing the game assets and levels
    /// </summary>
    public class Vsr
    {
        /// <summary>
        /// The pointer that holds the fun directory address in AssetData
        /// </summary>
        public uint FunDirectoryPointer { get; set; }

        /// <summary>
        /// The pointer that holds the tricky directory address in AssetData
        /// </summary>
        public uint TrickyDirectoryPointer { get; set; }

        /// <summary>
        /// The pointer that holds the taxing directory address in AssetData
        /// </summary>
        public uint TaxingDirectoryPointer { get; set; }

        /// <summary>
        /// The pointer that holds the mayhem directory address in AssetData
        /// </summary>
        public uint MayhemDirectoryPointer { get; set; }

        /// <summary>
        /// The pointer that holds the network directory address in AssetData
        /// </summary>
        public uint NetworkDirectoryPointer { get; set; }

        /// <summary>
        /// The static asset data, excluding the level directories at the end
        /// </summary>
        public byte[] AssetData { get; set; }

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
        /// The levels within the VSR
        /// </summary>
        public LevelPack LevelPack { get; set; }
    }
}
