using LemballEditor.Model;
using System;
using System.Windows.Forms;

namespace LemballEditor.View
{
    public partial class ObjectsList : UserControl
    {
        private readonly MainInterface mainInterface;

        public ObjectsList(MainInterface mainInterface)
        {
            InitializeComponent();

            this.mainInterface = mainInterface;

            // Add objects to list
            _ = lstObjects.Items.Add(new ObjectListItem("Ammo", typeof(Ammo)));
            _ = lstObjects.Items.Add(new ObjectListItem("Balloon (blue)", typeof(BlueBalloon)));
            _ = lstObjects.Items.Add(new ObjectListItem("Balloon (green)", typeof(GreenBalloon)));
            _ = lstObjects.Items.Add(new ObjectListItem("Balloon (red)", typeof(RedBalloon)));
            _ = lstObjects.Items.Add(new ObjectListItem("Balloon (yellow)", typeof(YellowBalloon)));
            _ = lstObjects.Items.Add(new ObjectListItem("Catapult", typeof(Catapult)));
            _ = lstObjects.Items.Add(new ObjectListItem("Enemy", typeof(Enemy)));
            _ = lstObjects.Items.Add(new ObjectListItem("Entrance", typeof(Entrance)));
            _ = lstObjects.Items.Add(new ObjectListItem("Flag (blue)", typeof(Flag)));
            //lstObjects.Items.Add(new ObjectListItem("Flag (red)", typeof(RedFlag)));
            _ = lstObjects.Items.Add(new ObjectListItem("Gate/Barrier", typeof(Gate)));
            _ = lstObjects.Items.Add(new ObjectListItem("Mine", typeof(Mine)));
            _ = lstObjects.Items.Add(new ObjectListItem("Switch", typeof(Lever)));

            UpdateObjectLimitCounter();
        }

        /// <summary>
        /// Called when the user selects a new object in the list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lstObjects_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Ensure maximum number of objects hasn't been exceeded
            if (!objectLimitReached())
            {
                // Ensure an item is selected
                if (lstObjects.SelectedIndex != -1)
                {
                    // Get selected object
                    ObjectListItem selected = (ObjectListItem)lstObjects.SelectedItem;

                    // Set the dragging object to a new object of the selected type
                    LevelObject newObject = selected.MakeGameObject();

                    try
                    {
                        mainInterface.StartPlacingNewObjectMode(newObject);
                        UpdateObjectLimitCounter();
                    }
                    catch (NotImplementedException)
                    {
                        _ = MessageBox.Show("Object graphic not supported", "Error");
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void UpdateObjectLimitCounter()
        {
            int value = MainInterface.GetObjectLimitRemaining();

            lblObjectLimit.Text = "Object limit remaining: " + value;

            lstObjects.Enabled = value > 0;
        }


        /// <summary>
        /// Called when an object has been deleted. Renables the list if object count is now
        /// below the limit
        /// </summary>
        public void objectDeleted()
        {
            if (!lstObjects.Enabled && !objectLimitReached())
            {
                lstObjects.Enabled = true;
            }
        }

        private bool objectLimitReached()
        {
            return MainInterface.GetObjectLimitRemaining() <= 0;
        }
    }
}
