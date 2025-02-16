using LemballEditor.Model;
using System.Drawing;

namespace LemballEditor.View.Level.ObjectGraphics
{
    /// <summary>
    /// Represents a drawn enemy
    /// </summary>
    internal class LeverGraphic : SwitchGraphic
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly Lever lever;

        /// <summary>
        /// 
        /// </summary>
        protected override Switch SwitchObject => lever;

        /// <summary>
        /// 
        /// </summary>
        private readonly Bitmap image = LemballEditor.View.Level.ImageCache.GetObjectImage("switch");

        /// <summary>
        /// 
        /// </summary>
        public override Bitmap Image => image;

        /// <summary>
        /// 
        /// </summary>
        public override Point DrawOffset =>
                // Inaccurate
                new Point(3, 14);

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
