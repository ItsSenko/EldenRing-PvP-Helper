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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            this.ChangeBackColorBtn = new System.Windows.Forms.Button();
            this.ChangeForeColorBtn = new System.Windows.Forms.Button();
            this.SeamlessCheckToggle = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // ChangeBackColorBtn
            // 
            this.ChangeBackColorBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ChangeBackColorBtn.ForeColor = System.Drawing.Color.Fuchsia;
            this.ChangeBackColorBtn.Location = new System.Drawing.Point(12, 12);
            this.ChangeBackColorBtn.Name = "ChangeBackColorBtn";
            this.ChangeBackColorBtn.Size = new System.Drawing.Size(121, 32);
            this.ChangeBackColorBtn.TabIndex = 5;
            this.ChangeBackColorBtn.Text = "Change Back Color";
            this.ChangeBackColorBtn.UseVisualStyleBackColor = true;
            this.ChangeBackColorBtn.Click += new System.EventHandler(this.ChangeBackColorBtn_Click);
            // 
            // ChangeForeColorBtn
            // 
            this.ChangeForeColorBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ChangeForeColorBtn.ForeColor = System.Drawing.Color.Fuchsia;
            this.ChangeForeColorBtn.Location = new System.Drawing.Point(139, 12);
            this.ChangeForeColorBtn.Name = "ChangeForeColorBtn";
            this.ChangeForeColorBtn.Size = new System.Drawing.Size(121, 32);
            this.ChangeForeColorBtn.TabIndex = 6;
            this.ChangeForeColorBtn.Text = "Change Fore Color";
            this.ChangeForeColorBtn.UseVisualStyleBackColor = true;
            this.ChangeForeColorBtn.Click += new System.EventHandler(this.ChangeForeColorBtn_Click);
            // 
            // SeamlessCheckToggle
            // 
            this.SeamlessCheckToggle.AutoSize = true;
            this.SeamlessCheckToggle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.SeamlessCheckToggle.Location = new System.Drawing.Point(12, 50);
            this.SeamlessCheckToggle.Name = "SeamlessCheckToggle";
            this.SeamlessCheckToggle.Size = new System.Drawing.Size(151, 19);
            this.SeamlessCheckToggle.TabIndex = 7;
            this.SeamlessCheckToggle.Text = "Disable Seamless Check";
            this.SeamlessCheckToggle.UseVisualStyleBackColor = true;
            this.SeamlessCheckToggle.CheckedChanged += new System.EventHandler(this.SeamlessCheckToggle_CheckedChanged);
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(272, 83);
            this.Controls.Add(this.SeamlessCheckToggle);
            this.Controls.Add(this.ChangeForeColorBtn);
            this.Controls.Add(this.ChangeBackColorBtn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SettingsForm";
            this.Text = "Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Button ChangeBackColorBtn;
        private Button ChangeForeColorBtn;
        private CheckBox SeamlessCheckToggle;
    }
}