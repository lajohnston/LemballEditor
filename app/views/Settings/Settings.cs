using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace LemballEditor.View.Settings
{
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();

            // Load exe path into path text box
            exePath.Text = Properties.Settings.Default.LemballExePath;

            // Check run options
            enableMovies.Checked = Properties.Settings.Default.EnableMovies;
            enableMusic.Checked = Properties.Settings.Default.EnableMusic;
            enableSoundEffects.Checked = Properties.Settings.Default.EnableSoundEffects;
        }

        /// <summary>
        /// Called when the exe browse button is pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            // Create open file dialog
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Lemmings Paintball Exe file | Lemball.exe";
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                exePath.Text = dialog.FileName;
            }
        }

        /// <summary>
        /// Called when the Ok button is pressed. Verifies that the EXE folder path is correct
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ok_Click(object sender, EventArgs e)
        {
            // Save exe path
            Properties.Settings.Default.LemballExePath = exePath.Text;

            // Save run options
            Properties.Settings.Default.EnableMovies = enableMovies.Checked;
            Properties.Settings.Default.EnableMusic = enableMusic.Checked;
            Properties.Settings.Default.EnableSoundEffects = enableSoundEffects.Checked;

            // Save settings
            Properties.Settings.Default.Save();
        }
    }
}
