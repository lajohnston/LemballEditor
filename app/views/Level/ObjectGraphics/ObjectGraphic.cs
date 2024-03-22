using LemballEditor.Model;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace LemballEditor.View.Level.ObjectGraphics
{
    /// <summary>
    /// Represents a drawn object
    /// </summary>
    public abstract class ObjectGraphic
    {
        /// <summary>
        /// 
        /// </summary>
        private Rectangle lastDrawnArea;

        /// <summary>
        /// 
        /// </summary>
        public abstract LevelObject LevelObject { get; }

        /// <summary>
        /// The transparent colour used when drawing objects
        /// </summary>
        protected static Color transparent = Color.FromArgb(255, 174, 201);
        public static Color TransparentColour => transparent;

        /// <summary>
        /// 
        /// </summary>
        public virtual Bitmap Image =>
                // Return placeholder image
                ImageCache.NullObjectImage;

        /// <summary>
        /// The offset that the object image should be drawn at from a given point
        /// </summary>
        public virtual Point DrawOffset =>
                // The drawOffset of the null object image
                new Point(7, 7);

        /// <summary>
        /// The image attributes used to draw object images (transparent colour)
        /// </summary>
        protected static ImageAttributes imageAttributes;

        /// <summary>
        /// 
        /// </summary>
        protected MapPanel mapPanel;

        /// <summary>
        /// Gets the centre point of the object (it last drawn position + offset)
        /// </summary>
        /// <returns></returns>
        public Point CentrePoint =>
                // Remove the previously added draw offset and return
                new Point(lastDrawnArea.X += DrawOffset.X, lastDrawnArea.Y += DrawOffset.Y);

        /// <summary>
        /// 
        /// </summary>
        public ContextMenu RightClickMenu
        {
            get
            {
                // Create menu
                ContextMenu menu = new ContextMenu();

                // Add the object specific menu items
                AddMenuItems(menu);

                // Delete item
                MenuItem delete = new MenuItem("Delete");
                delete.Click += delegate
                {
                    mapPanel.LoadedLevel.DeleteObject(LevelObject);
                };

                _ = menu.MenuItems.Add(delete);

                // Disable delete button if the object is not deletable
                if (!LevelObject.IsDeletable())
                {
                    delete.Enabled = false;
                }

                // Return menu
                return menu;
            }
        }

        /// <summary>
        /// The static constructor for ObjectGraphic, which initialises the imageAttributes
        /// </summary>
        static ObjectGraphic()
        {
            imageAttributes = new ImageAttributes();
            imageAttributes.SetColorKey(transparent, transparent);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="levelObject"></param>
        /// <param name="tile"></param>
        protected ObjectGraphic(MapPanel mapPanel)
        {
            this.mapPanel = mapPanel;
        }

        /// <summary>
        /// Factory method for the ObjectGraphic. Creates and returns a graphic object for the specified
        /// object.
        /// </summary>
        /// <param name="levelObject"></param>
        /// <param name="mapPanel"></param>
        /// <returns></returns>
        public static ObjectGraphic New(LevelObject levelObject, MapPanel mapPanel)
        {
            if (levelObject is Ammo)
            {
                return new AmmoGraphic((Ammo)levelObject, mapPanel);
            }
            else if (levelObject is Balloon)
            {
                return new BalloonGraphic((Balloon)levelObject, mapPanel);
            }
            else if (levelObject is BalloonPost)
            {
                return new BalloonPostGraphic((BalloonPost)levelObject, mapPanel);
            }
            else if (levelObject is Catapult)
            {
                return new CatapultGraphic((Catapult)levelObject, mapPanel);
            }
            else if (levelObject is Entrance)
            {
                return new EntranceGraphic((Entrance)levelObject, mapPanel);
            }
            else if (levelObject is Enemy)
            {
                return new EnemyGraphic((Enemy)levelObject, mapPanel);
            }
            else if (levelObject is Flag)
            {
                return new FlagGraphic((Flag)levelObject, mapPanel);
            }
            else if (levelObject is Gate)
            {
                return new GateGraphic((Gate)levelObject, mapPanel);
            }
            else if (levelObject is Mine)
            {
                return new MineGraphic((Mine)levelObject, mapPanel);
            }
            else if (levelObject is Node)
            {
                return new NodeGraphic((Node)levelObject, mapPanel);
            }
            else
            {
                return levelObject is Lever
                    ? new LeverGraphic((Lever)levelObject, mapPanel)
                    : levelObject is Lift ? (ObjectGraphic)new LiftGraphic((Lift)levelObject, mapPanel) : throw new NotImplementedException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        /// <param name="firstIsoPixel"></param>
        public void Draw(Graphics g, Point screenPosition, int elevation)
        {
            // Calculate position to draw object
            int xPos = screenPosition.X - DrawOffset.X;
            int yPos = screenPosition.Y - DrawOffset.Y;

            // Elevate
            if (LevelObject.CanElevate())
            {
                yPos -= elevation;
            }

            // Paste the object's image
            Bitmap objImage = Image;
            lastDrawnArea = new Rectangle(xPos, yPos, objImage.Width, objImage.Height);
            g.DrawImage(objImage, lastDrawnArea, 0, 0, objImage.Width, objImage.Height, GraphicsUnit.Pixel, MapPanel.imageAttributes);

            // Draw box around image for debugging
            /*
            if (Program.DebugMode)
                g.DrawRectangle(new Pen(Brushes.Red), new Rectangle(screenPosition.X - DrawOffset.X, screenPosition.Y - DrawOffset.Y, objImage.Width, objImage.Height));
            */
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        /// <param name="tile"></param>
        public void Draw(Graphics g, TileCoordinate tile)
        {
            if (SnapsToTile())
            {
                Point position = mapPanel.ConvertTileXYtoScreenXY(tile, true);
                _ = mapPanel.GetTileDrawElevation(tile, true);
                Draw(g, position, 0);
            }
            else
            {
                Draw(g);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        private void Draw(Graphics g)
        {
            Draw(g, mapPanel.ConvertIsoXYtoScreenXY(LevelObject.IsoPosition), mapPanel.GetTileDrawElevation(LevelObject.OnTile, true));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cursorPosition"></param>
        public virtual void DrawAtCursor(Graphics g)
        {
            // Calculate the position to draw the object
            Point position;
            if (SnapsToTile())
            {
                position = mapPanel.ConvertTileXYtoScreenXY(mapPanel.MouseOverTileCoords, false);
            }
            else
            {
                // Set position to the cursor position
                position = mapPanel.CursorPosition;

                /** 
                 * When placing the object, it will appear to shift left 1 pixel if 
                 * mousePosition.X is an odd number, due to the screenX to isoX conversion
                 * process. Shifting it left a pixel at this stage preempts this effect so that 
                 * when the user places the object, it will not appear to shift
                 */
                if (position.X % 2 != 0)
                {
                    position.X -= 1;
                }
            }

            // Draw the image at the mouse cursor
            var elevation = GetMouseOverTileElevation();
            Draw(g, position, elevation);
        }

        /// <summary>
        /// Gets the elevation of the tile the mouse is currently hovering over
        /// </summary>
        /// <returns></returns>
        private int GetMouseOverTileElevation()
        {
            if (!LevelObject.CanElevate())
            {
                return 0;
            }

            // Get the elevation offset
            var mouseOverTile = mapPanel.MouseOverTileCoords;

            return mouseOverTile == null ? 0 : mapPanel.GetTileDrawElevation(mouseOverTile, true);
        }

        /// <summary>
        /// States whether the object should be snapped to the nearest tile when being moved by the user.
        /// </summary>
        /// <returns></returns>
        protected virtual bool SnapsToTile()
        {
            return false;
        }

        /// <summary>
        /// Determines whether the specified point overlaps the object. This procedure
        /// takes into consideration the transparent sections of the object image, so that
        /// the rectangle is only considered to be over the object if is is over a visible
        /// portion of the object image.
        /// </summary>
        /// <param name="point">The position and area of the mouse cursor</param>
        /// <returns>True if the area is overlapping the object, otherwise false</returns>
        public virtual bool OverlapsPoint(Point point)
        {
            Rectangle clickArea = new Rectangle(point, new Size(1, 1));

            // If the object is currently drawn, and the rectangle overlaps with the draw area
            if (clickArea.IntersectsWith(lastDrawnArea))
            {
                // Get the colour of the pixel that was clicked on
                Color colour = Image.GetPixel(clickArea.X - lastDrawnArea.X, clickArea.Y - lastDrawnArea.Y);

                // If the pixel is not transparent, the object was clicked on
                return !colour.Equals(TransparentColour);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tile"></param>
        /// <returns></returns>
        public virtual bool OverlapsTile(TileCoordinate tile)
        {
            return LevelObject.OverlapsTile(tile);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="menu"></param>
        protected virtual void AddMenuItems(ContextMenu menu)
        {

        }

        /// <summary>
        /// Specifies whether the mouse cursor should be positioned at the object's centre when the user
        /// clicks on and drags the object.
        /// </summary>
        /// <returns></returns>
        public virtual bool CentreCursorOnDrag()
        {
            return true;
        }
    }
}
