using LemballEditor.View.Level.ObjectGraphics;
using System.Drawing;
using System.Windows.Forms;

namespace LemballEditor.View.Level
{
    public partial class MapPanel
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
                _ = base.PlaceObject(position);
                mapPanel.StartDefaultEditingMode();
            }
        }
    }
}
