namespace LemballEditor.Model
{
    /// <summary>
    /// A yellow balloon
    /// </summary>
    internal class YellowBalloon : Balloon
    {
        public override Balloon.Colours Colour => Colours.Yellow;

        public YellowBalloon(ushort id)
            : base(id)
        {

        }
    }
}
