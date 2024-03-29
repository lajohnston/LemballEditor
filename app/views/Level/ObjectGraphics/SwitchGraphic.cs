﻿using LemballEditor.Model;
using System.Windows.Forms;

namespace LemballEditor.View.Level.ObjectGraphics
{
    /// <summary>
    /// Represents a drawn entrance
    /// </summary>
    internal abstract class SwitchGraphic : ObjectGraphic
    {
        /// <summary>
        /// 
        /// </summary>
        protected abstract Switch SwitchObject { get; }

        /// <summary>
        /// 
        /// </summary>
        public override LevelObject LevelObject => SwitchObject;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mapPanel"></param>
        public SwitchGraphic(MapPanel mapPanel)
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
            MenuItem addConnection = new MenuItem("Edit connections");

            addConnection.Click += delegate
            {
                MainInterface.StartSwitchConnectionMode(SwitchObject);
            };

            _ = menu.MenuItems.Add(addConnection);
        }
    }
}
