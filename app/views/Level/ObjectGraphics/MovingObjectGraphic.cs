using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using LemballEditor.Model;
using System.Windows.Forms;

namespace LemballEditor.View.Level.ObjectGraphics
{
    /// <summary>
    /// Represents a drawn entrance
    /// </summary>
    abstract class MovingObjectGraphic : ObjectGraphic
    {
        /// <summary>
        /// 
        /// </summary>
        protected abstract MovingObject MovingObject { get; }

        public override LevelObject LevelObject
        {
            get { return MovingObject; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mapPanel"></param>
        public MovingObjectGraphic(MapPanel mapPanel)
            : base(mapPanel)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="menu"></param>
        /// <param name="level"></param>
        protected override void AddMenuItems(ContextMenu menu)
        {
            MenuItem editPath = new MenuItem("Edit path");
            editPath.Click += new EventHandler(editPath_Click);

            menu.MenuItems.Add(editPath);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void editPath_Click(object sender, EventArgs e)
        {
            Program.MainInterface.StartPathEditMode(MovingObject);
            //MapPanel.StartPathEditMode(this);
        }

        /// <summary>
        /// Checks whether the specified rectangle overlaps on of the object's nodes
        /// </summary>
        /// <param name="mousePosition">The rectangle are to check</param>
        /// <returns>The node the rectangle overlaps, otherwise null</returns>
        /*
        public Node GetMouseOverNode(Point mousePosition)
        {
            foreach (Node node in Nodes)
            {
                if (node.MouseIsOver(mousePosition))
                    return node;
            }

            return null;
        }
        */
    }
}
