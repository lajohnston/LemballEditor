namespace LemballEditor.View
{
    partial class ObjectsList
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
            this.lstObjects = new System.Windows.Forms.ListBox();
            this.lblObjectLimit = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lstObjects
            // 
            this.lstObjects.FormattingEnabled = true;
            this.lstObjects.Location = new System.Drawing.Point(3, 8);
            this.lstObjects.Name = "lstObjects";
            this.lstObjects.Size = new System.Drawing.Size(182, 173);
            this.lstObjects.TabIndex = 0;
            this.lstObjects.SelectedIndexChanged += new System.EventHandler(this.lstObjects_SelectedIndexChanged);
            // 
            // lblObjectLimit
            // 
            this.lblObjectLimit.AutoSize = true;
            this.lblObjectLimit.Location = new System.Drawing.Point(8, 187);
            this.lblObjectLimit.Name = "lblObjectLimit";
            this.lblObjectLimit.Size = new System.Drawing.Size(109, 13);
            this.lblObjectLimit.TabIndex = 2;
            this.lblObjectLimit.Text = "Object limit remaining:";
            // 
            // ObjectsList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblObjectLimit);
            this.Controls.Add(this.lstObjects);
            this.Name = "ObjectsList";
            this.Size = new System.Drawing.Size(188, 210);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lstObjects;
        private System.Windows.Forms.Label lblObjectLimit;
    }
}
