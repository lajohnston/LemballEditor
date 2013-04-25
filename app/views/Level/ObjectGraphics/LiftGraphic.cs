using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using LemballEditor.Model;
using System.Windows.Forms;

namespace LemballEditor.View.Level.ObjectGraphics
{
    /// <summary>
    /// Represents a drawn lift
    /// </summary>
    public class LiftGraphic : ObjectGraphic
    {
        /// <summary>
        /// The lift object this graphic represents
        /// </summary>
        private Lift lift;

        /// <summary>
        /// The lift object this graphic represents
        /// </summary>
        public override LevelObject LevelObject
        {
            get { return lift; }
        }

        /// <summary>
        /// The X tile size of the lift
        /// </summary>
        public ushort xTileSize
        {
            get
            {
                return lift.xTileSize;
            }
            set
            {
                lift.xTileSize = value;
                mapPanel.RenderMapAtNextUpdate();
            }
        }

        /// <summary>
        /// The Y tile size of the lift
        /// </summary>
        public ushort yTileSize
        {
            get
            {
                return lift.yTileSize;
            }
            set
            {
                lift.yTileSize = value;
                mapPanel.RenderMapAtNextUpdate();
            }
        }

        /// <summary>
        /// The lift's activation type (how it is activated)
        /// </summary>
        public Lift.ActivationTypes ActivationType
        {
            get
            {
                return lift.ActivationType;
            }
            set
            {
                lift.ActivationType = value;
            }
        }

        /// <summary>
        /// The height of the lift before it has been activated
        /// </summary>
        public ushort StartHeight
        {
            get
            {
                return lift.StartHeight;
            }
            set
            {
                lift.StartHeight = value;
                mapPanel.RenderMapAtNextUpdate();
            }
        }

        /// <summary>
        /// The height of the lift after it has been activated
        /// </summary>
        public ushort EndHeight
        {
            get
            {
                return lift.EndHeight;
            }
            set
            {
                lift.EndHeight = value;
                mapPanel.RenderMapAtNextUpdate();
            }
        }

        /// <summary>
        /// If true, the lift will be drawn at it's start height, otherwise it will be drawn at it's end height
        /// </summary>
        private Boolean previewStartHeight;
        public Boolean PreviewStartHeight
        {
            get
            {
                return previewStartHeight;
            }
            set
            {
                previewStartHeight = value;
                mapPanel.RenderMapAtNextUpdate();
            }
        }

        /// <summary>
        /// The height the lift is set to be drawn at on the map
        /// </summary>
        public ushort PreviewHeight
        {
            get
            {
                if (PreviewStartHeight)
                    return StartHeight;
                else
                    return EndHeight;
            }
        }

        /// <summary>
        /// The image for each tile of the lift's area
        /// </summary>
        private Bitmap image = LemballEditor.View.Level.ImageCache.GetObjectImage("lift");

        /// <summary>
        /// 
        /// </summary>
        public override Bitmap Image
        {
            get
            {
                return image;
            }
        }

        //public override Bitmap Image
        //{
        //    get
        //    {
        //        /*
        //        Bitmap tile = Lemball_Editor.Properties.Resources.nullTile;
        //        Bitmap image = new Bitmap(100, 100);
        //        Graphics g = Graphics.FromImage(image);
        //        g.DrawImage(tile, new Point(0, 0));
        //        g.DrawImage(tile, new Point(16, 8));
        //        */

        //        Bitmap image = new Bitmap(100, 100);
        //        Graphics g = Graphics.FromImage(image);

        //        g.Clear(transparent);

        //        //Point topPoint = new Point(16 * yTileSize, 0);
        //        //Point[] points = new Point[]
        //        //{
        //        //    topPoint,
        //        //    new Point (16 + 16 * xTileSize, 8 * xTileSize),
        //        //    new Point (16, 16 * yTileSize),
        //        //    new Point (0 - 16, 16),
        //        //    topPoint

        //        //};

        //        Point topPoint = new Point(32, 0);
        //        Point[] points = new Point[]
        //        {
        //            topPoint,
        //            new Point (16 * xTileSize, 8 * xTileSize),
        //            //new Point (0, (16 * yTileSize)),
        //            //new Point (0, 16),
        //            //topPoint

        //        };

        //        g.DrawLines(new Pen(Brushes.Yellow), points);
        //        //g.DrawLine(new Pen(Brushes.Yellow), new Point

        //        return image;
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        public override Point DrawOffset
        {
            get
            {
                return new Point(0, 0);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lift"></param>
        /// <param name="mapPanel"></param>
        public LiftGraphic(Lift lift, MapPanel mapPanel)
            : base(mapPanel)
        {
            this.lift = lift;
            PreviewStartHeight = true;
        }

        /// <summary>
        /// The lift graphic detects a point overlap by its tile so that if the user clicks any tile
        /// in its area it is selected without regard to trasparent pixels
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public override bool OverlapsPoint(Point point)
        {
            // Get the tile the point is part of
            TileCoordinate tile = base.mapPanel.ConvertScreenXYtoTileXY(point.X, point.Y);

            // If lift overlaps tile, return true, otherwise return false
            return OverlapsTile(tile);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="menu"></param>
        protected override void AddMenuItems(ContextMenu menu)
        {
            MenuItem settings = new MenuItem("Edit lift");

            settings.Click += delegate
            {
                ChangeLiftArea dialog = new ChangeLiftArea(this);
                dialog.ShowDialog();
            
            };

            menu.MenuItems.Add(settings);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        public override void DrawAtCursor(Graphics g)
        {
            // Render area image
            base.DrawAtCursor(g);
        }

        /// <summary>
        /// States that the lift should snap to the nearest tile when it is being moved by the user
        /// </summary>
        /// <returns></returns>
        protected override bool SnapsToTile()
        {
            return true;
        }

        /// <summary>
        /// States that the cursor should not be positioned at the lift's centre when the user drags the lift
        /// </summary>
        /// <returns></returns>
        public override bool CentreCursorOnDrag()
        {
            return false;
        }
    }
}