namespace ERPvPHelper
{
    partial class OtherOptionsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OtherOptionsForm));
            this.ChrTypeLabel = new System.Windows.Forms.Label();
            this.ChrTypeSelection = new System.Windows.Forms.ComboBox();
            this.TestBtn = new System.Windows.Forms.Button();
            this.ResetColorBtn = new System.Windows.Forms.Button();
            this.CustomFPS = new System.Windows.Forms.NumericUpDown();
            this.CustomFPSToggle = new System.Windows.Forms.CheckBox();
            this.CustomFOVToggle = new System.Windows.Forms.CheckBox();
            this.CustomFOV = new System.Windows.Forms.NumericUpDown();
            this.ShowHitboxesToggle = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.CustomFPS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CustomFOV)).BeginInit();
            this.SuspendLayout();
            // 
            // ChrTypeLabel
            // 
            this.ChrTypeLabel.AutoSize = true;
            this.ChrTypeLabel.ForeColor = System.Drawing.Color.Purple;
            this.ChrTypeLabel.Location = new System.Drawing.Point(22, 10);
            this.ChrTypeLabel.Name = "ChrTypeLabel";
            this.ChrTypeLabel.Size = new System.Drawing.Size(85, 15);
            this.ChrTypeLabel.TabIndex = 1;
            this.ChrTypeLabel.Text = "Character Type";
            // 
            // ChrTypeSelection
            // 
            this.ChrTypeSelection.BackColor = System.Drawing.Color.Black;
            this.ChrTypeSelection.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ChrTypeSelection.ForeColor = System.Drawing.Color.Purple;
            this.ChrTypeSelection.FormattingEnabled = true;
            this.ChrTypeSelection.Location = new System.Drawing.Point(22, 28);
            this.ChrTypeSelection.Name = "ChrTypeSelection";
            this.ChrTypeSelection.Size = new System.Drawing.Size(132, 23);
            this.ChrTypeSelection.TabIndex = 2;
            this.ChrTypeSelection.TabStop = false;
            this.ChrTypeSelection.Text = "Select ChrType...";
            this.ChrTypeSelection.SelectedIndexChanged += new System.EventHandler(this.ChrTypeSelection_SelectedIndexChanged);
            // 
            // TestBtn
            // 
            this.TestBtn.BackColor = System.Drawing.Color.Black;
            this.TestBtn.FlatAppearance.BorderColor = System.Drawing.Color.Purple;
            this.TestBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.TestBtn.ForeColor = System.Drawing.Color.Purple;
            this.TestBtn.Location = new System.Drawing.Point(22, 57);
            this.TestBtn.Name = "TestBtn";
            this.TestBtn.Size = new System.Drawing.Size(132, 27);
            this.TestBtn.TabIndex = 3;
            this.TestBtn.Text = "Custom Color";
            this.TestBtn.UseVisualStyleBackColor = false;
            this.TestBtn.Click += new System.EventHandler(this.CustomColor_Click);
            // 
            // ResetColorBtn
            // 
            this.ResetColorBtn.BackColor = System.Drawing.Color.Black;
            this.ResetColorBtn.FlatAppearance.BorderColor = System.Drawing.Color.Purple;
            this.ResetColorBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ResetColorBtn.ForeColor = System.Drawing.Color.Purple;
            this.ResetColorBtn.Location = new System.Drawing.Point(22, 90);
            this.ResetColorBtn.Name = "ResetColorBtn";
            this.ResetColorBtn.Size = new System.Drawing.Size(132, 27);
            this.ResetColorBtn.TabIndex = 4;
            this.ResetColorBtn.Text = "Reset Color";
            this.ResetColorBtn.UseVisualStyleBackColor = false;
            this.ResetColorBtn.Click += new System.EventHandler(this.ResetColorBtn_Click);
            // 
            // CustomFPS
            // 
            this.CustomFPS.BackColor = System.Drawing.Color.Black;
            this.CustomFPS.ForeColor = System.Drawing.Color.Fuchsia;
            this.CustomFPS.Location = new System.Drawing.Point(259, 61);
            this.CustomFPS.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.CustomFPS.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.CustomFPS.Name = "CustomFPS";
            this.CustomFPS.Size = new System.Drawing.Size(90, 23);
            this.CustomFPS.TabIndex = 5;
            this.CustomFPS.Value = new decimal(new int[] {
            75,
            0,
            0,
            0});
            this.CustomFPS.ValueChanged += new System.EventHandler(this.CustomFPS_ValueChanged);
            // 
            // CustomFPSToggle
            // 
            this.CustomFPSToggle.AutoSize = true;
            this.CustomFPSToggle.ForeColor = System.Drawing.Color.Fuchsia;
            this.CustomFPSToggle.Location = new System.Drawing.Point(163, 62);
            this.CustomFPSToggle.Name = "CustomFPSToggle";
            this.CustomFPSToggle.Size = new System.Drawing.Size(90, 19);
            this.CustomFPSToggle.TabIndex = 6;
            this.CustomFPSToggle.Text = "Custom FPS";
            this.CustomFPSToggle.UseVisualStyleBackColor = true;
            this.CustomFPSToggle.CheckedChanged += new System.EventHandler(this.CustomFPSToggle_CheckedChanged);
            // 
            // CustomFOVToggle
            // 
            this.CustomFOVToggle.AutoSize = true;
            this.CustomFOVToggle.ForeColor = System.Drawing.Color.Fuchsia;
            this.CustomFOVToggle.Location = new System.Drawing.Point(163, 95);
            this.CustomFOVToggle.Name = "CustomFOVToggle";
            this.CustomFOVToggle.Size = new System.Drawing.Size(93, 19);
            this.CustomFOVToggle.TabIndex = 8;
            this.CustomFOVToggle.Text = "Custom FOV";
            this.CustomFOVToggle.UseVisualStyleBackColor = true;
            this.CustomFOVToggle.CheckedChanged += new System.EventHandler(this.CustomFOVToggle_CheckedChanged);
            // 
            // CustomFOV
            // 
            this.CustomFOV.BackColor = System.Drawing.Color.Black;
            this.CustomFOV.ForeColor = System.Drawing.Color.Fuchsia;
            this.CustomFOV.Location = new System.Drawing.Point(259, 94);
            this.CustomFOV.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.CustomFOV.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.CustomFOV.Name = "CustomFOV";
            this.CustomFOV.Size = new System.Drawing.Size(90, 23);
            this.CustomFOV.TabIndex = 7;
            this.CustomFOV.Value = new decimal(new int[] {
            75,
            0,
            0,
            0});
            this.CustomFOV.ValueChanged += new System.EventHandler(this.CustomFOV_ValueChanged);
            // 
            // ShowHitboxesToggle
            // 
            this.ShowHitboxesToggle.AutoSize = true;
            this.ShowHitboxesToggle.ForeColor = System.Drawing.Color.Fuchsia;
            this.ShowHitboxesToggle.Location = new System.Drawing.Point(163, 28);
            this.ShowHitboxesToggle.Name = "ShowHitboxesToggle";
            this.ShowHitboxesToggle.Size = new System.Drawing.Size(105, 19);
            this.ShowHitboxesToggle.TabIndex = 9;
            this.ShowHitboxesToggle.Text = "Show Hitboxes";
            this.ShowHitboxesToggle.UseVisualStyleBackColor = true;
            this.ShowHitboxesToggle.CheckedChanged += new System.EventHandler(this.ShowHitboxesToggle_CheckedChanged);
            // 
            // OtherOptionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(376, 130);
            this.Controls.Add(this.ShowHitboxesToggle);
            this.Controls.Add(this.CustomFOVToggle);
            this.Controls.Add(this.CustomFOV);
            this.Controls.Add(this.CustomFPSToggle);
            this.Controls.Add(this.CustomFPS);
            this.Controls.Add(this.ResetColorBtn);
            this.Controls.Add(this.TestBtn);
            this.Controls.Add(this.ChrTypeSelection);
            this.Controls.Add(this.ChrTypeLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "OtherOptionsForm";
            this.Text = "Other Options";
            this.Load += new System.EventHandler(this.OtherOptionsForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.CustomFPS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CustomFOV)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Label ChrTypeLabel;
        private ComboBox ChrTypeSelection;
        private Button TestBtn;
        private Button ResetColorBtn;
        private NumericUpDown CustomFPS;
        private CheckBox CustomFPSToggle;
        private CheckBox CustomFOVToggle;
        private NumericUpDown CustomFOV;
        private CheckBox ShowHitboxesToggle;
    }
}