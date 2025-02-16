namespace LemballEditor.Model
{
    /// <summary>
    /// A blue balloon
    /// </summary>
    internal class BlueBalloon : Balloon
    {

        public override Balloon.Colours Colour => Colours.Blue;


        public BlueBalloon(ushort id)
            : base(id)
        {

        }
    }
}
