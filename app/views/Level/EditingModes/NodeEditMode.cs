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
        private class NodeEditMode : EditingMode
        {
            /// <summary>
            /// The object whose nodes are being edited
            /// </summary>
            private MovingObject editingObject;

            /// <summary>
            /// The node currently being dragged/moved, if any
            /// </summary>
            private Node heldNode;

            /// <summary>
            /// The nodes that are currently drawn
            /// </summary>
            private List<ObjectGraphic> drawnNodes;

            

            /// <summary>
            /// 
            /// </summary>
            /// <param name="mapPanel"></param>
            /// <param name="editingObject">The object whose nodes are being edited</param>
            public NodeEditMode(MapPanel mapPanel, MovingObject editingObject)
                : base(mapPanel)
            {
                this.editingObject = editingObject;
                heldNode = null;
                drawnNodes = new List<ObjectGraphic>();
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="levelObject"></param>
            /// <param name="screenPosition"></param>
            public override void LeftClickOnObject(LevelObject levelObject, Point screenPosition, List<Keys> heldKey)
            {
                if (levelObject is Node)
                {
                    // Pick up the node
                    heldNode = (Node)levelObject;
                }
                else if (!(levelObject == editingObject))
                {
                    // Return to the default mode
                    mapPanel.StartDefaultEditingMode();

                    // Inform the default mode of the click event
                    mapPanel.editingMode.LeftClickOnObject(levelObject, screenPosition, mapPanel.heldKeys);
                }
            }

            /// <summary>
            /// Called when an object is right clicked on. If the object is a node, a node menu is displayed
            /// that allows the user to delete the node
            /// </summary>
            /// <param name="levelObject"></param>
            /// <param name="screenPosition"></param>
            public override void RightClickOnObject(LevelObject levelObject, Point screenPosition, List<Keys> heldKey)
            {
                if (levelObject is Node)
                {
                    ContextMenu menu = new ContextMenu();

                    // Create menu item that deletes the node
                    MenuItem deleteMenu = new MenuItem("Delete node");
                    menu.MenuItems.Add(deleteMenu);
                    deleteMenu.Click += delegate(object sender, EventArgs e)
                    {
                        editingObject.DeleteNode((Node)levelObject);
                    };

                    // Show the menu at the mouse
                    mapPanel.ShowMenuAtMouse(menu);
                }
            }

            /// <summary>
            /// Drops the node that is being dragged with the mouse
            /// </summary>
            /// <param name="position"></param>
            public override void LeftMouseUp(Point position)
            {
                // Drop the held node
                heldNode = null;
            }

            /// <summary>
            /// Show the NodeEditMode right-click menu
            /// </summary>
            /// <param name="position">The position to display the menu, usually the mousePosition</param>
            public override void RightClickOnMap(Point position, List<Keys> heldKey)
            {
                ShowPathNodeMenu(position);
            }

            /// <summary>
            /// First checks whether one of the selected object's nodes has been clicked on before checking
            /// other objects
            /// </summary>
            /// <param name="mousePosition"></param>
            /// <returns></returns>
            public override LevelObject GetObjectMouseIsOver(Point mousePosition)
            {
                // Detect if a node was clicked on
                Node node = GetMouseOverNode(mousePosition);

                // If a node was clicked on, return the node, otherwise revert to base method
                if (node != null)
                    return node;
                else
                    return base.GetObjectMouseIsOver(mousePosition);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="mousePosition"></param>
            private Node GetMouseOverNode(Point mousePosition)
            {
                foreach (ObjectGraphic nodeImage in drawnNodes)
                {
                    if (nodeImage.OverlapsPoint(mousePosition))
                        return (Node)nodeImage.LevelObject;
                }

                return null;

            }

            /// <summary>
            /// Displays the right click context menu used in this mode
            /// </summary>
            /// <param name="mousePosition">The coorindate of the mouse</param>
            private void ShowPathNodeMenu(Point mousePosition)
            {
                // Create the menu
                ContextMenu menu = new ContextMenu();

                // Create the add node menu item
                MenuItem newNode = new MenuItem("Create node here");
                menu.MenuItems.Add(newNode);
                newNode.Click += delegate(object sender, EventArgs e)
                {
                    CreateNode(mousePosition);
                };
                
                // Create the exit menu item
                MenuItem exit = new MenuItem("Exit path mode");
                menu.MenuItems.Add(exit);
                exit.Click += delegate(object sender, EventArgs e)
                {
                    mapPanel.StartDefaultEditingMode();
                };
                
                // Show menu at mouse
                mapPanel.ShowMenuAtMouse(menu);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="screenPosition"></param>
            private void CreateNode(Point screenPosition)
            {
                Point isoPosition = mapPanel.ConvertScreenXYtoIsoXY(screenPosition.X, screenPosition.Y);
                editingObject.AddNode(new Node((ushort)isoPosition.X, (ushort)isoPosition.Y));
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="g"></param>
            public override void Update(Graphics g)
            {
                // If a node is being held, set it to the mouse position
                if (heldNode != null)
                {
                    heldNode.IsoPosition = mapPanel.MouseIsoPosition;
                }

                // Draw the nodes
                DrawNodes(g);
            }

            /// <summary>
            /// Draws the nodes of the moving object that is currently being edited
            /// </summary>
            /// <param name="g"></param>
            private void DrawNodes(Graphics g)
            {
                // Get the current level
                Model.Level level = mapPanel.LoadedLevel;

                // Clear the drawn nodes
                drawnNodes.Clear();

                // Get the editing object's nodes
                List<Node> nodes = editingObject.Nodes;

                // Create the pen to draw the lines between the nodes
                Pen pen = new Pen(Color.Yellow);

                // Draw a line from the object to the first node
                if (nodes.Count > 0)
                {
                    // Get the screen position of the enemy and its first node
                    Point objectPosition = mapPanel.ConvertIsoXYtoScreenXY(editingObject.IsoPosition);
                    Point firstNodePosition = mapPanel.ConvertIsoXYtoScreenXY(nodes[0].IsoPosition);

                    // Draw a line between the object and its first node
                    g.DrawLine(pen, objectPosition, firstNodePosition);
                }

                // Each node
                for (int i = 0; i < nodes.Count; i++)
                {
                    // Get the current node and the next node
                    Node currentNode = nodes[i];
                    
                    // Get the screen position of the node
                    Point position = mapPanel.ConvertIsoXYtoScreenXY(currentNode.IsoPosition);

                    // If there is another node after it, draw a line to it
                    if (i + 1 < nodes.Count)
                    {
                        Node nextNode = nodes[i + 1];
                        Point nextNodePosition = mapPanel.ConvertIsoXYtoScreenXY(nextNode.IsoPosition);
                        g.DrawLine(pen, position, nextNodePosition);
                    }

                    // Ensure node is on the visible portion of the map.
                    if (mapPanel.ObjectIsVisible(currentNode))
                    {
                        // Draw the node point
                        ObjectGraphic imageData = mapPanel.GetObjectGraphic(currentNode);
                        imageData.Draw(g, position, 0);
                        drawnNodes.Add(imageData);

                        //level.DrawObject(currentNode, g, position);
                    }
                }

                // Draw line from the last node to the first node
                if (nodes.Count > 2)
                {
                    // Get the screen positions of the first and last nodes
                    //Point firstNode = mapPanel.ConvertIsoXYtoScreenXY(nodes[0].getIsoPosition());
                    Point firstNode = new Point(0, 0);
                    Point lastNode = mapPanel.ConvertIsoXYtoScreenXY(nodes[nodes.Count - 1].IsoPosition);
                    
                    if (editingObject is Enemy)
                        firstNode = mapPanel.ConvertIsoXYtoScreenXY(nodes[0].IsoPosition);
                    else
                        firstNode = mapPanel.ConvertIsoXYtoScreenXY(editingObject.IsoPosition);

                    // Set the pen to a dotted line
                    pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                    
                    // Draw the line
                    g.DrawLine(pen, firstNode, lastNode);
                }
            }
        }
    }
}
