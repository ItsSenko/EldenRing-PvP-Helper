namespace ERPvPHelper.Features
{
    partial class ItemOptions
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
            this.InfusionBox = new System.Windows.Forms.ComboBox();
            this.AshOfWarBox = new System.Windows.Forms.ComboBox();
            this.DeleteBtn = new System.Windows.Forms.Button();
            this.SaveBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // InfusionBox
            // 
            this.InfusionBox.BackColor = System.Drawing.Color.Black;
            this.InfusionBox.ForeColor = System.Drawing.Color.Fuchsia;
            this.InfusionBox.FormattingEnabled = true;
            this.InfusionBox.Location = new System.Drawing.Point(12, 39);
            this.InfusionBox.Name = "InfusionBox";
            this.InfusionBox.Size = new System.Drawing.Size(165, 23);
            this.InfusionBox.TabIndex = 0;
            // 
            // AshOfWarBox
            // 
            this.AshOfWarBox.BackColor = System.Drawing.Color.Black;
            this.AshOfWarBox.ForeColor = System.Drawing.Color.Fuchsia;
            this.AshOfWarBox.FormattingEnabled = true;
            this.AshOfWarBox.Location = new System.Drawing.Point(183, 39);
            this.AshOfWarBox.Name = "AshOfWarBox";
            this.AshOfWarBox.Size = new System.Drawing.Size(161, 23);
            this.AshOfWarBox.TabIndex = 1;
            this.AshOfWarBox.SelectedIndexChanged += new System.EventHandler(this.AshOfWarBox_SelectedIndexChanged);
            // 
            // DeleteBtn
            // 
            this.DeleteBtn.BackColor = System.Drawing.Color.Transparent;
            this.DeleteBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.DeleteBtn.ForeColor = System.Drawing.Color.Fuchsia;
            this.DeleteBtn.Location = new System.Drawing.Point(12, 68);
            this.DeleteBtn.Name = "DeleteBtn";
            this.DeleteBtn.Size = new System.Drawing.Size(165, 36);
            this.DeleteBtn.TabIndex = 2;
            this.DeleteBtn.Text = "Delete";
            this.DeleteBtn.UseVisualStyleBackColor = false;
            this.DeleteBtn.Click += new System.EventHandler(this.DeleteBtn_Click);
            // 
            // SaveBtn
            // 
            this.SaveBtn.BackColor = System.Drawing.Color.Transparent;
            this.SaveBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SaveBtn.ForeColor = System.Drawing.Color.Fuchsia;
            this.SaveBtn.Location = new System.Drawing.Point(183, 68);
            this.SaveBtn.Name = "SaveBtn";
            this.SaveBtn.Size = new System.Drawing.Size(161, 36);
            this.SaveBtn.TabIndex = 3;
            this.SaveBtn.Text = "Save";
            this.SaveBtn.UseVisualStyleBackColor = false;
            this.SaveBtn.Click += new System.EventHandler(this.SaveBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label1.ForeColor = System.Drawing.Color.Fuchsia;
            this.label1.Location = new System.Drawing.Point(47, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 30);
            this.label1.TabIndex = 4;
            this.label1.Text = "Infusion";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label2.ForeColor = System.Drawing.Color.Fuchsia;
            this.label2.Location = new System.Drawing.Point(203, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(116, 30);
            this.label2.TabIndex = 5;
            this.label2.Text = "Ash of War";
            // 
            // ItemOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(356, 112);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.SaveBtn);
            this.Controls.Add(this.DeleteBtn);
            this.Controls.Add(this.AshOfWarBox);
            this.Controls.Add(this.InfusionBox);
            this.Name = "ItemOptions";
            this.Text = "Item Options";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ComboBox InfusionBox;
        private ComboBox AshOfWarBox;
        private Button DeleteBtn;
        private Button SaveBtn;
        private Label label1;
        private Label label2;
    }
}