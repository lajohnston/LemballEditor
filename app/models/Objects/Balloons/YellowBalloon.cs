using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace LemballEditor.Model
{
    /// <summary>
    /// A yellow balloon
    /// </summary>
    class YellowBalloon : Balloon
    {
        public override Balloon.Colours Colour
        {
            get
            {
                return Colours.Yellow;
            }
        }

        public YellowBalloon(ushort id)
            : base(id)
        {

        }
    }
}
