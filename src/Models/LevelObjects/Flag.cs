namespace LemballEditor.Models.LevelObjects
{
    public class Flag : ILevelObject
    {
        public Position _position;

        public Flag(Position position)
        {
            this._position = position;
        }

        public Position GetPosition()
        {
            return this._position;
        }

        public ILevelObject SetPosition(Position position)
        {
            this._position = position;
            return this;
        }
    }
}
