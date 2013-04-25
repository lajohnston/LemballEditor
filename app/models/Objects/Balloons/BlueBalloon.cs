using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace LemballEditor.Model
{
    /// <summary>
    /// A blue balloon
    /// </summary>
    class BlueBalloon : Balloon
    {

        public override Balloon.Colours Colour
        {
            get
            {
                return Colours.Blue;
            }
        }


        public BlueBalloon(ushort id)
            : base(id)
        {

        }
    }
}
