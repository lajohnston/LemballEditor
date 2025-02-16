using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using VsrCompiler;

namespace LemballEditor.Model
{
    /// <summary>
    /// An enemy Lemmings
    /// </summary>
    internal class Enemy : MovingObject
    {
        public const string XML_NODE_NAME = "enemy";



        /// <summary>
        /// The data block in which the object data is stored in compiled levels
        /// </summary>
        public override LevelObject.ObjectBlocks ObjectBlock => ObjectBlocks.YMNE;

        /// <summary>
        /// The enemy's behaviour type
        /// </summary>
        private readonly BehaviourTypes behaviour;

        /// <summary>
        /// The types of enemy behaviours
        /// </summary>
        public enum BehaviourTypes
        {
            /// <summary>
            /// Stands still. Shoots when the player is near
            /// </summary>
            StatueGuard = 0,

            /// <summary>
            /// Patrols, but doesn't shoot
            /// </summary>
            PatrolsWithoutGun = 1,

            /// <summary>
            /// The enemy patrols and shoots when the player is near. This is always the setting in
            /// official Lemmings Paintball levels
            /// </summary>
            Normal = 2,

            /// <summary>
            /// Patrols when the player is near. Doesn't shoot
            /// </summary>
            Type3 = 3,

            /// <summary>
            /// Patrols; shoots at first then becomes docile (based on old findings.
            /// Can't reproduce)
            /// </summary>
            Type4 = 4,

            /// <summary>
            /// Patrols when player is near. Shoots when player is closer
            /// </summary>
            Type5 = 5,
        }

        public Enemy(ushort id)
            : this(id, 0, 0)
        {

        }

        public Enemy(ushort id, ushort isoX, ushort isoY)
            : this(id, isoX, isoY, BehaviourTypes.Normal)
        {
        }

        public Enemy(ushort id, ushort isoX, ushort isoY, BehaviourTypes behaviour)
            : base(id, isoX, isoY)
        {
            this.behaviour = behaviour;

            // Create a node for the enemy's start position
            //AddNode(new Node(isoX, isoY));
            //AddNode(new Node(isoX, (ushort)(isoY + 16)));
        }



        public Enemy(XmlElement element)
            : base(element)
        {
            if (element.Name != XML_NODE_NAME)
            {
                throw new InvalidDataException();
            }

            behaviour = (BehaviourTypes)Enum.Parse(typeof(BehaviourTypes), element.GetAttribute("behaviour"));
        }


        public override XmlElement CompileXml(XmlDocument xmlDoc)
        {
            // Create xml element
            XmlElement element = xmlDoc.CreateElement(XML_NODE_NAME);

            // Behaviour
            element.SetAttribute("behaviour", behaviour.ToString());

            // Set position
            return base.CompileXml(element);
        }

        public override void CompileVsrBinary(BinaryEditor binary, Level level, ushort? id)
        {
            throw new NotSupportedException();
            //uint nodes = 0;
            //compileObject(ref binary, ref nodes);
        }

        public void compileObject(BinaryEditor binary, List<Node> addedNodes)
        {
            // Position
            base.AppendPosition(binary);

            // Null (usually 0, but is 6 in Cod's Law)
            binary.Append((byte)0);

            // Unknown
            binary.Append((byte)1);

            // The enemy's behaviour
            if (base.CountNodes() > 1)
            {
                binary.Append((byte)behaviour);
            }
            else
            {
                // If there are 0 or 1 nodes, set the guard to 'Statue' mode so that it doesn't twitch
                binary.Append((byte)BehaviourTypes.StatueGuard);
            }

            // Unknown
            binary.Append((byte)3);
            binary.Append((byte)3);
            binary.Append((byte)0);
            binary.Append((byte)0);
            binary.Append((byte)0);

            // Compile nodes
            compileNodes(binary, addedNodes);
        }

        private void compileNodes(BinaryEditor binary, List<Node> levelNodes)
        {
            // Header
            binary.Append("PYAW");

            // Unknown (always 1)
            binary.Append((byte)1);

            // Number of nodes
            binary.Append((byte)base.CountNodes());

            // Unknown
            binary.Append((byte)0);
            binary.Append((byte)1);

            // Each node
            List<Node> nodes = base.Nodes;
            foreach (Node node in nodes)
            {
                // Append nodeRef
                binary.Append((short)levelNodes.Count);

                // Add node to the main list of nodes
                levelNodes.Add(node);
            }
        }

    }
}