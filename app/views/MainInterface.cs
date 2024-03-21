using LemballEditor.Model;
using LemballEditor.View.Level;
using LemballEditor.View.Level.ObjectGraphics;
using System;
using System.IO;
using System.Windows.Forms;

namespace LemballEditor.View
{
    public partial class MainInterface : Form
    {
        /// <summary>
        /// A list of objects that the user can create
        /// </summary>
        private static ObjectsList objectsList;

        /// <summary>
        /// Displays the available tile images that can be pasted
        /// </summary>
        private static TilePaletteSelector tilePalette;

        /// <summary>
        /// The file name filter used by dialog boxes
        /// </summary>
        private static readonly string dialogFilter = "Lemball Editor Project files (*.lbp) | *.lbp";

        /// <summary>
        /// 
        /// </summary>
        private static readonly MapPanel mapPanel;

        /// <summary>
        /// 
        /// </summary>
        private static readonly LevelBrowser levelBrowser;

        /// <summary>
        /// 
        /// </summary>
        static MainInterface()
        {
            // Create map panel
            mapPanel = new MapPanel();

            // Create level browser
            levelBrowser = new LevelBrowser();
        }

        /// <summary>
        /// Initialises the main interface
        /// </summary>
        public MainInterface()
        {
            // Initialise components
            InitializeComponent();
            objectsList = new ObjectsList(this);
            tilePalette = new TilePaletteSelector();

            mapPanelContainer.Controls.Add(mapPanel);
            levelBrowserGroup.Controls.Add(levelBrowser);

            // Map editor refresh rate
            refreshMapTimer.Interval = 1000 / MapPanel.REFRESH_RATE;
            refreshMapTimer.Start();

            testAllLevelsToolStripMenuItem.Click += delegate { TestLevelPack(true); };
            testLevelToolStripMenuItem.Click += delegate { TestLevelPack(false); };

            //
            ShowObjectSelectionList();
            //showTilePalette();

            SetStatusMessage("Use mouse wheel to adjust elevation. Move objects by dragging with left button. Right click object for options. Use arrows keys to scroll.");
        }

        /// <summary>
        /// Called when an object is deleted. Updates the map panel and the object limit counter
        /// </summary>
        public void OnObjectDeletion()
        {
            // Refresh map
            mapPanel.RenderMapAtNextUpdate();

            // Update object count
            OnObjectCountChange();
        }

        /// <summary>
        /// Called whenever the object count is changed, such as if an object is created or deleted of if a different level is loaded
        /// </summary>
        public static void OnObjectCountChange()
        {
            objectsList.UpdateObjectLimitCounter();
        }

        /// <summary>
        /// Called when an object has been altered, for example if it has been rotated
        /// </summary>
        public static void OnObjectAlteration(LevelObject levelObject)
        {
            mapPanel.OnObjectAlteration(levelObject);
        }

        /// <summary>
        /// Returns the number of objects that can be created in the current level before reaching the object limit
        /// </summary>
        /// <returns></returns>
        public static int GetObjectLimitRemaining()
        {
            Model.Level level = Program.LoadedLevel;

            return level != null ? level.ObjectLimitRemaining : 0;
        }

        /*
        public static void setPlacingObject(LevelObject gameObject)
        {
            MapPanel.setHoldingObject(gameObject, true);
            objectsList.updateObjectLimitCounter();
        }
        */

        /// <summary>
        /// Refresh the list of levels
        /// </summary>
        /*
        private void RefreshLevelList()
        {
            Program.LoadLevelList(levelGroupSelector.SelectedLevelGroup, lstLevels);
            lstLevels.SelectedIndex = 0;
        }
        */

        /// <summary>
        /// Loads a level with the specified number
        /// </summary>
        /// <param name="levelNumber"></param>
        public void LoadLevel(Model.LevelGroupTypes levelGroup, int levelNumber)
        {
            // Change the selected level number
            //Program.LoadedLevelNumber = levelNumber;
            Program.LoadLevel(levelGroup, levelNumber);


            // Inform map panel that the level number has changed
            //OnLevelLoad();

            // Update the object limit counter
            //OnObjectCountChange();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            mapPanel.RefreshLogic();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void testLevel_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public static void OnLevelLoad()
        {
            // Inform map panel
            mapPanel.OnLevelLoad();

            // Inform level browser
            levelBrowser.OnLevelLoad();

            // Update the object limit counter
            OnObjectCountChange();
        }

        /// <summary>
        /// Called when a level has been added or deleted
        /// </summary>
        public static void OnLevelListChange()
        {
            levelBrowser.UpdateLevelList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tileRef"></param>
        public void StartTileEditMode(uint tileRef)
        {
            mapPanel.StartTileEditMode(tileRef);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnObjects_Click(object sender, EventArgs e)
        {
            ShowObjectSelectionList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTiles_Click(object sender, EventArgs e)
        {
            showTilePalette();
        }

        /// <summary>
        /// 
        /// </summary>
        private void ShowObjectSelectionList()
        {
            pallettePanel.Controls.Clear();
            pallettePanel.Controls.Add(objectsList);
        }

        /// <summary>
        /// 
        /// </summary>
        private void showTilePalette()
        {
            pallettePanel.Controls.Clear();
            pallettePanel.Controls.Add(tilePalette);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="levelObject"></param>
        public void StartPlacingNewObjectMode(LevelObject levelObject)
        {
            mapPanel.StartPlacingNewObjectMode(ObjectGraphic.New(levelObject, mapPanel));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="movingObject"></param>
        public void StartPathEditMode(MovingObject movingObject)
        {
            mapPanel.StartPathEditMode(movingObject);
        }

        /// <summary>
        /// Displays a OpenFile dialog that allows the user to select a Lemball Editor project file,
        /// which is then loaded.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void loadProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Create load file dialog
            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = dialogFilter,
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                Title = "Select a Lemball Editor project file"
            };

            // Load file
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                using (BinaryReader reader = new BinaryReader(dialog.OpenFile()))
                {
                    try
                    {
                        // Load the level pack
                        Program.LoadLevelPack(reader, dialog.FileName);
                        //Program.SetLoadedLevelPack(new LevelPack.LevelPack(reader), dialog.FileName);

                        // Refresh map and level list
                        //RefreshLevelList();
                        //mapPanel.OnLevelLoad();
                    }
                    catch (InvalidDataException error)
                    {
                        _ = MessageBox.Show(error.Message, "Invalid project file");
                    }
                }
            }
        }

        /// <summary>
        /// Called when the Save As menu item has been clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveProjectAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowSaveAsDialog();
        }

        /// <summary>
        /// Called when the Save menu item has been clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveLevelPack();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="switchObject"></param>
        public static void StartSwitchConnectionMode(Switch switchObject)
        {
            mapPanel.StartEditingConnectionsMode(switchObject);
        }

        /// <summary>
        /// Shows a SaveFile dialog that allows the user to select a file to save to
        /// </summary>
        /// <param name="fileName"></param>
        private void ShowSaveAsDialog()
        {
            // Initialise the save file dialog
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = dialogFilter,
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            };

            // Show dialog, and run code if a file is chosen
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Save project to the specified file
                Program.SaveLevelPackToFile(saveFileDialog.FileName);
            }
        }

        /// <summary>
        /// Saves the level pack to it's file path. If the level pack does not have a file path, displays the
        /// SaveFile dialog.
        /// </summary>
        public void SaveLevelPack()
        {
            // If the project has a file name
            if (Program.ProjectFileName != null)
            {
                // Save project to file name
                Program.SaveLevelPackToFile(Program.ProjectFileName);
            }
            // The level pack hasn't yet been saved
            else
            {
                // Show the SaveAs dialog
                ShowSaveAsDialog();
            }
        }


        private void levelToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            LevelProperties properties = new EditLevelProperties(Program.LoadedLevel);
            _ = properties.ShowDialog();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Settings.Settings settings = new Settings.Settings();
            _ = settings.ShowDialog();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="allLevels">If true, all levels are compiled, otherwise only the loaded level is compiled as first level of Fun.</param>
        private void TestLevelPack(bool allLevels)
        {

            if (allLevels)
            {
                Program.TestLevelPack();
            }
            else
            {
                Program.TestLoadedLevel();
            }


            /*
            BinaryEditor new2 = new BinaryEditor(@".\compressed.dat", true);
            new2.saveUncompressed(@".\decompressed.dat");
            new2.Close();
             */
        }


        public void SetStatusMessage(string message)
        {
            statusBarMessage.Text = message;
        }

        private void restoreOriginalLevelsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.RestoreOriginalLevels();
        }

        private void levelGroupSelector_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}