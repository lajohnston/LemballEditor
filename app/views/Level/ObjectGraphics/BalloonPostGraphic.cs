using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using LemballEditor.Model;
using System.Windows.Forms;

namespace LemballEditor.View.Level.ObjectGraphics
{
    /// <summary>
    /// Represents a drawn balloon
    /// </summary>
    class BalloonPostGraphic : ObjectGraphic
    {
        /// <summary>
        /// 
        /// </summary>
        private BalloonPost post;

        /// <summary>
        /// 
        /// </summary>
        public override LevelObject LevelObject
        {
            get { return post; }
        }

        /// <summary>
        /// 
        /// </summary>
        private static Bitmap bluePost = ImageCache.GetObjectImage("post_blue");
        private static Bitmap greenPost = ImageCache.GetObjectImage("post_green");
        private static Bitmap redPost = ImageCache.GetObjectImage("post_red");
        private static Bitmap yellowPost = ImageCache.GetObjectImage("post_yellow");

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
        public override Point DrawOffset
        {
            get
            {
                return new Point(16, 38);
            }
        }

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
