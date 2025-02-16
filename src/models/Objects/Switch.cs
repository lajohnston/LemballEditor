using System;
using System.Collections.Generic;
using System.Xml;
using VsrCompiler;

namespace LemballEditor.Model
{
    public abstract class Switch : LevelObject
    {
        /// <summary>
        /// A list of connections to other objects.
        /// </summary>
        protected List<int> connectedObjectIds;

        /// <summary>
        /// Returns a copy of the idrefs of the objects connected to this switch
        /// </summary>
        public int[] ConnectedIdrefs => connectedObjectIds.ToArray();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isoX"></param>
        /// <param name="isoY"></param>
        public Switch(ushort id, ushort isoX, ushort isoY)
            : base(id, isoX, isoY)
        {
            connectedObjectIds = new List<int>(0);
        }

        /// <summary>
        /// Creates a switch from an XML node
        /// </summary>
        /// <param name="xmlNode"></param>
        public Switch(XmlElement xmlNode)
            : base(xmlNode)
        {
            // Load the connections list
            XmlNodeList idrefs = xmlNode.SelectNodes("connections/object");
            connectedObjectIds = new List<int>(idrefs.Count);

            // Add each idref to connection list
            foreach (XmlElement connection in idrefs)
            {
                connectedObjectIds.Add(Convert.ToInt32(connection.GetAttribute("idref")));
            }
        }

        /// <summary>
        /// Connects an object to the switch
        /// </summary>
        /// <param name="idref">The id of the object to connect</param>
        public bool AddConnection(LevelObject levelObject)
        {
            if (CanConnectToObject(levelObject))
            {
                // If the idref list doesn't already contain the idref
                if (!connectedObjectIds.Contains(levelObject.Id))
                {
                    _ = levelObject.ConnectToSwitch();
                    connectedObjectIds.Add(levelObject.Id);
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Deletes the connection to the level object
        /// </summary>
        /// <param name="idref"></param>
        public void DeleteConnection(int idref)
        {
            _ = connectedObjectIds.Remove(idref);
        }

        /// <summary>
        /// States whether the specified object is connected to this switch
        /// </summary>
        /// <param name="levelObject"></param>
        /// <returns></returns>
        public bool IsConnectedTo(LevelObject levelObject)
        {
            return connectedObjectIds.Contains(levelObject.Id);
        }

        /// <summary>
        /// Specifies whether the switch can connect to the given level object
        /// </summary>
        /// <param name="levelObject"></param>
        /// <returns></returns>
        public virtual bool CanConnectToObject(LevelObject levelObject)
        {
            return levelObject is Gate
                || levelObject is MovingPlatform
                || levelObject is Lift;
        }

        /// <summary>
        /// Compiles the object so that it is Lemball compatible
        /// </summary>
        /// <param name="binary"></param>
        public override void CompileVsrBinary(BinaryEditor binary, Level level, ushort? id)
        {
            // Number of connections
            binary.Append((short)connectedObjectIds.Count);

            // Each connection
            foreach (int idref in connectedObjectIds)
            {
                CompileConnection(binary, level.GetObject(idref));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="binary"></param>
        /// <param name="levelObject"></param>
        protected abstract void CompileConnection(BinaryEditor binary, LevelObject levelObject);


    }
}
