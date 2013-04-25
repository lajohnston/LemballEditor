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
    public class Catapult : RotatableObject
    {
        public const String XML_NODE_NAME = "catapult";

        /// <summary>
        /// The data block in which the object data is stored in compiled levels
        /// </summary>
        public override LevelObject.ObjectBlocks ObjectBlock
        {
            get
            {
                return ObjectBlocks.BOMG;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isoX"></param>
        /// <param name="isoY"></param>
        public Catapult(ushort id, ushort isoX, ushort isoY)
            : base(id, isoX, isoY)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public Catapult(ushort id)
            : this(id, 0, 0)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        public Catapult(XmlElement element)
            : base (element)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="binary"></param>
        public override void CompileVsrBinary(BinaryEditor binary, Level level, ushort? id)
        {
            // Id
            binary.Append((short)0);

            // Position
            base.AppendPosition(binary);

            // Unknown. Usually 16 or 18
            binary.Append((short)16);

            // Type
            binary.Append((short)4);

            // Unknown
            binary.Append((short)0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <returns></returns>
        public override XmlElement CompileXml(XmlDocument xmlDoc)
        {
            // Create xml element
            XmlElement element = xmlDoc.CreateElement("catapult");

            // Set position
            base.CompileXml(element);

            // Return element
            return element;
        }

    }
}
