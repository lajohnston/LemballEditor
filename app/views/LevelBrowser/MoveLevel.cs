﻿using System;
using System.Windows.Forms;

namespace LemballEditor.View
{
    public partial class LevelBrowser
    {
        private partial class MoveLevel : Form
        {
            /// <summary>
            /// Reference to the main level browser
            /// </summary>
            private readonly LevelBrowser browser;

            /// <summary>
            /// The level group that is currently selected
            /// </summary>
            private Model.LevelGroupTypes SelectedLevelGroup => ((LevelBrowser.LevelGroupItem)levelGroupList.SelectedItem).LevelGroupType;

            /// <summary>
            /// 
            /// </summary>
            /// <param name="browser"></param>
            public MoveLevel(LevelBrowser browser)
            {
                // Initialise components
                InitializeComponent();

                this.browser = browser;

                // Add level groups to list
                levelGroupList.Items.AddRange(browser.LevelGroupItems);
                levelGroupList.Items.RemoveAt(browser.levelGroupSelector.SelectedIndex);

                // Select first item
                levelGroupList.SelectedIndex = 0;
            }

            /// <summary>
            /// Moves the loaded level to the selected level group
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void move_Click(object sender, EventArgs e)
            {
                try
                {
                    Program.MoveLoadedLevelToLevelGroup(SelectedLevelGroup);

                    browser.UpdateLevelList();
                    browser.UpdateSelectedLevel();

                    // Close dialog
                    Close();
                }
                catch (Model.LevelGroupFullException)
                {
                    _ = MessageBox.Show("The " + SelectedLevelGroup.ToString() + " level group has reached its capacity");
                }
            }

            /// <summary>
            /// Copies the selected level to the selected level group
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void copy_Click(object sender, EventArgs e)
            {
                // Copy level and test if operation is successful
                if (browser.CopySelectedLevel(SelectedLevelGroup))
                {
                    // If the operation is successful, close the form
                    Close();
                }
            }
        }
    }
}
