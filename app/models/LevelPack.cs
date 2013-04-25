using System;
using System.Collections.Generic;
using System.Text;
using LemballEditor.Model;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using VsrCompiler;

namespace LemballEditor.Model
{
    /// <summary>
    /// A user-made collection of levels
    /// </summary>
    public class LevelPack
    {
        /// <summary>
        /// An array of level groups
        /// </summary>
        private LevelGroup[] levelGroups;

        /// <summary>
        /// True if the level pack is for the full version of Lemmings Paintball
        /// </summary>
        //public bool FullVersion { get; set; }

        /// <summary>
        /// The name of the person who made the level pack
        /// </summary>
        //private String authorName;
        
        /// <summary>
        /// Creates a new level pack
        /// </summary>
        public LevelPack()
        {
            //FullVersion = fullVersion;

            // Fun, Tricky, Taxing, Mayhem, Network
            levelGroups = new LevelGroup[5];

            for (int i = 0; i < 5; i++)
            {
                levelGroups[i] = new LevelGroup((LevelGroupTypes)i);
            }

            //authorName = "";
        }

        /// <summary>
        /// Initialises a level pack from the data in a level pack file
        /// </summary>
        /// <param name="reader"></param>
        public LevelPack (BinaryReader reader)
        {
            // Check magic number to ensure it is a project file
            String header = new String(reader.ReadChars(4));

            if (header != "LPPR")
            {
                // If it is a compiled level pack
                if (header == "LPLP")
                    throw new InvalidDataException("This is a compiled level pack, and cannot be opened for re-editing");
                else
                    throw new InvalidDataException("This is not a Lemball Editor project file");
            }

            // Ensure project version is not higher than the program version
            if (Program.CompareVersion(reader.ReadBytes(4)) > 0)
                throw new InvalidDataException("This project file requires a newer version of Lemball Editor to open");

            // Extract xml data
            byte[] xml = ExtractNextBlock(reader);

            using (MemoryStream xmlStream = new MemoryStream(xml))
            {
                // Extract tile data
                byte[] tiles = ExtractNextBlock(reader);

                using (MemoryStream tileStream = new MemoryStream(tiles))
                {
                    BinaryReader tilesReader = new BinaryReader(tileStream);

                    // Load XML data
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(xmlStream);
                    XmlElement root = xmlDoc.DocumentElement;

                    // Determine whether the level pack is for the full version of Lemball or the demo version
                    /*
                    if (root.GetAttribute("full_version").ToLower() == "true")
                        FullVersion = true;
                    else
                        FullVersion = false;
                    */

                    // Check number of level groups
                    XmlNodeList groups = root.SelectNodes("group");
                    if (groups.Count != 5)
                        throw new InvalidDataException("XML data error: invalid number of level groups");

                    // Load each level group
                    levelGroups = new LevelGroup[5];
                    for (int i = 0; i < 5; i++)
                    {
                        XmlElement group = (XmlElement)groups.Item(i);
                        levelGroups[i] = new LevelGroup(group, tilesReader);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="group">The group type</param>
        /// <param name="levelNumber"></param>
        /// <returns></returns>
        public Level GetLevel(LevelGroupTypes group, int levelNumber)
        {
            return GetLevelGroup(group).GetLevel(levelNumber);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public void CreateNewLevel(LevelGroupTypes group)
        {
            CreateNewLevel(group, new Level(Level.TerrainTypes.Grass));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="level">The level to move</param>
        /// <param name="group">The level group to move the level to</param>
        public void MoveLevelToLevelGroup(Level level, LevelGroupTypes moveToGroup)
        {
            // Get the destination group
            LevelGroup dest = GetLevelGroup(moveToGroup);

            // If the destination group has capacity
            if (dest.HasCapacity())
            {
                // Delete level from 
                foreach (LevelGroup group in levelGroups)
                {
                    // Delete the level from the group if it exists in the group
                    group.DeleteLevel(level);
                }

                // Add the level to the destination group
                dest.AppendLevel(level);
            }
                // If the destination group has run out of capacity
            else
            {
                throw new LevelGroupFullException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="group"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public void CreateNewLevel(LevelGroupTypes group, Level level)
        {
            try
            {
                // Attempt to append level to the level group
                GetLevelGroup(group).AppendLevel(level);
                Program.OnLevelListChange();
            }
            catch (LevelGroupFullException e)
            {
                // Level group was full
                throw e;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="group"></param>
        /// <param name="levelNumber"></param>
        public void DeleteLevel(LevelGroupTypes group, int levelNumber)
        {
            GetLevelGroup(group).DeleteLevel(levelNumber);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="group"></param>
        /// <param name="levelNumber"></param>
        public void MoveLevelUp(LevelGroupTypes group, int levelNumber)
        {
            GetLevelGroup(group).MoveLevelUp(levelNumber);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="group"></param>
        /// <param name="levelNumber"></param>
        public void MoveLevelDown(LevelGroupTypes group, int levelNumber)
        {
            GetLevelGroup(group).MoveLevelDown(levelNumber);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="level"></param>
        /// <param name="group">The group to copy the level to</param>
        /// <returns></returns>
        public void CopyLevel(Level level, LevelGroupTypes group)
        {
            // Create a copy of the level
            Level copy = level.CreateCopy();

            // Append the copy to the specified level group
            GetLevelGroup(group).AppendLevel(copy);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public LevelGroupTypes? GetLevelGroupType(Level level)
        {
            // Search each level group
            foreach (LevelGroup group in levelGroups)
            {
                // Search group for level
                int number = group.GetLevelNumber(level);
                
                // If level was found, return the group type
                if (number != -1)
                {
                    return group.Group;
                }
            }

            // Level not found
            return null;
        }

        public bool LevelGroupHasCapacity(LevelGroupTypes group)
        {
            return GetLevelGroup(group).HasCapacity();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public int GetLevelNumber(Level level)
        {
            // Search each level group
            foreach (LevelGroup group in levelGroups)
            {
                // Search group for level
                int number = group.GetLevelNumber(level);

                // If level was found, return its number
                if (number != -1)
                {
                    return number;
                }
            }

            // Level not found
            return -1;
        }

        /// <summary>
        /// Retrieves a level group based on its number (0 to 4)
        /// </summary>
        /// <param name="levelGroupType"></param>
        /// <returns></returns>
        private LevelGroup GetLevelGroup(LevelGroupTypes levelGroupType)
        {
            switch (levelGroupType)
            {
                case LevelGroupTypes.Fun:
                    return levelGroups[0];
                case LevelGroupTypes.Tricky:
                    return levelGroups[1];
                case LevelGroupTypes.Taxing:
                    return levelGroups[2];
                case LevelGroupTypes.Mayhem:
                    return levelGroups[3];
                case LevelGroupTypes.Network:
                    return levelGroups[4];
            }

            // Invalid levelGroup type
            return null;
        }

        /// <summary>
        /// Compiles the Lembedit project file, which allows further editing in future
        /// </summary>
        public void SaveProjectFile(String filePath)
        {
            using (MemoryStream memStream = new MemoryStream())
            {
                // Create binary editor to store the compiled level pack
                BinaryEditor binary = new BinaryEditor(memStream);

                // Header (Lemmings Paintball project)
                binary.Append("LPPR");

                // Lemball Editor version
                binary.Append(Program.GetVersion());

                // Compile XML block
                CompileDataBlock(binary, CompileXMLData(), true);

                // Compile tile block
                CompileDataBlock(binary, CompileLevelTiles(false), true);

                // Return binary editor
                binary.SaveToFile(filePath);

                // Save project data
                //binary.SaveToFile(@".\project.dat");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="binary"></param>
        /// <param name="data"></param>
        /// <param name="compress"></param>
        private void CompileDataBlock (BinaryEditor binary, byte[] data, bool compress)
        {
            // Compile compressed data block
            if (compress)
            {
                // Compress data
                byte[] compressed = BinaryEditor.Compress(data);

                // Append size of uncompressed data (uint32)
                binary.Append(data.Length);

                // Append size of compressed data (uint32)
                binary.Append(compressed.Length);

                // Append compressed data
                binary.Append(compressed);
            }
            // Don't compress data
            else
            {
                // Compressed size (0 = not compressed)
                binary.Append(0);

                // Data block size
                binary.Append(data.Length);

                // Data
                binary.Append(data);
            }

            // Append END tag
            binary.Append(" END");
        }

        /// <summary>
        /// Extracts a data block from a project file
        /// </summary>
        /// <param name="reader">The reader reading the data. Should be positioned at the beginning of the block (it's size value)</param>
        /// <returns>A byte array containing the uncompressed data</returns>
        private byte[] ExtractNextBlock(BinaryReader reader)
        {
            // Read original size and compressed size
            int uncompressedSize = reader.ReadInt32();
            int dataSize = reader.ReadInt32();

            // Extract data
            byte[] data = reader.ReadBytes(dataSize);

            if (new String(reader.ReadChars(4)) != " END")
                throw new InvalidDataException();

            // If data is compressed
            if (uncompressedSize > 0)
            {
                // Decompress data
                BinaryEditor.Decompress(ref data, uncompressedSize);
            }

            // Return extracted data
            return data;
        }

        /// <summary>
        /// Compiles the level pack into an XML file. The XML data does not include the level tiles.
        /// </summary>
        /// <returns></returns>
        public byte[] CompileXMLData()
        {
            // Create XML document
            XmlDocument xmlDoc = new XmlDocument();

            // Create root node
            XmlElement root = xmlDoc.CreateElement("level_pack");
            //root.SetAttribute("lembedit_version", Program.GetVersionString());
            //root.SetAttribute("full_version", FullVersion.ToString());
            xmlDoc.AppendChild(root);

            // Add each level group
            for (int groupNumber = 0; groupNumber < 5; groupNumber++)
            {
                LevelGroup levelGroup = levelGroups[groupNumber];

                // Append levelGroup node to root
                root.AppendChild(levelGroup.CompileXML(xmlDoc, groupNumber));
            }

            // Save uncompressed XML data to separate file (debug)
            if (Program.DebugMode)
            {
                using (FileStream file = new FileStream(@".\levelXML.xml", FileMode.Create))
                {
                    xmlDoc.Save(file);
                }
            }

            // Compress XML data and return as a byte array
            using (MemoryStream memStream = new MemoryStream())
            {
                // Save XML data to memory stream
                xmlDoc.Save(memStream);

                // Extract byte array from memory stream
                byte[] data = memStream.ToArray();

                // Compress data
                //BinaryEditor.Compress(ref data);
                
                // Return XML data
                return data;
            }
        }

        /// <summary>
        /// Compiles the levels into Lemball level files, and stores then in a LevelPack file
        /// that can be released. The LevelPack file can be read by the LevelPack selector,
        /// which compiles the compiled level files into the VSR file that Lemball reads.
        /// </summary>
        public MemoryStream Compile(bool compress)
        {
            // Compile the level data
            byte[] levelData = CompileLevelData(false, false);

            // Remember uncompressed size
            int uncompressedSize = levelData.Length;

            // Compress level data
            if (compress)
                levelData = BinaryEditor.Compress(levelData);

            // Create level pack stream
            BinaryEditor levelPack = new BinaryEditor(new MemoryStream());

            // Header
            levelPack.Append("LPLP");

            // Level pack format version
            levelPack.Append((short)0);

            // Null
            levelPack.Append((short)0);

            // Lemball Editor version that saved the file
            levelPack.Append(Program.GetVersion());

            // Size of uncompressed level data (0 if not compressed)
            if (compress)
                levelPack.Append(uncompressedSize);
            else
                levelPack.Append(0);

            // Size of data chunk (if compressed, its compressed size)
            levelPack.Append(levelData.Length);

            // Append level data
            levelPack.Append(levelData);
            levelPack.Append(" END");

            // Save level pack (debug)
            if (Program.DebugMode)
                levelPack.SaveToFile(@".\levelPack.dat");

            // Return memory stream
            return levelPack.getStream();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="compress"></param>
        /// <param name="tilesOnly">If true, only the tile portion of each level is compiled, otherwise all the level is compiled</param>
        /// <returns>byte array containing compiled data</returns>
        private byte[] CompileLevelData(bool compress, bool tilesOnly)
        {
            // Create a stream editor to create the level data
            using (MemoryStream memStream = new MemoryStream(10485760))
            {
                BinaryEditor levelDataStream = new BinaryEditor(memStream);

                // Compile each level directory
                for (int i = 0; i != 5; i++)
                {
                    levelGroups[i].CompileForLevelPack(levelDataStream, tilesOnly);
                }

                // Save tile data (Debug)
                // levelDataStream.SaveToFile(@".\projectTileData.dat");

                // Get level data as a byte array
                byte[] levelData;
                if (compress)
                    levelData = levelDataStream.GetCompressedData();
                else
                    levelData = levelDataStream.GetData();

                // Return the level data
                return levelData;
            }
        }

        /// <summary>
        /// Compiles the tile data of the levels
        /// </summary>
        /// <param name="compress"></param>
        /// <returns></returns>
        private byte[] CompileLevelTiles(bool compress)
        {
            return CompileLevelData(compress, true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="levelGroup"></param>
        /// <param name="list"></param>
        public void LoadLevelList(LevelGroupTypes levelGroup, ListBox list)
        {
            GetLevelGroup(levelGroup).GetLevelList(list);
        }


    }
}