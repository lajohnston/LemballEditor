using LemballEditor.Model;
using System;
using System.Windows.Forms;

namespace LemballEditor.View.Level.ObjectGraphics
{
    public partial class ChangeLiftArea : Form
    {
        /// <summary>
        /// The lift graphic being edited
        /// </summary>
        private readonly LiftGraphic liftGraphic;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="liftGraphic"></param>
        public ChangeLiftArea(LiftGraphic liftGraphic)
        {
            InitializeComponent();
            this.liftGraphic = liftGraphic;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangeLiftArea_Load(object sender, EventArgs e)
        {
            // Load values
            xSize.Value = liftGraphic.xTileSize;
            ySize.Value = liftGraphic.yTileSize;

            // Load heights
            startHeight.Value = liftGraphic.StartHeight;
            endHeight.Value = liftGraphic.EndHeight;

            // Load preview height settings
            if (liftGraphic.PreviewStartHeight)
            {
                previewStartHeight.Checked = true;
            }
            else
            {
                previewEndHeight.Checked = true;
            }

            // Load activation type
            switch (liftGraphic.ActivationType)
            {
                case Lift.ActivationTypes.StartOfLevel:
                    startOfLevel.Checked = true;
                    break;
                case Lift.ActivationTypes.SwitchMultiple:
                    switchMultiple.Checked = true;
                    break;
                case Lift.ActivationTypes.SwitchOnceOnly:
                    switchOnce.Checked = true;
                    break;
                case Lift.ActivationTypes.TouchOnceOnly:
                    touchOnce.Checked = true;
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void startOfLevel_CheckedChanged(object sender, EventArgs e)
        {
            if (startOfLevel.Checked)
            {
                liftGraphic.ActivationType = Lift.ActivationTypes.StartOfLevel;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void switchMultiple_CheckedChanged(object sender, EventArgs e)
        {
            if (switchMultiple.Checked)
            {
                liftGraphic.ActivationType = Lift.ActivationTypes.SwitchMultiple;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void switchOnce_CheckedChanged(object sender, EventArgs e)
        {
            if (switchOnce.Checked)
            {
                liftGraphic.ActivationType = Lift.ActivationTypes.SwitchOnceOnly;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void touchOnce_CheckedChanged(object sender, EventArgs e)
        {
            if (touchOnce.Checked)
            {
                liftGraphic.ActivationType = Lift.ActivationTypes.TouchOnceOnly;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void startHeight_ValueChanged(object sender, EventArgs e)
        {
            liftGraphic.StartHeight = (ushort)startHeight.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void endHeight_ValueChanged(object sender, EventArgs e)
        {
            liftGraphic.EndHeight = (ushort)endHeight.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void xSize_ValueChanged(object sender, EventArgs e)
        {
            liftGraphic.xTileSize = (ushort)xSize.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ySize_ValueChanged(object sender, EventArgs e)
        {
            liftGraphic.yTileSize = (ushort)ySize.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void previewStartHeight_CheckedChanged(object sender, EventArgs e)
        {
            if (previewStartHeight.Checked)
            {
                liftGraphic.PreviewStartHeight = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void previewEndHeight_CheckedChanged(object sender, EventArgs e)
        {
            if (previewEndHeight.Checked)
            {
                liftGraphic.PreviewStartHeight = false;
            }
        }

    }
}
