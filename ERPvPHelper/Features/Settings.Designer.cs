namespace ERPvPHelper
{
    partial class SettingsForm
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
            this.ShowHitboxToggle = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // ShowHitboxToggle
            // 
            this.ShowHitboxToggle.AutoSize = true;
            this.ShowHitboxToggle.ForeColor = System.Drawing.Color.Fuchsia;
            this.ShowHitboxToggle.Location = new System.Drawing.Point(12, 12);
            this.ShowHitboxToggle.Name = "ShowHitboxToggle";
            this.ShowHitboxToggle.Size = new System.Drawing.Size(105, 19);
            this.ShowHitboxToggle.TabIndex = 0;
            this.ShowHitboxToggle.Text = "Show Hitboxes";
            this.ShowHitboxToggle.UseVisualStyleBackColor = true;
            this.ShowHitboxToggle.CheckedChanged += new System.EventHandler(this.ShowHitboxToggle_CheckedChanged);
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(419, 155);
            this.Controls.Add(this.ShowHitboxToggle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "SettingsForm";
            this.Text = "Other Options";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CheckBox ShowHitboxToggle;
    }
}