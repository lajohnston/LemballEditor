using System;
using System.Collections.Generic;
using System.IO;
using VsrCompiler;

namespace LemballEditor.Model
{
    public partial class Level
    {
        private abstract class LevelCompiler
        {
            /// <summary>
            /// 
            /// </summary>
            private static BinaryEditor binary;

            /// <summary>
            /// 
            /// </summary>
            private static Level level;

            /// <summary>
            /// The next id that should be assigned to an object
            /// </summary>
            private static ushort nextId;


            /// <summary>
            /// 
            /// </summary>
            /// <param name="binary"></param>
            /// <param name="level"></param>
            public static void Compile(BinaryEditor binary, Level level)
            {
                LevelCompiler.binary = binary;
                LevelCompiler.level = level;

                /* Find highest static id. Some objects such as gates can be referenced by other objects such as switches,
                 * and so their id value should remain unchanged. The highest static id needs to found, and only values that are
                 * larger than this can be assigned to other objects, as objects (such as switches) that reference other objects 
                 * need higher ID values than the objects they reference.
                 */
                nextId = 0;
                foreach (LevelObject gameObject in level.levelObjects)
                {
                    if (gameObject.RequiresStaticId() && gameObject.Id >= nextId)
                    {
                        nextId = (ushort)(gameObject.Id + 1);
                    }
                }

                // Compiler header
                CompileHeader();

                // Compile tiles
                CompileTiles(binary, level);

                // Write object blocks
                CompileObjectBlocks();
            }

            /// <summary>
            /// Compiles the levels header
            /// </summary>
            /// <param name="binary"></param>
            private static void CompileHeader()
            {
                // Write header
                binary.Append("  IA");
                binary.Append(18);

                // Unknown (6, 7, 9 or 10)
                binary.Append((short)10);

                // Theme
                switch (level.TerrainType)
                {
                    case TerrainTypes.Grass:
                        binary.Append((short)0);
                        break;
                    case TerrainTypes.Lego:
                        binary.Append((short)1);
                        break;
                    case TerrainTypes.Snow:
                        binary.Append((short)2);
                        break;
                    case TerrainTypes.Space:
                        binary.Append((short)3);
                        break;
                }

                // Time limit
                binary.Append(level.TimeLimit);

                // Unknown
                binary.Append((short)1);

                // Flags required
                binary.Append(GetFlagsRequired());

                // Unknown
                binary.Append((short)0);

                // Always 'FSDG'
                binary.Append("FSDG");

                // Size of level tiles chunk
                binary.Append((level.MapSizeX * level.MapSizeY * 6) + 12);

                // Number of X and Y tiles
                binary.Append((short)level.MapSizeX);
                binary.Append((short)level.MapSizeY);
            }

            /// <summary>
            /// Compiles the level's tiles into a Lemmings Paintball compatible format
            /// </summary>
            /// <param name="binary"></param>
            public static void CompileTiles(BinaryEditor binaryEditor, Level level)
            {
                // Write tiles
                int xTiles = level.MapSizeX;
                int yTiles = level.MapSizeY;

                // Each tile
                for (ushort yTile = 0; yTile < yTiles; yTile++)
                {
                    for (ushort xTile = 0; xTile < xTiles; xTile++)
                    {
                        // Create tile coordinate
                        TileCoordinate tile = new TileCoordinate(xTile, yTile);

                        // Tile image ref
                        binaryEditor.Append(level.GetTileImageRef(tile));

                        // Tile elevation
                        binaryEditor.Append((short)level.GetTileElevation(tile));
                    }
                }

                // Pad to 4-bytes
                binaryEditor.Pad();
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            private static int GetNextId()
            {
                nextId++;
                return nextId;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="binary"></param>
            private static void CompileObjectBlocks()
            {
                // Compile each block that requires nodes and extract their nodes in the process
                List<Node> nodes = new List<Node>();

                // Compiles enemy block and extract nodes
                byte[] enemyBlock = null;
                using (MemoryStream memStream = new MemoryStream(500))
                {
                    BinaryEditor enemyBinary = new BinaryEditor(memStream);
                    CompileYMNEBlock(enemyBinary, nodes);
                    enemyBlock = enemyBinary.GetData();
                }

                // Compile EVOM block and extract nodes
                byte[] evomBlock = null;
                using (MemoryStream memStream = new MemoryStream(500))
                {
                    BinaryEditor platformBinary = new BinaryEditor(memStream);
                    CompileEVOMBlock(platformBinary, nodes);
                    evomBlock = platformBinary.GetData();
                }


                // Compile each block
                CompileBOMGBlock();
                binary.Append(enemyBlock);
                CompileEmptyBlock("GPHS");
                CompileEDONBlock(nodes);
                CompileEmptyBlock("LLAB");
                CompileENIMBlock();
                CompileLLOCBlock();
                CompileEmptyBlock("MINA");
                CompileTFILBlock();
                CompileROODBlock();
                CompileEmptyBlock("KCOR");
                CompileEmptyBlock("DNAH");
                CompileEmptyBlock("RSAL");
                CompileNOOBBlock();
                CompileEMANBlock();
                CompileEmptyBlock("MART");
                CompileEmptyBlock(" ECI");
                binary.Append(evomBlock);
                CompileEmptyBlock("NUGP");
                Compile1SLPBlock();
                CompileGALFBlock();
                CompileTFEDBlock();
                CompileEmptyBlock("KNLS");
                CompileEmptyBlock("SVNI");
                CompileEmptyBlock("WTEN");

                // End of level file
                binary.Append("?DNE");
            }

            /// <summary>
            /// Compiles the block that stores raising platforms
            /// </summary>
            private static void CompileTFILBlock()
            {
                // Header
                binary.Append("TFIL");

                // Get objects
                List<LevelObject> lifts = GetObjectBlockObjects(LevelObject.ObjectBlocks.TFIL);
                int numberOfLifts = lifts.Count;

                // Size
                binary.Append(10 + (numberOfLifts * 24));

                // Number of lifts
                binary.Append((ushort)numberOfLifts);

                // Compile each lifts
                foreach (Lift lift in lifts)
                {
                    lift.CompileVsrBinary(binary, level, null);
                }

                // Null
                binary.Append((short)0);
            }

            /// <summary>
            /// Compiles the block that stores gates/doors
            /// </summary>
            /// <param name="binary"></param>
            private static void CompileROODBlock()
            {
                //compileEmptyBlock(ref binary, "ROOD");

                // Header
                binary.Append("ROOD");

                // Get objects
                List<LevelObject> gates = GetObjectBlockObjects(LevelObject.ObjectBlocks.ROOD);
                int numberOfGates = gates.Count;

                // Size
                binary.Append(10 + (numberOfGates * 12));

                // Number of gates
                binary.Append((ushort)numberOfGates);

                // Compile each gate
                foreach (LevelObject gameObject in gates)
                {
                    gameObject.CompileVsrBinary(binary, level, null);
                }

                // Pad
                binary.Pad();
            }

            /// <summary>
            /// Compiles the block that stores movement nodes that are used by moving platforms and enemies
            /// </summary>
            /// <param name="binary"></param>
            /// <param name="nodes"></param>
            private static void CompileEDONBlock(List<Node> nodes)
            {
                // Header
                binary.Append("EDON");

                // Size (set later)
                uint sizePointer = binary.getSize();
                binary.Append(0);

                // Number of nodes
                uint numberOfNodes = (uint)nodes.Count;
                binary.Append((ushort)numberOfNodes);

                // Each node
                foreach (Node node in nodes)
                {
                    node.CompileVsrBinary(binary, level, null);
                }

                // Padding
                binary.Pad();

                // Set size
                uint size = 10 + (numberOfNodes * 6);
                binary.Set(sizePointer, size);
            }

            /// <summary>
            /// Compiles the block that stores moving platforms
            /// </summary>
            /// <param name="binary"></param>
            /// <param name="nodes"></param>
            private static void CompileEVOMBlock(BinaryEditor binary, List<Node> nodes)
            {
                //compileEmptyBlock(ref binary, "EVOM");

                // Header
                binary.Append("EVOM");

                // Get objects
                List<LevelObject> platforms = GetObjectBlockObjects(LevelObject.ObjectBlocks.EVOM);
                int numberOfPlatforms = platforms.Count;

                // Size
                binary.Append(10 + (numberOfPlatforms * 8));

                // Number of platforms
                binary.Append((ushort)numberOfPlatforms);

                // Compile each platform
                foreach (LevelObject gameObject in platforms)
                {
                    MovingPlatform platform = (MovingPlatform)gameObject;
                    platform.CompileObject(binary, nodes);
                }

                // Pad
                binary.Pad();
            }

            /// <summary>
            /// Compiles the block that stores enemies
            /// </summary>
            /// <param name="binary"></param>
            /// <param name="addedNodes"></param>
            private static void CompileYMNEBlock(BinaryEditor binary, List<Node> addedNodes)
            {
                // Header
                binary.Append("YMNE");

                // Get a list of enemies enemies
                List<LevelObject> enemies = GetObjectBlockObjects(LevelObject.ObjectBlocks.YMNE);

                // Size (set later)
                uint sizePointer = binary.getSize();
                binary.Append(0);

                // Number of enemies
                int numberOfEnemies = enemies.Count;
                binary.Append((short)numberOfEnemies);

                // Null
                binary.Append((short)0);

                // Each enemy
                foreach (LevelObject gameObject in enemies)
                {
                    Enemy enemy = (Enemy)gameObject;

                    // When compiling the enemy, pass the number of nodes that currently exist
                    enemy.compileObject(binary, addedNodes);
                }

                // Set size
                uint size = binary.getSize() - sizePointer + 4;
                binary.Set(sizePointer, size);

                // Pad
                binary.Pad();
            }

            /// <summary>
            /// Collectables (flags, points, time)
            /// </summary>
            /// <param name="binary"></param>
            private static void CompileLLOCBlock()
            {
                // Header
                binary.Append("LLOC");

                // Get lloc objects
                List<LevelObject> collectables = GetObjectBlockObjects(LevelObject.ObjectBlocks.LLOC);
                int numberOfObjects = collectables.Count;

                // Size
                binary.Append((numberOfObjects * 10) + 10);

                // Number of objects
                binary.Append((short)numberOfObjects);

                // Compile each object
                CompileEachObject(collectables);

                // Pad to 4-byte multiple
                binary.Pad();
            }

            /// <summary>
            /// Gets all objects that are part of the specified object block
            /// </summary>
            /// <param name="objectBlock"></param>
            /// <returns></returns>
            private static List<LevelObject> GetObjectBlockObjects(LevelObject.ObjectBlocks objectBlock)
            {
                List<LevelObject> list = new List<LevelObject>();
                foreach (LevelObject gameObject in level.levelObjects)
                {
                    if (gameObject.ObjectBlock == objectBlock)
                    {
                        list.Add(gameObject);
                    }
                }

                return list;
            }

            /// <summary>
            /// Compiles each object in the specified list
            /// </summary>
            /// <param name="binary"></param>
            /// <param name="list"></param>
            private static void CompileEachObject(List<LevelObject> list)
            {
                foreach (LevelObject gameObject in list)
                {
                    // If the object doesn't require a static id, assign it a new one
                    ushort? id = null;
                    if (!gameObject.RequiresStaticId())
                    {
                        id = nextId;
                    }

                    gameObject.CompileVsrBinary(binary, level, id);
                }
            }

            /// <summary>
            /// Mines
            /// </summary>
            /// <param name="binary"></param>
            private static void CompileENIMBlock()
            {
                // Get mine objects
                List<LevelObject> mines = GetObjectBlockObjects(LevelObject.ObjectBlocks.ENIM);
                int numberOfMines = mines.Count;

                // Header
                binary.Append("ENIM");

                // Size
                binary.Append(10 + (numberOfMines * 8));

                // Number of mines
                binary.Append((ushort)numberOfMines);

                // Each mine
                CompileEachObject(mines);

                // Padding
                binary.Append((short)0);

            }

            /// <summary>
            /// Balloon posts
            /// </summary>
            /// <param name="binary"></param>
            private static void CompileNOOBBlock()
            {
                // Name
                binary.Append("NOOB");

                // Get the balloon posts
                List<LevelObject> posts = GetObjectBlockObjects(LevelObject.ObjectBlocks.NOOB);

                // Size
                binary.Append(34);

                // 15 (Allows all posts)
                binary.Append((short)15);

                // Get each post in order
                BalloonPost[] orderedPosts = new BalloonPost[] {
                level.GetBalloonPost(posts, Balloon.Colours.Red),
                level.GetBalloonPost(posts, Balloon.Colours.Blue),
                level.GetBalloonPost(posts, Balloon.Colours.Green),
                level.GetBalloonPost(posts, Balloon.Colours.Yellow)
                };

                // Compile each post in order
                for (int i = 0; i < 4; i++)
                {
                    BalloonPost post = orderedPosts[i];

                    // Post exists
                    if (post != null)
                    {
                        post.CompileVsrBinary(binary, level, null);
                    }
                    // Post doesn't exist
                    else
                    {
                        ushort nullVal = 42330;

                        // Null Position
                        binary.Append(nullVal);
                        binary.Append(nullVal);

                        // Null
                        binary.Append(nullVal);
                    }
                }

                // Null
                binary.Append((short)0);
            }

            /// <summary>
            /// Compiles the block that stores the level's name
            /// </summary>
            /// <param name="binary"></param>
            private static void CompileEMANBlock()
            {
                // Name of block
                binary.Append("EMAN");

                // Size
                binary.Append(40);

                // Level name
                binary.Append(level.name);
                binary.Append((byte)0);

                // Pad name to 32 bytes
                for (int length = level.name.Length + 1; length < 32; length++)
                {
                    binary.Append((byte)187);
                }
            }

            /// <summary>
            /// Compiles the TFED block which stores the out of bounds tile
            /// </summary>
            /// <param name="binary"></param>
            private static void CompileTFEDBlock()
            {
                // Header
                binary.Append("TFED");

                // Size
                binary.Append(12);

                // Border tile ref
                binary.Append((short)518);
                binary.Append((short)0);

            }

            /// <summary>
            /// Compiles the GALF block which stores the number of flags required to win the level.
            /// </summary>
            /// <param name="binary"></param>
            private static void CompileGALFBlock()
            {
                // Header
                binary.Append("GALF");

                // Size
                binary.Append(12);

                // Number of flags required
                binary.Append(GetFlagsRequired());

                // Unknown
                binary.Append((short)2);
            }

            /// <summary>
            /// Returns the number of flags required to finish the level. This is a value between 1 to 4,
            /// or 5 if no flags are required. 5 is the value used by Lemball to represent no flags
            /// </summary>
            /// <returns></returns>
            private static short GetFlagsRequired()
            {
                // Get the number of flag objects
                int numberOfFlags = level.CountPlayerOneFlags();

                // Return the number of flags depending on the FlagsRequired property
                switch (level.NumberOfFlagsRequiredToWin)
                {
                    case FlagsRequired.One:
                        return 1;
                    case FlagsRequired.Two:
                        return 2;
                    case FlagsRequired.Three:
                        return 3;
                    case FlagsRequired.Four:
                        return 4;
                    case FlagsRequired.All:
                        return (short)Math.Min(numberOfFlags, 4);
                    case FlagsRequired.None:
                        return 5;
                }

                // Return 5 ('No flags') if above process fails
                return 5;
            }

            /// <summary>
            /// Compiles the 1SLP block, which stores the trap door Lemming entrances
            /// </summary>
            /// <param name="binary">The BinaryEditor to which to write the compiled block</param>
            private static void Compile1SLPBlock()
            {
                // Gather relevant objects
                List<LevelObject> entrances = GetObjectBlockObjects(LevelObject.ObjectBlocks.OneSLP);

                // Header
                binary.Append("1SLP");

                // Size
                int numberOfEntrances = entrances.Count;
                binary.Append(10 + (numberOfEntrances * 8) + 2);

                // Number of entrances
                binary.Append((short)numberOfEntrances);

                // Each entrance
                CompileEachObject(entrances);

                // Unknown
                binary.Append((short)521);
            }

            /// <summary>
            /// Compiles the BOMG block, which stores interactive objects such as catapult, duplicator, 
            /// keys, balloons, ammo, crate.
            /// </summary>
            /// <param name="binary">The BinaryEditor to which to write the compiled block</param>
            private static void CompileBOMGBlock()
            {
                // Remember size of binary before the block
                uint sizeBefore = binary.getSize();

                // Gather relevant objects
                List<LevelObject> bomgObjects = GetObjectBlockObjects(LevelObject.ObjectBlocks.BOMG);

                // Header
                binary.Append("BOMG");

                // Size of block (set later)
                uint sizePointer = binary.getSize();
                binary.Append(0);

                // Number of objects
                binary.Append((short)bomgObjects.Count);

                // Compile each object
                CompileEachObject(bomgObjects);

                // Pad binary to 4-bytes
                binary.Pad();

                // Set size
                binary.Set(sizePointer, binary.getSize() - sizeBefore);
            }

            /// <summary>
            /// Compiles an empty block with the specified header string
            /// </summary>
            /// <param name="binary">The BinaryEditor to which to write the compiled block</param>
            /// <param name="name">The header string</param>
            private static void CompileEmptyBlock(string name)
            {
                // Name
                binary.Append(name);

                // Size
                binary.Append(10);

                // Null
                binary.Append(0);
            }


        }
    }
}
