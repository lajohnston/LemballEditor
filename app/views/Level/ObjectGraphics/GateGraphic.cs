using LemballEditor.Model;
using System.Drawing;

namespace LemballEditor.View.Level.ObjectGraphics
{
    /// <summary>
    /// Represents a drawn enemy
    /// </summary>
    internal class GateGraphic : ObjectGraphic
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly Gate gate;

        /// <summary>
        /// 
        /// </summary>
        public override LevelObject LevelObject => gate;

        /// <summary>
        /// The images used for each rotation of the gate
        /// </summary>
        private static readonly Bitmap x_image = LemballEditor.View.Level.ImageCache.GetObjectImage("gate_x");
        private static readonly Bitmap y_image = LemballEditor.View.Level.ImageCache.GetObjectImage("gate_y");

        /// <summary>
        /// Returns the gate's image, dependant on whether it is rotated or not.
        /// </summary>
        public override Bitmap Image => gate.Rotated ? x_image : y_image;

        /// <summary>
        /// 
        /// </summary>
        public override Point DrawOffset
        {
            get
            {
                if (gate.Rotated)
                {
                    // x-axis draw offset
                    // Inaccurate
                    return new Point(25, 16);
                }
                else
                {
                    // y-axis draw offset
                    return new Point(25, 16);
                }
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
