using System.IO;

namespace LemballEditor.Serializers.Vsr
{
    public class FirstLevelFileId : ISerializer<Models.Vsr>
    {
        /// <summary>
        /// Reads the first level file ID (FUN Level 0) from the VSR data and sets it in the model.
        /// </summary>
        public Models.Vsr Deserialize(BinaryReader reader, Models.Vsr vsr)
        {
            // Skip the file names and other data
            reader.BaseStream.Position = (int)vsr.FunAddress + 8;
            var levelCount = reader.ReadUInt32();
            reader.BaseStream.Position += 8 + (levelCount * 12) + 4; 

            var firstFileId = reader.ReadUInt32();
            vsr.FirstLevelFileId = firstFileId;

            reader.BaseStream.Position = 0;

            return vsr;
        }

        public void Serialize(Models.Vsr vsr, BinaryWriter writer)
        {
            // Do nothing
        }
    }
}
