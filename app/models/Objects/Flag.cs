using System.Xml;
using VsrCompiler;

namespace LemballEditor.Model
{
    internal class Flag : LevelObject
    {
        public const string XML_NODE_NAME = "flag";

        /// <summary>
        /// The data block in which the object data is stored in compiled levels
        /// </summary>
        public override LevelObject.ObjectBlocks ObjectBlock => ObjectBlocks.LLOC;



        public Flag(ushort id, ushort isoX, ushort isoY)
            : base(id, isoX, isoY)
        {

        }

        public Flag(ushort id)
            : this(id, 0, 0)
        {
        }

        public Flag(XmlElement xmlNode)
            : base(xmlNode)
        {
        }


        public override XmlElement CompileXml(XmlDocument xmlDoc)
        {
            // Create xml element
            XmlElement element = xmlDoc.CreateElement(XML_NODE_NAME);

            // Set position
            _ = base.CompileXml(element);

            // Player
            if (isPlayerOne())
            {
                element.SetAttribute("player", "1");
            }
            else
            {
                element.SetAttribute("player", "2");
            }

            // Return element
            return element;
        }

        public virtual bool isPlayerOne()
        {
            return true;
        }

        public override void CompileVsrBinary(BinaryEditor binary, Level level, ushort? id)
        {
            // Id
            binary.Append((short)0);

            // Object reference
            binary.Append(getObjectReference());

            // Position
            base.AppendPosition(binary);

            // Unknown
            binary.Append((short)0);
        }

        protected virtual short getObjectReference()
        {
            return 12;
        }
    }
}
