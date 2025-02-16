using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Xml;

namespace LemballEditor.View.Level
{
    /// <summary>
    /// Loads tile images from an XML cache
    /// </summary>
    internal static class ImageCache
    {
        /// <summary>
        /// The current level terrain
        /// </summary>
        private static Model.Level.TerrainTypes terrain;

        /// <summary>
        /// The XML tile archive
        /// </summary>
        private static readonly XmlDocument tileArchive;

        /// <summary>
        /// The node containing the tiles for the current terrain
        /// </summary>
        private static XmlElement terrainNode;

        /// <summary>
        /// The bitmap that is used to show elevation in the current terrain type
        /// </summary>
        private static Bitmap terrainElevationBitmap;

        /// <summary>
        /// Null tile
        /// </summary>
        private static readonly TileImage nullTile;

        /// <summary>
        /// A placeholder bitmap used for object images that could not be loaded or do not exist
        /// </summary>
        public static Bitmap NullObjectImage => new Bitmap(LemballEditor.Properties.Resources.nullObjectImage);

        /// <summary>
        /// Different types of tile that exist within the game
        /// </summary>
        public enum TileTypes
        {
            /// <summary>
            /// Accessible to Lemmings
            /// </summary>
            Land,
            /// <summary>
            /// Land tiles that are inaccessible to Lemmings
            /// </summary>
            //OutOfBounds,
            /// <summary>
            /// Tree tile
            /// </summary>
            Plant,
            /// <summary>
            /// The edge of a mud track path
            /// </summary>
            PathEdge,
            /// <summary>
            /// River and sea tiles
            /// </summary>
            Water,
            /// <summary>
            /// A sloping tile
            /// </summary>
            Slope,
            /// <summary>
            /// An animated tile
            /// </summary>
            //Animated,
            /// <summary>
            /// Tiles that kill Lemmings, such as lava and electricity
            /// </summary>
            Hazard,
            /// <summary>
            /// A rock
            /// </summary>
            Rock,
            /// <summary>
            /// A fence tile, which is used to block off an out of bounds area
            /// </summary>
            Fence,
            /// <summary>
            /// An elevated tile
            /// </summary>
            Elevated,
            /// <summary>
            /// An accesible land tile that has a section of beach or water in it
            /// </summary>
            Shore,
            /// <summary>
            /// The tile hasn't yet been categorised
            /// </summary>
            //None,
            /// <summary>
            /// A tile that doesn't fit into other categories (the 'None' value is for tiles that haven't
            /// been assessed yet).
            /// </summary>
            Other,
        }

        /// <summary>
        /// A hashtable that associates TileImage objects with their tileRef number
        /// </summary>
        private static readonly Hashtable cache;

        /// <summary>
        /// The static initialiser for the tile cache
        /// </summary>
        static ImageCache()
        {
            // Load tile cache XML data
            try
            {
                tileArchive = new XmlDocument();
                tileArchive.Load(@"..\..\assets\tiles.xml");
            }
            catch (Exception e)
            {
                throw new FileNotFoundException("There was an error loading the tile cache file: " + e.Message);
            }

            // Create the null tile
            nullTile = new TileImage();

            // Initialise hash table (tile cache)
            cache = new Hashtable(50);

            SetTerrainType(Model.Level.TerrainTypes.Grass);
        }

        /// <summary>
        /// Changes the terrain type that the cache handles
        /// </summary>
        /// <param name="terrainType"></param>
        public static void ChangeTerrainType(Model.Level.TerrainTypes newTerrain)
        {
            // If the new terrain type is different from the current one
            if (terrain != newTerrain)
            {
                SetTerrainType(newTerrain);
            }
        }

        /// <summary>
        /// Retrieves the tileRef numbers of the specified type.
        /// </summary>
        /// <param name="tileType">The type of tile to add to the list</param>
        /// <param name="tileRefs">The list to add the tileRefs to</param>
        public static void getTileRefs(TileTypes tileType, ref List<TileImage> tileRefs)
        {
            foreach (XmlNode tileNode in terrainNode)
            {
                if (tileNode is XmlElement && tileNode.Name == "tile")
                {
                    // Cast the tileNode as an XmlElement
                    XmlElement tileElement = (XmlElement)tileNode;

                    // Ensure that the tile is not hidden
                    if (tileElement.GetAttribute("hide") != "yes")
                    {
                        try
                        {
                            // Parse the 'type' attribute to a TileType
                            TileTypes type = (TileTypes)Enum.Parse(typeof(TileTypes), tileElement.GetAttribute("type"), true);

                            // If the tileType equals the parameter, add to the list
                            if (type == tileType)
                            {
                                //tileRefs.Add(Convert.ToUInt32(tileElement.GetAttribute("ref")));
                                uint tileRef = Convert.ToUInt32(tileElement.GetAttribute("ref"));
                                tileRefs.Add(loadTileFromXML(tileRef, false));
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Sets the terrain type, changes the XML tile terrain node and clears the cache
        /// </summary>
        /// <param name="newTerrain"></param>
        private static void SetTerrainType(Model.Level.TerrainTypes newTerrain)
        {
            terrain = newTerrain;
            terrainNode = (XmlElement)tileArchive.DocumentElement.ChildNodes[(int)newTerrain];
            cache.Clear();

            // Load elevation graphic
            XmlElement elevElement = (XmlElement)terrainNode.SelectSingleNode("elevation");
            terrainElevationBitmap = loadBitmap(elevElement);
        }

        /// <summary>
        /// Retrieves the tile with the specified tileRef. If addToCache is true and the tile is not in the cache, it is loaded
        /// from the cache then returned
        /// </summary>
        /// <param name="tileRef"></param>
        /// <param name="addToCache">Specified whether the tile should be kept in the cache when it is retrieved.</param>
        /// <returns></returns>
        public static TileImage getTile(uint tileRef, bool addToCache)
        {
            TileImage tile = (TileImage)cache[tileRef];

            if (tile == null)
            {
                tile = loadTileFromXML(tileRef, addToCache);
            }

            return tile;
        }

        /// <summary>
        /// Retrieves the tile with the specified tileRef. If the tile is not in the cache, it is
        /// added to it before being returned
        /// </summary>
        /// <param name="tileRef"></param>
        /// <returns></returns>
        public static TileImage getTile(uint tileRef)
        {
            // Return the tile, and add it to the cache
            return getTile(tileRef, true);
        }

        public static byte GetMaxElevation(uint tileRef)
        {
            return getTile(tileRef).getMaxElevation();
        }

        public static void DrawTile(Graphics g, uint tileRef, Point position)
        {
            getTile(tileRef).DrawTile(g, position);
        }



        public static Bitmap getElevationBitmap()
        {
            return terrainElevationBitmap;
        }

        public static Bitmap GetObjectImage(string name)
        {
            // Load tile cache XML data
            try
            {
                XmlDocument objects = new XmlDocument();
                objects.Load(@"..\..\assets\objects.xml");
                XmlElement element = (XmlElement)objects.DocumentElement.SelectSingleNode("image[@name=\"" + name + "\"]");

                return element != null ? loadBitmap(element) : NullObjectImage;
            }
            catch (Exception)
            {
                return NullObjectImage;
                //throw new FileNotFoundException("There was an error loading the object image cache:" + e.Message);
            }
        }

        /// <summary>
        /// Loads a tile from the XML cache
        /// </summary>
        /// <param name="tileRef"></param>
        /// <returns></returns>
        private static TileImage loadTileFromXML(uint tileRef, bool addToCache)
        {
            // Search for tile in the archive
            XmlElement tileNode = (XmlElement)terrainNode.SelectSingleNode("tile[@ref=" + tileRef + "]");

            if (tileNode != null)
            {
                TileImage tile = new TileImage(tileNode);

                if (addToCache)
                {
                    cache.Add(tileRef, tile);
                }

                return tile;
            }
            else
            {
                return nullTile;
            }
        }

        /// <summary>
        /// Loads the bitmap from the XML tile archive
        /// </summary>
        /// <returns></returns>
        public static Bitmap loadBitmap(XmlElement tileElement)
        {
            // Create a memory stream that will hold the tile data
            using (MemoryStream memStream = new MemoryStream(1024))
            {
                // Read the PNG data from the archive and return a bitmap image
                try
                {
                    string base64data = tileElement.FirstChild.Value;

                    // Convert base64 text into data
                    byte[] data = Convert.FromBase64String(base64data);
                    memStream.Write(data, 0, data.Length);

                    // Convert the data into a bitmap
                    Bitmap image = (Bitmap)Image.FromStream(memStream);

                    // Return the bitmap
                    return image;
                }
                catch (Exception)
                {
                    // If conversion fails, return null
                    return null;
                }
            }
        }
    }
}