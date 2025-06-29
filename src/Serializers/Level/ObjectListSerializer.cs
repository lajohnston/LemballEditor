using System;
using System.IO;
using LemballEditor.Models;
using LemballEditor.Serializers.Level.Objects;

namespace LemballEditor.Serializers.Level
{
    /// <summary>
    /// Serializes/deserializes level object data between the VSR binary data and the models
    /// </summary>
    public class ObjectListSerializer : ISerializer<ILevel>
    {
        private readonly Func<ILevel, PendingObjectList> createPendingObjectList;
        private readonly ISerializer<PendingObjectList> pendingObjectListSerializer;

        public ObjectListSerializer(Func<ILevel, PendingObjectList> createObjectList, ISerializer<PendingObjectList> objectListSerializer)
        {
            this.createPendingObjectList = createObjectList ?? throw new ArgumentNullException(nameof(createObjectList));
            this.pendingObjectListSerializer = objectListSerializer;
        }

        /// <summary>
        /// Deserializes a level's objects from the binary data and adds them to the level model.
        /// </summary>
        public ILevel Deserialize(BinaryReader reader, ILevel level)
        {
            var objectList = this.pendingObjectListSerializer.Deserialize(reader, this.createPendingObjectList(level));

            foreach (var levelObject in objectList.GetLevelObjects())
            {
                level.AddObject(levelObject);
            }

            return level;
        }

        /// <summary>
        /// Serializes the level's objects to binary data.
        /// </summary>
        public void Serialize(ILevel level, BinaryWriter writer)
        {
            var pendingObjectList = this.createPendingObjectList(level);

            foreach (var levelObject in level.GetObjects())
            {
                pendingObjectList.Add(levelObject);
            }

            pendingObjectList.AssignIds();
            this.pendingObjectListSerializer.Serialize(pendingObjectList, writer);
        }
    }
}
