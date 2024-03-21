using System.Windows.Forms;

namespace LemballEditor.View
{
    public partial class LevelBrowser
    {
        protected class LevelGroupSelector : ComboBox
        {
            /// <summary>
            /// 
            /// </summary>
            public Model.LevelGroupTypes SelectedLevelGroup
            {
                get => ((LevelGroupItem)SelectedItem).LevelGroupType;
                set => SelectedIndex = (int)value;
            }

            public override int SelectedIndex
            {
                set => base.SelectedIndex = value > -1 && value < (int)Model.LevelGroupTypes.Mayhem ? value : 0;
            }

            /// <summary>
            /// 
            /// </summary>
            public LevelGroupSelector()
            {
                // Disables text field
                DropDownStyle = ComboBoxStyle.DropDownList;

                // Add level groups
                _ = Items.Add(new LevelGroupItem(Model.LevelGroupTypes.Fun));
                _ = Items.Add(new LevelGroupItem(Model.LevelGroupTypes.Tricky));
                _ = Items.Add(new LevelGroupItem(Model.LevelGroupTypes.Taxing));
                _ = Items.Add(new LevelGroupItem(Model.LevelGroupTypes.Mayhem));

                // Select the Fun level group
                SelectedIndex = 0;
            }

            public LevelGroupItem[] GetItems()
            {
                // Copy items to an array and return
                LevelGroupItem[] array = new LevelGroupItem[Items.Count];
                Items.CopyTo(array, 0);
                return array;
            }
        }
    }
}
