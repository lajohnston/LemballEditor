using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

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

            this.FormClosing += OnFormClosing;
        }

        /// <summary>
        /// Called when the exe browse button is pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            // Create open file dialog
            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "Lemmings Paintball Exe file | Lemball.exe",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles)
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                exePath.Text = dialog.FileName;
            }
        }

        /// <summary>
        /// Indicates if the given path points to what looks like a valid Lemball.exe file
        /// </summary>
        /// <param name="exePath"></param>
        /// <returns></returns>
        private bool ExePathIsValid(string exePath)
        {
            if (!File.Exists(exePath)) {
                return false;
            }

            var filename = Path.GetFileName(exePath).ToLowerInvariant();
            return filename == "lemball.exe";
        }

        /// <summary>
        /// Called when the form is closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.Cancel) {
                return;
            }

            if (exePath.Text.Length > 0 && !ExePathIsValid(exePath.Text))
            {
                MessageBox.Show("The EXE path should point to a valid Lemball.exe file", "Invalid EXE Path");
                e.Cancel = true;
                return;
            }

            var settings = Properties.Settings.Default;

            // Save exe path
            settings.LemballExePath = exePath.Text;

            // Save run options
            settings.EnableMovies = enableMovies.Checked;
            settings.EnableMusic = enableMusic.Checked;
            settings.EnableSoundEffects = enableSoundEffects.Checked;

            // Save settings
            settings.Save();
        }
    }
}
