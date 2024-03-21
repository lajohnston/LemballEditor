using LemballEditor.Model;
using LemballEditor.View.Level.ObjectGraphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;


namespace LemballEditor.View.Level
{
    public partial class MapPanel
    {
        private class DefaultMode : EditingMode
        {
            /// <summary>
            /// The tile coordinates that exist within the mouse dragging area
            /// </summary>
            private List<TileCoordinate> DragSelectionTiles
            {
                get
                {
                    // Intialise list to store tiles
                    List<TileCoordinate> tiles = new List<TileCoordinate>();

                    // If the mouse is being dragged
                    if (mapPanel.leftMouseDrag)
                    {
                        // Get the position of the mouse when the left button was clicked, and where it currently is now
                        Point startPosition = mapPanel.leftClickPosition;
                        Point endPosition = mapPanel.CursorPosition;

                        // Get the tiles the the drag starts and ends at
                        TileCoordinate startTile = mapPanel.ConvertScreenXYtoTileXY(startPosition.X, startPosition.Y);
                        TileCoordinate endTile = mapPanel.ConvertScreenXYtoTileXY(endPosition.X, endPosition.Y);

                        ushort minY = Math.Min(startTile.yTile, endTile.yTile);
                        ushort maxY = Math.Max(startTile.yTile, endTile.yTile);
                        ushort minX = Math.Min(startTile.xTile, endTile.xTile);
                        ushort maxX = Math.Max(startTile.xTile, endTile.xTile);

                        // Ensure the tiles are different and that the first tile is on the visible map
                        if (!startTile.Equals(endTile) && mapPanel.TileIsVisible(startTile) && endTile != null)
                        {
                            // Y tiles    
                            for (ushort y = minY; y <= maxY; y++)
                            {
                                // X tiles
                                for (ushort x = minX; x <= maxX; x++)
                                {
                                    // Add tile to the list
                                    tiles.Add(new TileCoordinate(x, y));
                                }
                            }
                        }
                    }

                    // Return tiles
                    return tiles;
                }
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="mapPanel"></param>
            public DefaultMode(MapPanel mapPanel)
                : base(mapPanel)
            {

            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="position"></param>
            /// <param name="heldKeys"></param>
            public override void LeftClickOnMap(Point position, List<Keys> heldKeys)
            {
                // Get the tile that was clicked on
                TileCoordinate clickedTile = mapPanel.ConvertScreenXYtoTileXY(position.X, position.Y);

                // If shift is being held, add the tile to the selected list, otherwise select only that tile
                if (heldKeys.Contains(Keys.ShiftKey))
                {
                    if (!mapPanel.selectedTiles.Contains(clickedTile))
                    {
                        mapPanel.AddTileToSelection(clickedTile);
                    }
                    else
                    {
                        _ = mapPanel.selectedTiles.Remove(clickedTile);
                    }
                }
                else
                {
                    mapPanel.SelectSingleTile(clickedTile);
                }
            }

            public override void RightClickOnMap(Point position, List<Keys> heldKeys)
            {
                mapPanel.selectedTiles.Clear();
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="position"></param>
            public override void LeftMouseUp(Point position)
            {
                foreach (Model.TileCoordinate tile in DragSelectionTiles)
                {
                    mapPanel.AddTileToSelection(tile);
                }

                base.LeftMouseUp(position);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="keyCode"></param>
            public override void OnKeyPress(Keys keyCode)
            {
                if (keyCode == Keys.Space)
                {
                    StringBuilder sb = new StringBuilder();

                    foreach (Model.TileCoordinate tile in mapPanel.selectedTiles)
                    {
                        _ = sb.Append("x" + tile.xTile + " y" + tile.yTile + ",");
                    }

                    _ = MessageBox.Show(sb.ToString());
                }
            }

            /// <summary>
            /// Starts the MovingObject mode
            /// </summary>
            /// <param name="levelObject"></param>
            /// <param name="screenPosition"></param>
            public override void LeftClickOnObject(LevelObject levelObject, Point screenPosition, List<Keys> heldKey)
            {
                // Start the 'moving object' mode
                mapPanel.StartMovingObjectMode(levelObject);
            }

            /// <summary>
            /// Displays the object's right click menu
            /// </summary>
            /// <param name="levelObject"></param>
            /// <param name="screenPosition"></param>
            public override void RightClickOnObject(LevelObject levelObject, Point screenPosition, List<Keys> heldKey)
            {
                // Get the object's graphic representation
                ObjectGraphic objGraphic = mapPanel.GetObjectGraphic(levelObject);

                // Get the object's menu
                ContextMenu objectMenu = objGraphic.RightClickMenu;

                // Show menu at mouse
                Point position = new Point(screenPosition.X + 40, screenPosition.Y);
                objectMenu.Show(Program.MainInterface, position);
            }

            /// <summary>
            /// Highlights the tile the mouse is over
            /// </summary>
            /// <param name="g"></param>
            public override void Update(Graphics g)
            {
                // Highlight each tile in the drag selection
                foreach (Model.TileCoordinate tile in DragSelectionTiles)
                {
                    // Only highlight the tile if it isn't already selected, otherwise it'll be highlighted twice
                    if (!mapPanel.selectedTiles.Contains(tile))
                    {
                        mapPanel.HighlightTile(g, tile, true);
                    }
                }

                // Highlight the tile the mouse is over
                base.HighlightMouseOverTile(g);
            }
        }
    }
}
