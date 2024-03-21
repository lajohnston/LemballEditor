using LemballEditor.Model;
using System.Drawing;

namespace LemballEditor.View.Level.ObjectGraphics
{
    /// <summary>
    /// Represents a drawn enemy
    /// </summary>
    internal class FlagGraphic : ObjectGraphic
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly Flag flag;

        /// <summary>
        /// 
        /// </summary>
        public override LevelObject LevelObject => flag;

        /// <summary>
        /// The object's image
        /// </summary>
        private static readonly Bitmap image = LemballEditor.View.Level.ImageCache.GetObjectImage("flag_blue");

        /// <summary>
        /// Accessor to the object image
        /// </summary>
        public override Bitmap Image => image;

        /// <summary>
        /// 
        /// </summary>
        public override Point DrawOffset => new Point(11, 28);

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
