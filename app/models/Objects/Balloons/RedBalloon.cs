namespace LemballEditor.Model
{
    /// <summary>
    /// A red balloon
    /// </summary>
    internal class RedBalloon : Balloon
    {
        public override Balloon.Colours Colour => Colours.Red;

        public RedBalloon(ushort id)
            : base(id)
        {

        }

    }
}
