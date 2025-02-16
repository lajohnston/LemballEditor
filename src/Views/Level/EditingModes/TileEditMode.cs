using LemballEditor.Model;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace LemballEditor.View.Level
{
    public partial class MapPanel
    {
        private class TileEditMode : EditingMode
        {
            private readonly uint selectedTileRef;

            public TileEditMode(MapPanel mapPanel, uint tileRef)
                : base(mapPanel)
            {
                selectedTileRef = tileRef;
            }

            public override void LeftClickOnMap(Point position, List<Keys> heldKey)
            {
                SetTileRefAtPosition(position);
            }

            public override void LeftMouseHeld(Point position)
            {
                SetTileRefAtPosition(position);
            }

            public override void Update(Graphics g)
            {
                DrawHeldTile(g);

                // Highlight the tile the mouse is over
                base.HighlightMouseOverTile(g);
            }

            /// <summary>
            /// Changes the tile pattern of the tile the mouse is over to the selected tile in the
            /// tile palette, so the user can see what it looks like before pasting it. If the cursor
            /// isn't over the map, the tile is centred to the cursor.
            /// </summary>
            /// <param name="g"></param>
            private void DrawHeldTile(Graphics g)
            {
                // If the mouse is on the viewable portion of the map
                if (mapPanel.MouseIsOverMap())
                {
                    // Draw tile image on the tile the cursor is over
                    Point position = mapPanel.ConvertTileXYtoScreenXY(mapPanel.mouseTileCoordinate, true);
                    ImageCache.DrawTile(g, selectedTileRef, position);
                }
                // If the mouse is on the mapPanel, but not in the viewable portion of the map
                else if (mapPanel.mouseIsOverPanel)
                {
                    // Draw centre of tile at cursor
                    Point mousePosition = mapPanel.CursorPosition;
                    Point tilePosition = new Point(mousePosition.X - 16, mousePosition.Y - 8);
                    ImageCache.DrawTile(g, selectedTileRef, tilePosition);
                }
            }

            /// <summary>
            /// Pastes the selected tile on the tile the mouse is over
            /// </summary>
            private void SetTileRefAtPosition(Point position)
            {
                // Get the tile coordinate that the mouse is over
                TileCoordinate tileCoordinate = mapPanel.ConvertScreenXYtoTileXY(position.X, position.Y);

                // If the mouse if over the visible portion of the map
                if (mapPanel.TileIsVisible(tileCoordinate))
                {
                    if (mapPanel.LoadedLevel.ChangeTileImageRef(tileCoordinate, selectedTileRef))
                    {
                        // If the tileImageRef change was successful, update the map at the next update
                        mapPanel.RenderMapAtNextUpdate();
                    }
                }
            }
        }
    }
}
