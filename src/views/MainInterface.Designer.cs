namespace LemballEditor.View
{
    public partial class MainInterface
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
            this.components = new System.ComponentModel.Container();
            this.serviceController1 = new System.ServiceProcess.ServiceController();
            this.refreshMapTimer = new System.Windows.Forms.Timer(this.components);
            this.pallettePanel = new System.Windows.Forms.Panel();
            this.btnObjects = new System.Windows.Forms.Button();
            this.btnTiles = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveProjectAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.compileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testLevelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testAllLevelsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.compileReleasableLevelPackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.restoreOriginalLevelsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.propertiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.levelToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusBarMessage = new System.Windows.Forms.ToolStripStatusLabel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.mapPanelContainer = new System.Windows.Forms.GroupBox();
            this.levelBrowserGroup = new System.Windows.Forms.GroupBox();
            this.menuStrip1.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // refreshMapTimer
            // 
            this.refreshMapTimer.Enabled = true;
            this.refreshMapTimer.Interval = 1000;
            this.refreshMapTimer.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // pallettePanel
            // 
            this.pallettePanel.Location = new System.Drawing.Point(6, 58);
            this.pallettePanel.Name = "pallettePanel";
            this.pallettePanel.Size = new System.Drawing.Size(188, 253);
            this.pallettePanel.TabIndex = 8;
            // 
            // btnObjects
            // 
            this.btnObjects.Location = new System.Drawing.Point(6, 29);
            this.btnObjects.Name = "btnObjects";
            this.btnObjects.Size = new System.Drawing.Size(75, 23);
            this.btnObjects.TabIndex = 9;
            this.btnObjects.Text = "Objects";
            this.btnObjects.UseVisualStyleBackColor = true;
            this.btnObjects.Click += new System.EventHandler(this.btnObjects_Click);
            // 
            // btnTiles
            // 
            this.btnTiles.Location = new System.Drawing.Point(119, 29);
            this.btnTiles.Name = "btnTiles";
            this.btnTiles.Size = new System.Drawing.Size(75, 23);
            this.btnTiles.TabIndex = 10;
            this.btnTiles.Text = "Tiles";
            this.btnTiles.UseVisualStyleBackColor = true;
            this.btnTiles.Click += new System.EventHandler(this.btnTiles_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.compileToolStripMenuItem,
            this.propertiesToolStripMenuItem,
            this.settingsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(994, 24);
            this.menuStrip1.TabIndex = 11;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveProjectToolStripMenuItem,
            this.loadProjectToolStripMenuItem,
            this.saveProjectAsToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // saveProjectToolStripMenuItem
            // 
            this.saveProjectToolStripMenuItem.Name = "saveProjectToolStripMenuItem";
            this.saveProjectToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.saveProjectToolStripMenuItem.Text = "Save project";
            this.saveProjectToolStripMenuItem.ToolTipText = "Save the project as a file that can be edited later";
            this.saveProjectToolStripMenuItem.Click += new System.EventHandler(this.saveProjectToolStripMenuItem_Click);
            // 
            // loadProjectToolStripMenuItem
            // 
            this.loadProjectToolStripMenuItem.Name = "loadProjectToolStripMenuItem";
            this.loadProjectToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.loadProjectToolStripMenuItem.Text = "Load project";
            this.loadProjectToolStripMenuItem.ToolTipText = "Load a project file";
            this.loadProjectToolStripMenuItem.Click += new System.EventHandler(this.loadProjectToolStripMenuItem_Click);
            // 
            // saveProjectAsToolStripMenuItem
            // 
            this.saveProjectAsToolStripMenuItem.Name = "saveProjectAsToolStripMenuItem";
            this.saveProjectAsToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.saveProjectAsToolStripMenuItem.Text = "Save project as...";
            this.saveProjectAsToolStripMenuItem.Click += new System.EventHandler(this.saveProjectAsToolStripMenuItem_Click);
            // 
            // compileToolStripMenuItem
            // 
            this.compileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.testLevelToolStripMenuItem,
            this.testAllLevelsToolStripMenuItem,
            this.toolStripSeparator1,
            this.compileReleasableLevelPackToolStripMenuItem,
            this.toolStripSeparator2,
            this.restoreOriginalLevelsToolStripMenuItem});
            this.compileToolStripMenuItem.Name = "compileToolStripMenuItem";
            this.compileToolStripMenuItem.Size = new System.Drawing.Size(64, 20);
            this.compileToolStripMenuItem.Text = "Compile";
            // 
            // testLevelToolStripMenuItem
            // 
            this.testLevelToolStripMenuItem.Name = "testLevelToolStripMenuItem";
            this.testLevelToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.testLevelToolStripMenuItem.Size = new System.Drawing.Size(215, 22);
            this.testLevelToolStripMenuItem.Text = "Test current level";
            // 
            // testAllLevelsToolStripMenuItem
            // 
            this.testAllLevelsToolStripMenuItem.Name = "testAllLevelsToolStripMenuItem";
            this.testAllLevelsToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F6;
            this.testAllLevelsToolStripMenuItem.Size = new System.Drawing.Size(215, 22);
            this.testAllLevelsToolStripMenuItem.Text = "Test all levels";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(212, 6);
            // 
            // compileReleasableLevelPackToolStripMenuItem
            // 
            this.compileReleasableLevelPackToolStripMenuItem.Enabled = false;
            this.compileReleasableLevelPackToolStripMenuItem.Name = "compileReleasableLevelPackToolStripMenuItem";
            this.compileReleasableLevelPackToolStripMenuItem.Size = new System.Drawing.Size(215, 22);
            this.compileReleasableLevelPackToolStripMenuItem.Text = "Compile releasable version";
            this.compileReleasableLevelPackToolStripMenuItem.ToolTipText = "Create a level pack that can be distributed and played by others";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(212, 6);
            // 
            // restoreOriginalLevelsToolStripMenuItem
            // 
            this.restoreOriginalLevelsToolStripMenuItem.Name = "restoreOriginalLevelsToolStripMenuItem";
            this.restoreOriginalLevelsToolStripMenuItem.Size = new System.Drawing.Size(215, 22);
            this.restoreOriginalLevelsToolStripMenuItem.Text = "Restore original levels";
            this.restoreOriginalLevelsToolStripMenuItem.ToolTipText = "Restore the original Lemmings Paintball levels";
            this.restoreOriginalLevelsToolStripMenuItem.Click += new System.EventHandler(this.restoreOriginalLevelsToolStripMenuItem_Click);
            // 
            // propertiesToolStripMenuItem
            // 
            this.propertiesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.levelToolStripMenuItem1});
            this.propertiesToolStripMenuItem.Name = "propertiesToolStripMenuItem";
            this.propertiesToolStripMenuItem.Size = new System.Drawing.Size(72, 20);
            this.propertiesToolStripMenuItem.Text = "Properties";
            // 
            // levelToolStripMenuItem1
            // 
            this.levelToolStripMenuItem1.Name = "levelToolStripMenuItem1";
            this.levelToolStripMenuItem1.Size = new System.Drawing.Size(180, 22);
            this.levelToolStripMenuItem1.Text = "Level";
            this.levelToolStripMenuItem1.ToolTipText = "Change properties for the loaded level";
            this.levelToolStripMenuItem1.Click += new System.EventHandler(this.levelToolStripMenuItem1_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.settingsToolStripMenuItem.Text = "Settings";
            this.settingsToolStripMenuItem.ToolTipText = "Change the settings for Lemball Editor";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.statusBarMessage});
            this.statusStrip.Location = new System.Drawing.Point(0, 577);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.statusStrip.Size = new System.Drawing.Size(994, 22);
            this.statusStrip.TabIndex = 0;
            this.statusStrip.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // statusBarMessage
            // 
            this.statusBarMessage.Name = "statusBarMessage";
            this.statusBarMessage.Size = new System.Drawing.Size(118, 17);
            this.statusBarMessage.Text = "toolStripStatusLabel2";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.pallettePanel);
            this.groupBox2.Controls.Add(this.btnObjects);
            this.groupBox2.Controls.Add(this.btnTiles);
            this.groupBox2.Location = new System.Drawing.Point(784, 253);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 321);
            this.groupBox2.TabIndex = 13;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Objects and tiles";
            // 
            // mapPanelContainer
            // 
            this.mapPanelContainer.Location = new System.Drawing.Point(12, 27);
            this.mapPanelContainer.Name = "mapPanelContainer";
            this.mapPanelContainer.Size = new System.Drawing.Size(766, 547);
            this.mapPanelContainer.TabIndex = 14;
            this.mapPanelContainer.TabStop = false;
            this.mapPanelContainer.Text = "Level editor";
            // 
            // levelBrowserGroup
            // 
            this.levelBrowserGroup.Location = new System.Drawing.Point(784, 27);
            this.levelBrowserGroup.Name = "levelBrowserGroup";
            this.levelBrowserGroup.Size = new System.Drawing.Size(204, 220);
            this.levelBrowserGroup.TabIndex = 15;
            this.levelBrowserGroup.TabStop = false;
            this.levelBrowserGroup.Text = "Level browser";
            // 
            // MainInterface
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(994, 599);
            this.Controls.Add(this.levelBrowserGroup);
            this.Controls.Add(this.mapPanelContainer);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainInterface";
            this.Text = "Lemball Editor";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.ServiceProcess.ServiceController serviceController1;
        private System.Windows.Forms.Timer refreshMapTimer;
        private System.Windows.Forms.Panel pallettePanel;
        private System.Windows.Forms.Button btnObjects;
        private System.Windows.Forms.Button btnTiles;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveProjectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadProjectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem compileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem testLevelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem compileReleasableLevelPackToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem testAllLevelsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem saveProjectAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem propertiesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem levelToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem restoreOriginalLevelsToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel statusBarMessage;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox mapPanelContainer;
        private System.Windows.Forms.GroupBox levelBrowserGroup;
    }
}

