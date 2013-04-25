using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using VsrCompiler;

namespace LemballEditor.Model
{
    /// <summary>
    /// The types of level group (Fun, Tricky, Taxing, Mayhem, Network)
    /// </summary>
    public enum LevelGroupTypes
    {
        Fun = 0,
        Tricky = 1,
        Taxing = 2,
        Mayhem = 3,
        Network = 4
    }

    /// <summary>
    /// 
    /// </summary>
    public class LevelGroup
    {
        /// <summary>
        /// The number of levels that are hardcoded by Lemball for this LevelGroup. When encoding as VSR data
        /// the level group cannot have any more or less levels than this number. If there aren't enough
        /// user-created levels to meet this number then blank levels need to be used to pad the number out.
        /// </summary>
        private int MaxLevels
        {
            get
            {
                return new byte[] { 25, 26, 29, 22, 12 }[(int)Group];
                /*
                if (FullVersion)
                    return new byte[] { 25, 26, 29, 22, 12 }[(int)group];
                else
                    return new byte[] { 2, 1, 1, 1, 1 }[(int)group];
                */
            }
        }

        //private bool FullVersion;

        /// <summary>
        /// 
        /// </summary>
        private List<Level> levels;

        /// <summary>
        /// 
        /// </summary>
        public LevelGroupTypes Group { get; private set; }

        /// <summary>
        /// Creates a new level group
        /// </summary>
        /// <param name="maxLevels"></param>
        public LevelGroup(LemballEditor.Model.LevelGroupTypes levelGroup)
        {
            levels = new List<Level>();
            Group = levelGroup;
        }

        /// <summary>
        /// Creates a level group from XML data
        /// </summary>
        /// <param name="xmlElement"></param>
        /// <param name="tilesReader"></param>
        public LevelGroup(XmlElement xmlElement, BinaryReader tilesReader)
        {
            // Get the level group
            Group = (LevelGroupTypes)Enum.Parse(typeof(LevelGroupTypes), xmlElement.GetAttribute("name")); 

            // Verify tiles data
            if (new String(tilesReader.ReadChars(3)) != "DIR")
                throw new InvalidDataException("Invalid tile directory offset");
            
            // Number of level tiles
            int numberOfTileFiles = tilesReader.ReadByte();

            // Get level XML data
            XmlNodeList xmlLevels = xmlElement.SelectNodes("level");

            // Ensure number of XML levels is equal to the number of tile files
            if (xmlLevels.Count != numberOfTileFiles)
                throw new InvalidDataException("Invalid number of tile files");

            // Initialise level list
            levels = new List<Level>(xmlLevels.Count);

            // Create each level
            for (int i = 0; i < numberOfTileFiles; i++)
            {
                // Verify level data offset
                if (new String(tilesReader.ReadChars(4)) != "LEV ")
                    throw new InvalidDataException("Invalid tile file offset");
                
                // Get tile data
                int tileSize = tilesReader.ReadInt32();
                byte[] tileData = tilesReader.ReadBytes(tileSize);

                // Get level node
                XmlElement levelElement = (XmlElement) xmlLevels.Item(i);

                // Add level to list
                levels.Add(new Level(levelElement, tileData));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <param name="groupNumber"></param>
        /// <returns></returns>
        public XmlElement CompileXML(XmlDocument xmlDoc, int groupNumber)
        {
            // Create level group element
            XmlElement groupElement = xmlDoc.CreateElement("group");
            groupElement.SetAttribute("name", Group.ToString());

            // Compile each level
            foreach (Level level in levels)
            {
                // Add level data to group element
                groupElement.AppendChild(level.CompileXML(xmlDoc));
            }

            // Return new group element
            return groupElement;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="level"></param>
        public void AppendLevel(Level level)
        {
            if (HasCapacity())
                levels.Add(level);
            else
                throw new LevelGroupFullException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="levelNumber"></param>
        public void DeleteLevel(int levelNumber)
        {
            levels.RemoveAt(levelNumber);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="level"></param>
        public void DeleteLevel(Level level)
        {
            levels.Remove(level);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="levelNumber"></param>
        public void MoveLevelUp(int levelNumber)
        {
            SwapLevels(levelNumber, levelNumber - 1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="levelNumber"></param>
        public void MoveLevelDown(int levelNumber)
        {
            SwapLevels(levelNumber, levelNumber + 1);
        }

        /// <summary>
        /// Swaps the positions of two levels
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        private void SwapLevels(int a, int b)
        {
            // Get levels
            Level levelA = GetLevel(a);
            Level levelB = GetLevel(b);

            // If levels were retrieved successfully
            if (levelA != null && levelB != null)
            {
                // Move levelB to levelA's original position
                levels[a] = levelB;

                // Move levelA to levelB's original position
                levels[b] = levelA;
            }
        }

        /// <summary>
        /// Searches for a level in this level group and returns its number
        /// </summary>
        /// <param name="level"></param>
        /// <returns>The level's number (0-based), or -1 if level was not found</returns>
        public int GetLevelNumber(Level level)
        {
            // Each level
            if (level != null)
            {
                for (int i = 0; i < levels.Count; i++)
                {
                    // Get level
                    Level test = levels[i];

                    // Compare level to parameter
                    if (level.Equals(test))
                    {
                        // Return index if levels match
                        return i;
                    }
                }
            }

            // Level not found
            return -1;
        }

        /// <summary>
        /// Specifies whether there is room for another level
        /// </summary>
        /// <returns>true if there is room, otherwise false</returns>
        public bool HasCapacity()
        {
            if (levels.Count < MaxLevels)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public Level GetLevel(int number)
        {
            try
            {
                return levels[number];
            }
            catch (ArgumentOutOfRangeException)
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="binary"></param>
        /// <param name="tilesOnly"></param>
        public void CompileForLevelPack(BinaryEditor binary, bool tilesOnly)
        {
            // Marker for beginning of directory
            binary.Append("DIR");

            // Number of levels in group
            binary.Append((byte)levels.Count);

            // Each level
            foreach (Level level in levels)
            {
                // Level marker
                binary.Append("LEV ");

                // Address of last byte (set later)
                uint sizeAddr = binary.getSize();
                binary.Append(0);

                // Compiled level
                if (!tilesOnly)
                    level.CompileVsrData(binary);
                else
                    level.CompileTiles(binary);

                // Set address of last byte at sizeAddr pointer
                //binary.SetAddressPointer(sizeAddr, binary.getPosition() - 1 - sizeAddr - 4);
                binary.Set(sizeAddr, binary.getSize() - (sizeAddr + 4));
            }
        }

        /// <summary>
        /// Fills a list box with the names of each level
        /// </summary>
        /// <param name="list"></param>
        public void GetLevelList (ListBox list)
        {
            // Clear list
            list.Items.Clear();

            // Add names of each level to the list
            for (int i = 0; i < levels.Count; i++)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("(" + i + ") ");
                sb.Append(levels[i].Name);
                list.Items.Add(sb.ToString());
            }

        }
    }
}
