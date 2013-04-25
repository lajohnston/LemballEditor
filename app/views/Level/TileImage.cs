using System.Text;
using System;
using System.Drawing;
using LemballEditor.Model;
using System.Drawing.Imaging;
using System.Xml;
using System.IO;

namespace LemballEditor.View.Level
{
    /// <summary>
    /// Stores information about a tile type that has been loaded from the cache, including its image and height
    /// </summary>
    public class TileImage
    {
        /// <summary>
        /// The maximum elevation possible for any tile
        /// </summary>
        private const byte MAX_ELEVATION = 88;

        /// <summary>
        /// Stores the null tile, which is returned if the tileRef does not appear in the archive
        /// </summary>
        private static Bitmap nullTile = new Bitmap(LemballEditor.Properties.Resources.nullTile);

        /// <summary>
        /// The draw offset points to the top left pixel of the 2D boundary that surrounds the isometric tile.
        /// If the tile has height, then this points to tile at the base of the object.
        /// </summary>
        private Point drawOffset;

        public uint tileRef { get; private set; }

        /// <summary>
        /// The bitmap for the tile
        /// </summary>
        private Bitmap image;

        private ImageAttributes imageAttributes;

        /// <summary>
        /// Creates a new tileImage object
        /// </summary>
        /// <param name="terrain"></param>
        /// <param name="tileRef"></param>
        public TileImage(XmlElement tileNode)
        {
            tileRef = Convert.ToUInt32(tileNode.GetAttribute("ref"));

            if (tileNode.HasAttribute("image_ref"))
            {
                String imageRef = tileNode.GetAttribute("image_ref");
                XmlElement referencedTile = (XmlElement)tileNode.ParentNode.SelectSingleNode("tile[@ref=" + imageRef + "]");
                LoadImage(referencedTile);
            }
            else
            {
                LoadImage(tileNode);
            }
        }

        /// <summary>
        /// Initialises a blank tile image, which is used as a placeholder when a tile cannot be found in the cache
        /// </summary>
        public TileImage()
        {
            tileRef = 0;
            setImage(nullTile);
            drawOffset = new Point(0, 0);
        }

        /// <summary>
        /// Loads the image and the offset from the specified tileNode
        /// </summary>
        /// <param name="tileNode"></param>
        private void LoadImage(XmlElement tileNode)
        {
            // Load the image from the cache and set it as the tile's image
            setImage(ImageCache.loadBitmap(tileNode));

            // Initialise the Point object that stores the draw offset of the tile
            drawOffset = new Point(0, 0);

            // Use the xOffset attribute value if present, otherwise calculate the xOffset from the image's width
            if (tileNode.HasAttribute("xOffset"))
                drawOffset.X = Convert.ToInt32(tileNode.GetAttribute("xOffset"));
            else
                drawOffset.X = (int) Math.Ceiling((decimal)(image.Width - 32) / 2);

            // Use the yOffset attribute value if present, otherwise calculate the yOffset from the image's height
            if (tileNode.HasAttribute("yOffset"))
                drawOffset.Y = Convert.ToInt32(tileNode.GetAttribute("yOffset"));
            else
                // Calculate the yOffset automatically
                drawOffset.Y = image.Height - 16;
        }

        /// <summary>
        /// Sets the tile's bitmap image and its transparent colour
        /// </summary>
        /// <param name="image"></param>
        private void setImage(Bitmap image)
        {
            // The the tileImage's image
            this.image = image;

            // Set the transparent colour to the first pixel
            Color transparent = image.GetPixel(0, 0);
            imageAttributes = new ImageAttributes();
            imageAttributes.SetColorKey(transparent, transparent);
        }

        /// <summary>
        /// Retrieves the tile's bitmap. Returns a nullTile graphic if the tile didn't successfully load.
        /// </summary>
        /// <returns></returns>
        public Bitmap getBitmap()
        {
            return image;
        }

        /// <summary>
        /// Returns the size of the bitmap
        /// </summary>
        /// <returns></returns>
        public Size getBitmapSize()
        {
            return new Size(image.Width, image.Height);
        }

        /// <summary>
        /// Draws the tile at the specified position onto the specified Graphics object
        /// </summary>
        /// <param name="g"></param>
        /// <param name="position"></param>
        public void DrawTile(Graphics g, Point position, bool offset)
        {
            if (offset)
            {
                position.X -= drawOffset.X;
                position.Y -= drawOffset.Y;
            }

            // Paste the bitmap
            Rectangle rect = new Rectangle(position.X, position.Y, image.Width, image.Height);
            g.DrawImage(image, rect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, imageAttributes);
        }

        public void DrawTile(Graphics g, Point position)
        {
            DrawTile(g, position, true);
        }

        /// <summary>
        /// Returns the tile's maximum elevation value
        /// </summary>
        /// <returns></returns>
        public byte getMaxElevation()
        {
            int elevation = MAX_ELEVATION - image.Height + 16;

            if (elevation < 0)
                return 0;
            else
                return (byte) elevation;
        }
    }
    
}