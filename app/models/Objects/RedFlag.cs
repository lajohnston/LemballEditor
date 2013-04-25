using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace LemballEditor.Model
{
    class RedFlag : Flag
    {
        public RedFlag(XmlElement element)
            : base(element)
        {
        }

        public RedFlag(ushort id)
            : base(id)
        {
        }

        protected override short getObjectReference()
        {
            return 11;
        }

        public override bool isPlayerOne()
        {
            return false;
        }
    }
}
