using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using LemballEditor.Model;
using System.Windows.Forms;

namespace LemballEditor.View.Level.ObjectGraphics
{
    /// <summary>
    /// Represents a drawn entrance
    /// </summary>
    class AmmoGraphic : ObjectGraphic
    {
        /// <summary>
        /// 
        /// </summary>
        private Ammo ammo;

        /// <summary>
        /// 
        /// </summary>
        public override LevelObject LevelObject
        {
            get { return ammo; }
        }

        /// <summary>
        /// The object's image
        /// </summary>
        private static Bitmap image = LemballEditor.View.Level.ImageCache.GetObjectImage("ammo");

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
        /// The draw offset of the object's graphic
        /// </summary>
        public override Point DrawOffset
        {
            get
            {
                return new Point(4, 11);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ammo"></param>
        /// <param name="mapPanel"></param>
        public AmmoGraphic(Model.Ammo ammo, MapPanel mapPanel)
            : base(mapPanel)
        {
            this.ammo = ammo;
        }
    }
}
