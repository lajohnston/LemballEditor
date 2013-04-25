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
    /// Entrance (trapdoor)
    /// </summary>
    public class Entrance : LevelObject
    {
        /// <summary>
        /// The XML element name used by this type of object
        /// </summary>
        public const String XML_NODE_NAME = "entrance";
        
        /// <summary>
        /// The number of lemmings that use this entrance
        /// </summary>
        public byte NumberOfLemmings { get; set; }

        /// <summary>
        /// The data block in which the object data is stored in compiled levels
        /// </summary>
        public override LevelObject.ObjectBlocks ObjectBlock
        {
            get
            {
                return ObjectBlocks.OneSLP;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isoX"></param>
        /// <param name="isoY"></param>
        /// <param name="numberOfLemmings"></param>
        public Entrance(ushort id, ushort isoX, ushort isoY, byte numberOfLemmings)
            : base(id, isoX, isoY)
        {
            this.NumberOfLemmings = numberOfLemmings;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public Entrance(ushort id) : this(id, 0,0,1)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlNode"></param>
        public Entrance(XmlElement xmlNode)
            : base(xmlNode)
        {
            NumberOfLemmings = Convert.ToByte(xmlNode.GetAttribute("lemmings"));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <returns></returns>
        public override XmlElement CompileXml(XmlDocument xmlDoc)
        {
            // Create xml element
            XmlElement element = xmlDoc.CreateElement("entrance");

            // Set position
            base.CompileXml(element);

            // Set colour
            element.SetAttribute("lemmings", NumberOfLemmings.ToString());

            // Return element
            return element;
        }

        public override void CompileVsrBinary(BinaryEditor binary, Level level, ushort? id)
        {
            // Position
            base.AppendPosition(binary);

            // Unknown
            binary.Append((short)0);

            // Number of Lemmings
            binary.Append((short)NumberOfLemmings);
        }
    }
}

