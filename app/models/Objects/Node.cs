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
    /// Path node
    /// </summary>
    public class Node : LevelObject
    {
        public const String XML_NODE_NAME = "node";

        /// <summary>
        /// The maximum number of nodes that can exist for each level
        /// </summary>
        public const int MAX_NODES_PER_LEVEL = 300;

        /// <summary>
        /// The data block in which the object data is stored in compiled levels
        /// </summary>
        public override LevelObject.ObjectBlocks ObjectBlock
        {
            get
            {
                return ObjectBlocks.EDON;
            }
        }

        public Node(ushort isoX, ushort isoY)
            : base(0, isoX, isoY)
        {
        }

        public Node() : base(0)
        {
        }

        public Node(XmlElement xmlNode)
            : base(xmlNode)
        {
        }

        public void ShiftPosition(uint isoX, uint isoY)
        {
            Point newPosition = new Point (
                (int) (IsoPosition.X + isoX),
                (int) (IsoPosition.Y + isoY)
                );

            base.IsoPosition = newPosition;
        }

        public override void CompileVsrBinary(BinaryEditor binary, Level level, ushort? id)
        {
            // Position
            base.AppendPosition(binary);

            // Null
            binary.Append((short)0);
        }

        public override XmlElement CompileXml(XmlDocument xmlDoc)
        {
            return base.CompileXml(xmlDoc.CreateElement("node"));
        }
    }
}

