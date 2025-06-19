using System;
using System.Collections.Generic;
using LemballEditor.Models.LevelObjects;

namespace LemballEditor.Serializers.Level.Objects
{
    /// <summary>
    /// Stores a list of the level objects currently being serialized or deserialized.
    /// Handles the assignment of IDs to these objects and notifies subscribers when new objects are added.
    /// </summary>
    public class PendingObjectList
    {
        private readonly List<PendingObject> pendingObjects;

        private readonly List<Action<PendingObject>> subscribers = new List<Action<PendingObject>>();

        public PendingObjectList()
        {
            this.pendingObjects = new List<PendingObject>();
        }

        public PendingObject Add(ILevelObject levelObject)
        {
            if (levelObject == null)
            {
                throw new ArgumentNullException(nameof(levelObject));
            }

            var pendingObject = new PendingObject(levelObject);
            this.pendingObjects.Add(pendingObject);
            this.Publish(pendingObject);

            return pendingObject;
        }

        /// <summary>
        /// Assign unique IDs to each object, ready for serialization
        /// </summary>
        public void AssignIds()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns a list of all level objects in this PendingObjectList.
        /// </summary>
        /// <returns></returns>
        public List<ILevelObject> GetLevelObjects()
        {
            return this.pendingObjects.ConvertAll(pendingObject => pendingObject.LevelObject);
        }

        /// <summary>
        /// Adds the given handler to the list of subscribers that will be notified when a new PendingObject is added.
        /// </summary>
        public void Subscribe(Action<PendingObject> handler)
        {
            this.subscribers.Add(handler);
        }

        /// <summary>
        /// Publishes an event to all subscribers when a new PendingObject is added.
        /// </summary>
        private void Publish(PendingObject pendingObject)
        {
            foreach (var handler in this.subscribers)
            {
                handler(pendingObject);
            }
        }
    }
}
