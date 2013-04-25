using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using System.Xml;
using System.Windows.Forms;
using VsrCompiler;

namespace LemballEditor.Model
{
    /// <summary>
    /// A gate/barrier
    /// </summary>
    public class Gate : RotatableObject
    {
        /// <summary>
        /// The data block in which the object data is stored in compiled levels
        /// </summary>
        public override LevelObject.ObjectBlocks ObjectBlock
        {
            get
            {
                return ObjectBlocks.ROOD;
            }
        }

        /// <summary>
        /// The element name that is used to store this object as XML data
        /// </summary>
        public const String XML_NODE_NAME = "gate";

        /// <summary>
        /// Types of lock that gates can use
        /// </summary>
        public enum LockType
        {
            /// <summary>
            /// The door opens when clicked on
            /// </summary>
            None = 0,
            /// <summary>
            /// The door requires a red key
            /// </summary>
            Red = 1,
            /// <summary>
            /// The door requires a blue key
            /// </summary>
            Blue = 2,
            /// <summary>
            /// The door requires a green key
            /// </summary>
            Green = 3,
            /// <summary>
            /// The door is activated by a switch
            /// </summary>
            Switch = 4,
        }

        /// <summary>
        /// The lock type used by the gate
        /// </summary>
        private LockType lockType;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isoX"></param>
        /// <param name="isoY"></param>
        public Gate(ushort id, ushort isoX, ushort isoY)
            : base(id, isoX, isoY)
        {
            lockType = LockType.Switch;
        }

        public Gate(ushort id) : this(id, 0,0)
        {
        }

        /// <summary>
        /// Loads the object from the data stored in the xmlNode
        /// </summary>
        /// <param name="xmlNode"></param>
        public Gate(XmlElement xmlNode)
            : base(xmlNode)
        {
            String attribute = xmlNode.GetAttribute("lockType");
            lockType = (LockType)(Enum.Parse(typeof(LockType), attribute, true));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <returns></returns>
        public override XmlElement CompileXml(XmlDocument xmlDoc)
        {
            // Create xml element
            XmlElement element = xmlDoc.CreateElement(XML_NODE_NAME);

            // Lock type
            element.SetAttribute("lockType", System.Enum.GetName(typeof(LockType), lockType).ToLower());

            // Set rotation and position
            base.CompileXml(element);

            // Return element
            return element;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tileCoordinate"></param>
        /// <returns></returns>
        public override bool OverlapsTile(TileCoordinate tileCoordinate)
        {
            if (!Rotated)
                return tileCoordinate.Equals(new TileCoordinate(base.OnTile.xTile, (ushort)(base.OnTile.yTile + 2)));
            else
                return tileCoordinate.Equals(new TileCoordinate((ushort)(base.OnTile.xTile + 2), base.OnTile.yTile));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool RequiresStaticId()
        {
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="binary"></param>
        /// <param name="level"></param>
        /// <param name="id"></param>
        public override void CompileVsrBinary(BinaryEditor binary, Level level, ushort? id)
        {
            // Id
            binary.Append((short)Id);

            // Rotation
            if (Rotated)
                binary.Append((short)26);
            else
                binary.Append((short)25);

            // Lock
            binary.Append((short)lockType);

            // Position
            base.AppendPosition(binary);

            // Null
            binary.Append((short)0);
        }

    }
}

