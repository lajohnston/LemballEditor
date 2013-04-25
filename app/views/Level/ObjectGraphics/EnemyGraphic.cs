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
    class EnemyGraphic : MovingObjectGraphic
    {
        /// <summary>
        /// 
        /// </summary>
        private Enemy enemy;

        /// <summary>
        /// 
        /// </summary>
        protected override MovingObject MovingObject
        {
            get { return enemy; }
        }

        /// <summary>
        /// 
        /// </summary>
        private static Bitmap image = LemballEditor.View.Level.ImageCache.GetObjectImage("enemy");
        
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
                return new Point(8, 12);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ammo"></param>
        /// <param name="mapPanel"></param>
        public EnemyGraphic(Enemy enemy, MapPanel mapPanel)
            : base(mapPanel)
        {
            this.enemy = enemy;
        }
    }
}
