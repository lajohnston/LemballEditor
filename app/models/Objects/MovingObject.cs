using System.Collections.Generic;
using System.Drawing;
using System.Xml;

namespace LemballEditor.Model
{
    public abstract class MovingObject : LevelObject
    {
        /// <summary>
        /// List of path nodes that the moving object follows
        /// </summary>
        public List<Node> Nodes { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public override Point IsoPosition
        {
            get => base.IsoPosition;
            set
            {
                // Remember old position
                Point oldPosition = IsoPosition;

                // If nodes has been initialised
                if (Nodes != null)
                {
                    // Determine difference
                    uint xDiff = (uint)(value.X - oldPosition.X);
                    uint yDiff = (uint)(value.Y - oldPosition.Y);

                    // Shift nodes
                    if (xDiff != 0 || yDiff != 0)
                    {
                        foreach (Node node in Nodes)
                        {
                            node.ShiftPosition(xDiff, yDiff);
                        }
                    }
                }

                // Update new position
                base.IsoPosition = value;
            }
        }

        public MovingObject(ushort id, ushort isoX, ushort isoY) : base(id, isoX, isoY)
        {
            Nodes = new List<Node>();
        }

        /// <summary>
        /// Creates a moving object from its xml element
        /// </summary>
        /// <param name="element"></param>
        public MovingObject(XmlElement element)
            : base(element)
        {
            Nodes = new List<Node>();

            XmlNodeList xmlNodes = element.SelectNodes("nodes/node");
            foreach (XmlNode node in xmlNodes)
            {
                Nodes.Add(new Node((XmlElement)node));
            }


        }

        protected override XmlElement CompileXml(XmlElement objectElement)
        {
            // Get xml document
            XmlDocument xmlDoc = objectElement.OwnerDocument;

            // Create element to hold nodes
            XmlElement nodes = xmlDoc.CreateElement("nodes");
            _ = objectElement.AppendChild(nodes);

            // Compile each node
            foreach (Node node in Nodes)
            {
                _ = nodes.AppendChild(node.CompileXml(xmlDoc));
            }

            // Pass element to base
            return base.CompileXml(objectElement);
        }

        public void AddNode(Node node)
        {
            Nodes.Add(node);
        }

        public void DeleteNode(Node node)
        {
            _ = Nodes.Remove(node);
        }

        public int CountNodes()
        {
            return Nodes.Count;
        }



    }
}
