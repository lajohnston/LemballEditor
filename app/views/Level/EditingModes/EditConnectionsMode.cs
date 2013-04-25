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
        private class EditConnectionsMode : EditingMode
        {
            /// <summary>
            /// The switch that is having its connections edited
            /// </summary>
            private Switch switchObject;

            /// <summary>
            /// The objects connected to the switch
            /// </summary>
            private List<LevelObject> ConnectedObjects
            {
                get
                {
                    return Program.LoadedLevel.GetObjectsConnectedToSwitch(switchObject);  
                }
            }

            /// <summary>
            /// The idref of the object whose connection to the switch is selected
            /// </summary>
            private int? SelectedConnectionIdref;

            /// <summary>
            /// The size of the click area used when detecting whether the user has clicked
            /// a connection line
            /// </summary>
            private const int CLICK_AREA = 10;

            /// <summary>
            /// 
            /// </summary>
            /// <param name="mapPanel">Reference to the main map panel</param>
            /// <param name="switchObject">The switch to edit the connections of</param>
            public EditConnectionsMode(MapPanel mapPanel, Switch switchObject)
                : base(mapPanel)
            {
                this.switchObject = switchObject;
                SelectedConnectionIdref = null;
            }

            /// <summary>
            /// Detects whether the delete key has been pressed, and if so asks the user for confirmation
            /// to delete the selected connection
            /// </summary>
            /// <param name="keyCode"></param>
            public override void OnKeyPress(Keys keyCode)
            {
                // If a connection is selected and the delete key
                if (SelectedConnectionIdref.HasValue && keyCode == Keys.Delete)
                {
                    if (MessageBox.Show("Are you sure you wish to delete this connection?", "Delete connection", MessageBoxButtons.OKCancel) == DialogResult.OK)
                    {
                        // Delete connection
                        switchObject.DeleteConnection((int)SelectedConnectionIdref);

                        // De-select deleted connection
                        SelectedConnectionIdref = null;

                        // Render map
                        mapPanel.RenderMapAtNextUpdate();
                    }
                }

                //
                base.OnKeyPress(keyCode);
            }

            /// <summary>
            /// If the object clicked on is connected to the switch, its connection is selected
            /// </summary>
            /// <param name="levelObject"></param>
            /// <param name="screenPosition"></param>
            public override void LeftClickOnObject(LevelObject levelObject, Point screenPosition, List<Keys> heldKey)
            {
                if (switchObject.IsConnectedTo(levelObject))
                {
                    SelectConnection(levelObject);
                }
                else
                {
                    if (switchObject.CanConnectToObject(levelObject))
                    {
                        if (switchObject.AddConnection(levelObject))
                        {
                            SelectedConnectionIdref = levelObject.Id;
                        }

                    }
                }
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="levelObject"></param>
            /// <param name="screenPosition"></param>
            /// <param name="heldKey"></param>
            public override void RightClickOnObject(LevelObject levelObject, Point screenPosition, List<Keys> heldKey)
            {
                // Create menu
                ContextMenu menu = new ContextMenu();

                // If object can be connect to
                if (switchObject.CanConnectToObject(levelObject))
                {
                    // If object is not already connected
                    if (!switchObject.IsConnectedTo(levelObject))
                    {
                        MenuItem connect = new MenuItem("Add connection this object");
                        connect.Click += delegate
                        {
                            switchObject.AddConnection(levelObject);
                        };

                        menu.MenuItems.Add(connect);
                    }
                    else
                    {
                        MenuItem delete = new MenuItem("Delete connection to this object");
                        delete.Click += delegate
                        {
                            switchObject.DeleteConnection(levelObject.Id);

                            if (levelObject.Id == SelectedConnectionIdref)
                                SelectedConnectionIdref = null;
                        };

                        menu.MenuItems.Add(delete);
                    }
                }
                    // If object cannot be connect to
                else
                {
                    // Display disable menu item
                    MenuItem cannotConnect = new MenuItem("Cannot connect to this type of object");
                    cannotConnect.Enabled = false;
                    menu.MenuItems.Add(cannotConnect);
                }

                // Show menu
                menu.Show(Program.MainInterface, screenPosition);
            }

            /// <summary>
            /// Detects whether the user has clicked on a connection line
            /// </summary>
            /// <param name="position"></param>
            public override void LeftClickOnMap(Point position, List<Keys> heldKey)
            {
                // Get the switch's screen position
                Point switchPos = mapPanel.ConvertIsoXYtoScreenXY(switchObject.IsoPosition);

                // Detect each connection
                foreach (LevelObject levelObject in ConnectedObjects)
                {
                    if (LineIntersectsRectangle (
                            switchPos,
                            mapPanel.ConvertIsoXYtoScreenXY(levelObject.IsoPosition),
                            new Rectangle (
                                position.X - CLICK_AREA / 2,
                                position.Y - CLICK_AREA / 2,
                                CLICK_AREA,
                                CLICK_AREA)
                          )
                        )
                    {
                        // Select connection
                        SelectConnection(levelObject);

                        // Break for-each loop
                        break;
                    }
                }
            }

            /// <summary>
            /// Displays a menu that allows the user to confirm that they wish to exit this mode
            /// </summary>
            /// <param name="position"></param>
            public override void RightClickOnMap(Point position, List<Keys> heldKey)
            {
                // Create menu to confirm whether to exit the current mode
                ContextMenu menu = new ContextMenu();

                // Create exit menu item
                MenuItem exit = new MenuItem("Exit switch connection mode");
                exit.Click += delegate
                {
                    mapPanel.StartDefaultEditingMode();
                };

                // Add exit button to menu
                menu.MenuItems.Add(exit);

                // Show menu
                menu.Show(Program.MainInterface, position);
            }

            /// <summary>
            /// Selects the connection for the specified level object. If the connection doesn't exist then
            /// the previous selection remains unchanged
            /// </summary>
            /// <param name="levelObject"></param>
            private void SelectConnection(LevelObject levelObject)
            {
                // Select line
                if (switchObject.IsConnectedTo(levelObject))
                {
                    // Remember the idref of the object
                    SelectedConnectionIdref = levelObject.Id;

                    // Redraw map at next update
                    mapPanel.RenderMapAtNextUpdate();
                }
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="g"></param>
            public override void Update(Graphics g)
            {
                // Set anti-alias to on
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                // Create the pen to draw the connection lines
                Pen unselected = new Pen(Color.LawnGreen, 2);
                Pen selected = new Pen(Color.Yellow, 2);

                // Get the switch's screen position
                Point switchPos = mapPanel.ConvertIsoXYtoScreenXY(switchObject.IsoPosition);

                // Each connected object
                foreach (LevelObject levelObject in ConnectedObjects)
                {
                    // Determine what pen to use to draw the connection, depending on whether it is selected or not
                    Pen pen = null;
                    if (levelObject.Id == SelectedConnectionIdref)
                        pen = selected;
                    else
                        pen = unselected;

                    // Draw line
                    g.DrawLine(pen, switchPos, mapPanel.ConvertIsoXYtoScreenXY(levelObject.IsoPosition));
                }

                // Draw line from switch to cursor
                DrawNewConnection(g, switchPos);
            }

            /// <summary>
            /// Draws a new connection from the switch to the cursor
            /// </summary>
            /// <param name="g"></param>
            private void DrawNewConnection(Graphics g, Point switchPos)
            {
                // Get the object the mouse is over, if any
                LevelObject mouseOverObject = mapPanel.GetObjectMouseIsOver(mapPanel.CursorPosition);
                
                // The default pen to use to draw the new connection
                Pen pen = new Pen(Color.White, 3);

                // If mouse is over an object
                if (mouseOverObject != null)
                {
                    // If the switch can connect to this object
                    if (switchObject.CanConnectToObject(mouseOverObject))
                    {
                        // Give the pen a green colour
                        pen.Color = Color.LawnGreen;
                    }
                    // Cannot connect to object
                    else
                    {
                        pen.Color = Color.Red;
                    }
                }

                // Draw line from switch to cursor
                g.DrawLine(pen, switchPos, mapPanel.CursorPosition);
            }

            /// <summary>
            /// Determines whether a given line intersects with a given rectangle
            /// </summary>
            /// <param name="a0">The starting point of the line</param>
            /// <param name="a1">The end poing of the line</param>
            /// <param name="rect">The rectangle to test</param>
            /// <returns></returns>
            private bool LineIntersectsRectangle(Point a0, Point a1, Rectangle rect)
            {
                // Get the rectangle's points
                Point tl = new Point(rect.X, rect.Y);
                Point tr = new Point(rect.X + rect.Width, rect.Y);
                Point bl = new Point(rect.X, rect.Y + rect.Height);
                Point br = new Point(rect.X + rect.Width, rect.Y + rect.Height);

                // Top
                if (LinesIntersect(a0, a1, tl, tr))
                    return true;

                // Bottom
                if (LinesIntersect(a0, a1, bl, br))
                    return true;

                // Left side
                if (LinesIntersect(a0, a1, tl, bl))
                    return true;

                // Right side
                if (LinesIntersect(a0, a1, tr, br))
                    return true;

                // No intersection
                return false;
            }

            /// <summary>
            /// Intersects(): Uses basic linear algebra to determine if two 
            ///    integer-based line segments intersect.
            /// Input: a0,a1,b0,b1 -- 2D points, each with integers x and y.
            /// Returns: true if line segment a0-a1 intersects b0-b1.
            /// (c) Jake Askeland, January 2009, http://jake.askeland.ws/
            /// </summary>
            /// <param name="a0">The starting point of line A</param>
            /// <param name="a1">The ending point of line A</param>
            /// <param name="b0">The starting point of line B</param>
            /// <param name="b1">The ending poing ot line B</param>
            /// <returns>True if the lines intersect, otherwise false</returns>
            private bool LinesIntersect(Point a0, Point a1, Point b0, Point b1)
            {
                // detM = determinant of M, the matrix whose elements are
                // the coefficients of the parametric equations of lines
                // containing segments A and B.
                int detM = (a1.X - a0.X) * (b1.Y - b0.Y)
                         - (b1.X - b0.X) * (a1.Y - a0.Y);

                // special case: A and B are parallel.
                // when A and B are parallel, 
                // detM is 0 and a bounds test is needed.
                if (detM == 0)
                {

                    // special case: A and B are vertical line segments.
                    if (a0.X == a1.X && b0.X == b1.X)
                    {
                        // true if A and B are in the same vertical line.
                        if (a0.X == b0.X)
                            // true when some bounds on Ay 
                            // are in the bounds of By.
                            return (b0.Y <= a0.Y && a0.Y <= b1.Y)
                                   || (b0.Y <= a1.Y && a1.Y <= b1.Y);

                        // different vertical lines, no intersection.
                        else return false;
                    }

                    // for parallel lines to overlap, they need the 
                    // same y-intercept. integer relations to 
                    // y-intercepts of A and B are as follows.
                    int a_offset = ((a1.X - a0.X) * a0.Y - (a1.Y - a0.Y) * a0.X)
                               * (b1.X - b0.X);
                    int b_offset = ((b1.X - b0.X) * b0.Y - (b1.Y - b0.Y) * b0.X)
                               * (a1.X - a0.X);

                    // true only when A_y_intercept == B_y_intercept.
                    if (b_offset == a_offset)
                    {
                        // true when some bounds on ax 
                        // are in the bounds of bx.
                        return (b0.X <= a0.X && a0.X <= b1.X)
                               || (b0.X <= a1.X && a1.X <= b1.X);
                    }
                    // different y intercepts; no intersection.
                    else
                        return false;
                }

                // nMitc[0] = numerator_of_M_inverse_times_c0
                // nMitc[1] = numerator_of_M_inverse_times_c1
                int[] nMitc = {
                    (b0.X - a0.X) * (b1.Y - b0.Y) + (b0.Y - a0.Y) * (b0.X - b1.X),
                    (b0.X - a0.X) * (a0.Y - a1.Y) + (b0.Y - a0.Y) * (a1.X - a0.X)
                };

                // true if an intersection between two non-parallel lines
                // occurs between the given segment points.
                return ((0 <= nMitc[0] && nMitc[0] <= detM)
                        && (0 >= nMitc[1] && nMitc[1] >= -detM)) ||
                       ((0 >= nMitc[0] && nMitc[0] >= detM)
                        && (0 <= nMitc[1] && nMitc[1] <= -detM));
            }

        }
    }
}
