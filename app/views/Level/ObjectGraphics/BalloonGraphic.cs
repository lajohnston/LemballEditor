using LemballEditor.Model;
using System.Drawing;

namespace LemballEditor.View.Level.ObjectGraphics
{
    /// <summary>
    /// Represents a drawn balloon
    /// </summary>
    internal class BalloonGraphic : ObjectGraphic
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly Balloon balloon;

        /// <summary>
        /// 
        /// </summary>
        public override LevelObject LevelObject => balloon;

        /// <summary>
        /// The object's image
        /// </summary>
        private static readonly Bitmap blueBalloon = LemballEditor.View.Level.ImageCache.GetObjectImage("balloon_blue");
        private static readonly Bitmap redBalloon = LemballEditor.View.Level.ImageCache.GetObjectImage("balloon_red");
        private static readonly Bitmap greenBalloon = LemballEditor.View.Level.ImageCache.GetObjectImage("balloon_green");
        private static readonly Bitmap yellowBalloon = LemballEditor.View.Level.ImageCache.GetObjectImage("balloon_yellow");

        /// <summary>
        /// Accessor to the object image
        /// </summary>
        public override Bitmap Image
        {
            get
            {
                switch (balloon.Colour)
                {
                    case Balloon.Colours.Blue:
                        return blueBalloon;
                    case Balloon.Colours.Green:
                        return greenBalloon;
                    case Balloon.Colours.Red:
                        return redBalloon;
                    case Balloon.Colours.Yellow:
                        return yellowBalloon;
                }

                return base.Image;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override Point DrawOffset => new Point(13, 66);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ammo"></param>
        /// <param name="mapPanel"></param>
        public BalloonGraphic(Balloon balloon, MapPanel mapPanel)
            : base(mapPanel)
        {
            this.balloon = balloon;
        }
    }
}
