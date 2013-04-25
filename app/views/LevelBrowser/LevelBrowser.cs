﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace LemballEditor.View
{
    public partial class LevelBrowser : UserControl
    {
        /*
        public static LevelPack.LevelGroupTypes[] LevelGroups { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        static LevelBrowser()
        {
            LevelGroups = new LevelPack.LevelGroupTypes[]
            {
                LevelPack.LevelGroupTypes.Fun,
                LevelPack.LevelGroupTypes.Tricky,
                LevelPack.LevelGroupTypes.Taxing,
                LevelPack.LevelGroupTypes.Mayhem
            };
        }
        */

        /// <summary>
        /// 
        /// </summary>
        protected LevelGroupItem[] LevelGroupItems
        {
            get
            {
                return levelGroupSelector.GetItems();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mainInterface"></param>
        public LevelBrowser()
        {
            // Initialise
            InitializeComponent();

            // Fill container
            Anchor = AnchorStyles.None;
            Dock = DockStyle.Fill;

            // Set tooltips
            toolTips.SetToolTip(moveLevelUp, "Move level up");
            toolTips.SetToolTip(moveLevelDown, "Move level down");
            toolTips.SetToolTip(deleteLevel, "Delete level");
            toolTips.SetToolTip(newLevel, "Create new level");
            toolTips.SetToolTip(moveLevel, "Move or copy level to another level group");

            // Update level list whenever the level group is changed
            levelGroupSelector.SelectedIndexChanged += new EventHandler(levelGroupSelector_SelectedIndexChanged);

            // Add listener for when the level is changed
            levelList.SelectedIndexChanged += new EventHandler(levelList_SelectedIndexChanged);

            // Add listeners for buttons
            newLevel.Click += new EventHandler(newLevel_Click);
            deleteLevel.Click += new EventHandler(deleteLevel_Click);
            
        }

        /// <summary>
        /// Called when a new level group has been selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void levelGroupSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateLevelList();
            UpdateSelectedLevel();
        }

        /// <summary>
        /// Called when the delete level button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void deleteLevel_Click(object sender, EventArgs e)
        {
            Program.DeleteLevel((Model.LevelGroupTypes)levelGroupSelector.SelectedLevelGroup, levelList.SelectedIndex);
            UpdateLevelList();
        }

        /// <summary>
        /// Called when the new level button has been clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void newLevel_Click(object sender, EventArgs e)
        {
            try 
            {
                // Attempt to create level
                Program.CreateNewLevel((Model.LevelGroupTypes)levelGroupSelector.SelectedLevelGroup);

                // Refresh the level list so that it includes the new level
                UpdateLevelList();

                // Select the new level in the list (which also loads it)
                int levelNumber = levelList.Items.Count - 1;
                levelList.SelectedIndex = levelNumber;
            }
            catch (Model.LevelGroupFullException)
            {

            }
        }

        /// <summary>
        /// Called when a new level has been selected in the level list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void levelList_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Load the selected level
            Program.LoadLevel((Model.LevelGroupTypes)levelGroupSelector.SelectedLevelGroup, levelList.SelectedIndex);

            // If no level is selected
            if (levelList.SelectedIndex == -1)
            {
                // Disable buttons
                moveLevelUp.Enabled = false;
                moveLevelDown.Enabled = false;
                moveLevel.Enabled = false;
                deleteLevel.Enabled = false;
            }
            else
            {
                // Enable buttons
                deleteLevel.Enabled = true;

                // If first level is selected
                if (levelList.SelectedIndex == 0)
                    moveLevelUp.Enabled = false;
                else
                    moveLevelUp.Enabled = true;

                // If last level is selected
                if (levelList.SelectedIndex == levelList.Items.Count - 1)
                    moveLevelDown.Enabled = false;
                else
                    moveLevelDown.Enabled = true;
            }

        }

        /// <summary>
        /// Updates the list of levels for the selected level group
        /// </summary>
        public void UpdateLevelList()
        {
            // Load the selected level group
            Program.LoadLevelList(levelGroupSelector.SelectedLevelGroup, levelList);

            // Select the level in the list
            UpdateSelectedLevel();

            // Disable new level button if the capacity for the selected level group has been reached
            if (Program.LevelGroupHasCapacity((Model.LevelGroupTypes)levelGroupSelector.SelectedLevelGroup))
                newLevel.Enabled = true;
            else
                newLevel.Enabled = false;
        }

        /// <summary>
        /// If the loaded level is within the current group, it is selected in the list, otherwise
        /// no items are selected
        /// </summary>
        public void UpdateSelectedLevel()
        {
            try
            {
                // If the loaded level is within the current group
                if (Program.LoadedLevelGroup() == levelGroupSelector.SelectedLevelGroup)
                {
                    // Select the level in the list
                    levelList.SelectedIndex = Program.LoadedLevelNumber();
                }
                else
                {
                    // Select no items in the list
                    levelList.SelectedIndex = -1;
                }
            }
            // If no level is loaded
            catch (NullReferenceException)
            {
                levelList.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnLevelLoad()
        {
            // Get the loaded level group
            Model.LevelGroupTypes? loadedLevelGroup = Program.LoadedLevelGroup();

            // If a level is loaded
            if (loadedLevelGroup != null)
            {
                // Attempt to display the level group that the level is loaded in
                levelGroupSelector.SelectedLevelGroup = (Model.LevelGroupTypes)loadedLevelGroup;

                // Update the selected level in the level list
                UpdateSelectedLevel();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void moveLevelUp_Click(object sender, EventArgs e)
        {
            int levelNumber = levelList.SelectedIndex;

            if (levelNumber != -1)
                Program.MoveLevelUp((Model.LevelGroupTypes)levelGroupSelector.SelectedLevelGroup, levelNumber);

            UpdateLevelList();
        }

        private void moveLevelDown_Click(object sender, EventArgs e)
        {
            int levelNumber = levelList.SelectedIndex;

            if (levelNumber != -1)
                Program.MoveLevelDown((Model.LevelGroupTypes)levelGroupSelector.SelectedLevelGroup, levelNumber);

            UpdateLevelList();
        }

        public void MoveLevelToGroup(Model.LevelGroupTypes group)
        {
            Program.MoveLoadedLevelToLevelGroup(group);
        }

        private void moveLevel_Click(object sender, EventArgs e)
        {
            MoveLevel moveLevel = new MoveLevel(this);
            moveLevel.ShowDialog();
        }

        private void copyLevel_Click(object sender, EventArgs e)
        {
            CopySelectedLevel(levelGroupSelector.SelectedLevelGroup);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="destinationGroup"></param>
        public bool CopySelectedLevel(Model.LevelGroupTypes destinationGroup)
        {
            try
            {
                Program.CopyLoadedLevel(destinationGroup);

                UpdateLevelList();
                UpdateSelectedLevel();

                return true;
            }
            catch (Model.LevelGroupFullException)
            {
                MessageBox.Show("The " + destinationGroup.ToString() + " level group has reached its capacity");
                return false;
            }
        }


    }
}
