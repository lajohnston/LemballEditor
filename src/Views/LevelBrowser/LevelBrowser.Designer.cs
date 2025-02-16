namespace LemballEditor.View
{
    partial class LevelBrowser
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.levelList = new System.Windows.Forms.ListBox();
            this.moveLevelUp = new System.Windows.Forms.Button();
            this.moveLevelDown = new System.Windows.Forms.Button();
            this.newLevel = new System.Windows.Forms.Button();
            this.moveLevel = new System.Windows.Forms.Button();
            this.deleteLevel = new System.Windows.Forms.Button();
            this.toolTips = new System.Windows.Forms.ToolTip(this.components);
            this.copyLevel = new System.Windows.Forms.Button();
            this.levelGroupSelector = new LemballEditor.View.LevelBrowser.LevelGroupSelector();
            this.SuspendLayout();
            // 
            // levelList
            // 
            this.levelList.FormattingEnabled = true;
            this.levelList.Location = new System.Drawing.Point(3, 29);
            this.levelList.Name = "levelList";
            this.levelList.Size = new System.Drawing.Size(162, 160);
            this.levelList.TabIndex = 0;
            // 
            // moveLevelUp
            // 
            this.moveLevelUp.Location = new System.Drawing.Point(171, 87);
            this.moveLevelUp.Name = "moveLevelUp";
            this.moveLevelUp.Size = new System.Drawing.Size(26, 21);
            this.moveLevelUp.TabIndex = 1;
            this.moveLevelUp.Text = "↑";
            this.moveLevelUp.UseVisualStyleBackColor = true;
            this.moveLevelUp.Click += new System.EventHandler(this.moveLevelUp_Click);
            // 
            // moveLevelDown
            // 
            this.moveLevelDown.Location = new System.Drawing.Point(171, 114);
            this.moveLevelDown.Name = "moveLevelDown";
            this.moveLevelDown.Size = new System.Drawing.Size(26, 21);
            this.moveLevelDown.TabIndex = 4;
            this.moveLevelDown.Text = "↓";
            this.moveLevelDown.UseVisualStyleBackColor = true;
            this.moveLevelDown.Click += new System.EventHandler(this.moveLevelDown_Click);
            // 
            // newLevel
            // 
            this.newLevel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.newLevel.Location = new System.Drawing.Point(171, 33);
            this.newLevel.Name = "newLevel";
            this.newLevel.Size = new System.Drawing.Size(26, 21);
            this.newLevel.TabIndex = 5;
            this.newLevel.Text = "+";
            this.newLevel.UseVisualStyleBackColor = true;
            // 
            // moveLevel
            // 
            this.moveLevel.Location = new System.Drawing.Point(171, 141);
            this.moveLevel.Name = "moveLevel";
            this.moveLevel.Size = new System.Drawing.Size(26, 21);
            this.moveLevel.TabIndex = 7;
            this.moveLevel.Text = ">";
            this.moveLevel.UseVisualStyleBackColor = true;
            this.moveLevel.Click += new System.EventHandler(this.moveLevel_Click);
            // 
            // deleteLevel
            // 
            this.deleteLevel.ForeColor = System.Drawing.Color.DarkRed;
            this.deleteLevel.Location = new System.Drawing.Point(171, 60);
            this.deleteLevel.Name = "deleteLevel";
            this.deleteLevel.Size = new System.Drawing.Size(26, 21);
            this.deleteLevel.TabIndex = 9;
            this.deleteLevel.Text = "x";
            this.deleteLevel.UseVisualStyleBackColor = true;
            // 
            // copyLevel
            // 
            this.copyLevel.Location = new System.Drawing.Point(171, 168);
            this.copyLevel.Name = "copyLevel";
            this.copyLevel.Size = new System.Drawing.Size(26, 21);
            this.copyLevel.TabIndex = 10;
            this.copyLevel.Text = "c";
            this.toolTips.SetToolTip(this.copyLevel, "Copy level");
            this.copyLevel.UseVisualStyleBackColor = true;
            this.copyLevel.Click += new System.EventHandler(this.copyLevel_Click);
            // 
            // levelGroupSelector
            // 
            this.levelGroupSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.levelGroupSelector.FormattingEnabled = true;
            this.levelGroupSelector.Location = new System.Drawing.Point(3, 3);
            this.levelGroupSelector.Name = "levelGroupSelector";
            this.levelGroupSelector.SelectedLevelGroup = LemballEditor.Model.LevelGroupTypes.Fun;
            this.levelGroupSelector.Size = new System.Drawing.Size(162, 21);
            this.levelGroupSelector.TabIndex = 6;
            // 
            // LevelBrowser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.copyLevel);
            this.Controls.Add(this.deleteLevel);
            this.Controls.Add(this.moveLevel);
            this.Controls.Add(this.moveLevelUp);
            this.Controls.Add(this.levelGroupSelector);
            this.Controls.Add(this.moveLevelDown);
            this.Controls.Add(this.levelList);
            this.Controls.Add(this.newLevel);
            this.Name = "LevelBrowser";
            this.Size = new System.Drawing.Size(203, 198);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox levelList;
        private System.Windows.Forms.Button moveLevelUp;
        private System.Windows.Forms.Button moveLevelDown;
        private System.Windows.Forms.Button newLevel;
        private System.Windows.Forms.Button moveLevel;
        private System.Windows.Forms.Button deleteLevel;
        private System.Windows.Forms.ToolTip toolTips;
        private System.Windows.Forms.Button copyLevel;
        private LevelBrowser.LevelGroupSelector levelGroupSelector;
    }
}
