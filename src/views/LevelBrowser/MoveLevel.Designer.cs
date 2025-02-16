namespace LemballEditor.View
{
    public partial class LevelBrowser
    {
        partial class MoveLevel
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
                this.levelGroupList = new System.Windows.Forms.ListBox();
                this.move = new System.Windows.Forms.Button();
                this.cancel = new System.Windows.Forms.Button();
                this.copy = new System.Windows.Forms.Button();
                this.SuspendLayout();
                // 
                // levelGroupList
                // 
                this.levelGroupList.FormattingEnabled = true;
                this.levelGroupList.Location = new System.Drawing.Point(12, 12);
                this.levelGroupList.Name = "levelGroupList";
                this.levelGroupList.Size = new System.Drawing.Size(102, 95);
                this.levelGroupList.TabIndex = 0;
                // 
                // move
                // 
                this.move.Location = new System.Drawing.Point(132, 12);
                this.move.Name = "move";
                this.move.Size = new System.Drawing.Size(79, 28);
                this.move.TabIndex = 1;
                this.move.Text = "Move";
                this.move.UseVisualStyleBackColor = true;
                this.move.Click += new System.EventHandler(this.move_Click);
                // 
                // cancel
                // 
                this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                this.cancel.Location = new System.Drawing.Point(132, 79);
                this.cancel.Name = "cancel";
                this.cancel.Size = new System.Drawing.Size(79, 28);
                this.cancel.TabIndex = 2;
                this.cancel.Text = "Cancel";
                this.cancel.UseVisualStyleBackColor = true;
                // 
                // copy
                // 
                this.copy.Location = new System.Drawing.Point(132, 45);
                this.copy.Name = "copy";
                this.copy.Size = new System.Drawing.Size(79, 28);
                this.copy.TabIndex = 3;
                this.copy.Text = "Copy";
                this.copy.UseVisualStyleBackColor = true;
                this.copy.Click += new System.EventHandler(this.copy_Click);
                // 
                // MoveLevel
                // 
                this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
                this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.ClientSize = new System.Drawing.Size(234, 121);
                this.Controls.Add(this.copy);
                this.Controls.Add(this.cancel);
                this.Controls.Add(this.move);
                this.Controls.Add(this.levelGroupList);
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
                this.MaximizeBox = false;
                this.MinimizeBox = false;
                this.Name = "MoveLevel";
                this.Text = "Move or copy level to another group";
                this.ResumeLayout(false);

            }

            #endregion

            private System.Windows.Forms.ListBox levelGroupList;
            private System.Windows.Forms.Button move;
            private System.Windows.Forms.Button cancel;
            private System.Windows.Forms.Button copy;
        }
    }
}