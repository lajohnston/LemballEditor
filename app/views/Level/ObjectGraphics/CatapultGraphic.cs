using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using LemballEditor.Model;
using System.Windows.Forms;

namespace LemballEditor.View.Level.ObjectGraphics
{
    /// <summary>
    /// Represents a drawn catapult
    /// </summary>
    class CatapultGraphic : ObjectGraphic
    {
        /// <summary>
        /// 
        /// </summary>
        private Catapult catapult;

        /// <summary>
        /// 
        /// </summary>
        public override LevelObject LevelObject
        {
            get { return catapult; }
        }

        /// <summary>
        /// The object's image
        /// </summary>
        private static Bitmap image_x = LemballEditor.View.Level.ImageCache.GetObjectImage("catapult_x");
        private static Bitmap image_y = LemballEditor.View.Level.ImageCache.GetObjectImage("catapult_y");

        /// <summary>
        /// The draw offset of the object's graphic
        /// </summary>
        public override Point DrawOffset
        {
            get
            {
                return new Point(42, 42);
            }
        }


        /// <summary>
        /// Accessor to the object image
        /// </summary>
        public override Bitmap Image
        {
            get
            {
                return image_x;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ammo"></param>
        /// <param name="mapPanel"></param>
        public CatapultGraphic(Catapult catapult, MapPanel mapPanel)
            : base(mapPanel)
        {
            this.catapult = catapult;
        }
    }
}
