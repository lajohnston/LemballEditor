using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using LemballEditor.Model;
using System.Windows.Forms;

namespace LemballEditor.View.Level.ObjectGraphics
{
    /// <summary>
    /// Represents a drawn enemy
    /// </summary>
    class FlagGraphic : ObjectGraphic
    {
        /// <summary>
        /// 
        /// </summary>
        private Flag flag;

        /// <summary>
        /// 
        /// </summary>
        public override LevelObject LevelObject
        {
            get { return flag; }
        }

        /// <summary>
        /// The object's image
        /// </summary>
        private static Bitmap image = LemballEditor.View.Level.ImageCache.GetObjectImage("flag_blue");

        /// <summary>
        /// Accessor to the object image
        /// </summary>
        public override Bitmap Image
        {
            get
            {
                return image;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override Point DrawOffset
        {
            get
            {
                return new Point(11, 28);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ammo"></param>
        /// <param name="mapPanel"></param>
        public FlagGraphic(Flag flag, MapPanel mapPanel)
            : base(mapPanel)
        {
            this.flag = flag;
        }
    }
}
