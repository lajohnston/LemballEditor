using LemballEditor.View.Level.ObjectGraphics;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace LemballEditor.View.Level
{
    public partial class MapPanel
    {
        private class PlacingNewObjectMode : HoldingObjectMode
        {
            public PlacingNewObjectMode(MapPanel mapPanel, ObjectGraphic newObjectGraphic)
                : base(mapPanel, newObjectGraphic)
            {
            }

            public override void LeftClickOnMap(Point position, List<Keys> heldKey)
            {
                // Attempt to place the object
                if (base.PlaceObject(position))
                {
                    // If the object placement was successful, add the object to the level
                    Program.LoadedLevel.AddObject(selectedObject);
                }

                // Return to default mode
                mapPanel.StartDefaultEditingMode();
            }
        }
    }
}
