using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace LemballEditor.Model
{
    /// <summary>
    /// A Green balloon
    /// </summary>
    class GreenBalloon : Balloon
    {
        public override Balloon.Colours Colour
        {
            get
            {
                return Colours.Green;
            }
        }

        public GreenBalloon(ushort id)
            : base(id)
        {

        }
    }
}
