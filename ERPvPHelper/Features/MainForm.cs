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
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }
        private ErdHook hook;
        private BasePvPSettings pvpSettings;

        private Logger logger;

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();
        private void Form1_Load(object sender, EventArgs e)
        {
            
            logger = new Logger(this, LogsBox, new StringBuilder());
            Func<Process, bool> func = process => process.ProcessName == "eldenring";

            hook = new(1, 100, func);
            pvpSettings = new(this, hook, logger);

            hook.OnHooked += OnHooked;
            hook.OnUnhooked += OnUnhooked;

            HealthBox.KeyDown += HealthBox_KeyPress;
            ManaBox.KeyDown += ManaBox_KeyPress;

            pvpSettings.NoDeadToggle = NoDeadToggle;
            pvpSettings.NoDamageToggle = NoDamageToggle;
            pvpSettings.NoFPConsumeToggle = NoFPConsumeToggle;
            pvpSettings.NoStamLossToggle = NoStamLossToggle;
            pvpSettings.AutoReviveToggle = AutoReviveToggle;
            pvpSettings.MadHealToggle = MadHealToggle;
            pvpSettings.NoGoodsConsumeToggle = NoGoodsConsumeToggle;
            pvpSettings.SeamlessAnimChangeToggle = SeamlessAnimChangeToggle;

            pvpSettings.ManaBox = ManaBox;
            pvpSettings.HealthBox = HealthBox;

            logger.Log("Form loaded.");
            this.TopLevel = true;
            this.TopMost = true;
        }

        private void OnUnhooked(object? sender, PHEventArgs e)
        {

            Invoke(new Action(() => { LoadingLabel.Text = "Not Attached"; }));
            Invoke(new Action(() =>{ AttachButton.Text = "ReAttach"; }));

            logger.Log("Elden Ring is no longer detecting.");
        }

        private void OnHooked(object? sender, PropertyHook.PHEventArgs e)
        {
            Invoke(new Action(() => { LoadingLabel.Text = "Attached!"; }));
        }

        private void ManaBox_KeyPress(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                pvpSettings.SetMana(ManaBox.Text);
            }
        }

        private void HealthBox_KeyPress(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                pvpSettings.SetHealth(HealthBox.Text);
            }
        }

        private void AttachButton_Click(object sender, EventArgs e)
        {
            if (Process.GetProcessesByName("eldenring").FirstOrDefault() == null)
            {
                MessageBox.Show("Elden Ring not detected");
                logger.Log("Elden Ring was not detected, please check that your game is open. If this issue persists message senkopur on discord.");
                return;
            }

            hook.Start();
            logger.Log("Attaching To Elden Ring (Might take a moment)...");
            LoadingLabel.Text = "Attaching...";
        }

        private void HealBtn_Click(object sender, EventArgs e)
        {
            pvpSettings.HealToMax();
        }

        private void ManaRefillBtn_Click(object sender, EventArgs e)
        {
            pvpSettings.RefillFPToMax();
        }

        private void NoDeadToggle_CheckedChanged(object sender, EventArgs e)
        {
            pvpSettings.NoDeadtoggle();
        }

        private void NoFPConsumeToggle_CheckedChanged(object sender, EventArgs e)
        {
            pvpSettings.InfiniteFPToggle();
        }

        private void NoDamangeToggle_CheckedChanged(object sender, EventArgs e)
        {
            pvpSettings.NoDamagetoggle();
        }

        private void NoStamLossToggle_CheckedChanged(object sender, EventArgs e)
        {
            pvpSettings.InfiniteStamToggle();
        }
        private void NoGoodsConsumeToggle_CheckedChanged(object sender, EventArgs e)
        {
            pvpSettings.InfiniteConsumablesToggle();
        }

        private void AutoReviveToggle_CheckedChanged(object sender, EventArgs e)
        {
            pvpSettings.autoReviveToggle();
        }

        private void SeamlessAnimChangeToggle_CheckedChanged(object sender, EventArgs e)
        {
            pvpSettings.SeamlessAnimToggle();
        }
        
        private void MadHealToggle_CheckedChanged(object sender, EventArgs e)
        {
            pvpSettings.madHealToggle();
        }
        OtherOptionsForm otherOptForm;
        private void OtherOptBtn_Click(object sender, EventArgs e)
        {
            if (!pvpSettings.hook.Hooked)
            {
                logger.Log("The game is not attached. Attach the game first");
                return;
            }

            if (otherOptForm != null)
            {
                otherOptForm.Close();
                otherOptForm = null;
            }

            otherOptForm = new OtherOptionsForm(pvpSettings.hook, pvpSettings.player, logger);

            otherOptForm.StartPosition = FormStartPosition.CenterScreen;
            otherOptForm.Show();
        }
    }
}