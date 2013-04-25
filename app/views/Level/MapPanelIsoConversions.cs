using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using LemballEditor.Model;

namespace LemballEditor.View.Level
{
    partial class MapPanel
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="screenX"></param>
        /// <param name="screenY"></param>
        /// <returns></returns>
        public Point ConvertScreenXYtoIsoXY(int screenX, int screenY)
        {
            // Get the offsets
            int offsetX = (screenX - tile0Position.X) / 2;
            int offsetY = screenY - tile0Position.Y;

            Point iso = new Point(offsetX + offsetY, (0 - offsetX) + offsetY);
            iso.X = Math.Max(iso.X, 0);
            iso.Y = Math.Max(iso.Y, 0);
            return iso;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isoPosition"></param>
        /// <returns></returns>
        public Point ConvertIsoXYtoScreenXY(Point isoPosition)
        {
            // Calculate the screen position
            Point screenPoint = new Point(
                tile0Position.X + isoPosition.X - isoPosition.Y,
                tile0Position.Y + (int)Math.Ceiling((double)((isoPosition.X + isoPosition.Y) / 2))
                );

            return screenPoint;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isoX"></param>
        /// <param name="isoY"></param>
        /// <returns></returns>
        public static TileCoordinate ConvertIsoXYtoTileXY(int isoX, int isoY)
        {
            double xTile = Math.Floor((double)isoX / 16);
            double yTile = Math.Floor((double)isoY / 16);

            return new TileCoordinate((ushort)xTile, (ushort)yTile);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="screenX"></param>
        /// <param name="screenY"></param>
        /// <returns></returns>
        public TileCoordinate ConvertScreenXYtoTileXY(int screenX, int screenY)
        {
            // Get iso coordinates
            Point iso = ConvertScreenXYtoIsoXY(screenX, screenY);
            return ConvertIsoXYtoTileXY((int)iso.X, (int)iso.Y);
            
            /*
            // Get tileXY from iso coordinates
            if (iso.X >= 0 && iso.Y >= 0)
                return ConvertIsoXYtoTileXY((uint)iso.X, (uint)iso.Y);
            else
            {
                return null;
            }
            */
        }

        /// <summary>
        /// Gets the X and Y screen coordinates (relative to the mapPanel) for the specified tile.
        /// The returned value takes into consideration the offset of the tile graphic (X - 16),
        /// so a tile can be drawn at the returned position and be in the correct place.
        /// </summary>
        /// <param name="coordinate"></param>
        /// <param name="withElevation">If true, the position will take into consideration the elevation
        /// of the tile</param>
        /// <returns></returns>
        public Point ConvertTileXYtoScreenXY(TileCoordinate coordinate, bool withElevation)
        {
            Point position = new Point(
                tile0Position.X + (coordinate.xTile * 16) - (coordinate.yTile * 16) - 16,
                tile0Position.Y + (coordinate.xTile * 8) + (coordinate.yTile * 8)
                );

            if (withElevation)
            {
                int elevation = GetTileDrawElevation(coordinate, true);
                position.Y -= elevation;
            }

            return position;
        }

    }
}
