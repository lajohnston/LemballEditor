using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using LemballEditor.Models;
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

        public readonly ILevel Level;

        public PendingObjectList(ILevel level)
        {
            this.Level = level;
            this.pendingObjects = new List<PendingObject>();
        }

        public void Add(ILevelObject levelObject, ushort? id = null)
        {
            if (levelObject == null)
            {
                throw new ArgumentNullException(nameof(levelObject));
            }

            var pendingObject = new PendingObject(levelObject, id);
            this.pendingObjects.Add(pendingObject);
            this.Publish(pendingObject);
        }

        /// <summary>
        /// Assign unique IDs to each object, ready for serialization
        /// </summary>
        public void AssignIds()
        {
            ushort nextId = 0;

            foreach (var pendingObject in this.pendingObjects)
            {
                pendingObject.Id = nextId++;
            }
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
        /// Returns the pending level objects
        /// </summary>
        public ReadOnlyCollection<PendingObject> GetPendingObjects()
        {
            return this.pendingObjects.AsReadOnly();
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

        /// <summary>
        /// Returns all PendingObjects whose LevelObject is an instance of any of the specified types.
        /// </summary>
        public IEnumerable<PendingObject> GetObjectsOfTypes(params Type[] types)
        {
            if (types == null || types.Length == 0)
            {
                yield break;
            }

            foreach (var pendingObject in this.pendingObjects)
            {
                var objType = pendingObject.LevelObject.GetType();
                foreach (var type in types)
                {
                    if (type.IsAssignableFrom(objType))
                    {
                        yield return pendingObject;
                        break;
                    }
                }
            }
        }
    }
}
