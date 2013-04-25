using System;
using System.Collections.Generic;
using System.Text;
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
                get
                {
                    return ((LevelGroupItem)SelectedItem).LevelGroupType;
                }
                set
                {
                    SelectedIndex = (int)value;
                }
            }

            public override int SelectedIndex
            {
                set
                {
                    if (value > -1 && value < (int)Model.LevelGroupTypes.Mayhem)
                        base.SelectedIndex = value;
                    else
                        base.SelectedIndex = 0;
                }
            }

            /// <summary>
            /// 
            /// </summary>
            public LevelGroupSelector()
            {
                // Disables text field
                DropDownStyle = ComboBoxStyle.DropDownList;

                // Add level groups
                Items.Add(new LevelGroupItem(Model.LevelGroupTypes.Fun));
                Items.Add(new LevelGroupItem(Model.LevelGroupTypes.Tricky));
                Items.Add(new LevelGroupItem(Model.LevelGroupTypes.Taxing));
                Items.Add(new LevelGroupItem(Model.LevelGroupTypes.Mayhem));

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
