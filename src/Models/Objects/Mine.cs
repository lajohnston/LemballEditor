using System.Xml;
using VsrCompiler;

namespace LemballEditor.Model
{
    /// <summary>
    /// Land mine
    /// </summary>
    internal class Mine : LevelObject
    {
        public const string XML_NODE_NAME = "mine";


        /// <summary>
        /// The data block in which the object data is stored in compiled levels
        /// </summary>
        public override LevelObject.ObjectBlocks ObjectBlock => ObjectBlocks.ENIM;



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
            binary.Append((short)0);

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
            _ = base.CompileXml(element);

            // Return element
            return element;
        }
    }
}

