namespace LemballEditor.Models.LevelObjects
{
    public interface ILevelObject
    {
        Position GetPosition();

        ILevelObject SetPosition(Position position);
    }
}
