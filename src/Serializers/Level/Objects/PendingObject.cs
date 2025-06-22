using System;
using LemballEditor.Models.LevelObjects;

namespace LemballEditor.Serializers.Level.Objects
{
    /// <summary>
    /// A level object this in the process of begin serialized or deserialized.
    /// </summary>
    public class PendingObject
    {
        public ushort? Id { get; set; }

        public readonly ILevelObject LevelObject;

        public PendingObject(ILevelObject levelObject, ushort? id = null)
        {
            this.Id = id;
            this.LevelObject = levelObject ?? throw new ArgumentNullException(nameof(levelObject));
        }
    }
}
