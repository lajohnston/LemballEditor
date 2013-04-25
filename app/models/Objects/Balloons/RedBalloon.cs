using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace LemballEditor.Model
{
    /// <summary>
    /// A red balloon
    /// </summary>
    class RedBalloon : Balloon
    {
        public override Balloon.Colours Colour
        {
            get
            {
                return Colours.Red;
            }
        }

        public RedBalloon(ushort id)
            : base(id)
        {

        }

    }
}
