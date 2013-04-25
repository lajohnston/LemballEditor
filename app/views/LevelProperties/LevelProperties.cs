using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using LemballEditor.Model;

namespace LemballEditor.View
{
    public partial class LevelProperties : Form
    {
        /// <summary>
        /// The level to display and edit the properties of
        /// </summary>
        private Model.Level level;

        /// <summary>
        /// Creates a new Level Properties form based on the settings of the specified level
        /// </summary>
        /// <param name="level">The level whose properties will be edited</param>
        public LevelProperties(Model.Level level)
        {
            // Initailise components
            InitializeComponent();
            
            // Store level
            this.level = level;

            // Load level name
            levelName.Text = level.Name;

            // Set the terrain type
            LoadTerrainType();

            // Load the time limit values
            LoadTimeLimit();

            // Load the number of flags required to win
            LoadFlagsRequired();

            flagsInLevel.Text = "Flags currently placed: " + level.CountPlayerOneFlags();

            // 
            LoadMapSize();
        }

        /// <summary>
        /// 
        /// </summary>
        private void LoadMapSize()
        {
            xTiles.Text = level.MapSizeX.ToString();
            yTiles.Text = level.MapSizeY.ToString();
        }

        /// <summary>
        /// Loads the 'flags required' setting of the level and selects the appropriate
        /// radio box
        /// </summary>
        private void LoadFlagsRequired()
        {
            switch (level.NumberOfFlagsRequiredToWin)
            {
                case Model.Level.FlagsRequired.One:
                    oneFlag.Select();
                    break;
                case Model.Level.FlagsRequired.Two:
                    twoFlags.Select();
                    break;
                case Model.Level.FlagsRequired.Three:
                    threeFlags.Select();
                    break;
                case Model.Level.FlagsRequired.Four:
                    fourFlags.Select();
                    break;
                case Model.Level.FlagsRequired.All:
                    allFlags.Select();
                    break;
            }
        }

        /// <summary>
        /// Sets the terrain type option box
        /// </summary>
        private void LoadTerrainType()
        {
            // Set terrain type
            switch (level.TerrainType)
            {
                case LemballEditor.Model.Level.TerrainTypes.Grass:
                    grassTerrain.Select();
                    break;
                case LemballEditor.Model.Level.TerrainTypes.Lego:
                    legoTerrain.Select();
                    break;
                case LemballEditor.Model.Level.TerrainTypes.Snow:
                    snowTerrain.Select();
                    break;
                case LemballEditor.Model.Level.TerrainTypes.Space:
                    spaceTerrain.Select();
                    break;
            }
        }

        /// <summary>
        /// Loads the time limit values into the time limit boxes
        /// </summary>
        private void LoadTimeLimit()
        {
            // If the level has unlimited time
            if (level.HasUnlimitedTimeLimit())
            {
                // Set minutes and seconds text boxes to 9:59, and disable them
                timeLimitMinutes.Text = "9";
                timeLimitSeconds.Text = "59";

                // Set unlimitedTime checkbox to true
                unlimitedTime.Checked = true;
            }
            // Level has a time limit
            else
            {
                // Get time limit in seconds
                ushort timeLimit = level.TimeLimit;

                // Set minutes and seconds text boxes
                timeLimitMinutes.Text = Math.Floor((decimal)(timeLimit / 60)).ToString();
                timeLimitSeconds.Text = (timeLimit % 60).ToString();

                // Uncheck unlimiteTime checkbox
                unlimitedTime.Checked = false;
            }
        }


        /// <summary>
        /// Called when the Ok button is pressed. Validates the data and edits the level's properties
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void okButton_Click(object sender, EventArgs e)
        {
            // Set level name
            level.Name = levelName.Text;

            // Set time limit
            level.SetTimeLimit((ushort)
                (Convert.ToInt32(timeLimitMinutes.Text) * 60
                + Convert.ToInt32(timeLimitSeconds.Text)));

            // Set flags required
            if (oneFlag.Checked)
                level.NumberOfFlagsRequiredToWin = Model.Level.FlagsRequired.One;
            else if (twoFlags.Checked)
                level.NumberOfFlagsRequiredToWin = Model.Level.FlagsRequired.Two;
            else if (threeFlags.Checked)
                level.NumberOfFlagsRequiredToWin = Model.Level.FlagsRequired.Three;
            else if (fourFlags.Checked)
                level.NumberOfFlagsRequiredToWin = Model.Level.FlagsRequired.Four;
            else
                level.NumberOfFlagsRequiredToWin = Model.Level.FlagsRequired.All;
        }

        /// <summary>
        /// Called when the check value of unlimitedTime is changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void unlimitedTime_CheckedChanged(object sender, EventArgs e)
        {
            // If unlimitedTime has been checked
            if (unlimitedTime.Checked == true)
            {
                // Disable time limit input boxes
                timeLimitMinutes.Enabled = false;
                timeLimitSeconds.Enabled = false;
            }
            // If unlimitedTime has been unchecked
            else
            {
                // Enable time limit input boxes
                timeLimitMinutes.Enabled = true;
                timeLimitSeconds.Enabled = true;
            }
        }
    }
}
