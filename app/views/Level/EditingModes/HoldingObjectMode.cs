﻿using LemballEditor.Model;
using LemballEditor.View.Level.ObjectGraphics;
using System.Drawing;

namespace LemballEditor.View.Level
{
    public partial class MapPanel
    {
        private abstract class HoldingObjectMode : EditingMode
        {
            /// <summary>
            /// 
            /// </summary>
            public LevelObject SelectedObject => SelectedObjectImage.LevelObject;

            public ObjectGraphic SelectedObjectImage { get; private set; }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="mapPanel"></param>
            /// <param name="levelObject"></param>
            public HoldingObjectMode(MapPanel mapPanel, ObjectGraphic objectImage)
                : base(mapPanel)
            {
                SelectedObjectImage = objectImage;

            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="levelObject"></param>
            /// <returns></returns>
            public override bool ObjectIsBeingHeld(LevelObject levelObject)
            {
                return levelObject == SelectedObject;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="screenPosition"></param>
            /// <returns></returns>
            protected bool PlaceObject(Point screenPosition)
            {
                // Update the map render at the next update
                mapPanel.RenderMapAtNextUpdate();

                // Get the iso coordinates of the mouse
                Point isoPosition = mapPanel.ConvertScreenXYtoIsoXY(screenPosition.X, screenPosition.Y);

                // Place object is in visible portion of map, otherwise its position is left unchanged (where it was before being picked up)
                if (mapPanel.IsoPositionIsOnViewableMap(isoPosition))
                {
                    // Place the object
                    SelectedObject.IsoPosition = isoPosition;

                    // Object placement was successful
                    return true;
                }
                else
                {
                    // Object placement was unsuccessful as it was off the viewable portion of the map
                    return false;
                }
            }

            /// <summary>
            /// Draws the object the user is currently dragging or placing
            /// </summary>
            /// <param name="g"></param>
            /// <param name="mouseIsOverMap"></param>
            public override void Update(Graphics g)
            {
                SelectedObjectImage.DrawAtCursor(g);

                // Highlight the tile the mouse is over
                base.HighlightMouseOverTile(g);

                //// Set the elevation offset
                //byte elevation = 0;

                //MapTile mouseOverTile = mapPanel.MouseOverTile;
                //if (mouseOverTile != null)
                //{
                //    elevation = mouseOverTile.Elevation;
                //}

                //// Calculate the position to draw the object
                // Point position = mapPanel.mousePosition;

                ///* 
                // * When placing the object, it will appear to shift left 1 pixel if 
                // * mousePosition.X is an odd number, due to the screenX to isoX conversion
                // * process. Shifting it left a pixel at this stage preempts this effect so that 
                // * when the user places the object, it will not appear to shift
                // */
                //if (position.X % 2 != 0)
                //    position.X -= 1;

                //// Draw the image at the mouse cursor
                ////selectedObject.Draw(g, position, elevation);
                //selectedObjectImage.Draw(g, position, elevation);
            }
        }
    }
}
