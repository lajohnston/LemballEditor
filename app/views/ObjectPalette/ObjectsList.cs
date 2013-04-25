using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using LemballEditor.Model;
using LemballEditor.View.Level;

namespace LemballEditor.View
{
    public partial class ObjectsList : UserControl
    {
        private MainInterface mainInterface;

        public ObjectsList(MainInterface mainInterface)
        {
            InitializeComponent();

            this.mainInterface = mainInterface;

            // Add objects to list
            lstObjects.Items.Add(new ObjectListItem("Ammo", typeof(Ammo)));
            lstObjects.Items.Add(new ObjectListItem("Balloon (blue)", typeof(BlueBalloon)));
            lstObjects.Items.Add(new ObjectListItem("Balloon (green)", typeof(GreenBalloon)));
            lstObjects.Items.Add(new ObjectListItem("Balloon (red)", typeof(RedBalloon)));
            lstObjects.Items.Add(new ObjectListItem("Balloon (yellow)", typeof(YellowBalloon)));
            lstObjects.Items.Add(new ObjectListItem("Catapult", typeof(Catapult)));
            lstObjects.Items.Add(new ObjectListItem("Enemy", typeof(Enemy)));
            lstObjects.Items.Add(new ObjectListItem("Entrance", typeof(Entrance)));
            lstObjects.Items.Add(new ObjectListItem("Flag (blue)", typeof(Flag)));
            //lstObjects.Items.Add(new ObjectListItem("Flag (red)", typeof(RedFlag)));
            lstObjects.Items.Add(new ObjectListItem("Gate/Barrier", typeof(Gate)));
            lstObjects.Items.Add(new ObjectListItem("Mine", typeof(Mine)));
            lstObjects.Items.Add(new ObjectListItem("Switch", typeof(Lever)));

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
                        MessageBox.Show("Object graphic not supported", "Error");
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

            if (value <= 0)
                lstObjects.Enabled = false;
            else
                lstObjects.Enabled = true;
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
            if (MainInterface.GetObjectLimitRemaining() <= 0)
                return true;
            else
                return false;
        }
    }
}
