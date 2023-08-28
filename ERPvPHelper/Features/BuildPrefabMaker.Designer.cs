namespace ERPvPHelper.Features
{
    partial class BuildPrefabMaker
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BuildPrefabMaker));
            this.ItemsBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.CategoryBox = new System.Windows.Forms.ComboBox();
            this.InfusionsBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.QuantityBox = new System.Windows.Forms.NumericUpDown();
            this.UpgradeBox = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.GibBtn = new System.Windows.Forms.Button();
            this.AshOfWarBox = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.InventoryLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.SaveBtn = new System.Windows.Forms.Button();
            this.BuildName = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.WeaponsBtn = new System.Windows.Forms.Button();
            this.ArmorsBtn = new System.Windows.Forms.Button();
            this.TalismanBtn = new System.Windows.Forms.Button();
            this.MoveItemsToggle = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.QuantityBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UpgradeBox)).BeginInit();
            this.SuspendLayout();
            // 
            // ItemsBox
            // 
            this.ItemsBox.BackColor = System.Drawing.Color.Black;
            this.ItemsBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple;
            this.ItemsBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.ItemsBox.FormattingEnabled = true;
            this.ItemsBox.Location = new System.Drawing.Point(2, 135);
            this.ItemsBox.Name = "ItemsBox";
            this.ItemsBox.Size = new System.Drawing.Size(254, 382);
            this.ItemsBox.TabIndex = 0;
            this.ItemsBox.SelectedIndexChanged += new System.EventHandler(this.ItemsBox_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Fuchsia;
            this.label1.Location = new System.Drawing.Point(2, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "Category";
            // 
            // CategoryBox
            // 
            this.CategoryBox.BackColor = System.Drawing.Color.Black;
            this.CategoryBox.ForeColor = System.Drawing.Color.Fuchsia;
            this.CategoryBox.FormattingEnabled = true;
            this.CategoryBox.Location = new System.Drawing.Point(2, 23);
            this.CategoryBox.Name = "CategoryBox";
            this.CategoryBox.Size = new System.Drawing.Size(131, 23);
            this.CategoryBox.TabIndex = 3;
            this.CategoryBox.SelectedIndexChanged += new System.EventHandler(this.CategoryBox_SelectedIndexChanged);
            // 
            // InfusionsBox
            // 
            this.InfusionsBox.BackColor = System.Drawing.Color.Black;
            this.InfusionsBox.Enabled = false;
            this.InfusionsBox.ForeColor = System.Drawing.Color.Fuchsia;
            this.InfusionsBox.FormattingEnabled = true;
            this.InfusionsBox.Location = new System.Drawing.Point(2, 76);
            this.InfusionsBox.Name = "InfusionsBox";
            this.InfusionsBox.Size = new System.Drawing.Size(131, 23);
            this.InfusionsBox.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Fuchsia;
            this.label2.Location = new System.Drawing.Point(2, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 15);
            this.label2.TabIndex = 5;
            this.label2.Text = "Infusion";
            // 
            // QuantityBox
            // 
            this.QuantityBox.Location = new System.Drawing.Point(139, 23);
            this.QuantityBox.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.QuantityBox.Name = "QuantityBox";
            this.QuantityBox.Size = new System.Drawing.Size(64, 23);
            this.QuantityBox.TabIndex = 6;
            this.QuantityBox.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // UpgradeBox
            // 
            this.UpgradeBox.Location = new System.Drawing.Point(139, 76);
            this.UpgradeBox.Name = "UpgradeBox";
            this.UpgradeBox.Size = new System.Drawing.Size(64, 23);
            this.UpgradeBox.TabIndex = 7;
            this.UpgradeBox.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Fuchsia;
            this.label3.Location = new System.Drawing.Point(139, 5);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 15);
            this.label3.TabIndex = 8;
            this.label3.Text = "Quantity";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.Fuchsia;
            this.label4.Location = new System.Drawing.Point(139, 58);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 15);
            this.label4.TabIndex = 9;
            this.label4.Text = "Upgrade";
            // 
            // GibBtn
            // 
            this.GibBtn.BackColor = System.Drawing.Color.Transparent;
            this.GibBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.GibBtn.ForeColor = System.Drawing.Color.Fuchsia;
            this.GibBtn.Location = new System.Drawing.Point(209, 76);
            this.GibBtn.Name = "GibBtn";
            this.GibBtn.Size = new System.Drawing.Size(42, 23);
            this.GibBtn.TabIndex = 10;
            this.GibBtn.Text = "Add";
            this.GibBtn.UseVisualStyleBackColor = false;
            this.GibBtn.Click += new System.EventHandler(this.GibBtn_Click);
            // 
            // AshOfWarBox
            // 
            this.AshOfWarBox.BackColor = System.Drawing.Color.Black;
            this.AshOfWarBox.Enabled = false;
            this.AshOfWarBox.ForeColor = System.Drawing.Color.Fuchsia;
            this.AshOfWarBox.FormattingEnabled = true;
            this.AshOfWarBox.Location = new System.Drawing.Point(69, 105);
            this.AshOfWarBox.Name = "AshOfWarBox";
            this.AshOfWarBox.Size = new System.Drawing.Size(177, 23);
            this.AshOfWarBox.TabIndex = 11;
            this.AshOfWarBox.SelectedIndexChanged += new System.EventHandler(this.AshOfWarBox_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.Fuchsia;
            this.label5.Location = new System.Drawing.Point(2, 110);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 15);
            this.label5.TabIndex = 12;
            this.label5.Text = "Ash of War";
            // 
            // InventoryLayoutPanel
            // 
            this.InventoryLayoutPanel.AllowDrop = true;
            this.InventoryLayoutPanel.AutoScroll = true;
            this.InventoryLayoutPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.InventoryLayoutPanel.ForeColor = System.Drawing.Color.Fuchsia;
            this.InventoryLayoutPanel.Location = new System.Drawing.Point(386, 12);
            this.InventoryLayoutPanel.Name = "InventoryLayoutPanel";
            this.InventoryLayoutPanel.Size = new System.Drawing.Size(445, 462);
            this.InventoryLayoutPanel.TabIndex = 13;
            this.InventoryLayoutPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.InventoryLayoutPanel_Paint);
            // 
            // CancelBtn
            // 
            this.CancelBtn.BackColor = System.Drawing.Color.Transparent;
            this.CancelBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CancelBtn.ForeColor = System.Drawing.Color.Fuchsia;
            this.CancelBtn.Location = new System.Drawing.Point(267, 408);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(113, 30);
            this.CancelBtn.TabIndex = 14;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = false;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // SaveBtn
            // 
            this.SaveBtn.BackColor = System.Drawing.Color.Transparent;
            this.SaveBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SaveBtn.ForeColor = System.Drawing.Color.Fuchsia;
            this.SaveBtn.Location = new System.Drawing.Point(267, 444);
            this.SaveBtn.Name = "SaveBtn";
            this.SaveBtn.Size = new System.Drawing.Size(113, 30);
            this.SaveBtn.TabIndex = 15;
            this.SaveBtn.Text = "Save";
            this.SaveBtn.UseVisualStyleBackColor = false;
            this.SaveBtn.Click += new System.EventHandler(this.SaveBtn_Click);
            // 
            // BuildName
            // 
            this.BuildName.BackColor = System.Drawing.Color.Black;
            this.BuildName.ForeColor = System.Drawing.Color.Fuchsia;
            this.BuildName.Location = new System.Drawing.Point(394, 480);
            this.BuildName.Name = "BuildName";
            this.BuildName.Size = new System.Drawing.Size(437, 23);
            this.BuildName.TabIndex = 16;
            this.BuildName.Text = "Best Build fr fr";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label6.ForeColor = System.Drawing.Color.Fuchsia;
            this.label6.Location = new System.Drawing.Point(267, 477);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(121, 30);
            this.label6.TabIndex = 17;
            this.label6.Text = "Build Name";
            // 
            // WeaponsBtn
            // 
            this.WeaponsBtn.BackColor = System.Drawing.Color.Transparent;
            this.WeaponsBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.WeaponsBtn.ForeColor = System.Drawing.Color.Fuchsia;
            this.WeaponsBtn.Location = new System.Drawing.Point(267, 12);
            this.WeaponsBtn.Name = "WeaponsBtn";
            this.WeaponsBtn.Size = new System.Drawing.Size(113, 30);
            this.WeaponsBtn.TabIndex = 18;
            this.WeaponsBtn.Text = "Weapons";
            this.WeaponsBtn.UseVisualStyleBackColor = false;
            this.WeaponsBtn.Click += new System.EventHandler(this.WeaponsBtn_Click);
            // 
            // ArmorsBtn
            // 
            this.ArmorsBtn.BackColor = System.Drawing.Color.Transparent;
            this.ArmorsBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ArmorsBtn.ForeColor = System.Drawing.Color.Fuchsia;
            this.ArmorsBtn.Location = new System.Drawing.Point(267, 48);
            this.ArmorsBtn.Name = "ArmorsBtn";
            this.ArmorsBtn.Size = new System.Drawing.Size(113, 30);
            this.ArmorsBtn.TabIndex = 19;
            this.ArmorsBtn.Text = "Armors";
            this.ArmorsBtn.UseVisualStyleBackColor = false;
            this.ArmorsBtn.Click += new System.EventHandler(this.ArmorsBtn_Click);
            // 
            // TalismanBtn
            // 
            this.TalismanBtn.BackColor = System.Drawing.Color.Transparent;
            this.TalismanBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.TalismanBtn.ForeColor = System.Drawing.Color.Fuchsia;
            this.TalismanBtn.Location = new System.Drawing.Point(267, 84);
            this.TalismanBtn.Name = "TalismanBtn";
            this.TalismanBtn.Size = new System.Drawing.Size(113, 30);
            this.TalismanBtn.TabIndex = 20;
            this.TalismanBtn.Text = "Talismans";
            this.TalismanBtn.UseVisualStyleBackColor = false;
            this.TalismanBtn.Click += new System.EventHandler(this.TalismanBtn_Click);
            // 
            // MoveItemsToggle
            // 
            this.MoveItemsToggle.AutoSize = true;
            this.MoveItemsToggle.ForeColor = System.Drawing.Color.Fuchsia;
            this.MoveItemsToggle.Location = new System.Drawing.Point(297, 120);
            this.MoveItemsToggle.Name = "MoveItemsToggle";
            this.MoveItemsToggle.Size = new System.Drawing.Size(88, 19);
            this.MoveItemsToggle.TabIndex = 21;
            this.MoveItemsToggle.Text = "Move Items";
            this.MoveItemsToggle.UseVisualStyleBackColor = true;
            // 
            // BuildPrefabMaker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(839, 512);
            this.Controls.Add(this.MoveItemsToggle);
            this.Controls.Add(this.TalismanBtn);
            this.Controls.Add(this.ArmorsBtn);
            this.Controls.Add(this.WeaponsBtn);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.BuildName);
            this.Controls.Add(this.SaveBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.InventoryLayoutPanel);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.AshOfWarBox);
            this.Controls.Add(this.GibBtn);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.UpgradeBox);
            this.Controls.Add(this.QuantityBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.InfusionsBox);
            this.Controls.Add(this.CategoryBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ItemsBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "BuildPrefabMaker";
            this.Text = "Build Maker";
            this.Load += new System.EventHandler(this.BuildPrefabMaker_Load);
            ((System.ComponentModel.ISupportInitialize)(this.QuantityBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UpgradeBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ComboBox ItemsBox;
        private Label label1;
        private ComboBox CategoryBox;
        private ComboBox InfusionsBox;
        private Label label2;
        private NumericUpDown QuantityBox;
        private NumericUpDown UpgradeBox;
        private Label label3;
        private Label label4;
        private Button GibBtn;
        private ComboBox AshOfWarBox;
        private Label label5;
        private FlowLayoutPanel InventoryLayoutPanel;
        private Button CancelBtn;
        private Button SaveBtn;
        private TextBox BuildName;
        private Label label6;
        private Button WeaponsBtn;
        private Button ArmorsBtn;
        private Button TalismanBtn;
        private CheckBox MoveItemsToggle;
    }
}