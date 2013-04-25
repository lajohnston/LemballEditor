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
        /// EditingModes define what should occur when the user performs certain actions. It also handles any special graphical
        /// drawing that the mode displays to the user.
        /// </summary>
        private abstract class EditingMode
        {
            /// <summary>
            /// 
            /// </summary>
            protected MapPanel mapPanel;

            public EditingMode(MapPanel mapPanel)
            {
                this.mapPanel = mapPanel;

                // Deselect tiles
                mapPanel.selectedTiles.Clear();
            }

            public virtual void RightClickOnMap(Point position, List<Keys> heldKeys)
            {
                mapPanel.StartDefaultEditingMode();
            }

            public virtual void LeftClickOnMap(Point position, List<Keys> heldKeys)
            {

            }

            public virtual void LeftClickOnObject(LevelObject levelObject, Point screenPosition, List<Keys> heldKeys)
            {

            }

            public virtual void RightClickOnObject(LevelObject levelObject, Point screenPosition, List<Keys> heldKeys)
            {

            }

            public virtual void LeftMouseUp(Point position)
            {

            }

            public virtual void RightMouseUp(Point position)
            {

            }

            public virtual void LeftMouseHeld(Point position)
            {

            }

            public virtual void LeftMouseDragged(Point startPosition, Point currentPosition)
            {

            }

            public virtual void RightMouseDragged(Point position)
            {

            }

            public virtual void MouseWheelScrolledUp(List<Keys> heldKeys)
            {
                /* If shift is held, the elevation for the active tiles will only be altered if the alteration
                 * can occur for all the active tiles, otherwise all tiles are checked and altered individually */
                if (heldKeys.Contains(Keys.ShiftKey))
                    AlterElevation(1, true);
                else
                    AlterElevation(1, false);
            }

            public virtual void MouseWheelScrolledDown(List<Keys> heldKeys)
            {
                /* If shift is held, the elevation for the active tiles will only be altered if the alteration
                 * can occur for all the active tiles, otherwise all tiles are checked and altered individually */
                if (heldKeys.Contains(Keys.ShiftKey))
                    AlterElevation(-1, true);
                else
                    AlterElevation(-1, false);
            }

            /// <summary>
            /// Called when the user presses the Escape key
            /// </summary>
            public virtual void OnKeyPress(Keys keyCode)
            {
                if (keyCode == Keys.Escape)
                    mapPanel.StartDefaultEditingMode();
            }

            /// <summary>
            /// Returns the object the mouse is over. This is overriden by the NodeEditMode so that it can
            /// detect whether any of the selected object's nodes have been selected.
            /// </summary>
            /// <param name="mousePosition"></param>
            /// <returns></returns>
            public virtual LevelObject GetObjectMouseIsOver(Point mousePosition)
            {
                // Check each drawn object to see if it has been clicked on
                foreach (ObjectGraphic objectImage in mapPanel.drawnObjectImages)
                {
                    if (objectImage.OverlapsPoint(mousePosition))
                    {
                        return objectImage.LevelObject;
                    }
                }

                // No object found
                return null;
            }

            /// <summary>
            /// Called with each update to draw images on the overlay that are associated with the mode. The overlay
            /// is a graphic layer over the rendered map.
            /// </summary>
            /// <param name="g"></param>
            public virtual void Update(Graphics g)
            {

            }

            /// <summary>
            /// Alters the elevation of all the active tiles by the specified amount
            /// </summary>
            /// <param name="amount">The amount to alter the elevation by</param>
            /// <param name="checkAll">If true, each active tile is checked to ensure that the elevation alteration
            /// is possible. If it isn't, then none of the tiles will have their elevation changed.</param>
            private void AlterElevation(int amount, bool checkAll)
            {
                // Get the list of active tiles
                List<TileCoordinate> activeTiles = mapPanel.ActiveTiles;

                // If all tiles can have their elevation altered, or if the checkAll parameter is not set to true
                if (!checkAll || CanAlterElevation(activeTiles, amount))
                {
                    // Each tile
                    foreach (TileCoordinate tile in activeTiles)
                    {
                        // Alter the tile's elevation value 
                        //byte elevation = (byte)Math.Max((tile.Elevation + amount), 0);
                        //mapPanel.LoadedLevel.SetTileElevation(tile, elevation);
                        mapPanel.LoadedLevel.AlterTileElevation(tile, amount);
                    }

                    // Update the map rendering at next paint
                    mapPanel.RenderMapAtNextUpdate();
                }
            }

            /// <summary>
            /// Specifies whether all the tiles in a list of tiles can have their elevation altered by
            /// a given amount.
            /// </summary>
            /// <param name="activeTiles">The list of tiles</param>
            /// <param name="amount">The amount to alter by</param>
            /// <returns>True if all tiles can have their elevation altered by the specified amount, otherwise false</returns>
            private bool CanAlterElevation(List<TileCoordinate> activeTiles, int amount)
            {
                // Each selected tile
                foreach (TileCoordinate tile in mapPanel.ActiveTiles)
                {
                    // If one of the tiles cannot have its elevation changed, return false
                    if (!mapPanel.LoadedLevel.CanAlterElevationOfTile(tile, amount))
                        return false;
                }

                // All the active tiles can have their elevation altered
                return true;
            }

            /// <summary>
            /// Highlights the tile the mouse is over
            /// </summary>
            /// <param name="g"></param>
            protected void HighlightMouseOverTile (Graphics g)
            {
                mapPanel.HighlightTile(g, mapPanel.mouseTileCoordinate, false);
            }

            /// <summary>
            /// Determines whether the specified object is being held.
            /// </summary>
            /// <param name="levelObject">The level object to test</param>
            /// <returns></returns>
            public virtual bool ObjectIsBeingHeld(LevelObject levelObject)
            {
                // Always false, unless overriden by a HoldingObjectMode mode
                return false;
            }

            
        }
    }
}
