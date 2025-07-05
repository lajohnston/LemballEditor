namespace LemballEditor.Models.LevelObjects
{
    public class Flag : ILevelObject
    {
        public Position Position { get; }

        public Flag(Position position)
        {
            this.Position = position;
        }

        public ILevelObject WithPosition(Position position)
        {
            return new Flag(position);
        }
    }
}
