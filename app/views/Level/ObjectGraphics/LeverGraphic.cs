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
    class LeverGraphic : SwitchGraphic
    {
        /// <summary>
        /// 
        /// </summary>
        private Lever lever;

        /// <summary>
        /// 
        /// </summary>
        protected override Switch SwitchObject
        {
            get
            {
                return lever;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private Bitmap image = LemballEditor.View.Level.ImageCache.GetObjectImage("switch");

        /// <summary>
        /// 
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
                // Inaccurate
                return new Point(3, 14);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ammo"></param>
        /// <param name="mapPanel"></param>
        public LeverGraphic(Lever lever, MapPanel mapPanel)
            : base(mapPanel)
        {
            this.lever = lever;
        }
    }
}
