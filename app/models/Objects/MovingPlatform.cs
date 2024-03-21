using System;
using System.Collections.Generic;
using System.Xml;
using VsrCompiler;

namespace LemballEditor.Model
{
    /// <summary>
    /// A moving platform
    /// </summary>
    public class MovingPlatform : MovingObject
    {
        /// <summary>
        /// 
        /// </summary>
        public const string XML_NODE_NAME = "moving_platform";

        /// <summary>
        /// 
        /// </summary>
        public enum PlatformType
        {
            /// <summary>
            /// Platform is a square
            /// </summary>
            Square = 0,
            /// <summary>
            /// Platform is a spinning star
            /// </summary>
            Star = 1,
        }

        /// <summary>
        /// 
        /// </summary>
        public enum ActivationType
        {
            /// <summary>
            /// Platform moves by itself in a loop. When it gets to the last node it will move back to the first node.
            /// </summary>
            AutomaticLoop = 0,

            /// <summary>
            /// The same as AutomaticLoop except the platform is invisible
            /// </summary>
            //AutomaticLoopInvisible = 1,

            /// <summary>
            /// Platform is activated by a switch or pressure pad. Each time the switch is activated,
            /// the platform moves to the next node and stops. At the start of the level, the platform
            /// moves to the second node.
            /// </summary>
            SwitchMoveOnce = 128,
        }

        /// <summary>
        /// The path nodes. This list doesn't include the first node, which indicates
        /// the starting position of the platform. The first node is vital, and so isn't
        /// created until the object is being compiled
        /// </summary>
        //private List<Node> nodes;

        /// <summary>
        /// The type of platform
        /// </summary>
        private readonly PlatformType platformType;

        /// <summary>
        /// The data block in which the object data is stored in compiled levels
        /// </summary>
        public override LevelObject.ObjectBlocks ObjectBlock => ObjectBlocks.EVOM;

        /// <summary>
        /// How the switch is activated and how it behaves
        /// </summary>
        private readonly ActivationType activationType;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isoX"></param>
        /// <param name="isoY"></param>
        /// <param name="platformType"></param>
        /// <param name="activationType"></param>
        public MovingPlatform(ushort id, ushort isoX, ushort isoY, PlatformType platformType, ActivationType activationType)
            : base(id, isoX, isoY)
        {
            this.platformType = platformType;
            Nodes.Add(new Node(isoX, (ushort)(isoY + 50)));
            Nodes.Add(new Node((ushort)(isoX + 50), (ushort)(isoY + 60)));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public MovingPlatform(ushort id)
            : this(id, 0, 0, PlatformType.Square, ActivationType.AutomaticLoop)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlNode"></param>
        public MovingPlatform(XmlElement xmlNode)
            : base(xmlNode)
        {
            platformType = (PlatformType)Enum.Parse(typeof(PlatformType), xmlNode.GetAttribute("activation"));
            activationType = (ActivationType)Enum.Parse(typeof(ActivationType), xmlNode.GetAttribute("type"));
        }

        /// <summary>
        /// Platforms require a set id so that they can be referenced by other objects
        /// </summary>
        /// <returns></returns>
        public override bool RequiresStaticId()
        {
            return true;
        }

        public override void CompileVsrBinary(BinaryEditor binary, Level level, ushort? id)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="binary"></param>
        /// <param name="levelNodes"></param>
        public void CompileObject(BinaryEditor binary, List<Node> levelNodes)
        {
            // ID
            base.AppendIdToBinary(binary);

            // Platform type
            binary.Append((byte)platformType);

            // Activation and behaviour
            binary.Append((byte)activationType);

            // First node ref
            binary.Append((ushort)levelNodes.Count);

            // Number of nodes (+ 1 for first node)
            binary.Append((ushort)base.CountNodes() + 1);

            // Add first node (start position of platform)
            levelNodes.Add(new Node((ushort)IsoPosition.X, (ushort)IsoPosition.Y));

            // Add nodes to level node list
            List<Node> nodes = base.Nodes;
            foreach (Node node in nodes)
            {
                levelNodes.Add(node);
            }
        }

        public override XmlElement CompileXml(XmlDocument xmlDoc)
        {
            // Create xml element
            XmlElement element = xmlDoc.CreateElement(XML_NODE_NAME);

            // Set position and id
            _ = base.CompileXml(element);

            // Activation and behaviour
            element.SetAttribute("activation", ((int)activationType).ToString());

            // Type
            element.SetAttribute("type", ((int)platformType).ToString());

            // Return element
            return element;
        }

    }
}