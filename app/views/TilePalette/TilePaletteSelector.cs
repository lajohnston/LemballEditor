using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using LemballEditor.View.Level;

namespace LemballEditor.View
{
    /// <summary>
    /// A tab control that allows the user to select from a choice of tile palettes
    /// </summary>
    public partial class TilePaletteSelector : UserControl
    {
        private static uint selectedTileRef;

        /// <summary>
        /// Stores the rendere browser that displays the tiles
        /// </summary>
        public static Bitmap browser { get; private set; }

        public static int scrollValue { get; private set; }

        public TilePaletteSelector()
        {
            // Initalise components
            InitializeComponent();

            browser = null;
            scrollValue = 0;

            vScrollBar1.SmallChange = 50;

            // Add each tile palette as a tab
            tabControl1.Controls.Add(new TilePalette("Land", new ImageCache.TileTypes[] {
                ImageCache.TileTypes.Land,
                ImageCache.TileTypes.PathEdge,
                ImageCache.TileTypes.Slope,
                ImageCache.TileTypes.Elevated,
                ImageCache.TileTypes.Fence,
                ImageCache.TileTypes.Rock,
                ImageCache.TileTypes.Other,
            }));
            
            tabControl1.Controls.Add(new TilePalette("Trees", new ImageCache.TileTypes[] {
                ImageCache.TileTypes.Plant
            }));

            tabControl1.Controls.Add(new TilePalette("Water", new ImageCache.TileTypes[] {
                ImageCache.TileTypes.Water,
                ImageCache.TileTypes.Shore
            }));

            tabControl1.Controls.Add(new TilePalette("Hazards", new ImageCache.TileTypes[] {
                ImageCache.TileTypes.Hazard
            }));

            selectedTileRef = 0;

            tabControl1_SelectedIndexChanged(this, null);

        }

        public static void SetSelectedTileRef (uint tileRef)
        {
            selectedTileRef = tileRef;
            Program.MainInterface.StartTileEditMode(tileRef);
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Render the browser
            browser = ((TilePalette)tabControl1.SelectedTab).RenderBrowser();
 
            // Update the vScrollBar height
            vScrollBar1.Maximum = browser.Height;
            vScrollBar1.Value = 0;
            vScrollBar1_Scroll(this, new ScrollEventArgs(ScrollEventType.First, 0));
        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            //((TilePalette)tabControl1.SelectedTab).Scroll(e.NewValue);
            vScrollBar1.Value = e.NewValue;
            scrollValue = e.NewValue;
            tabControl1.SelectedTab.Invalidate();
        }
    }
}