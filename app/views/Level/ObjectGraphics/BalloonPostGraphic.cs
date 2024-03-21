using LemballEditor.Model;
using System.Drawing;

namespace LemballEditor.View.Level.ObjectGraphics
{
    /// <summary>
    /// Represents a drawn balloon
    /// </summary>
    internal class BalloonPostGraphic : ObjectGraphic
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly BalloonPost post;

        /// <summary>
        /// 
        /// </summary>
        public override LevelObject LevelObject => post;

        /// <summary>
        /// 
        /// </summary>
        private static readonly Bitmap bluePost = ImageCache.GetObjectImage("post_blue");
        private static readonly Bitmap greenPost = ImageCache.GetObjectImage("post_green");
        private static readonly Bitmap redPost = ImageCache.GetObjectImage("post_red");
        private static readonly Bitmap yellowPost = ImageCache.GetObjectImage("post_yellow");

        /// <summary>
        /// 
        /// </summary>
        public override Bitmap Image
        {
            get
            {
                switch (post.Colour)
                {
                    case Balloon.Colours.Blue:
                        return bluePost;
                    case Balloon.Colours.Green:
                        return greenPost;
                    case Balloon.Colours.Red:
                        return redPost;
                    case Balloon.Colours.Yellow:
                        return yellowPost;
                }

                return base.Image;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override Point DrawOffset => new Point(16, 38);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ammo"></param>
        /// <param name="mapPanel"></param>
        public BalloonPostGraphic(BalloonPost post, MapPanel mapPanel)
            : base(mapPanel)
        {
            this.post = post;
        }
    }
}
