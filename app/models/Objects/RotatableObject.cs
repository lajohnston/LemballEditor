using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Windows.Forms;

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
            if (element.HasAttribute("rotated"))
                Rotated = Convert.ToBoolean(element.GetAttribute("rotated"));
            else
                Rotated = false;
        }

        
        protected override XmlElement CompileXml(XmlElement element)
        {
            // Rotation
            if (Rotated)
                element.SetAttribute("rotated", "true");

            // Position
            return base.CompileXml(element);
        }
    }
}
