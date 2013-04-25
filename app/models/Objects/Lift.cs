using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Drawing;

namespace LemballEditor.Model
{
    public class Lift : LevelObject
    {
        /// <summary>
        /// 
        /// </summary>
        public override ObjectBlocks ObjectBlock
        {
            get { return ObjectBlocks.TFIL; }
        }

        /// <summary>
        /// How lifts can be activated
        /// </summary>
        public enum ActivationTypes
        {
            /// <summary>
            /// The lift is activated by a switch, and can be activated multiple times
            /// </summary>
            SwitchMultiple = 0,

            /// <summary>
            /// The lift is activated by a lemming walking on it, and can be activated multiple times
            /// </summary>
            //TouchMultiple = 1,

            /// <summary>
            /// The lift is activated at the start of the level
            /// </summary>
            StartOfLevel = 2,

            /// <summary>
            /// The lift is activated by a switch, but can only be activated once
            /// </summary>
            SwitchOnceOnly = 3,

            /// <summary>
            /// The lift is activated by a lemming walking on it, but can only be activated once 
            /// </summary>
            TouchOnceOnly = 4
        }

        /// <summary>
        /// How this lift is activated
        /// </summary>
        public ActivationTypes ActivationType { get; set; }

        /// <summary>
        /// The size of the lift in tiles, along the X axis
        /// </summary>
        public ushort xTileSize { get; set; }

        /// <summary>
        /// The size of the lift in tiles, along the Y axis
        /// </summary>
        public ushort yTileSize { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<TileCoordinate> tileCoordinates
        {
            get
            {
                List<TileCoordinate> tiles = new List<TileCoordinate>();

                for (ushort x = 0; x < xTileSize; x++)
                {
                    for (ushort y = 0; y < yTileSize; y++)
                    {
                        tiles.Add(new TileCoordinate((ushort)(OnTile.xTile + x),(ushort)(OnTile.yTile + y)));
                    }
                }

                return tiles;
            }
        }

        /// <summary>
        /// The height of the lift before it has been activated
        /// </summary>
        private ushort startHeight;
        public ushort StartHeight
        {
            get
            {
                return startHeight;
            }
            set
            {
                if (value > 88)
                    throw new ArgumentOutOfRangeException();
                else
                    startHeight = value;
            }
        }
        
        /// <summary>
        /// The height of the lift after it has been activated
        /// </summary>
        private ushort endHeight;
        public ushort EndHeight
        {
            get
            {
                return endHeight;
            }
            set
            {
                if (value > 88)
                    throw new ArgumentOutOfRangeException();
                else
                    endHeight = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public Lift(ushort id)
            :this(id, 0, 0, 1, 1)
        {

        }

        public Lift(ushort id, ushort isoX, ushort isoY, ushort xTileSize, ushort yTileSize)
            :base(id, isoX, isoY)
        {
            this.xTileSize = xTileSize;
            this.yTileSize = yTileSize;

            ActivationType = ActivationTypes.StartOfLevel;

            startHeight = 0;
            endHeight = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tileCoordinate"></param>
        /// <returns></returns>
        public override bool OverlapsTile(TileCoordinate tileCoordinate)
        {
            //return tileCoordinate.Equals(new TileCoordinate((ushort)(OnTile.xTile + xTileSize), (ushort)(OnTile.yTile + yTileSize)));
            foreach (TileCoordinate tile in tileCoordinates)
            {
                if (tile.Equals(tileCoordinate))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <returns></returns>
        public override XmlElement CompileXml(System.Xml.XmlDocument xmlDoc)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="binary"></param>
        /// <param name="level"></param>
        /// <param name="id"></param>
        public override void CompileVsrBinary(VsrCompiler.BinaryEditor binary, Level level, ushort? id)
        {
            // Id
            binary.Append((ushort)base.Id);

            // 1 = keeps going up and down once activated
            binary.Append((short)0);

            // Activation type
            binary.Append((short)ActivationType);

            // XY1 (top left of the lift's area)
            binary.Append((short)IsoPosition.X);
            binary.Append((short)IsoPosition.Y);

            // Default height
            binary.Append((short)StartHeight);
            
            // XY2 (bottom right of lift's area)
            // Get the size of the lift in pixels
            int xSizePx = 8 + (xTileSize - 1) * 16;
            int ySizePx = 8 + (yTileSize - 1) * 16;

            binary.Append((short)(IsoPosition.X + xSizePx));
            binary.Append((short)(IsoPosition.Y + ySizePx));

            // Default height (again)
            binary.Append((short)StartHeight);

            // Lowest and highest height
            binary.Append((short) Math.Min(startHeight, endHeight));
            binary.Append((short) Math.Max(startHeight, endHeight));

            // Null
            binary.Append((short)0);
        }

        /// <summary>
        /// States that the lift object type requires a static id, as it is referenced by other objects
        /// </summary>
        /// <returns></returns>
        public override bool RequiresStaticId()
        {
            return true;
        }


        public override bool ConnectToSwitch()
        {
            ActivationType = ActivationTypes.SwitchOnceOnly;

            return true;
        }

    }
}
