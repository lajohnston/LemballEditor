using LemballEditor.View;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using VsrCompiler;

namespace LemballEditor.Model
{
    /// <summary>
    /// A user-made level
    /// </summary>
    public partial class Level
    {
        /// <summary>
        /// Stores the map (an array of MapTiles)
        /// </summary>
        private MapTile[,] levelTiles;

        /// <summary>
        /// The number of x tiles in the map (number of tiles from top of isometric diamond to right point)
        /// </summary>
        public int MapSizeX => levelTiles.GetUpperBound(0) + 1;

        /// <summary>
        /// The number of y tiles in the map (tiles from the top of the isometric diamond to the left point)
        /// </summary>
        public int MapSizeY => levelTiles.GetUpperBound(1) + 1;

        /// <summary>
        /// Stores all the in-game objects in the level
        /// </summary>
        private List<LevelObject> levelObjects;
        public LevelObject[] LevelObjects => levelObjects.ToArray();

        /// <summary>
        /// Returns the number of objects that can be added to the level
        /// before the limit is reached
        /// </summary>
        /// <returns></returns>
        public int ObjectLimitRemaining
        {
            get
            {
                // Count the number of objects that exist
                int count = 0;
                foreach (LevelObject gameObject in levelObjects)
                {
                    if (gameObject.IsIncludedInObjectLimit())
                    {
                        count++;
                    }
                }

                // Return the number of objects left within the limit
                return MAX_OBJECTS - count;
            }
        }

        /// <summary>
        /// The level's terrain type
        /// </summary>
        public TerrainTypes TerrainType { get; set; }

        /// <summary>
        /// The level's time limit in seconds
        /// </summary>
        public ushort TimeLimit { get; set; }

        /// <summary>
        /// The number of flags the player needs to collect to complete the level
        /// </summary>
        public FlagsRequired NumberOfFlagsRequiredToWin { get; set; }

        public enum FlagsRequired
        {
            /// <summary>
            /// All flags are required (maximum of 4)
            /// </summary>
            All,
            /// <summary>
            /// One flag is required to complete the level
            /// </summary>
            One,
            /// <summary>
            /// Two flags are required to complete the level
            /// </summary>
            Two,
            /// <summary>
            /// Three flags are required to complete the level
            /// </summary>
            Three,
            /// <summary>
            /// Four flags are required to complete the level
            /// </summary>
            Four,
            /// <summary>
            /// The level cannot be won by collecting flags
            /// </summary>
            None
        }

        /// <summary>
        /// The level's name
        /// </summary>
        private string name;
        public string Name
        {
            get => name;
            set
            {
                if (value.Length == 0 || value.Length > 31)
                {
                    throw new ArgumentException();
                }

                name = value;
            }
        }

        /// <summary>
        /// The maximum number of in-game objects that each level can have
        /// </summary>
        private const byte MAX_OBJECTS = 190;

        /// <summary>
        /// The maximum number of tiles (xTiles * yTiles) that levels can have
        /// </summary>
        private const short MAX_TILES = 19600;

        /// <summary>
        /// The maximum size of each map dimension (i.e. number of xTiles or number of yTiles)
        /// </summary>
        private const short MAX_TILE_DIMENSION = 600;

        /// <summary>
        /// The next free id that should be assigned to a new object
        /// </summary>
        private ushort FreeId
        {
            get
            {
                // Search each object until a free id value is found
                for (ushort id = 0; id < ushort.MaxValue; id++)
                {
                    // If object with the id doesn't exist, return the id
                    if (GetObject(id) == null)
                    {
                        return id;
                    }
                }

                // No free ids were found
                throw new OverflowException("No more object ids available");
            }
        }

        /// <summary>
        /// Counts the number of Lemmings in the level. Iterates through each entrance and returns
        /// the total sum of Lemmings that they release.
        /// </summary>
        /// <returns></returns>
        public int NumberOfLemmings
        {
            get
            {
                int count = 0;
                foreach (LevelObject gameObject in levelObjects)
                {
                    if (gameObject is Entrance)
                    {
                        count += ((Entrance)gameObject).NumberOfLemmings;
                    }
                }

                return count;
            }
        }

        /// <summary>
        /// The types of terrain
        /// </summary>
        public enum TerrainTypes
        {
            Grass = 0,
            Lego = 1,
            Snow = 2,
            Space = 3
        }

        public Level()
            : this(TerrainTypes.Grass)
        {

        }

        /// <summary>
        /// Creates a new level
        /// </summary>
        /// <param name="terrainType">The terrain type of the level</param>
        public Level(TerrainTypes terrainType)
        {
            TerrainType = TerrainTypes.Grass;

            // Create a blank map
            CreateNewMap(64, 64, 521);

            levelObjects = new List<LevelObject>();

            TimeLimit = 600;
            NumberOfFlagsRequiredToWin = FlagsRequired.None;
            Name = "Level";
        }

        /// <summary>
        /// Loads a level from level XML data
        /// </summary>
        /// <param name="xmlData">The XML data</param>
        /// <param name="tileData">The tile data</param>
        public Level(XmlElement xmlData, byte[] tileData)
        {
            try
            {
                // Load level values
                name = xmlData.GetAttribute("name");
                SetTimeLimit(Convert.ToUInt16(xmlData.GetAttribute("time_limit")));
                NumberOfFlagsRequiredToWin = (FlagsRequired)Enum.Parse(typeof(FlagsRequired), xmlData.GetAttribute("flags_required"));
                TerrainType = (TerrainTypes)Enum.Parse(typeof(TerrainTypes), xmlData.GetAttribute("terrain"), true);

                // Load tiles
                int xTiles = Convert.ToInt32(xmlData.GetAttribute("xTiles"));
                int yTiles = Convert.ToInt32(xmlData.GetAttribute("yTiles"));
                LoadTiles(tileData, xTiles, yTiles);
            }
            catch (Exception e)
            {
                throw new InvalidDataException("Invalid XML level data: " + e.Message);
            }

            // Load each object
            LoadObjects((XmlElement)xmlData.SelectSingleNode("objects"));
        }

        /// <summary>
        /// Compiles the level into an XML file
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <returns></returns>
        public XmlElement CompileXML(XmlDocument xmlDoc)
        {
            // Create level element
            XmlElement level = xmlDoc.CreateElement("level");

            // Set attributes
            level.SetAttribute("name", name);
            level.SetAttribute("time_limit", TimeLimit.ToString());
            level.SetAttribute("flags_required", NumberOfFlagsRequiredToWin.ToString());
            level.SetAttribute("terrain", TerrainType.ToString());

            // Number of tiles
            level.SetAttribute("xTiles", MapSizeX.ToString());
            level.SetAttribute("yTiles", MapSizeY.ToString());

            // Compile objects
            XmlElement objectsNode = xmlDoc.CreateElement("objects");
            foreach (LevelObject gameObject in levelObjects)
            {
                _ = xmlDoc.CreateElement("object");
                _ = objectsNode.AppendChild(gameObject.CompileXml(xmlDoc));
            }

            // Append objects node
            _ = level.AppendChild(objectsNode);

            // Return xml data
            return level;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public LevelObject GetObject(int id)
        {
            // Iterate through each object to find the object with the specified id
            foreach (LevelObject levelObject in levelObjects)
            {
                // If the object's id matches the paramter, return the object
                if (levelObject.Id == id)
                {
                    return levelObject;
                }
            }

            // Object wasn't found
            return null;
        }

        /// <summary>
        /// Adds an object to the level. Doesn't enforce max object count.
        /// </summary>
        /// <param name="newObject">The object to add</param>
        public void AddObject(LevelObject newObject)
        {
            // Add the object to the objects list
            levelObjects.Add(newObject);

            // Set the object's id
            newObject.Id = FreeId;

            // If the object is a balloon
            if (newObject is Balloon)
            {
                OnBalloonCreation((Balloon)newObject);
            }

            // Update object limit counter
            MainInterface.OnObjectCountChange();
        }

        /// <summary>
        /// Called when a balloon has been created
        /// </summary>
        /// <param name="balloon"></param>
        private void OnBalloonCreation(Balloon balloon)
        {
            // If post doesn't exist, create a new one
            if (GetBalloonPost(levelObjects, balloon.Colour) == null)
            {
                BalloonPost post = balloon.CreatePost(FreeId);
                levelObjects.Add(post);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameObjects"></param>
        /// <param name="colour"></param>
        /// <returns></returns>
        private BalloonPost GetBalloonPost(List<LevelObject> gameObjects, Balloon.Colours colour)
        {
            foreach (LevelObject gameObject in gameObjects)
            {
                if (gameObject is BalloonPost post)
                {
                    if (post.Colour == colour)
                    {
                        return post;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Returns the objects that are connected to the specified switch
        /// </summary>
        /// <param name="switchObject"></param>
        /// <returns></returns>
        public List<LevelObject> GetObjectsConnectedToSwitch(Switch switchObject)
        {
            // Get idrefs
            int[] idrefs = switchObject.ConnectedIdrefs;

            // Create list to hold objects
            List<LevelObject> connectedObjects = new List<LevelObject>(idrefs.Length);

            // Get object from idref
            foreach (int idref in idrefs)
            {
                connectedObjects.Add(GetObject(idref));
            }

            // Return connected objects
            return connectedObjects;
        }

        /// <summary>
        /// Counts the number of balloons with the specified colour. Used when
        /// determining whether a balloon post can be deleted.
        /// </summary>
        /// <param name="colour"></param>
        /// <returns></returns>
        public int CountBalloons(Balloon.Colours colour)
        {
            int count = 0;
            foreach (LevelObject gameObject in levelObjects)
            {
                if (gameObject is Balloon)
                {
                    if (((Balloon)gameObject).Colour == colour)
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        /// <summary>
        /// Deletes an object from the level
        /// </summary>
        /// <param name="gameObject">The object to delete</param>
        public void DeleteObject(LevelObject gameObject)
        {
            // Remove object
            _ = levelObjects.Remove(gameObject);

            // Inform switches and pressure pads that object has been deleted
            foreach (LevelObject levelObject in levelObjects)
            {
                if (levelObject is Switch)
                {
                    ((Switch)levelObject).DeleteConnection(gameObject.Id);
                }
            }

            // Notify GUI
            Program.MainInterface.OnObjectDeletion();
        }


        /// <summary>
        /// Returns a list of objects that should be drawn just after a specific tile has been drawn
        /// </summary>
        /// <param name="tileCoordinate">The coordinate to check</param>
        /// <returns>A list of every object that is on the specified tile</returns>
        public List<LevelObject> GetObjectsOverlapingTile(TileCoordinate tileCoordinate)
        {
            // Create object list
            List<LevelObject> objectsOnTile = new List<LevelObject>();

            // Get list of all objects on the tile
            foreach (LevelObject gameObject in levelObjects)
            {
                if (gameObject.OverlapsTile(tileCoordinate))
                {
                    objectsOnTile.Add(gameObject);
                }
            }

            // To do: order objects based on position

            // Return list of objects
            return objectsOnTile;
        }

        /// <summary>
        /// Returns a list of objects that should be drawn just after a specific tile has been drawn
        /// </summary>
        /// <param name="tileCoordinate">The coordinate to check</param>
        /// <returns>A list of every object that is on the specified tile</returns>
        public List<LevelObject> OldGetObjectsOverlapingTile(TileCoordinate tileCoordinate)
        {
            // Create object list
            List<LevelObject> objectsOnTile = new List<LevelObject>();

            // Get list of all objects on the tile
            foreach (LevelObject gameObject in levelObjects)
            {
                if (gameObject.OverlapsTile(tileCoordinate))
                {
                    objectsOnTile.Add(gameObject);
                }
            }

            // To do: order objects based on position

            // Return list of objects
            return objectsOnTile;
        }

        /// <summary>
        /// Creates and returns a copy of the level that can be edited independently
        /// </summary>
        /// <returns></returns>
        public Level CreateCopy()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement levelXml = CompileXML(xmlDoc);

            using (MemoryStream memStream = new MemoryStream())
            {
                BinaryEditor binary = new BinaryEditor(memStream);
                LevelCompiler.CompileTiles(binary, this);
                return new Level(levelXml, binary.GetData());
            }
        }

        /// <summary>
        /// Loads objects from a list of XML objects
        /// </summary>
        /// <param name="objectsNode">The XML element that contains the objects</param>
        private void LoadObjects(XmlElement objectsNode)
        {
            // Create list of objects
            levelObjects = new List<LevelObject>();

            foreach (XmlElement objectElement in objectsNode)
            {
                LevelObject loadedObject = LevelObject.New(objectElement);

                if (loadedObject == null)
                {
                    throw new InvalidDataException("Invalid object");
                }

                levelObjects.Add(loadedObject);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tileData"></param>
        /// <param name="xTiles"></param>
        /// <param name="yTiles"></param>
        private void LoadTiles(byte[] tileData, int xTiles, int yTiles)
        {
            // Ensure tile file size matches the size of the xTile and yTile attributes
            if (xTiles * yTiles * 6 != tileData.Length)
            {
                throw new InvalidDataException("Invalid tile file size");
            }

            // Open streams to read tile data
            using (MemoryStream tileStream = new MemoryStream(tileData))
            {
                using (BinaryReader reader = new BinaryReader(tileStream))
                {
                    // Initialise map array
                    levelTiles = new MapTile[xTiles, yTiles];

                    // Load each tile
                    for (int yTile = 0; yTile < yTiles; yTile++)
                    {
                        for (int xTile = 0; xTile < xTiles; xTile++)
                        {
                            uint tileRef = reader.ReadUInt32();
                            byte elevation = (byte)reader.ReadUInt16();

                            levelTiles[xTile, yTile] = new MapTile(tileRef, elevation);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void SetTimeLimit(ushort value)
        {
            // If the time limit is equal or above 10 minutes
            TimeLimit = value >= 600 ? (ushort)600 : value;
        }

        public bool HasUnlimitedTimeLimit()
        {
            // If timeLimit = 600, return true, otherwise return false
            return TimeLimit == 600;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xTile"></param>
        /// <param name="yTile"></param>
        /// <returns></returns>
        private MapTile GetTile(ushort xTile, ushort yTile)
        {
            return xTile >= MapSizeX || yTile >= MapSizeY ? null : levelTiles[xTile, yTile];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="coordinate"></param>
        /// <returns></returns>
        private MapTile GetTile(TileCoordinate coordinate)
        {
            return coordinate == null ? null : GetTile(coordinate.xTile, coordinate.yTile);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tile"></param>
        /// <returns></returns>
        public uint GetTileImageRef(TileCoordinate tile)
        {
            return GetTile(tile).TileImageRef;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tile"></param>
        /// <returns>True if the image was changed, otherwise false if the tile already had the specified imageRef</returns>
        public bool ChangeTileImageRef(TileCoordinate tile, uint imageRef)
        {
            MapTile mapTile = GetTile(tile);

            // If the tile doesn't exist or already has the specified imageRef
            if (mapTile == null || mapTile.TileImageRef == imageRef)
            {
                return false;
            }
            else
            {
                // Change the tile's imageRef and return true
                mapTile.TileImageRef = imageRef;
                return true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tile"></param>
        /// <returns></returns>
        public int GetTileElevation(TileCoordinate tile)
        {
            // Attempt to fetch the maptile with the specified coordinate
            MapTile mapTile = GetTile(tile);

            // If maptile doesn't exists, return 0, otherwise return it's elevation
            return mapTile == null ? 0 : mapTile.Elevation;
        }

        /// <summary>
        /// Determines whether the specified tile can have its elevation altered by the given amount of pixels
        /// </summary>
        /// <param name="tile"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public bool CanAlterElevationOfTile(TileCoordinate tile, int amount)
        {
            return GetTile(tile).CanAlterElevation(amount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tile"></param>
        /// <param name="amount"></param>
        public void AlterTileElevation(TileCoordinate tile, int amount)
        {
            GetTile(tile).AlterElevation(amount);
        }

        /// <summary>
        /// 
        /// </summary>
        public void SetTileElevation(TileCoordinate tile, byte elevation)
        {
            GetTile(tile).Elevation = elevation;
        }

        /// <summary>
        /// Creates a new map and fills it with the specified tile image reference
        /// </summary>
        /// <param name="terrain">The terrain type of the map</param>
        /// <param name="xSize">The X size of the map</param>
        /// <param name="ySize">The Y size of the map</param>
        /// <param name="tileRef">The tileRef of the tile to fill the map with</param>
        private void CreateNewMap(ushort xSize, ushort ySize, uint tileRef)
        {
            // Check number of tiles
            if (xSize * ySize > MAX_TILES)
            {
                throw new Exception("Max tiles exceeded");
            }

            // Check tile dimension
            if (xSize > MAX_TILE_DIMENSION || ySize > MAX_TILE_DIMENSION)
            {
                throw new ArgumentOutOfRangeException();
            }


            // Map array is an array of MapTiles
            levelTiles = new MapTile[xSize, ySize];

            // Each row
            for (int y = 0; y != ySize; ++y)
            {
                //Each cell
                for (int x = 0; x != xSize; ++x)
                {
                    levelTiles[x, y] = new MapTile(tileRef, 0);
                }
            }
        }

        /// <summary>
        /// Compiles the level into a Lemmings Paintball compatible level file
        /// </summary>
        /// <param name="binary"></param>
        public void CompileVsrData(BinaryEditor binary)
        {
            LevelCompiler.Compile(binary, this);
        }

        public void CompileTiles(BinaryEditor binary)
        {
            LevelCompiler.CompileTiles(binary, this);
        }

        /// <summary>
        /// Counts the number of player one (blue) flags in the level
        /// </summary>
        /// <returns></returns>
        public int CountPlayerOneFlags()
        {
            // Number of flags counted
            short count = 0;

            // Each object
            foreach (LevelObject levelObject in levelObjects)
            {
                // If object is a flag and belongs to playerOne, increment count
                if (levelObject is Flag && ((Flag)levelObject).isPlayerOne())
                {
                    count++;
                }
            }

            // Return number of flags
            return count;
        }
    }
}