using System.Xml;
using VsrCompiler;

namespace LemballEditor.Model
{
    /// <summary>
    /// Ammunition
    /// </summary>
    public class Ammo : LevelObject
    {
        /// <summary>
        /// The data block in which the object data is stored in compiled levels
        /// </summary>
        public override LevelObject.ObjectBlocks ObjectBlock => ObjectBlocks.BOMG;

        /// <summary>
        /// The name used when creating an xml element of the object
        /// </summary>
        public const string XML_NODE_NAME = "ammo";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isoX"></param>
        /// <param name="isoY"></param>
        public Ammo(ushort id, ushort isoX, ushort isoY)
            : base(id, isoX, isoY)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public Ammo(ushort id) : this(id, 0, 0)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlNode"></param>
        public Ammo(XmlElement xmlNode)
            : base(xmlNode)
        {
            // Empty: Ammo has no additional properties to load
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
            _ = base.CompileXml(element);

            // Return element
            return element;
        }

        public override void CompileVsrBinary(BinaryEditor binary, Level level, ushort? id)
        {
            // Id
            binary.Append((short)0);

            // Position
            base.AppendPosition(binary);

            // Unknown
            binary.Append((short)0);

            // Type
            binary.Append((short)5);

            // Unknown
            binary.Append(0);
        }


    }
}

