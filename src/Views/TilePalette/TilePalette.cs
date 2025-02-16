using LemballEditor.View.Level;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace LemballEditor.View
{
    /// <summary>
    /// Displays a selection of tiles according to their type. The tiles are divided into
    /// pages, which the user can navigate through
    /// </summary>
    internal class TilePalette : TabPage
    {
        /// <summary>
        /// The maximum tile row width of pixels
        /// </summary>
        private const int MAX_ROW_WIDTH = 150;

        /// <summary>
        /// The minimum height for each row
        /// </summary>
        public const int MIN_ROW_HEIGHT = 20;

        /// <summary>
        /// The size in pixels between each tile on a row
        /// </summary>
        private const int X_SPACE_BETWEEN_TILES = 5;

        /// <summary>
        /// A list of all the tiles that were drawn in the last update
        /// </summary>
        private readonly List<TilePaletteIcon> drawnIcons;

        /// <summary>
        /// The tile types that this tilePallette displays
        /// </summary>
        private readonly ImageCache.TileTypes[] tileTypes;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="tileTypes"></param>
        public TilePalette(string name, ImageCache.TileTypes[] tileTypes) : base(name)
        {
            this.tileTypes = tileTypes;
            base.BackColor = Color.White;
            DoubleBuffered = true;

            drawnIcons = new List<TilePaletteIcon>();
            MouseDown += new MouseEventHandler(TilePalettePage_MouseClick);
        }

        /// <summary>
        /// User clicks on the panel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="mouse"></param>
        public void TilePalettePage_MouseClick(object sender, MouseEventArgs mouse)
        {
            Point click = new Point(mouse.X, mouse.Y + TilePaletteSelector.scrollValue);

            foreach (TilePaletteIcon icon in drawnIcons)
            {
                if (icon.isClickedOn(click))
                {
                    SelectIcon(icon);
                    break;
                }
            }

        }

        /// <summary>
        /// Selects an icon
        /// </summary>
        /// <param name="icon"></param>
        public void SelectIcon(TilePaletteIcon icon)
        {
            TilePaletteSelector.SetSelectedTileRef(icon.getTileRef());

            if (Program.DebugMode)
            {
                System.Console.WriteLine("Selected tile: " + icon.getTileRef());
            }

            // Refresh display
            Invalidate();
        }

        /// <summary>
        /// Renders the tile browser
        /// </summary>
        /// <returns>A bitmap containing the rendered browser</returns>
        public Bitmap RenderBrowser()
        {
            // Clear the drawn icons list
            drawnIcons.Clear();

            // Load the tile images
            List<TileImage> tileImages = new List<TileImage>(50);
            foreach (ImageCache.TileTypes tileType in tileTypes)
            {
                ImageCache.getTileRefs(tileType, ref tileImages);
            }

            // Create a temporary bitmap to render the browser in, which will later be cropped
            Bitmap browserTmp = new Bitmap(MAX_ROW_WIDTH, 1000);
            Graphics g = Graphics.FromImage(browserTmp);

            // The position of the first row
            Point rowTopLeftPosition = new Point(0, 10);

            // Each row of tiles
            int tileIndex = 0;
            while (tileIndex < tileImages.Count)
            {
                // Create a variable to store the row's height
                int rowHeight = MIN_ROW_HEIGHT;

                // Render the row
                RenderTileRow(g, ref tileIndex, rowTopLeftPosition, ref rowHeight, tileImages);

                // Set the position of the next row
                rowTopLeftPosition.Y += rowHeight + 5;
            }

            // Crop the bitmap and return it
            Rectangle rect = new Rectangle(0, 0, browserTmp.Width, rowTopLeftPosition.Y);
            Bitmap cropped = browserTmp.Clone(rect, browserTmp.PixelFormat);
            return cropped;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g">The graphics object to draw on</param>
        /// <param name="nextTileIndex">The index of the next tile to draw in the tileImages list</param>
        /// <param name="rowYPosition">The top Y position of the row</param>
        /// <param name="rowHeight">The variable used to return the height of the row</param>
        /// <param name="tileImages">The list of tile images</param>
        private void RenderTileRow(Graphics g, ref int nextTileIndex, Point rowTopLeftPosition, ref int rowHeight, List<TileImage> tileImages)
        {
            int firstTileIndex = nextTileIndex;

            // Set the space left in this row in pixels
            int roomLeftInRow = MAX_ROW_WIDTH;

            // Iterate through each remaining tile
            while (nextTileIndex < tileImages.Count)
            {
                // Get the width of the tile
                Size tileSize = tileImages[nextTileIndex].getBitmapSize();

                // If there is room for the tile in this row
                int roomLeftAfter = roomLeftInRow - tileSize.Width - X_SPACE_BETWEEN_TILES;
                if (roomLeftAfter > 0)
                {
                    // Subtract the tileWidth from the space left in the row
                    roomLeftInRow = roomLeftAfter;

                    // If the tile exceeds the row's height, increase the row's height
                    if (tileSize.Height > rowHeight)
                    {
                        rowHeight = tileSize.Height;
                    }

                    // Iterate tileIndex
                    nextTileIndex++;
                }
                else
                {
                    // Tile doesn't fit in this row, so exit loop
                    break;
                }
            }

            // Each tile that's in the row
            Point tilePosition = rowTopLeftPosition;

            // Draw an outline of the row (debugging)
            //Rectangle rowArea = new Rectangle(rowTopLeftPosition, new Size(MAX_ROW_WIDTH, rowHeight));
            //g.DrawRectangle(new Pen(Color.YellowGreen), rowArea);

            // Draw each tile in the row
            for (int i = firstTileIndex; i < nextTileIndex; i++)
            {
                // Get the tile to draw
                TileImage tileImage = tileImages[i];
                Size tileSize = tileImage.getBitmapSize();

                // The the tile
                Point drawPosition = new Point(tilePosition.X, tilePosition.Y + (rowHeight - tileSize.Height));
                tileImage.DrawTile(g, drawPosition, false);

                // Add the image to the drawn icons list
                Rectangle clickArea = new Rectangle(drawPosition, tileSize);
                drawnIcons.Add(new TilePaletteIcon(clickArea, tileImage.tileRef));

                // Set the position of the next tile
                tilePosition.X += tileSize.Width + X_SPACE_BETWEEN_TILES;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            // Get the graphics object
            Graphics g = e.Graphics;

            // Draw the rendered browser
            g.DrawImage(TilePaletteSelector.browser, new Point(0, 0 - TilePaletteSelector.scrollValue));
        }
    }
}