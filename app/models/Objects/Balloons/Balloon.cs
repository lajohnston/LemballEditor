using System;
using System.IO;
using System.Xml;
using VsrCompiler;

namespace LemballEditor.Model
{
    public abstract class Balloon : LevelObject
    {
        /// <summary>
        /// 
        /// </summary>
        public const string XML_NODE_NAME = "balloon";

        /// <summary>
        /// 
        /// </summary>
        public abstract Colours Colour { get; }

        /// <summary>
        /// The data block in which the object data is stored in compiled levels
        /// </summary>
        public override LevelObject.ObjectBlocks ObjectBlock => ObjectBlocks.BOMG;

        /// <summary>
        /// 
        /// </summary>
        public enum Colours
        {
            Red = 39,
            Blue = 41,
            Green = 43,
            Yellow = 45,
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public Balloon(ushort id)
            : base(id)
        {

        }

        /// <summary>
        /// Creates a balloon based from its XML representation
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static Balloon CreateBalloon(XmlElement element)
        {
            // Get the balloon's colour
            Colours colour = (Colours)Enum.Parse(typeof(Colours), element.GetAttribute("colour"));

            // Get the balloon's id
            ushort id = Convert.ToUInt16(element.GetAttribute("id"));

            switch (colour)
            {
                case Colours.Red:
                    return new RedBalloon(id);
                case Colours.Blue:
                    return new BlueBalloon(id);
                case Colours.Green:
                    return new GreenBalloon(id);
                case Colours.Yellow:
                    return new YellowBalloon(id);
                default:
                    throw new InvalidDataException();
            }
        }



        /// <summary>
        /// Creates the relevant balloon post for this colour of balloon
        /// </summary>
        /// <returns></returns>
        public BalloonPost CreatePost(ushort id)
        {
            BalloonPost post = new BalloonPost(id, Colour)
            {
                IsoPosition = IsoPosition
            };
            return post;
        }

        public override void CompileVsrBinary(BinaryEditor binary, Level level, ushort? id)
        {
            // Id
            binary.Append((short)0);

            // Position
            base.AppendPosition(binary);

            // Null
            binary.Append((short)0);

            // Colour
            binary.Append((short)Colour);

            // Unknown
            binary.Append((short)0);
        }



        public override XmlElement CompileXml(XmlDocument xmlDoc)
        {
            // Create xml element
            XmlElement element = xmlDoc.CreateElement("balloon");

            // Set position
            _ = base.CompileXml(element);

            // Set colour
            element.SetAttribute("colour", Colour.ToString());

            // Return element
            return element;
        }
    }
}
