namespace LemballEditor.View
{
    partial class LevelProperties
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.levelName = new System.Windows.Forms.TextBox();
            this.unlimitedTime = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.terrainGroup = new System.Windows.Forms.GroupBox();
            this.spaceTerrain = new System.Windows.Forms.RadioButton();
            this.snowTerrain = new System.Windows.Forms.RadioButton();
            this.legoTerrain = new System.Windows.Forms.RadioButton();
            this.grassTerrain = new System.Windows.Forms.RadioButton();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.fourFlags = new System.Windows.Forms.RadioButton();
            this.threeFlags = new System.Windows.Forms.RadioButton();
            this.twoFlags = new System.Windows.Forms.RadioButton();
            this.oneFlag = new System.Windows.Forms.RadioButton();
            this.allFlags = new System.Windows.Forms.RadioButton();
            this.mapSizeGroup = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.flagsInLevel = new System.Windows.Forms.Label();
            this.yTiles = new LemballEditor.View.NumericTextBox();
            this.xTiles = new LemballEditor.View.NumericTextBox();
            this.timeLimitSeconds = new LemballEditor.View.NumericTextBox();
            this.timeLimitMinutes = new LemballEditor.View.NumericTextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.terrainGroup.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.mapSizeGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.levelName);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(280, 55);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Name";
            // 
            // levelName
            // 
            this.levelName.Location = new System.Drawing.Point(9, 19);
            this.levelName.MaxLength = 31;
            this.levelName.Name = "levelName";
            this.levelName.ShortcutsEnabled = false;
            this.levelName.Size = new System.Drawing.Size(262, 20);
            this.levelName.TabIndex = 0;
            // 
            // unlimitedTime
            // 
            this.unlimitedTime.AutoSize = true;
            this.unlimitedTime.Location = new System.Drawing.Point(73, 32);
            this.unlimitedTime.Name = "unlimitedTime";
            this.unlimitedTime.Size = new System.Drawing.Size(69, 17);
            this.unlimitedTime.TabIndex = 2;
            this.unlimitedTime.Text = "Unlimited";
            this.unlimitedTime.UseVisualStyleBackColor = true;
            this.unlimitedTime.CheckedChanged += new System.EventHandler(this.unlimitedTime_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.timeLimitSeconds);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.timeLimitMinutes);
            this.groupBox2.Controls.Add(this.unlimitedTime);
            this.groupBox2.Location = new System.Drawing.Point(12, 246);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(148, 63);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Time limit";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(44, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(21, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "SS";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(16, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "M";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(34, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(10, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = ":";
            // 
            // terrainGroup
            // 
            this.terrainGroup.Controls.Add(this.spaceTerrain);
            this.terrainGroup.Controls.Add(this.snowTerrain);
            this.terrainGroup.Controls.Add(this.legoTerrain);
            this.terrainGroup.Controls.Add(this.grassTerrain);
            this.terrainGroup.Location = new System.Drawing.Point(168, 73);
            this.terrainGroup.Name = "terrainGroup";
            this.terrainGroup.Size = new System.Drawing.Size(124, 167);
            this.terrainGroup.TabIndex = 4;
            this.terrainGroup.TabStop = false;
            this.terrainGroup.Text = "Terrain type";
            // 
            // spaceTerrain
            // 
            this.spaceTerrain.AutoSize = true;
            this.spaceTerrain.Location = new System.Drawing.Point(13, 88);
            this.spaceTerrain.Name = "spaceTerrain";
            this.spaceTerrain.Size = new System.Drawing.Size(56, 17);
            this.spaceTerrain.TabIndex = 3;
            this.spaceTerrain.Text = "Space";
            this.spaceTerrain.UseVisualStyleBackColor = true;
            // 
            // snowTerrain
            // 
            this.snowTerrain.AutoSize = true;
            this.snowTerrain.Location = new System.Drawing.Point(13, 42);
            this.snowTerrain.Name = "snowTerrain";
            this.snowTerrain.Size = new System.Drawing.Size(52, 17);
            this.snowTerrain.TabIndex = 2;
            this.snowTerrain.Text = "Snow";
            this.snowTerrain.UseVisualStyleBackColor = true;
            // 
            // legoTerrain
            // 
            this.legoTerrain.AutoSize = true;
            this.legoTerrain.Location = new System.Drawing.Point(13, 65);
            this.legoTerrain.Name = "legoTerrain";
            this.legoTerrain.Size = new System.Drawing.Size(49, 17);
            this.legoTerrain.TabIndex = 1;
            this.legoTerrain.Text = "Lego";
            this.legoTerrain.UseVisualStyleBackColor = true;
            // 
            // grassTerrain
            // 
            this.grassTerrain.AutoSize = true;
            this.grassTerrain.Checked = true;
            this.grassTerrain.Location = new System.Drawing.Point(13, 19);
            this.grassTerrain.Name = "grassTerrain";
            this.grassTerrain.Size = new System.Drawing.Size(52, 17);
            this.grassTerrain.TabIndex = 0;
            this.grassTerrain.TabStop = true;
            this.grassTerrain.Text = "Grass";
            this.grassTerrain.UseVisualStyleBackColor = true;
            // 
            // okButton
            // 
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(53, 340);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(77, 28);
            this.okButton.TabIndex = 5;
            this.okButton.Text = "Ok";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(172, 340);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(77, 28);
            this.cancelButton.TabIndex = 6;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.flagsInLevel);
            this.groupBox4.Controls.Add(this.fourFlags);
            this.groupBox4.Controls.Add(this.threeFlags);
            this.groupBox4.Controls.Add(this.twoFlags);
            this.groupBox4.Controls.Add(this.oneFlag);
            this.groupBox4.Controls.Add(this.allFlags);
            this.groupBox4.Location = new System.Drawing.Point(12, 73);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(148, 167);
            this.groupBox4.TabIndex = 7;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Flags needed to win level";
            // 
            // fourFlags
            // 
            this.fourFlags.AutoSize = true;
            this.fourFlags.Location = new System.Drawing.Point(9, 88);
            this.fourFlags.Name = "fourFlags";
            this.fourFlags.Size = new System.Drawing.Size(46, 17);
            this.fourFlags.TabIndex = 4;
            this.fourFlags.TabStop = true;
            this.fourFlags.Text = "Four";
            this.fourFlags.UseVisualStyleBackColor = true;
            // 
            // threeFlags
            // 
            this.threeFlags.AutoSize = true;
            this.threeFlags.Location = new System.Drawing.Point(9, 65);
            this.threeFlags.Name = "threeFlags";
            this.threeFlags.Size = new System.Drawing.Size(53, 17);
            this.threeFlags.TabIndex = 3;
            this.threeFlags.TabStop = true;
            this.threeFlags.Text = "Three";
            this.threeFlags.UseVisualStyleBackColor = true;
            // 
            // twoFlags
            // 
            this.twoFlags.AutoSize = true;
            this.twoFlags.Location = new System.Drawing.Point(9, 42);
            this.twoFlags.Name = "twoFlags";
            this.twoFlags.Size = new System.Drawing.Size(46, 17);
            this.twoFlags.TabIndex = 2;
            this.twoFlags.TabStop = true;
            this.twoFlags.Text = "Two";
            this.twoFlags.UseVisualStyleBackColor = true;
            // 
            // oneFlag
            // 
            this.oneFlag.AutoSize = true;
            this.oneFlag.Location = new System.Drawing.Point(9, 19);
            this.oneFlag.Name = "oneFlag";
            this.oneFlag.Size = new System.Drawing.Size(45, 17);
            this.oneFlag.TabIndex = 1;
            this.oneFlag.TabStop = true;
            this.oneFlag.Text = "One";
            this.oneFlag.UseVisualStyleBackColor = true;
            // 
            // allFlags
            // 
            this.allFlags.AutoSize = true;
            this.allFlags.Location = new System.Drawing.Point(9, 111);
            this.allFlags.Name = "allFlags";
            this.allFlags.Size = new System.Drawing.Size(78, 17);
            this.allFlags.TabIndex = 0;
            this.allFlags.TabStop = true;
            this.allFlags.Text = "All (up to 4)";
            this.allFlags.UseVisualStyleBackColor = true;
            // 
            // mapSizeGroup
            // 
            this.mapSizeGroup.Controls.Add(this.yTiles);
            this.mapSizeGroup.Controls.Add(this.label1);
            this.mapSizeGroup.Controls.Add(this.xTiles);
            this.mapSizeGroup.Location = new System.Drawing.Point(168, 246);
            this.mapSizeGroup.Name = "mapSizeGroup";
            this.mapSizeGroup.Size = new System.Drawing.Size(124, 63);
            this.mapSizeGroup.TabIndex = 8;
            this.mapSizeGroup.TabStop = false;
            this.mapSizeGroup.Text = "Map size in tiles";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(51, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(18, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "by";
            // 
            // flagsInLevel
            // 
            this.flagsInLevel.AutoSize = true;
            this.flagsInLevel.Location = new System.Drawing.Point(6, 140);
            this.flagsInLevel.Name = "flagsInLevel";
            this.flagsInLevel.Size = new System.Drawing.Size(35, 13);
            this.flagsInLevel.TabIndex = 5;
            this.flagsInLevel.Text = "label5";
            // 
            // yTiles
            // 
            this.yTiles.AllowSpace = false;
            this.yTiles.Location = new System.Drawing.Point(75, 22);
            this.yTiles.Name = "yTiles";
            this.yTiles.Size = new System.Drawing.Size(40, 20);
            this.yTiles.TabIndex = 9;
            this.yTiles.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // xTiles
            // 
            this.xTiles.AllowSpace = false;
            this.xTiles.Location = new System.Drawing.Point(6, 22);
            this.xTiles.MaxLength = 3;
            this.xTiles.Name = "xTiles";
            this.xTiles.Size = new System.Drawing.Size(40, 20);
            this.xTiles.TabIndex = 0;
            this.xTiles.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // timeLimitSeconds
            // 
            this.timeLimitSeconds.AllowSpace = false;
            this.timeLimitSeconds.Location = new System.Drawing.Point(42, 30);
            this.timeLimitSeconds.MaxLength = 2;
            this.timeLimitSeconds.Name = "timeLimitSeconds";
            this.timeLimitSeconds.Size = new System.Drawing.Size(25, 20);
            this.timeLimitSeconds.TabIndex = 6;
            this.timeLimitSeconds.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // timeLimitMinutes
            // 
            this.timeLimitMinutes.AllowSpace = false;
            this.timeLimitMinutes.Location = new System.Drawing.Point(9, 30);
            this.timeLimitMinutes.Margin = new System.Windows.Forms.Padding(3, 3, 4, 3);
            this.timeLimitMinutes.MaxLength = 1;
            this.timeLimitMinutes.Name = "timeLimitMinutes";
            this.timeLimitMinutes.Size = new System.Drawing.Size(25, 20);
            this.timeLimitMinutes.TabIndex = 4;
            this.timeLimitMinutes.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // LevelProperties
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(303, 382);
            this.Controls.Add(this.mapSizeGroup);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.terrainGroup);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "LevelProperties";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Level Properties";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.terrainGroup.ResumeLayout(false);
            this.terrainGroup.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.mapSizeGroup.ResumeLayout(false);
            this.mapSizeGroup.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox unlimitedTime;
        private System.Windows.Forms.TextBox levelName;
        private System.Windows.Forms.GroupBox groupBox2;
        private NumericTextBox timeLimitSeconds;
        private System.Windows.Forms.Label label2;
        private NumericTextBox timeLimitMinutes;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton spaceTerrain;
        private System.Windows.Forms.RadioButton snowTerrain;
        private System.Windows.Forms.RadioButton legoTerrain;
        private System.Windows.Forms.RadioButton grassTerrain;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.GroupBox groupBox4;
        private NumericTextBox xTiles;
        private System.Windows.Forms.Label label1;
        private NumericTextBox yTiles;
        private System.Windows.Forms.RadioButton fourFlags;
        private System.Windows.Forms.RadioButton threeFlags;
        private System.Windows.Forms.RadioButton twoFlags;
        private System.Windows.Forms.RadioButton oneFlag;
        private System.Windows.Forms.RadioButton allFlags;
        protected System.Windows.Forms.GroupBox terrainGroup;
        protected System.Windows.Forms.GroupBox mapSizeGroup;
        private System.Windows.Forms.Label flagsInLevel;
    }
}