using LemballEditor.Models;

namespace LemballEditor.Serializers.LevelDirectory
{
    /// <summary>
    /// Gathers temporary serialized data for deserializing a LevelGroup
    /// </summary>
    public class LevelDirectory
    {
        public byte LevelCount { get; set; }

        public uint BaseAddress { get; set; }

        public LevelGroup LevelGroup { get; set; }
    }
}
