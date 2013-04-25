using System;
using System.Collections.Generic;
using System.Text;

namespace LemballEditor.Model
{
    /// <summary>
    /// Stores an X and Y tile coordinate
    /// </summary>
    public class TileCoordinate
    {
        public ushort xTile { get; set; }
        public ushort yTile { get; set; }

        public TileCoordinate(ushort xTile, ushort yTile)
        {
            this.xTile = xTile;
            this.yTile = yTile;
        }

        public override bool Equals(object obj)
        {
            // Return false if object is not a tile coordinate
            if (!(obj is TileCoordinate))
                return false;

            TileCoordinate compareTo = (TileCoordinate)obj;
            if (xTile == compareTo.xTile && yTile == compareTo.yTile)
                return true;
            else
                return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
