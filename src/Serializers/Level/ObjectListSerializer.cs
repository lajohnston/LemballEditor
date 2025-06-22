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
        private readonly Func<PendingObjectList> createPendingObjectList;
        private readonly ISerializer<PendingObjectList> pendingObjectListSerializer;

        public ObjectListSerializer(Func<PendingObjectList> createObjectList, ISerializer<PendingObjectList> objectListSerializer)
        {
            this.createPendingObjectList = createObjectList ?? throw new ArgumentNullException(nameof(createObjectList));
            this.pendingObjectListSerializer = objectListSerializer;
        }

        /// <summary>
        /// Deserializes a level's objects from the binary data and adds them to the level model.
        /// </summary>
        public ILevel Deserialize(BinaryReader reader, ILevel model)
        {
            var objectList = this.pendingObjectListSerializer.Deserialize(reader, this.createPendingObjectList());

            foreach (var levelObject in objectList.GetLevelObjects())
            {
                model.AddObject(levelObject);
            }

            return model;
        }

        /// <summary>
        /// Serializes the level's objects to binary data.
        /// </summary>
        public void Serialize(ILevel model, BinaryWriter writer)
        {
            var pendingObjectList = this.createPendingObjectList();

            foreach (var levelObject in model.GetObjects())
            {
                pendingObjectList.Add(levelObject);
            }

            pendingObjectList.AssignIds();
            this.pendingObjectListSerializer.Serialize(pendingObjectList, writer);
        }
    }
}
