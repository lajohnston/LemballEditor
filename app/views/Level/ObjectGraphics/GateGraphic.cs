using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using LemballEditor.Model;
using System.Windows.Forms;

namespace LemballEditor.View.Level.ObjectGraphics
{
    /// <summary>
    /// Represents a drawn enemy
    /// </summary>
    class GateGraphic : ObjectGraphic
    {
        /// <summary>
        /// 
        /// </summary>
        private Gate gate;

        /// <summary>
        /// 
        /// </summary>
        public override LevelObject LevelObject
        {
            get { return gate; }
        }

        /// <summary>
        /// The images used for each rotation of the gate
        /// </summary>
        private static Bitmap x_image = LemballEditor.View.Level.ImageCache.GetObjectImage("gate_x");
        private static Bitmap y_image = LemballEditor.View.Level.ImageCache.GetObjectImage("gate_y");

        /// <summary>
        /// Returns the gate's image, dependant on whether it is rotated or not.
        /// </summary>
        public override Bitmap Image
        {
            get
            {
                if (gate.Rotated)
                    return x_image;
                else
                    return y_image;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override Point DrawOffset
        {
            get
            {
                if (gate.Rotated)
                    // x-axis draw offset
                    // Inaccurate
                    return new Point(25, 16);
                else
                    // y-axis draw offset
                    return new Point(25, 16);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ammo"></param>
        /// <param name="mapPanel"></param>
        public GateGraphic(Gate gate, MapPanel mapPanel)
            : base(mapPanel)
        {
            this.gate = gate;
        }
    }
}
