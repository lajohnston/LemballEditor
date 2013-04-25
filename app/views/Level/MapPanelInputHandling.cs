using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using LemballEditor.Model;
using System.Windows.Forms;
using LemballEditor.View.Level.ObjectGraphics;

namespace LemballEditor.View.Level
{
    partial class MapPanel
    {
        /// <summary>
        /// Whether the left mouse button is being held
        /// </summary>
        private bool leftMouseDown;

        /// <summary>
        /// True if the user is holding down the left mouse button while moving the mouse, otherwise false
        /// </summary>
        private bool leftMouseDrag;

        /// <summary>
        /// The screen position where the left mouse button was last clicked
        /// </summary>
        private Point leftClickPosition;

        /// <summary>
        /// True if the right mouse button is being held
        /// </summary>
        private bool rightMouseDown;

        /// <summary>
        /// True if the user is holding down the right mouse button while moving it, otherwise false
        /// </summary>
        private bool rightMouseDrag;

        /// <summary>
        /// The delta of the mouse wheel
        /// </summary>
        private int mouseWheelDelta;

        /// <summary>
        /// The cursor position at the last call to update(). Position is relative to the mapPanel.
        /// </summary>
        public Point CursorPosition { get; private set; }

        /// <summary>
        /// The tile the mouse is over. Calculated at each update
        /// </summary>
        private TileCoordinate mouseTileCoordinate;

        /// <summary>
        /// The keys that were being held at the last update
        /// </summary>
        private List<Keys> heldKeys;

        /// <summary>
        /// Returns the isometric coordinate of the mouse cursor
        /// </summary>
        /// <returns></returns>
        private Point MouseIsoPosition
        {
            get
            {
                return ConvertScreenXYtoIsoXY(CursorPosition.X, CursorPosition.Y);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public TileCoordinate MouseOverTileCoords
        {
            get
            {
                return ConvertScreenXYtoTileXY(CursorPosition.X, CursorPosition.Y);
            }
        }

        /// <summary>
        /// The tile coordinate the cursor was over at the last update
        /// </summary>
        /*
        public MapTile MouseOverTile
        {
            get
            {
                return Program.LoadedLevel.GetTile(MouseOverTileCoords);
            }
        }
        */


        /// <summary>
        /// When a mouse button has been lifted
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void MapPanel_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                leftMouseDown = false;
                editingMode.LeftMouseUp(CursorPosition);
            }

            if (e.Button == MouseButtons.Right)
            {
                rightMouseDown = false;
                editingMode.RightMouseUp(CursorPosition);
            }
        }

        /// <summary>
        /// Detects when a key is not longer being pressed
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyUp(KeyEventArgs e)
        {
            heldKeys.Remove(e.KeyCode);
        }

        /// <summary>
        /// Detects key presses
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            // Store key in list
            if (!heldKeys.Contains(e.KeyCode))
                heldKeys.Add(e.KeyCode);

            // Get the current cursor isometric coordinate
            TileCoordinate mouseIso = ConvertScreenXYtoTileXY(CursorPosition.X, CursorPosition.Y);

            // Scroll east
            if (e.KeyCode == Keys.Down && firstViewTile.xTile + mapViewTileDimensions.Width < LoadedLevel.MapSizeX)
            {
                firstViewTile.xTile++;
                RenderMapAtNextUpdate();
            }

            // Scroll west
            if (e.KeyCode == Keys.Up && firstViewTile.xTile > 0)
            {
                firstViewTile.xTile--;
                RenderMapAtNextUpdate();
            }

            // Scroll north
            if (e.KeyCode == Keys.Right && firstViewTile.yTile > 0)
            {
                firstViewTile.yTile--;
                RenderMapAtNextUpdate();
            }

            // Scroll south
            if (e.KeyCode == Keys.Left && firstViewTile.yTile + mapViewTileDimensions.Height < LoadedLevel.MapSizeY)
            {
                firstViewTile.yTile++;
                RenderMapAtNextUpdate();
            }

            // Inform editing mode
            editingMode.OnKeyPress(e.KeyCode);
        }

        /// <summary>
        /// When the mouse moves over the map panel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void MapPanel_MouseEnter(object sender, EventArgs e)
        {
            mouseIsOverPanel = true;
            this.Focus();
        }

        /// <summary>
        /// When the mouse leaves the mapPanel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void MapPanel_MouseLeave(object sender, EventArgs e)
        {
            mouseIsOverPanel = false;
        }

        /// <summary>
        /// Called when the mouse wheel has been scrolled
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void MapPanel_MouseWheel(object sender, MouseEventArgs e)
        {
            mouseWheelDelta = e.Delta;
        }

        /// <summary>
        /// Called when the user moves the mouse while it is in the map panel
        /// </summary>
        /// <param name="mouseX">The x position of the mouse</param>
        /// <param name="mouseY">The y position of the mouse</param>
        public void MapPanel_MouseMove(object sender, MouseEventArgs e)
        {
            // Store the mouse position, relative to the map panel (as opposed to the actual screen coordinates)
            CursorPosition = new Point(e.X, e.Y);

            // Detect if the user is drag scrolling (true is mouse is down, otherwise false)
            leftMouseDrag = leftMouseDown;
            rightMouseDrag = rightMouseDown;
        }

        /// <summary>
        /// Called when a mouse button is pressed down
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void MapPanel_MouseDown(object sender, MouseEventArgs e)
        {
            // Set the mouseDown variable to true
            if (e.Button == MouseButtons.Left)
                leftMouseDown = true;

            if (e.Button == MouseButtons.Right)
                rightMouseDown = true;

            // Stores the new position of the mouse. The mousePosition variable is only set with each update
            leftClickPosition = new Point(e.X, e.Y);

            // Get the object the mouse is over (null if no object)
            LevelObject clickObject = GetObjectMouseIsOver(leftClickPosition);
            //ObjectGraphic clickObjectGraphic = GetObjectGraphic(clickObject);

            // Determine whether the user has clicked on an object
            if (clickObject == null)
            {
                // User hasn't clicked on an object

                // If the mouse is over the visible portion of the map
                if (PositionIsOnMap(leftClickPosition))
                {
                    if (e.Button == MouseButtons.Left)
                        editingMode.LeftClickOnMap(leftClickPosition, heldKeys);
                    else if (e.Button == MouseButtons.Right)
                        editingMode.RightClickOnMap(leftClickPosition, heldKeys);
                }
            }
            else
            {
                // User has clicked on an object
                if (e.Button == MouseButtons.Left)
                    editingMode.LeftClickOnObject(clickObject, leftClickPosition, heldKeys);
                else if (e.Button == MouseButtons.Right)
                    editingMode.RightClickOnObject(clickObject, leftClickPosition, heldKeys);
            }
        }

        /// <summary>
        /// Checks user input and repaints display
        /// </summary>
        public void RefreshLogic()
        {
            // Get the tile the mouse is over
            LevelObject mouseOnObject = GetObjectMouseIsOver(CursorPosition);

            // If the mouse isn't over an object
            if (mouseOnObject == null || !(editingMode is HoldingObjectMode) || editingMode is TileEditMode)
            {
                // Highlight the tile the mouse is over
                mouseTileCoordinate = ConvertScreenXYtoTileXY(CursorPosition.X, CursorPosition.Y);
            }
            else
            {
                // Highlight the tile the object is over
                mouseTileCoordinate = mouseOnObject.OnTile;
            }

            // If mouse wheel has been scrolled since last update
            if (mouseWheelDelta != 0)
            {
                if (MouseIsOverMap())
                {
                    // Mouse wheel has been scrolled up
                    if (mouseWheelDelta > 0)
                        editingMode.MouseWheelScrolledUp(heldKeys);
                    else
                        editingMode.MouseWheelScrolledDown(heldKeys);
                }

                // Reset mouse delta
                mouseWheelDelta = 0;
            }

            // User is holding down the mouse button while moving the cursor
            if (leftMouseDrag)
                editingMode.LeftMouseDragged(leftClickPosition, CursorPosition);
    
            // User is holding down the mouse button
            if (leftMouseDown)
                editingMode.LeftMouseHeld(CursorPosition);

            // Repaint
            this.Invalidate();
        }

        /// <summary>
        /// 
        /// </summary>
        private void StartDefaultEditingMode()
        {
            editingMode = new DefaultMode(this);

            // Update rendering, required if the user was holding an object and right clicked at the same time,
            // as the object will need to be re-drawn in its former location
            RenderMapAtNextUpdate();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tileRef"></param>
        public void StartTileEditMode(uint tileRef)
        {
            editingMode = new TileEditMode(this, tileRef);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="levelObject"></param>
        private void StartMovingObjectMode(LevelObject levelObject)
        {
            if (levelObject == null)
            {
                throw new ArgumentNullException();
            }

            ObjectGraphic imageData = GetObjectGraphic(levelObject);

            if (imageData != null)
            {
                editingMode = new MovingObjectMode(this, imageData);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="levelObject"></param>
        public void StartEditingConnectionsMode(Switch switchObject)
        {
            editingMode = new EditConnectionsMode(this, switchObject);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="levelObject">The object that is having its nodes edited</param>
        public void StartPathEditMode(MovingObject levelObject)
        {
            editingMode = new NodeEditMode(this, levelObject);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newObject"></param>
        public void StartPlacingNewObjectMode(ObjectGraphic newObjectGraphic)
        {
            editingMode = new PlacingNewObjectMode(this, newObjectGraphic);
        }
    }
}
