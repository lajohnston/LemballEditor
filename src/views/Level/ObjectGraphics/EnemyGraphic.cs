using LemballEditor.Model;
using System.Drawing;

namespace LemballEditor.View.Level.ObjectGraphics
{
    /// <summary>
    /// Represents a drawn enemy
    /// </summary>
    internal class EnemyGraphic : MovingObjectGraphic
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly Enemy enemy;

        /// <summary>
        /// 
        /// </summary>
        protected override MovingObject MovingObject => enemy;

        /// <summary>
        /// 
        /// </summary>
        private static readonly Bitmap image = LemballEditor.View.Level.ImageCache.GetObjectImage("enemy");

        /// <summary>
        /// 
        /// </summary>
        public override Bitmap Image => image;

        /// <summary>
        /// 
        /// </summary>
        public override Point DrawOffset => new Point(8, 12);

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
