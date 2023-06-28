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
        public static PHPointer dHitbox;

        public SettingsForm(ErdHook hook, Logger logger)
        {
            InitializeComponent();
            this.hook = hook;
            this.logger = logger;
        }

        private void ShowHitboxToggle_CheckedChanged(object sender, EventArgs e)
        {
            if (!hook.Loaded)
                return;
            dHitbox.WriteByte(0xA1, ShowHitboxToggle.Checked ? (byte)1 : (byte)0);
        }
    }

    
}
