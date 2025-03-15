namespace LemballEditor.Serializers.LevelGroup
{
    /// <summary>
    /// Gathers temporary serialized data for deserializing a LevelGroup
    /// </summary>
    public class LevelGroupContext
    {
        public byte LevelCount { get; set; }

        public uint BaseAddress { get; set; }
    }
}
