using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using LemballEditor.Model;
using System.Windows.Forms;

namespace LemballEditor.View.Level.ObjectGraphics
{
    /// <summary>
    /// 
    /// </summary>
    abstract class RotatableObjectGraphic : ObjectGraphic
    {
        /// <summary>
        /// 
        /// </summary>
        protected abstract RotatableObject RotatableObject { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mapPanel"></param>
        public RotatableObjectGraphic(MapPanel mapPanel)
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
            // Create rotation menu item
            MenuItem rotate = new MenuItem("Rotate");
            rotate.Click += delegate { RotatableObject.Rotate(); };

            menu.MenuItems.Add(rotate);

            // Add base items
            //base.AddMenuItems(menu, mapPanel.LoadedLevel);
        }

    }
}
