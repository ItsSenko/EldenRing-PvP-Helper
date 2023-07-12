using System.Collections;
using System.Diagnostics;
using Erd_Tools;
using Erd_Tools.Models;
using Erd_Tools.Models.Entities;
using Erd_Tools.Models.Items;
using PropertyHook;
using SoulsFormats;
using static Erd_Tools.Models.Param;
using System.Runtime.InteropServices;
using System.Text;

namespace ERPvPHelper
{
    public partial class SettingsForm : Form
    {
        private ErdHook hook { get; set; }
        private Logger logger { get; set; }
        private MainForm form { get; set; }
        private bool hasLoaded = false;
        public static PHPointer dHitbox;

        public SettingsForm(ErdHook hook, Logger logger, MainForm form)
        {
            InitializeComponent();
            this.hook = hook;
            this.logger = logger;
            this.form = form;
            SetColors();
            SeamlessCheckToggle.CheckedChanged -= SeamlessCheckToggle_CheckedChanged;
            SeamlessCheckToggle.CheckState = Settings.Default.DisableSeamlessCheck ? CheckState.Checked : CheckState.Unchecked;
            SeamlessCheckToggle.CheckedChanged += SeamlessCheckToggle_CheckedChanged;
        }
        public void SetColors()
        {
            this.BackColor = Settings.Default.BackgroundColor;

            foreach (Control control in this.Controls)
            {
                if (control is GroupBox box)
                {
                    foreach (Control boxControl in box.Controls)
                    {
                        boxControl.BackColor = Settings.Default.BackgroundColor;
                        boxControl.ForeColor = Settings.Default.ForegroundColor;
                    }
                    continue;
                }
                control.BackColor = Settings.Default.BackgroundColor;
                control.ForeColor = Settings.Default.ForegroundColor;
            }
        }
        

        private void ChangeBackColorBtn_Click(object sender, EventArgs e)
        {
            var dialog = new ColorDialog();
            var result = dialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                Settings.Default.BackgroundColor = dialog.Color;
                Settings.Default.Save();
            }
            else
                logger.Log("Error setting color, 'Invalid Color'. Please try agian.", Logger.LogType.Error);
        }

        private void ChangeForeColorBtn_Click(object sender, EventArgs e)
        {
            var dialog = new ColorDialog();
            var result = dialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                Settings.Default.ForegroundColor = dialog.Color;
                Settings.Default.Save();
            }
            else
                logger.Log("Error setting color, 'Invalid Color'. Please try agian.", Logger.LogType.Error);
        }

        private void SeamlessCheckToggle_CheckedChanged(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to change this check?", "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                Settings.Default.DisableSeamlessCheck = SeamlessCheckToggle.Checked ? true : false;
                Settings.Default.Save();
            }
            else
            {
                SeamlessCheckToggle.CheckedChanged -= SeamlessCheckToggle_CheckedChanged;
                SeamlessCheckToggle.Checked = !SeamlessCheckToggle.Checked;
                SeamlessCheckToggle.CheckedChanged += SeamlessCheckToggle_CheckedChanged;
                return;
            }
        }
    }

    
}
