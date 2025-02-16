using LemballEditor.Model;
using System.Windows.Forms;

namespace LemballEditor.View.Level.ObjectGraphics
{
    /// <summary>
    /// 
    /// </summary>
    internal abstract class RotatableObjectGraphic : ObjectGraphic
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

            _ = menu.MenuItems.Add(rotate);

            // Add base items
            //base.AddMenuItems(menu, mapPanel.LoadedLevel);
        }

    }
}
