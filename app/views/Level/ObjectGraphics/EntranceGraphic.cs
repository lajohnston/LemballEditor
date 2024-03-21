using LemballEditor.Model;
using System.Drawing;
using System.Windows.Forms;

namespace LemballEditor.View.Level.ObjectGraphics
{
    /// <summary>
    /// Represents a drawn entrance
    /// </summary>
    internal class EntranceGraphic : ObjectGraphic
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly Entrance entrance;

        /// <summary>
        /// 
        /// </summary>
        public override LevelObject LevelObject => entrance;

        /// <summary>
        /// The object's image
        /// </summary>
        private static readonly Bitmap image = LemballEditor.View.Level.ImageCache.GetObjectImage("entrance");

        /// <summary>
        /// Accessor to the object image
        /// </summary>
        public override Bitmap Image => image;

        /// <summary>
        /// The entrance's draw offset
        /// </summary>
        public override Point DrawOffset => new Point(48, 116);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entrance"></param>
        /// <param name="mapPanel"></param>
        public EntranceGraphic(Model.Entrance entrance, MapPanel mapPanel)
            : base(mapPanel)
        {
            this.entrance = entrance;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="menu"></param>
        protected override void AddMenuItems(ContextMenu menu)
        {
            /* Get the number of additional Lemmings that can be created in the current level,
             * excuding the number of Lemmings that have already been assigned to this entrance */
            int availableLemmings = 4 - mapPanel.LoadedLevel.NumberOfLemmings + entrance.NumberOfLemmings;

            // Number of Lemmings menu item
            MenuItem lemmings = new MenuItem("Number of Lemmings");

            // Each Lemmings count (1 - 4)
            for (int i = 1; i < 5; i++)
            {
                // Add the number to the menu item
                MenuItem value = new MenuItem(i.ToString());
                _ = lemmings.MenuItems.Add(value);

                // The number of lemmings this menu item represents
                switch (i)
                {
                    case 1:
                        value.Click += delegate { entrance.NumberOfLemmings = 1; };
                        break;
                    case 2:
                        value.Click += delegate { entrance.NumberOfLemmings = 2; };
                        break;
                    case 3:
                        value.Click += delegate { entrance.NumberOfLemmings = 3; };
                        break;
                    case 4:
                        value.Click += delegate { entrance.NumberOfLemmings = 4; };
                        break;
                }

                // Check the menu item if it represents the number of Lemmings that are currently assigned to the Entrance
                if (i == entrance.NumberOfLemmings)
                {
                    value.Checked = true;
                }

                // Disable the menu item if the number exceeds the number of available Lemmings
                if (i > availableLemmings)
                {
                    value.Enabled = false;
                }
            }

            _ = menu.MenuItems.Add(lemmings);
        }


    }
}
