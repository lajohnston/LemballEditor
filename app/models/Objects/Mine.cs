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
    /// Land mine
    /// </summary>
    class Mine : LevelObject
    {
        public const String XML_NODE_NAME = "mine";


        /// <summary>
        /// The data block in which the object data is stored in compiled levels
        /// </summary>
        public override LevelObject.ObjectBlocks ObjectBlock
        {
            get
            {
                return ObjectBlocks.ENIM;
            }
        }

      

        public Mine(ushort id, ushort isoX, ushort isoY)
            : base(id, isoX, isoY)
        {
        }

        public Mine(ushort id) : base(id)
        {
        }

        public Mine(XmlElement xmlNode)
            : base(xmlNode)
        {
        }

        public override void CompileVsrBinary(BinaryEditor binary, Level level, ushort? id)
        {
            // Id
            binary.Append((short) 0);

            // Position
            base.AppendPosition(binary);

            // Unknown
            binary.Append((short)0);
        }

        public override XmlElement CompileXml(XmlDocument xmlDoc)
        {
            // Create xml element
            XmlElement element = xmlDoc.CreateElement("mine");

            // Set position
            base.CompileXml(element);

            // Return element
            return element;
        }
    }
}

