using System;
using System.Drawing;
using System.IO;
using System.Xml;
using VsrCompiler;

namespace LemballEditor.Model
{
    /// <summary>
    /// An object found in Lemmings Paintball levels
    /// </summary>
    public abstract partial class LevelObject
    {
        /// <summary>
        /// The object's unique id, which is used internally by Lemball Editor
        /// </summary>
        public ushort Id { get; set; }

        /// <summary>
        /// The isometric coordinate of the object
        /// </summary>
        private Point isoPosition;
        public virtual Point IsoPosition
        {
            get => isoPosition;
            set
            {
                // Set the isometric position
                isoPosition = value;

                // Calculate the tile the object is on
                OnTile = new TileCoordinate(
                        (byte)Math.Floor((double)isoPosition.X / 16),
                        (byte)Math.Floor((double)isoPosition.Y / 16)
                    );
            }
        }

        /// <summary>
        /// The tile coordinate that the object resides on
        /// </summary>
        public TileCoordinate OnTile { get; private set; }

        /// <summary>
        /// The data block the object data is stored in within a compiled level file
        /// </summary>
        public abstract ObjectBlocks ObjectBlock { get; }

        /// <summary>
        /// The types of object blocks, which are used in compiled level files
        /// </summary>
        public enum ObjectBlocks
        {
            BOMG,
            YMNE,
            GPHS,
            EDON,
            LLAB,
            ENIM,
            LLOC,
            MINA,
            TFIL,
            ROOD,
            KCOR,
            DNAH,
            RSAL,
            NOOB,
            MART,
            ECI,
            EVOM,
            OneSLP,
            SVNI,
        }


        /// <summary>
        /// Positions a new game object at the specified isometric coordinate
        /// </summary>
        /// <param name="isoX"></param>
        /// <param name="isoY"></param>
        public LevelObject(ushort id, ushort isoX, ushort isoY)
        {
            IsoPosition = new Point(isoX, isoY);
            Id = id;
        }

        /// <summary>
        /// Positions a new game object at the isometric coordinate of 0, 0
        /// </summary>
        public LevelObject(ushort id)
            : this(id, 0, 0) { }

        /// <summary>
        /// Positions a GameObject based on the values stored in an XML element
        /// </summary>
        /// <param name="element"></param>
        public LevelObject(XmlElement element)
        {
            try
            {
                // Retrieve the object's position from the XML attributes
                int xPos = Convert.ToInt32(element.GetAttribute("x"));
                int yPos = Convert.ToInt32(element.GetAttribute("y"));
                IsoPosition = new Point(xPos, yPos);

                Id = Convert.ToUInt16(element.GetAttribute("id"));
            }
            catch (Exception)
            {
                throw new InvalidDataException();

                // If data is invalid, set position to 0,0
                //IsoPosition = new Point(0, 0);
                //Id = -1;
            }
        }

        /// <summary>
        /// A factory method that creates a new object based on its XML element
        /// </summary>
        /// <param name="xmlElement">The XML element to read from</param>
        /// <param name="level">The level that is loading the level</param>
        /// <returns>The new object</returns>
        public static LevelObject New(XmlElement xmlElement)
        {
            switch (xmlElement.Name)
            {
                case Ammo.XML_NODE_NAME:
                    return new Ammo(xmlElement);

                case Balloon.XML_NODE_NAME:
                    return Balloon.CreateBalloon(xmlElement);

                case BalloonPost.XML_NODE_NAME:
                    return BalloonPost.CreatePost(xmlElement);

                case Entrance.XML_NODE_NAME:
                    return new Entrance(xmlElement);

                case Enemy.XML_NODE_NAME:
                    return new Enemy(xmlElement);

                case Catapult.XML_NODE_NAME:
                    return new Catapult(xmlElement);

                case Gate.XML_NODE_NAME:
                    return new Gate(xmlElement);

                case Mine.XML_NODE_NAME:
                    return new Mine(xmlElement);

                case Lever.XML_NODE_NAME:
                    return new Lever(xmlElement);

                case Flag.XML_NODE_NAME:
                    return xmlElement.GetAttribute("player") == "1" ? new Flag(xmlElement) : (LevelObject)new RedFlag(xmlElement);
            }

            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns whether the object is included within the main object limit (~190). This method returns
        /// true unless an inheriting class overrides it.
        /// </summary>
        /// <returns></returns>
        public virtual bool IsIncludedInObjectLimit()
        {
            return true;
        }

        /// <summary>
        /// Indicates whether the object requires a static id. Objects that can be referenced
        /// by others (gate; platform; slide; lift) should not have their id changed. Objects
        /// that require a static id should override this method to provide a 'true' value
        /// </summary>
        /// <returns>Unless overridden, always returns false, which indicates that the object does not require a static id</returns>
        public virtual bool RequiresStaticId()
        {
            return false;
        }

        /// <summary>
        /// Indicates whether the object can be placed on elevated tiles. Some objects
        /// (like the gate) can only be on ground level. These objects should override this
        /// method to return a 'false' value.
        /// </summary>
        /// <returns>Unless overridden, always returns true, which indicates the object can levitate</returns>
        public virtual bool CanElevate()
        {
            return true;
        }

        /// <summary>
        /// Indicates whether the object can be deleted. This is overriden
        /// by the BalloonPost class which can only be deleted if no balloons
        /// of the same colour exist.
        /// </summary>
        /// <returns></returns>
        public virtual bool IsDeletable()
        {
            return true;
        }

        /// <summary>
        /// All classes that inherit GameObject must provide a means to compile their data
        /// into a Lemmings Paintball level file compatible format.
        /// </summary>
        /// <param name="binary">The binary editor to use to write the compiled data</param>
        public abstract void CompileVsrBinary(BinaryEditor binary, Level level, ushort? id);

        /// <summary>
        /// All classes that inherit GameObject must provide a means to compile their data
        /// to XML, which is used when saving project files
        /// </summary>
        /// <param name="xmlDoc">The XML document to use to create new elements</param>
        /// <returns>The XML element containing the data</returns>
        public abstract XmlElement CompileXml(XmlDocument xmlDoc);

        /// <summary>
        /// Stores the object's position and id (if required) within the specified element
        /// </summary>
        /// <param name="element"></param>
        protected virtual XmlElement CompileXml(XmlElement element)
        {
            // Store id if required
            element.SetAttribute("id", Id.ToString());

            // Store position
            element.SetAttribute("x", IsoPosition.X.ToString());
            element.SetAttribute("y", IsoPosition.Y.ToString());

            return element;
        }

        /// <summary>
        /// Determines whether the object should be drawn on the specified tile coordinate
        /// </summary>
        /// <param name="tileCoordinate">The map coordinate to test</param>
        /// <returns>true if the object is on the tile, otherwise false</returns>
        public virtual bool OverlapsTile(TileCoordinate tileCoordinate)
        {
            return tileCoordinate.Equals(OnTile);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="binary"></param>
        protected void AppendIdToBinary(BinaryEditor binary)
        {
            binary.Append((short)Id);
        }

        /// <summary>
        /// Appends the object's position to a binary
        /// </summary>
        /// <param name="binary">The binary to write to</param>
        protected void AppendPosition(BinaryEditor binary)
        {
            binary.Append((ushort)IsoPosition.X);
            binary.Append((ushort)IsoPosition.Y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual bool ConnectToSwitch()
        {
            return false;
        }
    }
}