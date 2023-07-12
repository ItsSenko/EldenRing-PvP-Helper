namespace ERPvPHelper
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.TitleLabel = new System.Windows.Forms.Label();
            this.AuthorLabel = new System.Windows.Forms.Label();
            this.TitleGroup = new System.Windows.Forms.GroupBox();
            this.BasicInfoGroup = new System.Windows.Forms.GroupBox();
            this.MadHealToggle = new System.Windows.Forms.CheckBox();
            this.SeamlessAnimChangeToggle = new System.Windows.Forms.CheckBox();
            this.AutoReviveToggle = new System.Windows.Forms.CheckBox();
            this.NoGoodsConsumeToggle = new System.Windows.Forms.CheckBox();
            this.NoStamLossToggle = new System.Windows.Forms.CheckBox();
            this.NoDamageToggle = new System.Windows.Forms.CheckBox();
            this.NoFPConsumeToggle = new System.Windows.Forms.CheckBox();
            this.NoDeadToggle = new System.Windows.Forms.CheckBox();
            this.ManaRefillBtn = new System.Windows.Forms.Button();
            this.HealBtn = new System.Windows.Forms.Button();
            this.ManaBox = new System.Windows.Forms.TextBox();
            this.HealthBox = new System.Windows.Forms.TextBox();
            this.ManaLabel = new System.Windows.Forms.Label();
            this.HealthLabel = new System.Windows.Forms.Label();
            this.AttachButton = new System.Windows.Forms.Button();
            this.LoadingLabel = new System.Windows.Forms.Label();
            this.LogsBox = new System.Windows.Forms.RichTextBox();
            this.SettingsBtn = new System.Windows.Forms.Button();
            this.BuildCreationBtn = new System.Windows.Forms.Button();
            this.OtherOptBtn = new System.Windows.Forms.Button();
            this.TitleGroup.SuspendLayout();
            this.BasicInfoGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // TitleLabel
            // 
            this.TitleLabel.AutoSize = true;
            this.TitleLabel.BackColor = System.Drawing.Color.Black;
            this.TitleLabel.Font = new System.Drawing.Font("Gabriola", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.TitleLabel.ForeColor = System.Drawing.Color.Purple;
            this.TitleLabel.Location = new System.Drawing.Point(7, 12);
            this.TitleLabel.Name = "TitleLabel";
            this.TitleLabel.Size = new System.Drawing.Size(310, 68);
            this.TitleLabel.TabIndex = 0;
            this.TitleLabel.Text = "Elden Ring PVP Helper";
            // 
            // AuthorLabel
            // 
            this.AuthorLabel.AutoSize = true;
            this.AuthorLabel.Font = new System.Drawing.Font("Gabriola", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.AuthorLabel.ForeColor = System.Drawing.Color.Purple;
            this.AuthorLabel.Location = new System.Drawing.Point(107, 69);
            this.AuthorLabel.Name = "AuthorLabel";
            this.AuthorLabel.Size = new System.Drawing.Size(113, 45);
            this.AuthorLabel.TabIndex = 1;
            this.AuthorLabel.Text = "By Senkopur";
            // 
            // TitleGroup
            // 
            this.TitleGroup.BackColor = System.Drawing.Color.Transparent;
            this.TitleGroup.Controls.Add(this.AuthorLabel);
            this.TitleGroup.Controls.Add(this.TitleLabel);
            this.TitleGroup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.TitleGroup.ForeColor = System.Drawing.Color.Black;
            this.TitleGroup.Location = new System.Drawing.Point(128, 12);
            this.TitleGroup.Name = "TitleGroup";
            this.TitleGroup.Size = new System.Drawing.Size(323, 128);
            this.TitleGroup.TabIndex = 2;
            this.TitleGroup.TabStop = false;
            // 
            // BasicInfoGroup
            // 
            this.BasicInfoGroup.BackColor = System.Drawing.Color.Transparent;
            this.BasicInfoGroup.Controls.Add(this.MadHealToggle);
            this.BasicInfoGroup.Controls.Add(this.SeamlessAnimChangeToggle);
            this.BasicInfoGroup.Controls.Add(this.AutoReviveToggle);
            this.BasicInfoGroup.Controls.Add(this.NoGoodsConsumeToggle);
            this.BasicInfoGroup.Controls.Add(this.NoStamLossToggle);
            this.BasicInfoGroup.Controls.Add(this.NoDamageToggle);
            this.BasicInfoGroup.Controls.Add(this.NoFPConsumeToggle);
            this.BasicInfoGroup.Controls.Add(this.NoDeadToggle);
            this.BasicInfoGroup.Controls.Add(this.ManaRefillBtn);
            this.BasicInfoGroup.Controls.Add(this.HealBtn);
            this.BasicInfoGroup.Controls.Add(this.ManaBox);
            this.BasicInfoGroup.Controls.Add(this.HealthBox);
            this.BasicInfoGroup.Controls.Add(this.ManaLabel);
            this.BasicInfoGroup.Controls.Add(this.HealthLabel);
            this.BasicInfoGroup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BasicInfoGroup.ForeColor = System.Drawing.Color.Transparent;
            this.BasicInfoGroup.Location = new System.Drawing.Point(12, 146);
            this.BasicInfoGroup.Name = "BasicInfoGroup";
            this.BasicInfoGroup.Size = new System.Drawing.Size(561, 154);
            this.BasicInfoGroup.TabIndex = 3;
            this.BasicInfoGroup.TabStop = false;
            // 
            // MadHealToggle
            // 
            this.MadHealToggle.AutoSize = true;
            this.MadHealToggle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.MadHealToggle.ForeColor = System.Drawing.Color.Purple;
            this.MadHealToggle.Location = new System.Drawing.Point(300, 82);
            this.MadHealToggle.Name = "MadHealToggle";
            this.MadHealToggle.Size = new System.Drawing.Size(74, 19);
            this.MadHealToggle.TabIndex = 13;
            this.MadHealToggle.Text = "Mad Heal";
            this.MadHealToggle.UseVisualStyleBackColor = true;
            this.MadHealToggle.CheckedChanged += new System.EventHandler(this.MadHealToggle_CheckedChanged);
            // 
            // SeamlessAnimChangeToggle
            // 
            this.SeamlessAnimChangeToggle.AutoSize = true;
            this.SeamlessAnimChangeToggle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SeamlessAnimChangeToggle.ForeColor = System.Drawing.Color.Purple;
            this.SeamlessAnimChangeToggle.Location = new System.Drawing.Point(173, 132);
            this.SeamlessAnimChangeToggle.Name = "SeamlessAnimChangeToggle";
            this.SeamlessAnimChangeToggle.Size = new System.Drawing.Size(144, 19);
            this.SeamlessAnimChangeToggle.TabIndex = 12;
            this.SeamlessAnimChangeToggle.Text = "SeamlessAnim Change";
            this.SeamlessAnimChangeToggle.UseVisualStyleBackColor = true;
            this.SeamlessAnimChangeToggle.CheckedChanged += new System.EventHandler(this.SeamlessAnimChangeToggle_CheckedChanged);
            // 
            // AutoReviveToggle
            // 
            this.AutoReviveToggle.AutoSize = true;
            this.AutoReviveToggle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AutoReviveToggle.ForeColor = System.Drawing.Color.Purple;
            this.AutoReviveToggle.Location = new System.Drawing.Point(173, 82);
            this.AutoReviveToggle.Name = "AutoReviveToggle";
            this.AutoReviveToggle.Size = new System.Drawing.Size(86, 19);
            this.AutoReviveToggle.TabIndex = 11;
            this.AutoReviveToggle.Text = "Auto Revive";
            this.AutoReviveToggle.UseVisualStyleBackColor = true;
            this.AutoReviveToggle.CheckedChanged += new System.EventHandler(this.AutoReviveToggle_CheckedChanged);
            // 
            // NoGoodsConsumeToggle
            // 
            this.NoGoodsConsumeToggle.AutoSize = true;
            this.NoGoodsConsumeToggle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.NoGoodsConsumeToggle.ForeColor = System.Drawing.Color.Purple;
            this.NoGoodsConsumeToggle.Location = new System.Drawing.Point(173, 107);
            this.NoGoodsConsumeToggle.Name = "NoGoodsConsumeToggle";
            this.NoGoodsConsumeToggle.Size = new System.Drawing.Size(135, 19);
            this.NoGoodsConsumeToggle.TabIndex = 10;
            this.NoGoodsConsumeToggle.Text = "Infinite Consumables";
            this.NoGoodsConsumeToggle.UseVisualStyleBackColor = true;
            this.NoGoodsConsumeToggle.CheckedChanged += new System.EventHandler(this.NoGoodsConsumeToggle_CheckedChanged);
            // 
            // NoStamLossToggle
            // 
            this.NoStamLossToggle.AutoSize = true;
            this.NoStamLossToggle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.NoStamLossToggle.ForeColor = System.Drawing.Color.Purple;
            this.NoStamLossToggle.Location = new System.Drawing.Point(300, 57);
            this.NoStamLossToggle.Name = "NoStamLossToggle";
            this.NoStamLossToggle.Size = new System.Drawing.Size(95, 19);
            this.NoStamLossToggle.TabIndex = 9;
            this.NoStamLossToggle.Text = "No Stam Loss";
            this.NoStamLossToggle.UseVisualStyleBackColor = true;
            this.NoStamLossToggle.CheckedChanged += new System.EventHandler(this.NoStamLossToggle_CheckedChanged);
            // 
            // NoDamageToggle
            // 
            this.NoDamageToggle.AutoSize = true;
            this.NoDamageToggle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.NoDamageToggle.ForeColor = System.Drawing.Color.Purple;
            this.NoDamageToggle.Location = new System.Drawing.Point(173, 57);
            this.NoDamageToggle.Name = "NoDamageToggle";
            this.NoDamageToggle.Size = new System.Drawing.Size(86, 19);
            this.NoDamageToggle.TabIndex = 8;
            this.NoDamageToggle.Text = "No Damage";
            this.NoDamageToggle.UseVisualStyleBackColor = true;
            this.NoDamageToggle.CheckedChanged += new System.EventHandler(this.NoDamangeToggle_CheckedChanged);
            // 
            // NoFPConsumeToggle
            // 
            this.NoFPConsumeToggle.AutoSize = true;
            this.NoFPConsumeToggle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.NoFPConsumeToggle.ForeColor = System.Drawing.Color.Purple;
            this.NoFPConsumeToggle.Location = new System.Drawing.Point(300, 32);
            this.NoFPConsumeToggle.Name = "NoFPConsumeToggle";
            this.NoFPConsumeToggle.Size = new System.Drawing.Size(81, 19);
            this.NoFPConsumeToggle.TabIndex = 7;
            this.NoFPConsumeToggle.Text = "No FP Loss";
            this.NoFPConsumeToggle.UseVisualStyleBackColor = true;
            this.NoFPConsumeToggle.CheckedChanged += new System.EventHandler(this.NoFPConsumeToggle_CheckedChanged);
            // 
            // NoDeadToggle
            // 
            this.NoDeadToggle.AutoSize = true;
            this.NoDeadToggle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.NoDeadToggle.ForeColor = System.Drawing.Color.Purple;
            this.NoDeadToggle.Location = new System.Drawing.Point(173, 32);
            this.NoDeadToggle.Name = "NoDeadToggle";
            this.NoDeadToggle.Size = new System.Drawing.Size(69, 19);
            this.NoDeadToggle.TabIndex = 6;
            this.NoDeadToggle.Text = "No Dead";
            this.NoDeadToggle.UseVisualStyleBackColor = true;
            this.NoDeadToggle.CheckedChanged += new System.EventHandler(this.NoDeadToggle_CheckedChanged);
            // 
            // ManaRefillBtn
            // 
            this.ManaRefillBtn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.ManaRefillBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.ManaRefillBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ManaRefillBtn.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ManaRefillBtn.ForeColor = System.Drawing.Color.Purple;
            this.ManaRefillBtn.Location = new System.Drawing.Point(387, 105);
            this.ManaRefillBtn.Name = "ManaRefillBtn";
            this.ManaRefillBtn.Size = new System.Drawing.Size(95, 34);
            this.ManaRefillBtn.TabIndex = 5;
            this.ManaRefillBtn.Text = "Refill";
            this.ManaRefillBtn.UseVisualStyleBackColor = true;
            this.ManaRefillBtn.Click += new System.EventHandler(this.ManaRefillBtn_Click);
            // 
            // HealBtn
            // 
            this.HealBtn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.HealBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.HealBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.HealBtn.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.HealBtn.ForeColor = System.Drawing.Color.Purple;
            this.HealBtn.Location = new System.Drawing.Point(54, 105);
            this.HealBtn.Name = "HealBtn";
            this.HealBtn.Size = new System.Drawing.Size(113, 34);
            this.HealBtn.TabIndex = 4;
            this.HealBtn.Text = "Heal";
            this.HealBtn.UseVisualStyleBackColor = true;
            this.HealBtn.Click += new System.EventHandler(this.HealBtn_Click);
            // 
            // ManaBox
            // 
            this.ManaBox.BackColor = System.Drawing.Color.Black;
            this.ManaBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ManaBox.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ManaBox.ForeColor = System.Drawing.Color.Purple;
            this.ManaBox.Location = new System.Drawing.Point(387, 67);
            this.ManaBox.MaxLength = 4;
            this.ManaBox.Name = "ManaBox";
            this.ManaBox.Size = new System.Drawing.Size(95, 22);
            this.ManaBox.TabIndex = 3;
            this.ManaBox.TabStop = false;
            this.ManaBox.Text = "???";
            this.ManaBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // HealthBox
            // 
            this.HealthBox.BackColor = System.Drawing.Color.Black;
            this.HealthBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.HealthBox.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.HealthBox.ForeColor = System.Drawing.Color.Purple;
            this.HealthBox.Location = new System.Drawing.Point(63, 67);
            this.HealthBox.MaxLength = 4;
            this.HealthBox.Name = "HealthBox";
            this.HealthBox.Size = new System.Drawing.Size(95, 22);
            this.HealthBox.TabIndex = 2;
            this.HealthBox.TabStop = false;
            this.HealthBox.Text = "???";
            this.HealthBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // ManaLabel
            // 
            this.ManaLabel.AutoSize = true;
            this.ManaLabel.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ManaLabel.ForeColor = System.Drawing.Color.Purple;
            this.ManaLabel.Location = new System.Drawing.Point(387, 19);
            this.ManaLabel.Name = "ManaLabel";
            this.ManaLabel.Size = new System.Drawing.Size(99, 45);
            this.ManaLabel.TabIndex = 1;
            this.ManaLabel.Text = "Mana";
            // 
            // HealthLabel
            // 
            this.HealthLabel.AutoSize = true;
            this.HealthLabel.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.HealthLabel.ForeColor = System.Drawing.Color.Purple;
            this.HealthLabel.Location = new System.Drawing.Point(54, 19);
            this.HealthLabel.Name = "HealthLabel";
            this.HealthLabel.Size = new System.Drawing.Size(113, 45);
            this.HealthLabel.TabIndex = 0;
            this.HealthLabel.Text = "Health";
            // 
            // AttachButton
            // 
            this.AttachButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.AttachButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AttachButton.ForeColor = System.Drawing.Color.Purple;
            this.AttachButton.Location = new System.Drawing.Point(469, 12);
            this.AttachButton.Name = "AttachButton";
            this.AttachButton.Size = new System.Drawing.Size(104, 28);
            this.AttachButton.TabIndex = 4;
            this.AttachButton.Text = "Attach Game";
            this.AttachButton.UseVisualStyleBackColor = true;
            this.AttachButton.Click += new System.EventHandler(this.AttachButton_Click);
            // 
            // LoadingLabel
            // 
            this.LoadingLabel.AutoSize = true;
            this.LoadingLabel.ForeColor = System.Drawing.Color.Purple;
            this.LoadingLabel.Location = new System.Drawing.Point(487, 43);
            this.LoadingLabel.Name = "LoadingLabel";
            this.LoadingLabel.Size = new System.Drawing.Size(0, 15);
            this.LoadingLabel.TabIndex = 5;
            this.LoadingLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LogsBox
            // 
            this.LogsBox.BackColor = System.Drawing.Color.Black;
            this.LogsBox.Font = new System.Drawing.Font("JetBrains Mono", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.LogsBox.ForeColor = System.Drawing.Color.Purple;
            this.LogsBox.Location = new System.Drawing.Point(12, 306);
            this.LogsBox.Name = "LogsBox";
            this.LogsBox.ReadOnly = true;
            this.LogsBox.Size = new System.Drawing.Size(561, 180);
            this.LogsBox.TabIndex = 6;
            this.LogsBox.TabStop = false;
            this.LogsBox.Text = "             Welcome to Elden Ring PVP Helper! Made by Senkopur!";
            // 
            // SettingsBtn
            // 
            this.SettingsBtn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.SettingsBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SettingsBtn.ForeColor = System.Drawing.Color.Purple;
            this.SettingsBtn.Location = new System.Drawing.Point(12, 493);
            this.SettingsBtn.Name = "SettingsBtn";
            this.SettingsBtn.Size = new System.Drawing.Size(104, 28);
            this.SettingsBtn.TabIndex = 7;
            this.SettingsBtn.Text = "Settings";
            this.SettingsBtn.UseVisualStyleBackColor = true;
            this.SettingsBtn.Click += new System.EventHandler(this.SettingsBtn_Click);
            // 
            // BuildCreationBtn
            // 
            this.BuildCreationBtn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.BuildCreationBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BuildCreationBtn.ForeColor = System.Drawing.Color.Purple;
            this.BuildCreationBtn.Location = new System.Drawing.Point(469, 492);
            this.BuildCreationBtn.Name = "BuildCreationBtn";
            this.BuildCreationBtn.Size = new System.Drawing.Size(104, 28);
            this.BuildCreationBtn.TabIndex = 8;
            this.BuildCreationBtn.Text = "Build Creation";
            this.BuildCreationBtn.UseVisualStyleBackColor = true;
            // 
            // OtherOptBtn
            // 
            this.OtherOptBtn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.OtherOptBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.OtherOptBtn.ForeColor = System.Drawing.Color.Purple;
            this.OtherOptBtn.Location = new System.Drawing.Point(235, 492);
            this.OtherOptBtn.Name = "OtherOptBtn";
            this.OtherOptBtn.Size = new System.Drawing.Size(104, 28);
            this.OtherOptBtn.TabIndex = 9;
            this.OtherOptBtn.Text = "Other Options";
            this.OtherOptBtn.UseVisualStyleBackColor = true;
            this.OtherOptBtn.Click += new System.EventHandler(this.OtherOptBtn_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(585, 533);
            this.Controls.Add(this.OtherOptBtn);
            this.Controls.Add(this.BuildCreationBtn);
            this.Controls.Add(this.SettingsBtn);
            this.Controls.Add(this.LogsBox);
            this.Controls.Add(this.LoadingLabel);
            this.Controls.Add(this.AttachButton);
            this.Controls.Add(this.BasicInfoGroup);
            this.Controls.Add(this.TitleGroup);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "MainForm";
            this.Text = "ER PvP Helper";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.TitleGroup.ResumeLayout(false);
            this.TitleGroup.PerformLayout();
            this.BasicInfoGroup.ResumeLayout(false);
            this.BasicInfoGroup.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label TitleLabel;
        private Label AuthorLabel;
        private GroupBox TitleGroup;
        private GroupBox BasicInfoGroup;
        private Label HealthLabel;
        private TextBox HealthBox;
        private Label ManaLabel;
        private CheckBox NoFPConsumeToggle;
        private CheckBox NoDeadToggle;
        private Button ManaRefillBtn;
        private Button HealBtn;
        private TextBox ManaBox;
        private Button AttachButton;
        private Label LoadingLabel;
        private CheckBox NoGoodsConsumeToggle;
        private CheckBox NoStamLossToggle;
        private CheckBox NoDamageToggle;
        private CheckBox AutoReviveToggle;
        private Label TestingLabel;
        private CheckBox SeamlessAnimChangeToggle;
        private RichTextBox LogsBox;
        private CheckBox MadHealToggle;
        private Button SettingsBtn;
        private Button BuildCreationBtn;
        private Button OtherOptBtn;
    }
}