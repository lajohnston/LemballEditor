using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using LemballEditor.Model;
using LemballEditor.View.Level.ObjectGraphics;

namespace LemballEditor.View.Level
{
    /// <summary>
    /// The graphical interface used to edit tiles and place objects
    /// </summary>
    public partial class MapPanel : Panel
    {
        /// <summary>
        /// The portion of the map that can be viewed at a time. Value is for both X and Y dimensions.
        /// </summary>
        private const byte MAX_VIEW_SIZE_XY = 22;

        /// <summary>
        /// The number of times the map display is refresh per second
        /// </summary>
        public const byte REFRESH_RATE = 60;

        /// <summary>
        /// The size of the visible portion of the map, measured in number of tiles. The Width
        /// indicates the number of tiles from the top of the isometric diamond to the right point,
        /// and the Height represents the number of tiles from the top of the isometric diamond
        /// to the left point
        /// </summary>
        private Size mapViewTileDimensions;

        /// <summary>
        /// Indicates whether the map needs to be re-rendered
        /// </summary>
        private bool updateRendering;

        /// <summary>
        /// Stores a bitmap of the rendered map
        /// </summary>
        private Bitmap renderedMap;

        /// <summary>
        /// True if the mouse is over the mapPanel, otherwise false
        /// </summary>
        private bool mouseIsOverPanel;

        /// <summary>
        /// The map coordinate of the first viewable tile (at the top of the isometric diamond)
        /// </summary>
        private TileCoordinate firstViewTile;

        /// <summary>
        /// The screen position of the first isometric pixel, which may be outside the viewable
        /// portion of the map depending on the scroll position. This pixel points to the top pixel
        /// of the first tile in the map.
        /// </summary>
        private Point tile0Position;

        /// <summary>
        /// The top pixel of the map, relative to the mapPanel. Calculated each time the map is rendered.
        /// </summary>
        private Point topPixelOfViewableMap;

        /// <summary>
        /// The bitmap used to highlight the outline of tiles
        /// </summary>
        private static Bitmap selectedTileOutline = new Bitmap(LemballEditor.Properties.Resources.mouseOverTile);

        /// <summary>
        /// The bitmap used to highlight tiles tiles
        /// </summary>
        private static Bitmap selectedTileFill = new Bitmap(LemballEditor.Properties.Resources.selectedTile);

        /// <summary>
        /// The attributes used to draw the selected tile fill graphic
        /// </summary>
        private static ImageAttributes selectedTileFillAttributes;

        /// <summary>
        /// The image attributes used to draw images with transparency
        /// </summary>
        public static ImageAttributes imageAttributes { get; private set; }

        /// <summary>
        /// Objects that were drawn at the last update
        /// </summary>
        //private List<LevelObject> drawnObjects;

        private List<ObjectGraphic> drawnObjectImages;
        private List<ObjectGraphic> objectViews;

        /// <summary>
        /// The current editing mode, which defines what should occur when certain events take place
        /// </summary>
        private EditingMode editingMode;

        /// <summary>
        /// A list of tiles that have been selected by the user
        /// </summary>
        private List<TileCoordinate> selectedTiles;

        /// <summary>
        /// A list of active tiles, that is, tiles that will be edited by an action such as altering elevation. 
        /// If multiple tiles are selected, then they are returned. If no tiles are selected, the list contains
        /// the tile the mouse is over
        /// </summary>
        private List<TileCoordinate> ActiveTiles
        {
            get
            {
                // Create list to hold tiles
                List<TileCoordinate> list = new List<TileCoordinate>();

                // If no tiles are selected
                if (selectedTiles.Count == 0)
                {
                    // Add the mouseovertile to the list
                    list.Add(MouseOverTileCoords);
                }
                else
                {
                    // Convert each tile coordinate to a MapTile, and add to the list
                    foreach (TileCoordinate tileCoord in selectedTiles)
                    {
                        list.Add(tileCoord);
                    }
                }

                // Return the list of tiles
                return list;
            }
        }

        /// <summary>
        /// The level that is currently loaded
        /// </summary>
        public Model.Level LoadedLevel
        {
            get
            {
                return Program.LoadedLevel;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        static MapPanel()
        {
            Color transparentColour = Color.FromArgb(255, 174, 201);

            // Create the image attribute, which is used when drawing images such as the tile selection
            imageAttributes = new ImageAttributes();
            imageAttributes.SetColorKey(transparentColour, transparentColour);

            // Set transparent colour of tile fill graphic
            selectedTileFillAttributes = new ImageAttributes();
            selectedTileFillAttributes.SetColorKey(transparentColour, transparentColour);

            // Create a remap table to make the fill semi-transparent
            ColorMap map = new ColorMap();
            map.OldColor = selectedTileFill.GetPixel(16, 8);
            map.NewColor = Color.FromArgb(80, map.OldColor);
            selectedTileFillAttributes.SetRemapTable(new ColorMap[] { map });
        }

        /// <summary>
        /// Creates a new MapPanel, which is used to edit the actual map of the level
        /// </summary>
        public MapPanel()
        {
            // Initialise the position of the first pixel of the map (set each time it is drawn)
            topPixelOfViewableMap = new Point(0, 0);

            // Set the top tile in the map view
            firstViewTile = new TileCoordinate(0, 0);

            // Initialise the mouse and keyboard variables
            CursorPosition = new Point(0, 0);
            mouseIsOverPanel = true;
            mouseTileCoordinate = new TileCoordinate(0, 0);
            heldKeys = new List<Keys>();

            // Fill container
            Anchor = AnchorStyles.None;
            Dock = DockStyle.Fill;

            // Double buffers the display so that it doesn't flicker
            this.DoubleBuffered = true;

            //
            drawnObjectImages = new List<ObjectGraphic>(10);
            objectViews = new List<ObjectGraphic>(10);

            // 
            renderedMap = new Bitmap(MAX_VIEW_SIZE_XY * 32, MAX_VIEW_SIZE_XY * 16 + 90);

            //
            selectedTiles = new List<Model.TileCoordinate>();

            // The size of the viewable portion of the map
            mapViewTileDimensions = new Size(MAX_VIEW_SIZE_XY, MAX_VIEW_SIZE_XY);

            // Add event handlers
            MouseWheel += new MouseEventHandler(MapPanel_MouseWheel);
            MouseMove += new MouseEventHandler(MapPanel_MouseMove);
            MouseEnter += new EventHandler(MapPanel_MouseEnter);
            MouseLeave += new EventHandler(MapPanel_MouseLeave);
            MouseDown += new MouseEventHandler(MapPanel_MouseDown);
            MouseUp += new MouseEventHandler(MapPanel_MouseUp);

            // Set the default editing mode
            StartDefaultEditingMode();
        }

        /// <summary>
        /// Called when a new level has been loaded
        /// </summary>
        public void OnLevelLoad()
        {
            if (LoadedLevel != null)
            {
                // Enable and make visible
                Enabled = true;
                Visible = true;

                // Resize the viewable portion of the map
                mapViewTileDimensions.Width = (byte)Math.Min(MAX_VIEW_SIZE_XY, LoadedLevel.MapSizeX);
                mapViewTileDimensions.Height = (byte)Math.Min(MAX_VIEW_SIZE_XY, LoadedLevel.MapSizeY);

                // Load object views
                objectViews.Clear();
                LevelObject[] levelObjects = LoadedLevel.LevelObjects;
                foreach (LevelObject levelObject in levelObjects)
                {
                    objectViews.Add(ObjectGraphic.New(levelObject, this));
                }


                SetFirstViewTile(0, 0);

                StartDefaultEditingMode();

                // Update rendering at next update
                RenderMapAtNextUpdate();

                // Call for immediate refresh
                this.Invalidate();
            }
            else
            {
                // Disable panel and make invisible
                this.Enabled = false;
                this.Visible = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="menu"></param>
        private void ShowMenuAtMouse(ContextMenu menu)
        {
            Point position = new Point(CursorPosition.X + 30, CursorPosition.Y);
            menu.Show(Program.MainInterface, position);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        /// <param name="image"></param>
        /// <param name="xPos"></param>
        /// <param name="yPos"></param>
        private void DrawImage(Graphics g, Bitmap image, int xPos, int yPos)
        {
            DrawImage(g, image, xPos, yPos, imageAttributes);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        /// <param name="image"></param>
        /// <param name="xPos"></param>
        /// <param name="yPos"></param>
        /// <param name="attributes"></param>
        private void DrawImage(Graphics g, Bitmap image, int xPos, int yPos, ImageAttributes attributes)
        {
            // Paste the bitmap
            Rectangle rect = new Rectangle(xPos, yPos, image.Width, image.Height);
            g.DrawImage(image, rect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, attributes);
        }

        /// <summary>
        /// Sets the tile at the top of isometric map view diamond
        /// </summary>
        /// <param name="xTile"></param>
        /// <param name="yTile"></param>
        public void SetFirstViewTile(ushort xTile, ushort yTile)
        {
            firstViewTile.xTile = xTile;
            firstViewTile.yTile = yTile;

            // Request redraw
            RenderMapAtNextUpdate();
        }

        /// <summary>
        /// Returns the LevelObject that the specified point overlaps. This can be used to detect which object has
        /// been clicked on (if any) or whether the mouse is above the object
        /// </summary>
        /// <param name="mousePosition"></param>
        /// <returns></returns>
        private LevelObject GetObjectMouseIsOver(Point mousePosition)
        {
            return editingMode.GetObjectMouseIsOver(mousePosition);
        }

        /// <summary>
        /// Called when an object graphic has been altered, for example if it has been rotated
        /// </summary>
        /// <param name="levelObject"></param>
        public void OnObjectAlteration(LevelObject levelObject)
        {
            RenderMapAtNextUpdate();
        }

        /// <summary>
        /// Paints the map
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            if (Program.LevelPackIsLoaded())
            {
                // Get the graphics object
                Graphics g = e.Graphics;

                // Re-render the map if it needs to be
                if (updateRendering)
                {
                    try
                    {
                        RenderMap();
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Graphic error when rendering map");
                    }
                }

                // Draw the rendered map
                g.DrawImageUnscaled(renderedMap, 0, 0);

                // Highlight the selected tiles
                HighlightTileSelection(g);

                /* 
                 * Draw the overlay that the editing mode specifies, such as held objects, tile pasting preview,
                 * or the highlighting of the tile the mouse is over)
                 */
                editingMode.Update(g);

                //
                base.OnPaint(e);
            }
        }

        /// <summary>
        /// Highlights the specified tile if it is within the visible portion of the map
        /// </summary>
        /// <param name="g"></param>
        /// <param name="tileCoordinate"></param>
        private void HighlightTile (Graphics g, TileCoordinate tileCoordinate, bool fill)
        {
            if (tileCoordinate != null && TileIsVisible(tileCoordinate))
            {
                Point tilePosition = ConvertTileXYtoScreenXY(tileCoordinate, false);

                // Get the tile's the elevation
                //int elevation = LoadedLevel.GetTile(tileCoordinate).Elevation;
                int elevation = GetTileDrawElevation(tileCoordinate, true);

                //
                if (fill)
                {
                    DrawImage(g, selectedTileFill, tilePosition.X, tilePosition.Y - elevation, selectedTileFillAttributes);

                    // Highlight top of tile pillar
                    DrawImage(g, selectedTileOutline, tilePosition.X, tilePosition.Y - elevation);
                }
                else
                {
                    // Highlight the outline of the base of the tile
                    DrawImage(g, selectedTileOutline, tilePosition.X, tilePosition.Y);

                    // If the elevation is above 0
                    if (elevation > 0)
                    {
                        // Highlight top of tile pillar
                        DrawImage(g, selectedTileOutline, tilePosition.X, tilePosition.Y - elevation);

                        // Highlight each side of the pillar
                        Pen pen = new Pen(Color.Yellow, 1);
                        int y1 = tilePosition.Y + 8;
                        int y2 = y1 - elevation;

                        g.DrawLine(pen, tilePosition.X, y1, tilePosition.X, y2);
                        g.DrawLine(pen, tilePosition.X + 31, y1, tilePosition.X + 31, y2);
                    }
                }

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        private void HighlightTileSelection(Graphics g)
        {
            foreach (TileCoordinate tile in selectedTiles)
            {
                HighlightTile(g, tile, true);
            }
        }

        /// <summary>
        /// Checks whether the mouse is over the visible portion of the map
        /// </summary>
        /// <returns>true if mouse is over the map, otherwise false</returns>
        private bool MouseIsOverMap()
        {
            return TileIsVisible(mouseTileCoordinate);
        }

        /// <summary>
        /// Checks whether the specified screen position is on the visible portion of the map
        /// </summary>
        /// <returns></returns>
        private bool PositionIsOnMap(Point position)
        {
            TileCoordinate tile = ConvertScreenXYtoTileXY(position.X, position.Y);
            return TileIsVisible(tile);
        }

        /// <summary>
        /// States whether the specified tile is in the viewable portion of the map
        /// </summary>
        /// <returns></returns>
        private bool TileIsVisible(TileCoordinate tile)
        {
            // Detect if the tile is off the viewable portion of the map
            if (tile == null
                || tile.xTile < firstViewTile.xTile
                || tile.yTile < firstViewTile.yTile
                || tile.xTile >= firstViewTile.xTile + mapViewTileDimensions.Width
                || tile.yTile >= firstViewTile.yTile + mapViewTileDimensions.Height
                )
                return false;
            else
                // Tile is on the map
                return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="levelObject"></param>
        /// <returns></returns>
        private bool ObjectIsVisible(LevelObject levelObject)
        {
            return TileIsVisible(levelObject.OnTile);
        }

        /// <summary>
        /// Returns the image data of the specified object. Returns null if the object isn't currently drawn
        /// </summary>
        /// <param name="levelObject"></param>
        /// <returns></returns>
        private ObjectGraphic GetObjectGraphic(LevelObject levelObject)
        {
            foreach (ObjectGraphic imageData in objectViews)
            {
                if (imageData.LevelObject.Equals(levelObject))
                    return imageData;
            }

            // Initialise object view and add to list
            ObjectGraphic objectView = ObjectGraphic.New(levelObject, this);
            objectViews.Add(objectView);
            return objectView;
        }

        /// <summary>
        /// States whether the given isometric coordinate is within the bounds of the viewable portion of the map
        /// </summary>
        /// <param name="isoPosition"></param>
        /// <returns></returns>
        private bool IsoPositionIsOnViewableMap(Point isoPosition)
        {
            // Ensure the isoposition is within the bounds of the viewable portion of the map
            if (isoPosition.X < 0
                || isoPosition.Y < 0
                || isoPosition.X > (firstViewTile.xTile + mapViewTileDimensions.Width) * 16
                || isoPosition.Y > (firstViewTile.yTile + mapViewTileDimensions.Height) * 16
                )
            {
                return false;
            }
            else
            {
                // Position is visible
                return true;
            }
        }

        /// <summary>
        /// Renders the current map
        /// </summary>
        private void RenderMap()
        {
            // Get the graphics object from the rendered map
            Graphics g = Graphics.FromImage(renderedMap);

            // Clear the old rendered map
            g.Clear(Color.White);

            // The coordinate to draw the first tile
            topPixelOfViewableMap.X = renderedMap.Width / 2;
            topPixelOfViewableMap.Y = 90;

            /* Calculate the position of the first tile in the map, that is, the very first tile
                * (which may be off screen depending on the scroll position) rather than the first
            * tile in the viewable portion of the map */
            tile0Position = new Point(
                topPixelOfViewableMap.X - (firstViewTile.xTile * 16) + (firstViewTile.yTile * 16),
                topPixelOfViewableMap.Y - (firstViewTile.xTile * 8) - (firstViewTile.yTile * 8));

            /*
            Bitmap mapTop = new Bitmap(1, 1);
            mapTop.SetPixel(0, 0, Color.Indigo);
            drawImage(g, mapTop, topOfMap.X, topOfMap.Y);
            */

            // Get the elevation graphic for the current terrain type
            Bitmap elevationBmp = ImageCache.getElevationBitmap();

            // Get the number of tiles in the map
            int xTiles = LoadedLevel.MapSizeX;
            int yTiles = LoadedLevel.MapSizeY;

            // Erase previously drawn objects
            drawnObjectImages.Clear();

            // Stores the tileX and tileY values of the actual tile being drawn
            TileCoordinate currentTileCoordinate = new TileCoordinate(firstViewTile.xTile, firstViewTile.yTile);

            // Each row of tiles
            for (ushort viewTileY = 0; viewTileY < mapViewTileDimensions.Height; viewTileY++)
            {
                // Each tile in row
                for (ushort viewTileX = 0; viewTileX < mapViewTileDimensions.Width; viewTileX++)
                {
                    // Get the current tile being drawn
                    //MapTile currentTile = LoadedLevel.GetTile(currentTileCoordinate);

                    if (currentTileCoordinate == null)
                        break;

                    // Get the current tile's elevation
                    int elevation = GetTileDrawElevation(currentTileCoordinate, true);

                    // The screen X and Y coordinates to draw the tile
                    //Point tilePos = getTileScreenPosition(currentTileCoordinate);
                    int xPos = topPixelOfViewableMap.X + (viewTileX * 16) - (viewTileY * 16) - 16;
                    int yPos = topPixelOfViewableMap.Y + (viewTileX * 8) + (viewTileY * 8) - elevation;
                    Point tilePos = new Point(xPos, yPos);

                    // Draw the tile
                    ImageCache.DrawTile(g, LoadedLevel.GetTileImageRef(currentTileCoordinate), tilePos);

                    // Draw elevation
                    int elevCount = (int)Math.Ceiling((double)elevation / 16);
                    for (int elev = 0; elev != elevCount; elev++)
                    {
                        // Get position
                        int elevX = tilePos.X;
                        int elevY = tilePos.Y + 8 + (elev * 16);

                        // Draw elevation
                        DrawImage(g, elevationBmp, elevX, elevY);
                    }

                    // Draw objects
                    DrawObjectsOnTile(g, LoadedLevel, currentTileCoordinate);
                    

                    // Increment the currentTile X value
                    currentTileCoordinate.xTile++;
                }

                // End of the row
                currentTileCoordinate.xTile = firstViewTile.xTile;
                currentTileCoordinate.yTile++;
            }

            // Finalise
            g.Dispose();

            // The map is now rendered
            updateRendering = false;
        }

        /// <summary>
        /// Returns the elevation to draw the specified tile at. If a lift overlaps it, the lift's preview height is returned
        /// </summary>
        /// <returns></returns>
        public int GetTileDrawElevation(TileCoordinate tile, bool withLiftOverride)
        {
            // If lift override mode is selected
            if (withLiftOverride)
            {
                // Check each liftGraphic
                //List<LevelObject> objectsOnTile = LoadedLevel.GetObjectsOverlapingTile(tile);

                foreach (ObjectGraphic objectView in objectViews)
                {
                    if (objectView is LiftGraphic)
                    {
                        // If the lift overlaps the tile, return it's preview height
                        if (objectView.OverlapsTile(tile))
                        {
                            return ((LiftGraphic)objectView).PreviewHeight;
                        }
                    }
                }
            }

            // If no lifts overlap the tile, return the height of the maptile
            return LoadedLevel.GetTileElevation(tile);
        }

        /// <summary>
        /// Draws all the objects on the specified tile coordinate
        /// </summary>
        /// <param name="g"></param>
        /// <param name="level"></param>
        /// <param name="mapCoordinate"></param>
        /*
        private void OldDrawObjectsOnTile(Graphics g, Level level, TileCoordinate mapCoordinate)
        {
            // Get a list of all objects that are on the specified tile
            List<LevelObject> objects = level.OldGetObjectsOverlapingTile(mapCoordinate);

            // Draw each object that is on the tile
            foreach (LevelObject levelObject in objects)
            {
                // Determine whether the object is currently being held by the user
                if (!editingMode.ObjectIsBeingHeld(levelObject))
                {
                    // Draw object
                    level.DrawObject(levelObject, g, ConvertIsoXYtoScreenXY(levelObject.IsoPosition));

                    // Add object to list of drawn objects
                    if (!drawnObjects.Contains(levelObject))
                        drawnObjects.Add(levelObject);
                }

                // Draw a dot at the object's position, without its image offset
                //Bitmap dot = new Bitmap(1, 1);
                //dot.SetPixel(0, 0, Color.HotPink);
                //drawImage(g, dot, screenPosition.X, screenPosition.Y);
            }
        }
        */

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        /// <param name="level"></param>
        /// <param name="mapCoordinate"></param>
        private void DrawObjectsOnTile(Graphics g, Model.Level level, TileCoordinate mapCoordinate)
        {
            // Get ObjectImageData objects
            List<LevelObject> objectsOnTile = level.GetObjectsOverlapingTile(mapCoordinate);

            foreach (LevelObject levelObject in objectsOnTile)
            {
                if (!editingMode.ObjectIsBeingHeld(levelObject))
                {
                    ObjectGraphic imageData = GetObjectGraphic(levelObject);
                    imageData.Draw(g, mapCoordinate);

                    // Add to list of drawn objects
                    drawnObjectImages.Add(imageData);
                }
            }
        }

        /// <summary>
        /// Indicates that the map should be re-rendered at the next paint update.
        /// </summary>
        public void RenderMapAtNextUpdate()
        {
            updateRendering = true;
        }

        private void SelectSingleTile(TileCoordinate tile)
        {
            selectedTiles.Clear();
            AddTileToSelection(tile);
        }

        private void AddTileToSelection(TileCoordinate tile)
        {
            if (!selectedTiles.Contains(tile))
                selectedTiles.Add(tile);
        }
    }
}