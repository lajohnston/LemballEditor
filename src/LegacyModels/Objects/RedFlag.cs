using System.Xml;

namespace LemballEditor.LegacyModels
{
    internal class RedFlag : Flag
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
