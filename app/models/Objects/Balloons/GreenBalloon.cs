namespace LemballEditor.Model
{
    /// <summary>
    /// A Green balloon
    /// </summary>
    internal class GreenBalloon : Balloon
    {
        public override Balloon.Colours Colour => Colours.Green;

        public GreenBalloon(ushort id)
            : base(id)
        {

        }
    }
}
