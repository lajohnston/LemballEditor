using LemballEditor.Model;
using System.Drawing;

namespace LemballEditor.View.Level.ObjectGraphics
{
    /// <summary>
    /// Represents a drawn enemy
    /// </summary>
    internal class MineGraphic : ObjectGraphic
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly Mine mine;

        /// <summary>
        /// 
        /// </summary>
        public override LevelObject LevelObject => mine;

        /// <summary>
        /// The object's image
        /// </summary>
        private static readonly Bitmap image = LemballEditor.View.Level.ImageCache.GetObjectImage("mine");

        /// <summary>
        /// Accessor to the object image
        /// </summary>
        public override Bitmap Image => image;

        /// <summary>
        /// 
        /// </summary>
        public override Point DrawOffset => new Point(2, 3);

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
