namespace LemballEditor.View
{
    /// <summary>
    /// A version of the Level Properties that is used to edit an existing level rather than for creating a new one.
    /// This version disables some options that cannot be edited once the level is created
    /// </summary>
    internal class EditLevelProperties : LevelProperties
    {
        public EditLevelProperties(Model.Level level)
            : base(level)
        {
            // Disable the buttons to change the terrain type
            base.terrainGroup.Enabled = false;

            // Disable the group to edit the level size
            base.mapSizeGroup.Enabled = false;
        }
    }
}
