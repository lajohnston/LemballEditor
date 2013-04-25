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
    /// A balloon post
    /// </summary>
    public class BalloonPost : LevelObject
    {
        /// <summary>
        /// 
        /// </summary>
        public const String XML_NODE_NAME = "balloon_post";

        public Balloon.Colours Colour { get; private set; }

        /// <summary>
        /// The data block in which the object data is stored in compiled levels
        /// </summary>
        public override LevelObject.ObjectBlocks ObjectBlock
        {
            get
            {
                return ObjectBlocks.NOOB;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="colour"></param>
        public BalloonPost(ushort id, Balloon.Colours colour)
            : base(id)
        {
            this.Colour = colour;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        internal static BalloonPost CreatePost(XmlElement element)
        {
            // Get the balloon post's colour
            Balloon.Colours colour = (Balloon.Colours)Enum.Parse(typeof(Balloon.Colours), element.GetAttribute("colour"), true);

            // Get the balloon post's id
            ushort id = Convert.ToUInt16(element.GetAttribute("id"));

            //
            return new BalloonPost(id, colour);
        }

        /// <summary>
        /// Indicates that the BalloonPost is not included in the main object limit
        /// </summary>
        /// <returns></returns>
        public override bool IsIncludedInObjectLimit()
        {
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="binary"></param>
        /// <param name="level"></param>
        /// <param name="id"></param>
        public override void CompileVsrBinary(BinaryEditor binary, Level level, ushort? id)
        {
            // Position
            base.AppendPosition(binary);

            // Null
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
            XmlElement element = xmlDoc.CreateElement(XML_NODE_NAME);

            // Set position
            base.CompileXml(element);

            // Set colour
            element.SetAttribute("colour", Colour.ToString());
            
            // Return element
            return element;
        }
    }
}

