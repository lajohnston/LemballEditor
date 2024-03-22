using LemballEditor.View;
using System;
using System.IO;
using System.Text;
using System.Windows.Forms;



namespace LemballEditor
{
    internal static class Program
    {
        /// <summary>
        /// Byte array containing the version number
        /// (major version, year, month, release number)
        /// </summary>
        private static readonly byte[] version = new byte[] { 0, 0, 5, 0 };

        /// <summary>
        /// The file name to which the loaded level pack was last saved
        /// </summary>
        public static string ProjectFileName { get; set; }

        /// <summary>
        /// The currently loaded level pack
        /// </summary>
        private static Model.LevelPack loadedLevelPack;
        private static Model.LevelPack LoadedLevelPack
        {
            get => loadedLevelPack;
            set
            {
                loadedLevelPack = value;

                // Load first level
                LoadLevel(Model.LevelGroupTypes.Fun, 0);

                // Inform GUI
                //MainInterface.OnLevelPackLoad(loadedLevelPack);
            }
        }

        /// <summary>
        /// The current loaded level
        /// </summary>
        private static Model.Level loadedLevel;
        public static Model.Level LoadedLevel
        {
            get => loadedLevel;
            set
            {
                loadedLevel = value;

                // Inform GUI
                MainInterface.OnLevelLoad();
            }
        }

        /// <summary>
        /// Whether the debug mode is active
        /// </summary>
        public static bool DebugMode = true;

        /// <summary>
        /// The path to the Lemball Exe file
        /// </summary>
        public static string LemballExePath
        {
            get
            {
                string path = Properties.Settings.Default.LemballExePath;
                return path.Length == 0 || !File.Exists(path) ? null : path;
            }
        }

        public static bool LemballPathIsSet => LemballExePath != null;

        /// <summary>
        /// The path to the Lemmings Paintball directory
        /// </summary>
        public static string LemballDirectory => LemballExePath == null ? null : Path.GetDirectoryName(LemballExePath);

        /// <summary>
        /// The path to the current Vsr file
        /// </summary>
        public static string VsrPath => LemballExePath == null ? null : Path.Combine(LemballDirectory, "pbaimog.vsr");

        /// <summary>
        /// The path used to backup the original Vsr file
        /// </summary>
        private static string BackupVsrPath => LemballExePath == null ? null : Path.Combine(LemballDirectory, "pbaimog (backup).vsr");

        /// <summary>
        /// Called when an object graphic has been altered, for example if it has been rotated
        /// </summary>
        public static void OnObjectAlteration(Model.LevelObject levelObject)
        {
            MainInterface.OnObjectAlteration(levelObject);
        }

        /// <summary>
        /// The main editor interface
        /// </summary>
        public static MainInterface MainInterface { get; private set; }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            //
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Initialise main interface
            MainInterface = new MainInterface();

            // File name isn't set until the project is first saved 
            ProjectFileName = null;

            // Create and load a new level pack with a level in Fun
            LoadedLevelPack = new LemballEditor.Model.LevelPack();
            LoadedLevelPack.CreateNewLevel(Model.LevelGroupTypes.Fun);

            // Load first level
            LoadLevel(Model.LevelGroupTypes.Fun, 0);


            //Properties.Settings.Default.Reset();
            //Properties.Settings.Default.Save();

            /*
            Properties.Settings.Default.LemballFolder = "blah";
            System.Console.WriteLine("Property = " + Properties.Settings.Default.LemballFolder);
            Properties.Settings.Default.Save();
            */

            // Run application
            Application.Run(MainInterface);
        }

        /// <summary>
        /// Returns the version number as an uint that can be used to easily compare to other version numbers
        /// </summary>
        /// <returns>An array containing the version number.</returns>
        public static uint GetVersion()
        {
            int versionNumber = version[0]
                + (version[1] * 256)
                + (version[2] * 65536)
                + (version[3] * 16777216)
                ;

            return (uint)versionNumber;
        }

        /// <summary>
        /// Compares this version of the program to another version
        /// </summary>
        /// <param name="toVersion">A byte array containing four values (major, year, month, release)</param>
        /// <returns>-1 if the specified version is older than this version; 0 if both versions are the same; 1 if the specified version is newer than this one</returns>
        public static int CompareVersion(byte[] toVersion)
        {
            // Ensure the version to compare is 4-bytes long
            if (toVersion.Length != 4)
            {
                throw new ArgumentOutOfRangeException();
            }

            // Check if newer or older
            for (int i = 0; i < 4; i++)
            {
                int difference = toVersion[i] - version[i];

                if (difference < 0)
                {
                    return -1;
                }
                else if (difference > 0)
                {
                    return 1;
                }
            }

            // Both version are the same
            return 0;
        }

        /// <summary>
        /// Formats the the Lemball Editor version number as a string
        /// </summary>
        /// <returns>A string containing the version number</returns>
        public static string GetVersionString()
        {
            // Create a string builder to store the version
            StringBuilder builder = new StringBuilder(11);

            // Each subversion
            for (int i = 0; i < 4; i++)
            {
                // Append sub version number
                _ = builder.Append(version[i]);

                // Append a decimal point if there are more subversions to go
                if (i != version.Length - 1)
                {
                    _ = builder.Append('.');
                }
            }

            return builder.ToString();
        }

        /// <summary>
        /// Loads a level pack from the specified reader
        /// </summary>
        /// <param name="reader">The reader that is reading a level pack file</param>
        /// <param name="fileName">The file name of the file</param>
        public static void LoadLevelPack(BinaryReader reader, string fileName)
        {
            // Loads the level pack
            Model.LevelPack levelPack = new LemballEditor.Model.LevelPack(reader);

            // Verify that the level pack has been loaded successfully
            if (levelPack != null)
            {
                // Set the loaded level pack
                LoadedLevelPack = levelPack;

                // Set the file name
                ProjectFileName = fileName;
            }
        }

        /// <summary>
        /// Returns the level group of the loaded level
        /// </summary>
        /// <returns></returns>
        public static Model.LevelGroupTypes? LoadedLevelGroup()
        {
            if (LoadedLevel != null)
            {
                // Retrieve the level group
                Model.LevelGroupTypes? group = LoadedLevelPack.GetLevelGroupType(LoadedLevel);

                return group != null
                    ? (Model.LevelGroupTypes?)(Model.LevelGroupTypes)group
                    : throw new ApplicationException("Loaded level is not part of the loaded level pack");
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Returns the level number of the loaded level
        /// </summary>
        /// <returns></returns>
        public static int LoadedLevelNumber()
        {
            return LoadedLevel != null ? LoadedLevelPack.GetLevelNumber(LoadedLevel) : throw new NullReferenceException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static bool LevelGroupHasCapacity(Model.LevelGroupTypes group)
        {
            return LoadedLevelPack.LevelGroupHasCapacity(group);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="levelGroup"></param>
        /// <param name="levelNumber"></param>
        public static void LoadLevel(Model.LevelGroupTypes levelGroup, int levelNumber)
        {
            LoadedLevel = LoadedLevelPack.GetLevel(levelGroup, levelNumber);
        }

        /// <summary>
        /// States whether a level pack is currently loaded
        /// </summary>
        /// <returns>True if a level pack is loaded, otherwise false</returns>
        public static bool LevelPackIsLoaded()
        {
            return LoadedLevelPack != null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        public static void SaveLevelPackToFile(string fileName)
        {
            // Compile the project
            LoadedLevelPack.SaveProjectFile(fileName);

            // Remember the file name
            ProjectFileName = fileName;
        }

        /// <summary>
        /// Loads a list of levels in the current difficulty group within the specified ListBox object
        /// </summary>
        /// <param name="list">The ListBox to load the level list into</param>
        public static void LoadLevelList(Model.LevelGroupTypes levelGroup, ListBox list)
        {
            LoadedLevelPack.LoadLevelList(levelGroup, list);
        }

        /// <summary>
        /// 
        /// </summary>
        public static void OnLevelListChange()
        {
            MainInterface.OnLevelListChange();
        }

        /// <summary>
        /// 
        /// </summary>
        public static void TestLevelPack()
        {
            CompileVsr(LoadedLevelPack, VsrPath);
            Program.RunLemball();
        }

        /// <summary>
        /// 
        /// </summary>
        public static void TestLoadedLevel()
        {
            // Create a new level pack
            Model.LevelPack levelPack = new Model.LevelPack();

            // Make the loaded level the first level of fun
            levelPack.CreateNewLevel(Model.LevelGroupTypes.Fun, LoadedLevel);

            // Compile level pack as a VSR file
            CompileVsr(levelPack, VsrPath);

            // Run Lemball
            Program.RunLemball();
        }

        /// <summary>
        /// Attempts to create a new level within the current difficulty group. Returns a boolean
        /// value that specifies whether the level creation was successful. Level creation may fail
        /// if the maximum number of levels within the current difficulty has been reached.
        /// </summary>
        /// <returns>True if the level creation was successful, otherwise false</returns>
        public static void CreateNewLevel(Model.LevelGroupTypes levelGroup)
        {
            LoadedLevelPack.CreateNewLevel(levelGroup);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="group"></param>
        /// <param name="levelNumber"></param>
        public static void DeleteLevel(Model.LevelGroupTypes group, int levelNumber)
        {
            LoadedLevelPack.DeleteLevel(group, levelNumber);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="destinationGroup"></param>
        public static void CopyLoadedLevel(Model.LevelGroupTypes destinationGroup)
        {
            if (LoadedLevel != null)
            {
                LoadedLevelPack.CopyLevel(LoadedLevel, destinationGroup);
            }
            else
            {
                throw new NullReferenceException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="group"></param>
        /// <param name="levelNumber"></param>
        public static void MoveLevelUp(Model.LevelGroupTypes group, int levelNumber)
        {
            LoadedLevelPack.MoveLevelUp(group, levelNumber);
        }

        public static void MoveLoadedLevelToLevelGroup(Model.LevelGroupTypes group)
        {
            LoadedLevelPack.MoveLevelToLevelGroup(LoadedLevel, group);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="group"></param>
        /// <param name="levelNumber"></param>
        public static void MoveLevelDown(Model.LevelGroupTypes group, int levelNumber)
        {
            LoadedLevelPack.MoveLevelDown(group, levelNumber);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="levelPack"></param>
        /// <param name="outputPath"></param>
        private static void CompileVsr(Model.LevelPack levelPack, string outputPath)
        {
            VsrCompiler.VsrCompiler vsrCompiler = new VsrCompiler.VsrCompiler(VsrPath, BackupVsrPath);

            // Compile the level pack without compression
            using (MemoryStream compiledLevelPack = levelPack.Compile(false))
            {
                // Compile the VSR file from the level pack
                using (BinaryReader binReader = new BinaryReader(compiledLevelPack))
                {
                    vsrCompiler.CompileVSR(binReader, outputPath);
                }
            }
        }

        /// <summary>
        /// Executes the Lemball exe.
        /// </summary>
        public static void RunLemball()
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo.WorkingDirectory = LemballDirectory;
            process.StartInfo.FileName = LemballExePath;

            // Arguments
            StringBuilder args = new StringBuilder(27);
            if (!Properties.Settings.Default.EnableMovies)
            {
                _ = args.Append("/noanim");
            }

            if (!Properties.Settings.Default.EnableMusic)
            {
                _ = args.Append(" /nomusic");
            }

            if (!Properties.Settings.Default.EnableSoundEffects)
            {
                _ = args.Append(" /noeffects");
            }

            process.StartInfo.Arguments = args.ToString();

            // Start Lemball
            _ = process.Start();
        }

        public static void RestoreOriginalLevels()
        {
            File.Copy(BackupVsrPath, VsrPath, true);
        }
    }
}