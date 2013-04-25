namespace LemballEditor.View.Settings
{
    partial class Settings
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
            this.browseExe = new System.Windows.Forms.Button();
            this.exePath = new System.Windows.Forms.TextBox();
            this.ok = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.enableSoundEffects = new System.Windows.Forms.CheckBox();
            this.enableMusic = new System.Windows.Forms.CheckBox();
            this.enableMovies = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.browseExe);
            this.groupBox1.Controls.Add(this.exePath);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(678, 48);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Lemmings Paintball EXE Path";
            // 
            // browseExe
            // 
            this.browseExe.Location = new System.Drawing.Point(635, 18);
            this.browseExe.Name = "browseExe";
            this.browseExe.Size = new System.Drawing.Size(32, 21);
            this.browseExe.TabIndex = 1;
            this.browseExe.Text = "...";
            this.browseExe.UseVisualStyleBackColor = true;
            this.browseExe.Click += new System.EventHandler(this.button1_Click);
            // 
            // exePath
            // 
            this.exePath.Location = new System.Drawing.Point(6, 19);
            this.exePath.Name = "exePath";
            this.exePath.Size = new System.Drawing.Size(623, 20);
            this.exePath.TabIndex = 0;
            // 
            // ok
            // 
            this.ok.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ok.Location = new System.Drawing.Point(259, 178);
            this.ok.Name = "ok";
            this.ok.Size = new System.Drawing.Size(75, 32);
            this.ok.TabIndex = 1;
            this.ok.Text = "Ok";
            this.ok.UseVisualStyleBackColor = true;
            this.ok.Click += new System.EventHandler(this.ok_Click);
            // 
            // cancel
            // 
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(366, 178);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 32);
            this.cancel.TabIndex = 2;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.enableSoundEffects);
            this.groupBox2.Controls.Add(this.enableMusic);
            this.groupBox2.Controls.Add(this.enableMovies);
            this.groupBox2.Location = new System.Drawing.Point(12, 66);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(678, 93);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Run options (disabling options improves the game\'s startup speed when testing lev" +
                "els)";
            // 
            // enableSoundEffects
            // 
            this.enableSoundEffects.AutoSize = true;
            this.enableSoundEffects.Location = new System.Drawing.Point(6, 65);
            this.enableSoundEffects.Name = "enableSoundEffects";
            this.enableSoundEffects.Size = new System.Drawing.Size(126, 17);
            this.enableSoundEffects.TabIndex = 2;
            this.enableSoundEffects.Text = "Enable sound effects";
            this.enableSoundEffects.UseVisualStyleBackColor = true;
            // 
            // enableMusic
            // 
            this.enableMusic.AutoSize = true;
            this.enableMusic.Location = new System.Drawing.Point(6, 42);
            this.enableMusic.Name = "enableMusic";
            this.enableMusic.Size = new System.Drawing.Size(89, 17);
            this.enableMusic.TabIndex = 1;
            this.enableMusic.Text = "Enable music";
            this.enableMusic.UseVisualStyleBackColor = true;
            // 
            // enableMovies
            // 
            this.enableMovies.AutoSize = true;
            this.enableMovies.Location = new System.Drawing.Point(6, 19);
            this.enableMovies.Name = "enableMovies";
            this.enableMovies.Size = new System.Drawing.Size(95, 17);
            this.enableMovies.TabIndex = 0;
            this.enableMovies.Text = "Enable movies";
            this.enableMovies.UseVisualStyleBackColor = true;
            // 
            // Settings
            // 
            this.AcceptButton = this.ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.ClientSize = new System.Drawing.Size(702, 222);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.ok);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Settings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Settings";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button browseExe;
        private System.Windows.Forms.TextBox exePath;
        private System.Windows.Forms.Button ok;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox enableSoundEffects;
        private System.Windows.Forms.CheckBox enableMusic;
        private System.Windows.Forms.CheckBox enableMovies;
    }
}