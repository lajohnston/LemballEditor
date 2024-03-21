using System.Drawing;

namespace LemballEditor.View
{
    /// <summary>
    /// Represents an icon in a tile palette. Handles the drawing of the tile, its highlighting
    /// if it is selected, and whether it has been clicked on.
    /// </summary>
    internal class TilePaletteIcon
    {
        /// <summary>
        /// The icon's position and visible size
        /// </summary>
        private Rectangle area;

        /// <summary>
        /// The tileRef the icon points to
        /// </summary>
        private readonly uint tileRef;

        /// <summary>
        /// Creates a new TilePaletteIcon with the specified area and tileRef
        /// </summary>
        /// <param name="area">The icon's position and size</param>
        /// <param name="tileRef">The tileRef of the tile this icon represents</param>
        public TilePaletteIcon(Rectangle area, uint tileRef)
        {
            this.area = area;
            this.tileRef = tileRef;
        }

        /// <summary>
        /// Used to detect whether the icon has been clicked
        /// </summary>
        /// <param name="click">The coordinates of the click (relative to TilePalettePage)</param>
        /// <returns>True if the icon has been clicked on, otherwise false</returns>
        public bool isClickedOn(Point click)
        {
            Rectangle clickArea = new Rectangle(click.X, click.Y, 1, 1);
            return clickArea.IntersectsWith(area);
        }

        /// <summary>
        /// Returns the tileRef of the tile this icon represents
        /// </summary>
        /// <returns></returns>
        public uint getTileRef()
        {
            return tileRef;
        }

        /// <summary>
        /// Draws the icon, and highlights it if it is selected
        /// </summary>
        /// <param name="g">The graphics object to draw on</param>
        /// <param name="isSelected">Indicates whether the current icon is selected</param>

        /*
        public void draw(Graphics g, bool isSelected)
        {
            // Highlight tile if it is selected
            if (isSelected)
            {
                g.DrawRectangle(new Pen(Color.Thistle), area);
                g.FillRectangle(Brushes.Thistle, area);
            }

            // Retrieve the tile image
            TileImage tile = ImageCache.getTile(tileRef, false);
            Size graphicSize = tile.getBitmapSize();

            // Centre the image within the icon and draw it
            Point areaCentre = new Point(area.X + (area.Width / 2), area.Y + (area.Height / 2));
            Point imageCentre = new Point(graphicSize.Width / 2, graphicSize.Height / 2);
            Point drawPosition = new Point(areaCentre.X - imageCentre.X, areaCentre.Y - imageCentre.Y);
            tile.DrawTile(g, drawPosition);
        }
        */
    }
}