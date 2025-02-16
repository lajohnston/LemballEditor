namespace LemballEditor.View.Level.ObjectGraphics
{
    partial class ChangeLiftArea
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
            this.XSizeLabel = new System.Windows.Forms.Label();
            this.YSizeLabel = new System.Windows.Forms.Label();
            this.previewStartHeight = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.HeightGroup = new System.Windows.Forms.GroupBox();
            this.endHeight = new System.Windows.Forms.NumericUpDown();
            this.startHeight = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.xSize = new System.Windows.Forms.NumericUpDown();
            this.SizeGroup = new System.Windows.Forms.GroupBox();
            this.ySize = new System.Windows.Forms.NumericUpDown();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.touchOnce = new System.Windows.Forms.RadioButton();
            this.switchOnce = new System.Windows.Forms.RadioButton();
            this.switchMultiple = new System.Windows.Forms.RadioButton();
            this.startOfLevel = new System.Windows.Forms.RadioButton();
            this.previewEndHeight = new System.Windows.Forms.RadioButton();
            this.okButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.HeightGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.endHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.startHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xSize)).BeginInit();
            this.SizeGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ySize)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // XSizeLabel
            // 
            this.XSizeLabel.AutoSize = true;
            this.XSizeLabel.Location = new System.Drawing.Point(7, 15);
            this.XSizeLabel.Name = "XSizeLabel";
            this.XSizeLabel.Size = new System.Drawing.Size(68, 13);
            this.XSizeLabel.TabIndex = 2;
            this.XSizeLabel.Text = "X Size (Tiles)";
            // 
            // YSizeLabel
            // 
            this.YSizeLabel.AutoSize = true;
            this.YSizeLabel.Location = new System.Drawing.Point(86, 15);
            this.YSizeLabel.Name = "YSizeLabel";
            this.YSizeLabel.Size = new System.Drawing.Size(68, 13);
            this.YSizeLabel.TabIndex = 3;
            this.YSizeLabel.Text = "Y Size (Tiles)";
            // 
            // previewStartHeight
            // 
            this.previewStartHeight.AutoSize = true;
            this.previewStartHeight.Location = new System.Drawing.Point(7, 19);
            this.previewStartHeight.Name = "previewStartHeight";
            this.previewStartHeight.Size = new System.Drawing.Size(47, 17);
            this.previewStartHeight.TabIndex = 4;
            this.previewStartHeight.TabStop = true;
            this.previewStartHeight.Text = "Start";
            this.previewStartHeight.UseVisualStyleBackColor = true;
            this.previewStartHeight.CheckedChanged += new System.EventHandler(this.previewStartHeight_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.previewEndHeight);
            this.groupBox1.Controls.Add(this.previewStartHeight);
            this.groupBox1.Location = new System.Drawing.Point(6, 144);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(164, 42);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Preview Height";
            // 
            // HeightGroup
            // 
            this.HeightGroup.Controls.Add(this.endHeight);
            this.HeightGroup.Controls.Add(this.startHeight);
            this.HeightGroup.Controls.Add(this.label2);
            this.HeightGroup.Controls.Add(this.label1);
            this.HeightGroup.Location = new System.Drawing.Point(6, 69);
            this.HeightGroup.Name = "HeightGroup";
            this.HeightGroup.Size = new System.Drawing.Size(164, 69);
            this.HeightGroup.TabIndex = 6;
            this.HeightGroup.TabStop = false;
            this.HeightGroup.Text = "Height";
            // 
            // endHeight
            // 
            this.endHeight.Location = new System.Drawing.Point(86, 31);
            this.endHeight.Maximum = new decimal(new int[] {
            88,
            0,
            0,
            0});
            this.endHeight.Name = "endHeight";
            this.endHeight.Size = new System.Drawing.Size(68, 20);
            this.endHeight.TabIndex = 11;
            this.endHeight.ValueChanged += new System.EventHandler(this.endHeight_ValueChanged);
            // 
            // startHeight
            // 
            this.startHeight.Location = new System.Drawing.Point(7, 32);
            this.startHeight.Maximum = new decimal(new int[] {
            88,
            0,
            0,
            0});
            this.startHeight.Name = "startHeight";
            this.startHeight.Size = new System.Drawing.Size(65, 20);
            this.startHeight.TabIndex = 10;
            this.startHeight.ValueChanged += new System.EventHandler(this.startHeight_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(86, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(26, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "End";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Start";
            // 
            // xSize
            // 
            this.xSize.Location = new System.Drawing.Point(7, 31);
            this.xSize.Name = "xSize";
            this.xSize.Size = new System.Drawing.Size(65, 20);
            this.xSize.TabIndex = 7;
            this.xSize.ValueChanged += new System.EventHandler(this.xSize_ValueChanged);
            // 
            // SizeGroup
            // 
            this.SizeGroup.Controls.Add(this.ySize);
            this.SizeGroup.Controls.Add(this.XSizeLabel);
            this.SizeGroup.Controls.Add(this.xSize);
            this.SizeGroup.Controls.Add(this.YSizeLabel);
            this.SizeGroup.Location = new System.Drawing.Point(6, 1);
            this.SizeGroup.Name = "SizeGroup";
            this.SizeGroup.Size = new System.Drawing.Size(164, 69);
            this.SizeGroup.TabIndex = 8;
            this.SizeGroup.TabStop = false;
            this.SizeGroup.Text = "Size";
            // 
            // ySize
            // 
            this.ySize.Location = new System.Drawing.Point(86, 31);
            this.ySize.Maximum = new decimal(new int[] {
            88,
            0,
            0,
            0});
            this.ySize.Name = "ySize";
            this.ySize.Size = new System.Drawing.Size(68, 20);
            this.ySize.TabIndex = 9;
            this.ySize.ValueChanged += new System.EventHandler(this.ySize_ValueChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.touchOnce);
            this.groupBox2.Controls.Add(this.switchOnce);
            this.groupBox2.Controls.Add(this.switchMultiple);
            this.groupBox2.Controls.Add(this.startOfLevel);
            this.groupBox2.Location = new System.Drawing.Point(176, 1);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(121, 139);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Activation";
            // 
            // touchOnce
            // 
            this.touchOnce.AutoSize = true;
            this.touchOnce.Location = new System.Drawing.Point(6, 97);
            this.touchOnce.Name = "touchOnce";
            this.touchOnce.Size = new System.Drawing.Size(111, 17);
            this.touchOnce.TabIndex = 3;
            this.touchOnce.TabStop = true;
            this.touchOnce.Text = "Touch (once only)";
            this.touchOnce.UseVisualStyleBackColor = true;
            this.touchOnce.CheckedChanged += new System.EventHandler(this.touchOnce_CheckedChanged);
            // 
            // switchOnce
            // 
            this.switchOnce.AutoSize = true;
            this.switchOnce.Location = new System.Drawing.Point(6, 74);
            this.switchOnce.Name = "switchOnce";
            this.switchOnce.Size = new System.Drawing.Size(112, 17);
            this.switchOnce.TabIndex = 2;
            this.switchOnce.TabStop = true;
            this.switchOnce.Text = "Switch (once only)";
            this.switchOnce.UseVisualStyleBackColor = true;
            this.switchOnce.CheckedChanged += new System.EventHandler(this.switchOnce_CheckedChanged);
            // 
            // switchMultiple
            // 
            this.switchMultiple.AutoSize = true;
            this.switchMultiple.Location = new System.Drawing.Point(6, 51);
            this.switchMultiple.Name = "switchMultiple";
            this.switchMultiple.Size = new System.Drawing.Size(101, 17);
            this.switchMultiple.TabIndex = 1;
            this.switchMultiple.TabStop = true;
            this.switchMultiple.Text = "Switch (multiple)";
            this.switchMultiple.UseVisualStyleBackColor = true;
            this.switchMultiple.CheckedChanged += new System.EventHandler(this.switchMultiple_CheckedChanged);
            // 
            // startOfLevel
            // 
            this.startOfLevel.AutoSize = true;
            this.startOfLevel.Location = new System.Drawing.Point(6, 28);
            this.startOfLevel.Name = "startOfLevel";
            this.startOfLevel.Size = new System.Drawing.Size(95, 17);
            this.startOfLevel.TabIndex = 0;
            this.startOfLevel.TabStop = true;
            this.startOfLevel.Text = "At start of level";
            this.startOfLevel.UseVisualStyleBackColor = true;
            this.startOfLevel.CheckedChanged += new System.EventHandler(this.startOfLevel_CheckedChanged);
            // 
            // previewEndHeight
            // 
            this.previewEndHeight.AutoSize = true;
            this.previewEndHeight.Location = new System.Drawing.Point(86, 18);
            this.previewEndHeight.Name = "previewEndHeight";
            this.previewEndHeight.Size = new System.Drawing.Size(44, 17);
            this.previewEndHeight.TabIndex = 10;
            this.previewEndHeight.TabStop = true;
            this.previewEndHeight.Text = "End";
            this.previewEndHeight.UseVisualStyleBackColor = true;
            this.previewEndHeight.CheckedChanged += new System.EventHandler(this.previewEndHeight_CheckedChanged);
            // 
            // okButton
            // 
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(193, 144);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(87, 42);
            this.okButton.TabIndex = 10;
            this.okButton.Text = "Ok";
            this.okButton.UseVisualStyleBackColor = true;
            // 
            // ChangeLiftArea
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(303, 194);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.SizeGroup);
            this.Controls.Add(this.HeightGroup);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ChangeLiftArea";
            this.Text = "Set Lift Size";
            this.Load += new System.EventHandler(this.ChangeLiftArea_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.HeightGroup.ResumeLayout(false);
            this.HeightGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.endHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.startHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xSize)).EndInit();
            this.SizeGroup.ResumeLayout(false);
            this.SizeGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ySize)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label XSizeLabel;
        private System.Windows.Forms.Label YSizeLabel;
        private System.Windows.Forms.RadioButton previewStartHeight;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox HeightGroup;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown xSize;
        private System.Windows.Forms.GroupBox SizeGroup;
        private System.Windows.Forms.NumericUpDown ySize;
        private System.Windows.Forms.NumericUpDown endHeight;
        private System.Windows.Forms.NumericUpDown startHeight;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton startOfLevel;
        private System.Windows.Forms.RadioButton switchMultiple;
        private System.Windows.Forms.RadioButton switchOnce;
        private System.Windows.Forms.RadioButton touchOnce;
        private System.Windows.Forms.RadioButton previewEndHeight;
        private System.Windows.Forms.Button okButton;
    }
}