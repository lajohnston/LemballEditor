﻿using LemballEditor.Model;
using System.Drawing;

namespace LemballEditor.View.Level.ObjectGraphics
{
    /// <summary>
    /// Represents a drawn catapult
    /// </summary>
    internal class CatapultGraphic : ObjectGraphic
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly Catapult catapult;

        /// <summary>
        /// 
        /// </summary>
        public override LevelObject LevelObject => catapult;

        /// <summary>
        /// The object's image
        /// </summary>
        private static readonly Bitmap image_x = LemballEditor.View.Level.ImageCache.GetObjectImage("catapult_x");
        private static readonly Bitmap image_y = LemballEditor.View.Level.ImageCache.GetObjectImage("catapult_y");

        /// <summary>
        /// The draw offset of the object's graphic
        /// </summary>
        public override Point DrawOffset => new Point(42, 42);


        /// <summary>
        /// Accessor to the object image
        /// </summary>
        public override Bitmap Image => image_x;

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
