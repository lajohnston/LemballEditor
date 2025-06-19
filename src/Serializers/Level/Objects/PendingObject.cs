using System;
using LemballEditor.Models.LevelObjects;

namespace LemballEditor.Serializers.Level.Objects
{
    /// <summary>
    /// A level object this in the process of begin serialized or deserialized.
    /// </summary>
    public class PendingObject
    {
        public uint Id { get; set; }

        public readonly ILevelObject LevelObject;

        public PendingObject(ILevelObject levelObject)
        {
            this.LevelObject = levelObject ?? throw new ArgumentNullException(nameof(levelObject));
        }
    }
}
