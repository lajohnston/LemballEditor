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
        private class MovingObjectMode : HoldingObjectMode
        {
            public MovingObjectMode(MapPanel mapPanel, ObjectGraphic objectImage)
                : base(mapPanel, objectImage)
            {
                // Check whether cursor should be centred at the object's centre position
                if (objectImage.CentreCursorOnDrag())
                {
                    // Move the cursor to the object's centre
                    Point objectCentre = objectImage.CentrePoint;
                    Cursor.Position = mapPanel.PointToScreen(objectCentre);
                }

                // Render the map at the next update
                mapPanel.RenderMapAtNextUpdate();
            }

            public override void LeftMouseUp(Point position)
            {
                base.PlaceObject(position);
                mapPanel.StartDefaultEditingMode();
            }
        }
    }
}
