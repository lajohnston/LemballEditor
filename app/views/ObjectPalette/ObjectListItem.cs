using System;
using System.Collections.Generic;
using System.Text;
using LemballEditor.Model;

namespace LemballEditor.View
{
    class ObjectListItem
    {
        private String text;
        private Type gameObjectType;

        public ObjectListItem(String text, Type gameObjectType)
        {
            this.text = text;
            //gameObjectType = gameObject.GetType();
            this.gameObjectType = gameObjectType;
        }

        public LevelObject MakeGameObject()
        {
            return (LevelObject)Activator.CreateInstance(gameObjectType, (ushort)0);
        }

        public override String ToString()
        {
            return text;
        }

    }
}
