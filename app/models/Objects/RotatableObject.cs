using System;
using System.Xml;

namespace LemballEditor.Model
{
    public abstract class RotatableObject : LevelObject
    {
        /// <summary>
        /// 
        /// </summary>
        public bool Rotated { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isoX"></param>
        /// <param name="isoY"></param>
        public RotatableObject(ushort id, ushort isoX, ushort isoY)
            : base(id, isoX, isoY)
        {
            Rotated = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public RotatableObject(ushort id)
            : this(id, 0, 0)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public void Rotate()
        {
            // Invert the rotate value
            Rotated = !Rotated;

            // Inform interface
            Program.OnObjectAlteration(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        public RotatableObject(XmlElement element)
            : base(element)
        {
            Rotated = element.HasAttribute("rotated") && Convert.ToBoolean(element.GetAttribute("rotated"));
        }


        protected override XmlElement CompileXml(XmlElement element)
        {
            // Rotation
            if (Rotated)
            {
                element.SetAttribute("rotated", "true");
            }

            // Position
            return base.CompileXml(element);
        }
    }
}
