namespace Liquid
{
    partial class EntryForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EntryForm));
            this.initButton = new MaterialSkin.Controls.MaterialRaisedButton();
            this.launcherButton = new MaterialSkin.Controls.MaterialRaisedButton();
            this.SuspendLayout();
            // 
            // initButton
            // 
            this.initButton.BackColor = System.Drawing.SystemColors.HotTrack;
            this.initButton.Depth = 0;
            this.initButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.initButton.Location = new System.Drawing.Point(-15, 90);
            this.initButton.Margin = new System.Windows.Forms.Padding(12, 6, 6, 6);
            this.initButton.MouseState = MaterialSkin.MouseState.HOVER;
            this.initButton.Name = "initButton";
            this.initButton.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.initButton.Primary = true;
            this.initButton.Size = new System.Drawing.Size(415, 79);
            this.initButton.TabIndex = 10;
            this.initButton.Text = "Connect to the Client";
            this.initButton.UseVisualStyleBackColor = false;
            this.initButton.Click += new System.EventHandler(this.initButton_Click);
            // 
            // launcherButton
            // 
            this.launcherButton.BackColor = System.Drawing.Color.Peru;
            this.launcherButton.Depth = 0;
            this.launcherButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 28.125F);
            this.launcherButton.ForeColor = System.Drawing.Color.Black;
            this.launcherButton.Location = new System.Drawing.Point(-15, -5);
            this.launcherButton.Margin = new System.Windows.Forms.Padding(6);
            this.launcherButton.MouseState = MaterialSkin.MouseState.HOVER;
            this.launcherButton.Name = "launcherButton";
            this.launcherButton.Primary = true;
            this.launcherButton.Size = new System.Drawing.Size(415, 83);
            this.launcherButton.TabIndex = 13;
            this.launcherButton.Text = "Launch The Game";
            this.launcherButton.UseVisualStyleBackColor = false;
            // 
            // EntryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(392, 168);
            this.Controls.Add(this.launcherButton);
            this.Controls.Add(this.initButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.MaximizeBox = false;
            this.Name = "EntryForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.ResumeLayout(false);

        }

        #endregion
        private MaterialSkin.Controls.MaterialRaisedButton initButton;
        private MaterialSkin.Controls.MaterialRaisedButton launcherButton;
    }
}