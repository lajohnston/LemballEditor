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
    class MineGraphic : ObjectGraphic
    {
        /// <summary>
        /// 
        /// </summary>
        private Mine mine;

        /// <summary>
        /// 
        /// </summary>
        public override LevelObject LevelObject
        {
            get { return mine; }
        }

        /// <summary>
        /// The object's image
        /// </summary>
        private static Bitmap image = LemballEditor.View.Level.ImageCache.GetObjectImage("mine");

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
                return new Point(2, 3);
            }
        }  

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ammo"></param>
        /// <param name="mapPanel"></param>
        public MineGraphic(Mine mine, MapPanel mapPanel)
            : base(mapPanel)
        {
            this.mine = mine;
        }
    }
}
