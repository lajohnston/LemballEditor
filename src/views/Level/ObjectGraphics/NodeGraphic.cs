using LemballEditor.Model;
using System.Drawing;

namespace LemballEditor.View.Level.ObjectGraphics
{
    /// <summary>
    /// Represents a drawn entrance
    /// </summary>
    internal class NodeGraphic : ObjectGraphic
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly Node node;

        /// <summary>
        /// 
        /// </summary>
        public override LevelObject LevelObject => node;

        /// <summary>
        /// The object's image
        /// </summary>
        private static readonly Bitmap image = LemballEditor.View.Level.ImageCache.GetObjectImage("node");

        /// <summary>
        /// Accessor to the object image
        /// </summary>
        public override Bitmap Image => image;

        /// <summary>
        /// The draw offset of the object's graphic
        /// </summary>
        public override Point DrawOffset => new Point(4, 11);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ammo"></param>
        /// <param name="mapPanel"></param>
        public NodeGraphic(Model.Node node, MapPanel mapPanel)
            : base(mapPanel)
        {
            this.node = node;
        }
    }
}
