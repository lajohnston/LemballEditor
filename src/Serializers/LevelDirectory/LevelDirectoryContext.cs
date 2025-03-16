namespace LemballEditor.Serializers.LevelDirectory
{
    /// <summary>
    /// Gathers temporary serialized data for deserializing a LevelGroup
    /// </summary>
    public class LevelDirectoryContext
    {
        public byte LevelCount { get; set; }

        public uint BaseAddress { get; set; }
    }
}
