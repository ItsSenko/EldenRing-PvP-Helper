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
            this.ChrTypeLabel = new System.Windows.Forms.Label();
            this.ChrTypeSelection = new System.Windows.Forms.ComboBox();
            this.TestBtn = new System.Windows.Forms.Button();
            this.ResetColorBtn = new System.Windows.Forms.Button();
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
            // OtherOptionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(419, 155);
            this.Controls.Add(this.ResetColorBtn);
            this.Controls.Add(this.TestBtn);
            this.Controls.Add(this.ChrTypeSelection);
            this.Controls.Add(this.ChrTypeLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "OtherOptionsForm";
            this.Text = "Other Options";
            this.Load += new System.EventHandler(this.OtherOptionsForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Label ChrTypeLabel;
        private ComboBox ChrTypeSelection;
        private Button TestBtn;
        private Button ResetColorBtn;
    }
}