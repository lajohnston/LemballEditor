using System;
using System.Xml;
using VsrCompiler;

namespace LemballEditor.Model
{
    public partial class Lever : Switch
    {
        /// <summary>
        /// The xml node name
        /// </summary>
        public const string XML_NODE_NAME = "switch";

        /// <summary>
        /// The data block in which the object data is stored in compiled levels
        /// </summary>
        public override LevelObject.ObjectBlocks ObjectBlock => ObjectBlocks.BOMG;

        /// <summary>
        /// Creates a new switch
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isoX"></param>
        /// <param name="isoY"></param>
        public Lever(ushort id, ushort isoX, ushort isoY)
            : base(id, isoX, isoY)
        {

        }

        /// <summary>
        /// Creates a new switch at position 0, 0
        /// </summary>
        /// <param name="id"></param>
        public Lever(ushort id)
            : this(id, 0, 0)
        {
        }

        /// <summary>
        /// Creates a switch from an XML node
        /// </summary>
        /// <param name="xmlNode"></param>
        public Lever(XmlElement xmlNode)
            : base(xmlNode)
        {
        }

        /// <summary>
        /// Compiles an XML element that contains the switch's data
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <returns></returns>
        public override XmlElement CompileXml(XmlDocument xmlDoc)
        {
            // Create xml element
            XmlElement objectElement = xmlDoc.CreateElement(XML_NODE_NAME);

            // Set position
            _ = base.CompileXml(objectElement);

            // Create connections
            XmlElement connectionsNode = xmlDoc.CreateElement("connections");
            _ = objectElement.AppendChild(connectionsNode);

            foreach (int idref in connectedObjectIds)
            {
                // Create the connection node with the connected object's id
                //XmlElement connectionElement = connection.MakeXmlNode(xmlDoc);
                XmlElement connection = xmlDoc.CreateElement("object");
                connection.SetAttribute("idref", idref.ToString());

                // Add to the connection node
                _ = connectionsNode.AppendChild(connection);
            }

            return objectElement;
        }

        /// <summary>
        /// Compiles the object so that it is Lemball compatible
        /// </summary>
        /// <param name="binary"></param>
        public override void CompileVsrBinary(BinaryEditor binary, Level level, ushort? id)
        {
            // Id (must be higher than the objects it is connected to)
            //binary.Append((short)(GetHighestIdref() + 1));
            binary.Append((ushort)id);

            // Position
            base.AppendPosition(binary);

            // Null
            binary.Append((short)0);

            // Type
            binary.Append((short)20);

            // Null
            binary.Append((short)0);

            // Compile connections
            base.CompileVsrBinary(binary, level, id);
        }

        /// <summary>
        /// Compiles a connection to the specified level object. Used when compiling to a VSR file
        /// </summary>
        /// <param name="binary">The binary to compile to</param>
        /// <param name="levelObject">The level object to connect to</param>
        protected override void CompileConnection(BinaryEditor binary, LevelObject levelObject)
        {
            // Type of object
            int objectTypeNumber;

            /*
            Lift = 1,
            Gate = 3,
            Platform = 4,
            Slide = 5,
            */

            if (levelObject is Lift)
            {
                objectTypeNumber = 1;
            }
            else
            {
                objectTypeNumber = levelObject is Gate ? 3 : levelObject is MovingPlatform ? 4 : throw new NotImplementedException();
            }

            // Append object type number
            binary.Append((short)objectTypeNumber);

            // Idref
            binary.Append((short)levelObject.Id);
        }

        /// <summary>
        /// Returns the highest id value that the switch connects to
        /// </summary>
        /// <returns></returns>
        /*
        private int GetHighestIdref()
        {
            // The highest idref found so far
            int highest = -1;

            // Each connection
            foreach (int idref in connectedObjectIds)
            {
                if (idref > highest)
                    highest = Id;
            }

            // Return the highest idref
            return highest;
        }
        */

    }
}
