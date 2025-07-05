namespace LemballEditor.Models.LevelObjects
{
    public interface ILevelObject
    {
        Position Position { get; }

        ILevelObject WithPosition(Position position);
    }
}
