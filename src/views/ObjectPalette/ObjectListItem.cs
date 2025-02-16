using LemballEditor.Model;
using System;

namespace LemballEditor.View
{
    internal class ObjectListItem
    {
        private readonly string text;
        private readonly Type gameObjectType;

        public ObjectListItem(string text, Type gameObjectType)
        {
            this.text = text;
            //gameObjectType = gameObject.GetType();
            this.gameObjectType = gameObjectType;
        }

        public LevelObject MakeGameObject()
        {
            return (LevelObject)Activator.CreateInstance(gameObjectType, (ushort)0);
        }

        public override string ToString()
        {
            return text;
        }

    }
}
